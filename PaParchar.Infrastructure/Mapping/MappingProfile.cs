using AutoMapper;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.DTOs.Usuario;
using PaParchar.Domain.Entities;


namespace PaParchar.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Parche, ListParcheDto>()
                .ForMember(dest => dest.FechaParche, opt => opt.MapFrom(src => src.FechaInicio))
                .ForMember(dest => dest.ParcheId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Ubicacion, opt => opt.MapFrom(src => src.Ubicacion))
                .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src => src.Latitud))
                .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src => src.Longitud));

            CreateMap<Parche, ShowParcheDto>()
                .ForMember(dest => dest.ParcheId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateParcheDto, Parche>();
            
            CreateMap<UpdateParcheDto, Parche>().ReverseMap();

            CreateMap<CreateParcheHorarioDto, ParcheHorario>();
            CreateMap<ParcheHorario, CreateParcheHorarioDto>();
            
            CreateMap<string, TimeOnly>().ConvertUsing(s => ParseTimeString(s));
            CreateMap<TimeOnly, string>().ConvertUsing(t => t.ToString("HH:mm"));

            CreateMap<Usuario, ListUsuarioDto>()
                .ReverseMap();

            CreateMap<CreateUsuarioDto, Usuario>();

            CreateMap<Usuario, ShowUsuarioDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id));
        }

        private TimeOnly ParseTimeString(string timeString)
        {
            return TimeOnly.ParseExact(timeString, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
