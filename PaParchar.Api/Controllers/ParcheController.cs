using Microsoft.AspNetCore.Mvc;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.DTOs.Usuario;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Utils.Results;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcheController(IParcheService parcheService) : ControllerBase
    {
        protected readonly IParcheService _parcheService = parcheService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListParcheDto>>> ListParches()
        {
            IResult<IEnumerable<ListParcheDto>> result = await _parcheService.GetProjectedOrderedAsync<ListParcheDto, string>(parche => parche.Nombre, true);

            return result.ToHttpResponse();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowParcheDto>> GetParcheById(Guid id)
        {
            IResult<ShowParcheDto> result = await _parcheService.GetProjectedByIdAsync<ShowParcheDto>(id);

            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ShowParcheDto>> CreateParche([FromBody] CreateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await _parcheService.CreateFromDto<ShowParcheDto, CreateParcheDto>(parcheDto);

            return result.ToHttpResponse();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ShowParcheDto>> UpdateParche(Guid id, [FromBody] UpdateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await _parcheService.UpdateFromDto<ShowParcheDto, UpdateParcheDto>(id, parcheDto);

            return result.ToHttpResponse();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteParche(Guid id)
        {
            IResult<object> result = await this._parcheService.DeleteAsync(id);

            return result.ToHttpResponse();
        }
    }
}
