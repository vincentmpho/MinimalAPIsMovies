﻿using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Repositories.Interface;

namespace MinimalAPIsMovies.Repositories
{
    public class ActorsRepository : IActorsRepository
    {
        private readonly ApplicationDbContext _context;


        public ActorsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Actor>> GetAll()
        {
            return await _context.Actors.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<Actor> GetById(int id)
        {
            return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> Create(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Actors.AnyAsync(a => a.Id == id);
        }

        public async Task Update (Actor actor)
        {
            _context.Actors.Update(actor);
            await _context.SaveChangesAsync();
        }

        public async Task Delete (int id)
        {
            await _context.Actors.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
