﻿using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Entites;

namespace MinimalAPIsMovies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);

            modelBuilder.Entity<Actor>().Property(p => p.Name).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(p => p.Picture).IsUnicode();
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet <Actor> Actors { get; set; }
    }
}
