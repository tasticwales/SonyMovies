using SonyMovies.Models;
using SonyMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SonyMovies.Services
{
    public interface IStatsService
    {
        Task<List<StatsSummaryModel>> GetAll();
    }

    public class StatsService : IStatsService
    {
        private readonly IStatsModelRepository _StatsModelRepository;

        public StatsService(IStatsModelRepository StatsModelRepository)
        {
            _StatsModelRepository = StatsModelRepository;
        }

        public async Task<List<StatsSummaryModel>> GetAll()
        {
            return await _StatsModelRepository.GetAll();
        }
    }
}
