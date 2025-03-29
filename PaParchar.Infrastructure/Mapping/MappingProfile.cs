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
            CreateMap<Parche, ListParcheDto>();

            CreateMap<Parche, ShowParcheDto>();

            CreateMap<CreateParcheDto, Parche>();
        }
    }
}
