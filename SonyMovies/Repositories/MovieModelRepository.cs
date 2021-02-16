using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SonyMovies.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SonyMovies.Repositories
{
    public interface IMovieModelRepository
    {
        Task<int> Add(MovieModel m);

        Task<List<MovieModel>> GetByID(int Id);
    }

    public class MovieModelRepository : IMovieModelRepository
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _accessor;

        public MovieModelRepository(ILogger<MovieModelRepository> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        public async Task<int> Add(MovieModel m)
        {
            // session used to represent database table
            Extensions.SessionExtensions.SetObjectAsJson(_accessor.HttpContext.Session, "tableMovieModel", m);

            // normally we would get the ID of the newly created record back from the DB at this point.  For now, just return a 99
            return 99;
        }

        public async Task<List<MovieModel>> GetByID(int Id)
        {
            // session used to represent database table
            if (!Extensions.SessionExtensions.ObjectExists(_accessor.HttpContext.Session, "tableMovieModel"))
            {
                InitMovieTable();
            }

            List<MovieModel> mList = Extensions.SessionExtensions.GetObjectFromJson<List<MovieModel>>(_accessor.HttpContext.Session, "tableMovieModel");

            return mList.Where(m => m.MovieId == Id).GroupBy(m => m.Language).Select(x => x.First()).OrderBy(m => m.Language).ToList();
        }

        private void InitMovieTable()
        {
            // populates our "dummy" database when empty
            TextReader reader = new StreamReader("data/metadata.csv");
            var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var records = csvReader.GetRecords<MovieModel>();

            Extensions.SessionExtensions.SetObjectAsJson(_accessor.HttpContext.Session, "tableMovieModel", records);
        }


    }
}
