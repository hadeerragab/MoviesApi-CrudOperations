using AutoMapper;

namespace MoviesApi.Helper
{
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Movie, MovieDto>();
        }
    }
}
