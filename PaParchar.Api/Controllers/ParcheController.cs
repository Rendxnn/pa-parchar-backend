using Microsoft.AspNetCore.Mvc;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcheController : ControllerBase
    {
        protected readonly IParcheService parcheService;

        public ParcheController(IParcheService parcheService)
        {
            this.parcheService = parcheService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ListParcheDto>>> ListParches()
        {
            IResult<IEnumerable<ListParcheDto>> result = await this.parcheService.GetAllProjected();

            return Ok(result.Data);
        }
    }
}
