using AutoMapper;
using MinimalAPIsMovies.Entites;
using MinimalAPIsMovies.Entites.DTOs;

namespace MinimalAPIsMovies.Utilities
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenreDTO, Genre>();

        }
    }
}
