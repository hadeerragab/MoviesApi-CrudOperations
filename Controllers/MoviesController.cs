using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MoviesApi.Dtos;
using MoviesApi.Models;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMovieServices _movieServices;
        private readonly IGenerServices _generServices;
        private new List<string> AllowedExtentions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;


        public MoviesController(IMovieServices movieServices, IGenerServices generServices, IMapper mapper)
        {
            _movieServices = movieServices;
            _generServices = generServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var MoviesData = await _movieServices.GetAll();

            var data = _mapper.Map<IEnumerable<MovieDto>>(MoviesData);

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            var MovieData = await _movieServices.GetById(id);

            if (MovieData == null)
                return NotFound($"Movie Is Not Found");

            return Ok(MovieData);

        }

        [HttpGet("GetMoviesByGenreId")]
        public async Task<IActionResult> GetMoviesByGenreIdAsync (Byte id)
        {
            var MoviesData = await _movieServices.GetByGenreId(id);
            
            if (MoviesData == null)
            {
                var GenreData = await _generServices.GetById(id);
                if (GenreData.Id == null)
                    return NotFound($"Genre Is Not Found");

                return NotFound($"There Is No Movies With THis Genre");

            }

            return Ok(MoviesData);
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieAsync (int id ,[FromForm] MovieDto movieDto)
        {
            var MovieData = await _movieServices.GetById(id);

            if (MovieData == null)
                return NotFound($"No Movie Was Found");


            var IsValidGenre = await _generServices.GetById(movieDto.GenreId);

            if (IsValidGenre == null)
                return BadRequest($"Invalid Id");

            if (movieDto.Poster != null)
            {

                if (!AllowedExtentions.Contains(Path.GetExtension(movieDto.Poster.FileName).ToLower()))
                    return BadRequest($"Only .png & jpg");

                if (movieDto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest($"Max Size = " + _maxAllowedPosterSize);

                using var dataStream = new MemoryStream();
                await movieDto.Poster.CopyToAsync(dataStream);

                MovieData.Poster = dataStream.ToArray();
            }

            MovieData.Title = movieDto.Title;
            MovieData.Year = movieDto.Year;
            MovieData.GenreId = movieDto.GenreId;
            MovieData.Rate = movieDto.Rate;
            MovieData.StoreLine = movieDto.StoreLine;

            _movieServices.Update(id,MovieData);

            return Ok(MovieData);
        }

        [HttpPost]
        public async Task< IActionResult> Create([FromForm]MovieDto movieDto)
        {
            if (movieDto.Poster == null)
                return BadRequest("Poster is rwquird");

            if (!AllowedExtentions.Contains(Path.GetExtension(movieDto.Poster.FileName).ToLower()))
                return BadRequest($"Only .png & jpg");

            if (movieDto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest($"Max Size = "+ _maxAllowedPosterSize);

            var IsValidGenre = await _generServices.GetById(movieDto.GenreId);

            if (IsValidGenre == null)
                return BadRequest($"Invalid Id");

            using var dataStream = new MemoryStream();
            await movieDto.Poster.CopyToAsync(dataStream);

            var MovieData = new Movie()
            {
                Title = movieDto.Title,
                Year = movieDto.Year,
                GenreId = movieDto.GenreId,
                Poster = dataStream.ToArray(),
                Rate = movieDto.Rate,
                StoreLine = movieDto.StoreLine
            };

            _movieServices.Create(MovieData);

            return Ok(MovieData);
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync(int id)
        {
            var MovieData = await _movieServices.GetById(id);

            if (MovieData == null)
                return NotFound($"No Movie Was Found");

            _movieServices.Delete(MovieData);

            return Ok(MovieData);

        }

    }
}
