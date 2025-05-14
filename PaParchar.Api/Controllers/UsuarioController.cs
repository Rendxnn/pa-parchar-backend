using Microsoft.AspNetCore.Mvc;
using PaParchar.Application.DTOs.Usuario;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Utils.Results;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
    {
        protected readonly IUsuarioService usuarioService = usuarioService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListUsuarioDto>>> ListUsuario()
        {
            IResult<IEnumerable<ListUsuarioDto>> result = await this.usuarioService.GetProjectedOrderedAsync<ListUsuarioDto, string>(usuario => usuario.Nombre);

            return result.ToHttpResponse();
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<ShowUsuarioDto>> ShowUsuario(Guid usuarioId)
        {
            IResult<ShowUsuarioDto> result = await this.usuarioService.GetProjectedByIdAsync<ShowUsuarioDto>(usuarioId);

            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ShowUsuarioDto>> CreateUsuario([FromBody] CreateUsuarioDto usuarioDto)
        {
            IResult<ShowUsuarioDto> result = await this.usuarioService.CreateUsuario(usuarioDto);

            return result.ToHttpResponse();
        }

        [HttpPut("{usuarioId}")]
        public async Task<ActionResult<ShowUsuarioDto>> UpdateUsuario(Guid usuarioId, [FromBody] UpdateUsuarioDto usuarioDto)
        {
            IResult<ShowUsuarioDto> result = await this.usuarioService.UpdateFromDto<ShowUsuarioDto, UpdateUsuarioDto>(usuarioId, usuarioDto);

            return result.ToHttpResponse();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteParche(Guid id)
        {
            IResult<object> result = await this.usuarioService.DeleteAsync(id);
            return result.ToHttpResponse();
        }
    }
}
