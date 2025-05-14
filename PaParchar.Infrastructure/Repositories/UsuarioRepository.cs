using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Domain.Entities;
using PaParchar.Infrastructure.Configuration.Contexts;

namespace PaParchar.Infrastructure.Repositories
{
    public class UsuarioRepository : _BaseRepository<Usuario, Guid>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
