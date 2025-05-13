using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.DTOs.Usuario;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaParchar.Infrastructure.Services
{
    public class UsuarioService : _BaseService<Usuario, Guid>, IUsuarioService
    {
        protected readonly IMapper mapper;

        public UsuarioService(_IBaseRepository<Usuario, Guid> repository, IMapper mapper) : base(repository, mapper)
        {
            this.mapper = mapper;
        }

        public async Task<IResult<ShowUsuarioDto>> CreateUsuario(CreateUsuarioDto usuarioDto)
        {
            try
            {
                Usuario newUsuario = mapper.Map<Usuario>(usuarioDto);

                Usuario created = await _repository.AddAsync(newUsuario);

                await _repository.SaveChangesAsync();

                ShowUsuarioDto result = mapper.Map<ShowUsuarioDto>(created);

                return Result<ShowUsuarioDto>.Success(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Result<ShowUsuarioDto>.Failure("Error creando el parche", ex.Message);
            }


        }
    }

}
