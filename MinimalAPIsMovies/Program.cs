using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Service Zone- BEGIN

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//congif repository
builder.Services.AddScoped<IGenresRepository, GenresRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Service Zone- END

var app = builder.Build();


//Middlewares Zone- BEGIN

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var genresEndpoint = app.MapGroup("/genres");

genresEndpoint.MapGet("/", async (IGenresRepository repository) =>
{
    return await repository.GetAll();

}
);

genresEndpoint.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genres = await repository.GetById(id);

    // Check if the genre is not found
    if (genres is null)
    {
        return Results.NotFound($"Genre with ID {id} was not found in the database.");
    }

    return Results.Ok(genres);
});


genresEndpoint.MapPost("/", async (Genre genre, IGenresRepository repository) =>
{
    var id = await repository.Create(genre);
    return Results.Created($"/genres/{id}", genre);
});

genresEndpoint.MapPut("/{id:int}", async (int id, Genre genre, IGenresRepository repository) =>
{
    // Check if the genre with the given ID exists
    var exists = await repository.Exists(id);

    if (!exists)
    {
    
        return Results.NotFound($"Genre with ID {id} does not exist in the database. Update failed.");
    }

    await repository.Update(genre);

    return Results.Ok($"Genre with ID {id} was successfully updated.");
});

genresEndpoint.MapDelete("/{id:int}", async (int id, IGenresRepository repository) =>
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



//Middlewares Zone-END

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
