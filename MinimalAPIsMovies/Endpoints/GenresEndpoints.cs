using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Entites.DTOs;
using MinimalAPIsMovies.Repositories.Interface;

namespace MinimalAPIsMovies.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IGenresRepository repository) =>
            {
                // Fetch all genres and map to GenreDTO
                var genres = await repository.GetAll();
                var genreDTOs = genres.Select(genre => new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                }).ToList();

                return Results.Ok(genreDTOs);
            });

            group.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
            {
                var genre = await repository.GetById(id);

                // Check if the genre is not found
                if (genre is null)
                {
                    return Results.NotFound($"Genre with ID {id} was not found in the database.");
                }

                // Map to GenreDTO
                var genreDTO = new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                };

                return Results.Ok(genreDTO);
            });

            group.MapPost("/", async (CreateGenreDTO createGenreDTO, IGenresRepository repository) =>
            {
                // Map the DTO to the Genre entity
                var genre = new Genre
                {
                    Name = createGenreDTO.Name
                };

                var id = await repository.Create(genre);

                // Return created Genre as a GenreDTO
                var genreDTO = new GenreDTO
                {
                    Id = id,
                    Name = genre.Name
                };

                return Results.Created($"/genres/{id}", genreDTO);
            });

            group.MapPut("/{id:int}", async (int id, CreateGenreDTO createGenreDTO, IGenresRepository repository) =>
            {
                // Check if the genre with the given ID exists
                var exists = await repository.Exists(id);

                if (!exists)
                {
                    return Results.NotFound($"Genre with ID {id} does not exist in the database. Update failed.");
                }

                // Map the DTO to the Genre entity
                var genre = new Genre
                {
                    Id = id,
                    Name = createGenreDTO.Name
                };

                await repository.Update(genre);

                // Return updated Genre as a GenreDTO
                var updatedGenreDTO = new GenreDTO
                {
                    Id = genre.Id,
                    Name = genre.Name
                };

                return Results.Ok(updatedGenreDTO);
            });

            group.MapDelete("/{id:int}", async (int id, IGenresRepository repository) =>
            {
                // Check if the genre with the given ID exists
                var exists = await repository.Exists(id);

                if (!exists)
                {
                    return Results.NotFound($"Genre with ID {id} does not exist in the database. Delete failed.");
                }

                await repository.Delete(id);
                return Results.Ok("Successfully deleted.");
            });

            return group;
        }
    }
}
