using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Services
{
    public interface IMovieServices
    {
        Task<IEnumerable<Movie>> GetAll();

        Task<Movie> GetById(int id);

        Task<Movie> Create(Movie movie);

        Task<Movie> Update(int id, Movie movie);

        Task<Movie> Delete(Movie movie);

        Task<IEnumerable<Movie>> GetByGenreId(Byte id);

    }
}
