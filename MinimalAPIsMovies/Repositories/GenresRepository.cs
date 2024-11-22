using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Repositories.Interface;

namespace MinimalAPIsMovies.Repositories
{
    public class GenresRepository : IGenresRepository
    {
        private readonly ApplicationDbContext _context;

        public GenresRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Create(Genre genre)
        {
            // Add the genre to the database 
            var createdGenre = _context.Genres.Add(genre);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the ID of the newly created Genre
            return createdGenre.Entity.Id;
        }

        public async Task Delete(int id)
        {

            await _context.Genres.Where(g => g.Id==id).ExecuteDeleteAsync();
          
        }

        public async Task<bool> Exists(int id)
        {
             return await _context.Genres.AnyAsync(g => g.Id == id); 
        }

        public async Task<List<Genre>> GetAll() 
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public  async Task<Genre?> GetById(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id==id);
        }

        public async Task Update(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}
