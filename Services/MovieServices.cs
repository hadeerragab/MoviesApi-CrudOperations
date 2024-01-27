using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDbContext _context;

        public MovieServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Create(Movie movie)
        {
            await _context.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> Delete(Movie movie)
        {
            _context.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var movies = await _context.Movies.OrderByDescending(m => m.Rate).Include(m => m.Genre).ToListAsync();

            return (movies);
        }

        public async Task<IEnumerable<Movie>> GetByGenreId(Byte id)
        {
            var movies = await _context.Movies.Include(m => m.Genre).Where(m => m.Genre.Id == id).ToListAsync();

            return movies;
        }

        public async Task<Movie> GetById(int id) => await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

        public async Task<Movie> Update(int id, Movie movie)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();

            return movie;
        }
    }
}
