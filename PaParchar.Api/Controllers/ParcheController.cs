using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Application.UseCases.Parche;
using PaParchar.Domain.Entities;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Utils.Results;
using System.Text.Json;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcheController(
        ListParchesHandler listHandler,
        GetParcheByIdHandler getByIdHandler,
        CreateParcheHandler createHandler,
        UpdateParcheHandler updateHandler,
        DeleteParcheHandler deleteHandler,
        SubirPortadaHandler portadaHandler,
        SubirImagenesHandler imagenesHandler
    ) : ControllerBase
    {
        protected readonly ListParchesHandler _listHandler = listHandler;
        protected readonly GetParcheByIdHandler _getByIdHandler = getByIdHandler;
        protected readonly CreateParcheHandler _createHandler = createHandler;
        protected readonly UpdateParcheHandler _updateHandler = updateHandler;
        protected readonly DeleteParcheHandler _deleteHandler = deleteHandler;
        protected readonly SubirPortadaHandler _portadaHandler = portadaHandler;
        protected readonly SubirImagenesHandler _imagenesHandler = imagenesHandler;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListParcheDto>>> ListParches()
        {
            IResult<IEnumerable<ListParcheDto>> result = await _listHandler.Execute();
            return result.ToHttpResponse();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowParcheDto>> GetParcheById(Guid id)
        {
            IResult<ShowParcheDto> result = await _getByIdHandler.Execute(id);
            return result.ToHttpResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ShowParcheDto>> CreateParche([FromBody] CreateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await _createHandler.Execute(parcheDto);
            return result.ToHttpResponse();
        }
        
        [HttpPost("{id}/portada")]
        public async Task<ActionResult<ShowParcheDto>> SubirPortada(Guid id, IFormFile portada)
        {
            IResult<ShowParcheDto> result = await _portadaHandler.Execute(id, portada);
            return result.ToHttpResponse();
        }
        
        [HttpPost("{id}/imagenes")]
        public async Task<ActionResult<ShowParcheDto>> SubirImagenes(Guid id, IList<IFormFile> imagenes)
        {
            IResult<ShowParcheDto> result = await _imagenesHandler.Execute(id, imagenes);
            return result.ToHttpResponse();
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<ShowParcheDto>> UpdateParche(Guid id, [FromBody] UpdateParcheDto parcheDto)
        {
            IResult<ShowParcheDto> result = await _updateHandler.Execute(id, parcheDto);
            return result.ToHttpResponse();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteParche(Guid id)
        {
            IResult<object> result = await _deleteHandler.Execute(id);
            return result.ToHttpResponse();
        }
    }
}
