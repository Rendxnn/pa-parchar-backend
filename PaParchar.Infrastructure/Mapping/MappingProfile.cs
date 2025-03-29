using AutoMapper;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Domain.Entities;


namespace PaParchar.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Parche
            CreateMap<Parche, ListParcheDto>()
                .ForMember(dest => dest.FechaParche, opt => opt.MapFrom(src => src.FechaInicio));

            CreateMap<Parche, ShowParcheDto>();

            CreateMap<CreateParcheDto, Parche>();

            // ParcheHorario
            CreateMap<ParcheHorarioDto, ParcheHorario>();
            CreateMap<ParcheHorario, ParcheHorarioDto>();
        }
    }
}
