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
    public interface IStatsModelRepository
    {
        Task<List<StatsSummaryModel>> GetAll();
    }

    public class StatsModelRepository : IStatsModelRepository
    {
        private readonly IHttpContextAccessor _accessor;

        public StatsModelRepository(ILogger<MovieModelRepository> logger, IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<List<StatsSummaryModel>> GetAll()
        {
            // session used to represent database table
            if (!Extensions.SessionExtensions.ObjectExists(_accessor.HttpContext.Session, "tableStatsModel"))
            {
                InitStatsTables();
            }

            List<StatsModel> sList = Extensions.SessionExtensions.GetObjectFromJson<List<StatsModel>>(_accessor.HttpContext.Session, "tableStatsModel");
            List<MovieModel> mList = Extensions.SessionExtensions.GetObjectFromJson<List<MovieModel>>(_accessor.HttpContext.Session, "tableMovieModel");

            var queryList =
                        from movie in mList
                        join stats in sList on movie.MovieId equals stats.movieId
                        select new
                        {
                            movie,
                            stats
                        } into t1
                        group t1 by t1.movie.MovieId into g
                        select new StatsSummaryModel()
                        {
                            MovieId = g.FirstOrDefault().movie.MovieId,
                            Title = g.FirstOrDefault().movie.Title,
                            AverageWatchDurationS = (long)(g.Average(m => m.stats.watchDurationMs)) / 1000,
                            Watches = g.Count(),
                            ReleaseYear = g.FirstOrDefault().movie.ReleaseYear
                        };

            return queryList.OrderByDescending(x => x.Watches).ThenByDescending(x => x.ReleaseYear).ToList();
        }

        private void InitStatsTables()
        {
            // populates our "dummy" database when empty
            TextReader reader = new StreamReader("data/stats.csv");
            var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var stats = csvReader.GetRecords<StatsModel>();

            Extensions.SessionExtensions.SetObjectAsJson(_accessor.HttpContext.Session, "tableStatsModel", stats);

            reader = new StreamReader("data/metadata.csv");
            csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var movies = csvReader.GetRecords<MovieModel>();

            Extensions.SessionExtensions.SetObjectAsJson(_accessor.HttpContext.Session, "tableMovieModel", movies);
        }


    }
}
