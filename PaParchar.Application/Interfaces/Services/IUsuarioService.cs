using PaParchar.Application.DTOs.Usuario;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IUsuarioService : _IBaseService<Usuario, Guid>
    {
        Task<IResult<ShowUsuarioDto>> CreateUsuario(CreateUsuarioDto usuario);
    }
}
