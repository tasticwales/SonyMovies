using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SonyMovies.Models;
using SonyMovies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SonyMovies.Controllers
{
    [ApiController]
    [Route("/")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IStatsService _statsService;
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger, IMovieService movieService, IStatsService statsService)
        {
            _logger = logger;
            _movieService = movieService;
            _statsService = statsService;
        }


        [HttpPost]
        [Route("metadata")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MovieModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody()]MovieModel movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object received by Post");
                    return BadRequest("Invalid model");
                }

                int newId = await _movieService.Add(movie);

                if (newId > 0)
                {
                    return CreatedAtRoute("metadata", new MovieModel { MovieId = newId }, movie);
                }

                return BadRequest("Unable to add movie");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error within Post action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Route("metadata/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MovieModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> metadata(int Id)
        {
            try
            {
                if (Id < 1)
                {
                    return BadRequest("Invalid ID supplied");
                }

                List<MovieModel> m = await _movieService.GetByID(Id);

                if (m.Count < 1)
                {
                    return NotFound();
                }

                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error within Get By ID: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("movies/stats")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StatsSummaryModel>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> stats()
        {
            try
            {
                List<StatsSummaryModel> m = await _statsService.GetAll();

                if (m.Count < 1)
                {
                    return NotFound();
                }

                return Ok(m);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error within Get Stats: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
