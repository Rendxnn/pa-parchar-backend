using Microsoft.AspNetCore.Mvc;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Utils.Results;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcheController(IParcheService parcheService) : ControllerBase
    {
        protected readonly IParcheService parcheService = parcheService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListParcheDto>>> ListParches()
        {
            IResult<IEnumerable<ListParcheDto>> result = await this.parcheService.GetProjectedOrderedAsync<ListParcheDto, string>(parche => parche.Nombre, true);

            return result.ToHttpResponse();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowParcheDto>> GetParcheById(Guid id)
        {
            IResult<ShowParcheDto> result = await this.parcheService.GetParcheById(id);

            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ShowParcheDto>> CreateParche([FromBody] CreateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await this.parcheService.CreateParche(parcheDto);

            return result.ToHttpResponse();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ShowParcheDto>> UpdateParche(Guid id, [FromBody] UpdateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await this.parcheService.UpdateParche(id, parcheDto);

            return result.ToHttpResponse();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParche(Guid id)
        {
            IResult<bool> result = await this.parcheService.DeleteParche(id);
            
            if (!result.Successful.GetValueOrDefault())
            {
                return StatusCode(result.Found == false ? 404 : 500, new { message = result.Message, exceptionMessage = result.ExceptionMessage });
            }
            
            return NoContent();
        }
    }
}
