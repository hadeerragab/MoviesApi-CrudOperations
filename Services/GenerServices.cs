using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services
{
    public class GenerServices : IGenerServices
    {

        private readonly ApplicationDbContext _context;
        public GenerServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Create(Genre genre)
        {
            await _context.AddAsync(genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<Genre> Delete(Genre genre)
        {
            _context.Remove( genre);
            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            var genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();
            return (genres);
        }

        public async Task<Genre> GetById(byte id) => await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);


        public async Task<Genre> Update(int id, Genre genre)
        {
             _context.Update(genre);
            await _context.SaveChangesAsync();

            return genre;
        }
    }
}
