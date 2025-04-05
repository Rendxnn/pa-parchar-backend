using AutoMapper;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class ParcheService : _BaseService<Parche, Guid>, IParcheService
    {
        public ParcheService(_IBaseRepository<Parche, Guid> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
