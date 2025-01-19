using Microsoft.EntityFrameworkCore;
using MinimalAPIsMovies.Data;
using MinimalAPIsMovies.Endpoints;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Repositories.Interface;
using MinimalAPIsMovies.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Service Zone- BEGIN

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//congif repository
builder.Services.AddScoped<IGenresRepository, GenresRepository>();
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


//Service Zone- END

var app = builder.Build();


//Middlewares Zone- BEGIN

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("/genres").MapGenres();

//Middlewares Zone-END

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
