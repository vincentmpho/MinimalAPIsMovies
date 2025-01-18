using AutoMapper;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Entites.DTOs;
using MinimalAPIsMovies.Repositories.Interface;

namespace MinimalAPIsMovies.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IGenresRepository repository, IMapper mapper) =>
            {
                // Fetch all genres and map to GenreDTO using AutoMapper
                var genres = await repository.GetAll();
                var genreDTOs = mapper.Map<List<GenreDTO>>(genres);

                return Results.Ok(genreDTOs);
            });

            group.MapGet("/{id:int}", async (int id, IGenresRepository repository, IMapper mapper) =>
            {
                var genre = await repository.GetById(id);

                // Check if the genre is not found
                if (genre is null)
                {
                    return Results.NotFound($"Genre with ID {id} was not found in the database.");
                }

                // Map to GenreDTO using AutoMapper
                var genreDTO = mapper.Map<GenreDTO>(genre);

                return Results.Ok(genreDTO);
            });

            group.MapPost("/", async (CreateGenreDTO createGenreDTO, IGenresRepository repository, IMapper mapper) =>
            {
                // Map the CreateGenreDTO to the Genre entity using AutoMapper
                var genre = mapper.Map<Genre>(createGenreDTO);

                var id = await repository.Create(genre);

                // Return created Genre as a GenreDTO using AutoMapper
                var genreDTO = mapper.Map<GenreDTO>(genre);
                genreDTO.Id = id;  // Set the generated ID

                return Results.Created($"/genres/{id}", genreDTO);
            });

            group.MapPut("/{id:int}", async (int id, CreateGenreDTO createGenreDTO, IGenresRepository repository, IMapper mapper) =>
            {
                // Check if the genre with the given ID exists
                var exists = await repository.Exists(id);

                if (!exists)
                {
                    return Results.NotFound($"Genre with ID {id} does not exist in the database. Update failed.");
                }

                // Map the CreateGenreDTO to the Genre entity using AutoMapper
                var genre = mapper.Map<Genre>(createGenreDTO);
                genre.Id = id;  // Ensure we set the ID correctly

                await repository.Update(genre);

                // Return updated Genre as a GenreDTO using AutoMapper
                var updatedGenreDTO = mapper.Map<GenreDTO>(genre);

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
