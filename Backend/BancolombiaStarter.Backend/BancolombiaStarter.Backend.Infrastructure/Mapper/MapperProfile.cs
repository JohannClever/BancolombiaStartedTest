using AutoMapper;
using BancolombiaStarter.Backend.Domain.Dto;
using BancolombiaStarter.Backend.Domain.Entities;
using BancolombiaStarter.Backend.Infrastructure.Authorization.Entities;

namespace BancolombiaStarter.Backend.Infrastructure.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Comments, CommentsDto>().ReverseMap();
            CreateMap<Comments, CommentsDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.CompanyServiceId, opt => opt.MapFrom(src => src.ProjectId))
               .ForMember(dest => dest.CompanyServiceName, opt => opt.MapFrom(src => src.Project.Name))
               .ForMember(dest => dest.IdUser, opt => opt.MapFrom(src => src.IdUser))
               .ForMember(dest => dest.Observations, opt => opt.MapFrom(src => src.Observations))
               .ForMember(dest => dest.CreationOn, opt => opt.MapFrom(src => src.CreationOn));
            CreateMap<Finance, FinanceDto>().ReverseMap();
            CreateMap<Finance, FinanceCreateDto>().ReverseMap();
            CreateMap<Projects, ProjectsDto>().ReverseMap();
            CreateMap<Projects, ProjectsCreateDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();

        }
    }
}
