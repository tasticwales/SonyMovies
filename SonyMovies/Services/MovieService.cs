using SonyMovies.Models;
using SonyMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonyMovies.Services
{
    public interface IMovieService
    {
        Task<List<MovieModel>> GetByID(int Id);

        Task<int> Add(MovieModel m);
    }

    public class MovieService : IMovieService
    {
        private readonly IMovieModelRepository _MoviewModelRepository;

        public MovieService(IMovieModelRepository MovieModelRepository)
        {
            _MoviewModelRepository = MovieModelRepository;
        }

        public async Task<List<MovieModel>> GetByID (int Id)
        {
            return await _MoviewModelRepository.GetByID(Id);
        }

        public async Task<int> Add(MovieModel m)
        {
            return await _MoviewModelRepository.Add(m);
        }
    }
}
