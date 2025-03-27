using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Domain.Entities;
using PaParchar.Infrastructure.Configuration.Contexts;

namespace PaParchar.Infrastructure.Repositories
{
    public class ParcheRepository : _BaseRepository<Parche, Guid>, IParcheRepository
    {
        public ParcheRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
