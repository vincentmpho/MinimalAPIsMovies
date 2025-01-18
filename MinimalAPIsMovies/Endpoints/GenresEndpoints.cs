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
                return await repository.GetAll();
            });

            group.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
            {
                var genres = await repository.GetById(id);

                // Check if the genre is not found
                if (genres is null)
                {
                    return Results.NotFound($"Genre with ID {id} was not found in the database.");
                }

                return Results.Ok(genres);
            });

            group.MapPost("/", async (CreateGenreDTO createGenreDTO, IGenresRepository repository) =>
            {
                // Map the DTO to the Genre entity
                var genre = new Genre
                {
                    Name = createGenreDTO.Name
                };

                var id = await repository.Create(genre);
                return Results.Created($"/genres/{id}", genre);
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
                return Results.Ok($"Genre with ID {id} was successfully updated.");
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
