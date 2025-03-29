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

        [HttpPost]
        public async Task<ActionResult<ShowParcheDto>> CreateParche([FromBody] CreateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await this.parcheService.CreateParche(parcheDto);

            return result.ToHttpResponse();
        }
    }
}
