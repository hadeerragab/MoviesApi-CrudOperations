using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Services
{
    public interface IGenerServices 
    {
         Task<IEnumerable<Genre>> GetAll();

         Task<Genre> GetById(byte id);

         Task<Genre> Create(Genre genre);

         Task<Genre> Update(int id, Genre genre);

        Task<Genre> Delete(Genre genre);
    }
}
