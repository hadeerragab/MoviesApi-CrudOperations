using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Dtos;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneresController : ControllerBase
    {
        
        private readonly IGenerServices _generServices;

        public GeneresController(IGenerServices generServices)
        {
            _generServices = generServices;
        }

       

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _generServices.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewGenre([FromBody] GenreDto genreDto)
        {
            var NewGenreData = new Genre { Name = genreDto.Name };

            _generServices.Create(NewGenreData);
            return Ok(NewGenreData);
        }

        [HttpPut("{id}")]
        //api / genres / UpdateAsync /id 
        public async Task<IActionResult> UpdateAsync(Byte id, [FromBody] GenreDto genreDto)
        {
            var NewGenreData = await _generServices.GetById(id);
            
            if (NewGenreData == null)
                return NotFound($"No Genre Was Found");

            NewGenreData.Name = genreDto.Name;

            await _generServices.Update(id,NewGenreData);
            
            return Ok(NewGenreData);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Byte id)
        {

            var GenreData = await _generServices.GetById(id);
           

            if (GenreData == null)
                return NotFound($"No Genre Was Found");
            await _generServices.Delete(GenreData);

            return Ok(GenreData);

        }
    }
}
