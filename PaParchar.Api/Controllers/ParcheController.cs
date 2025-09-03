using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Utils.Results;
using System.Text.Json;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcheController(IParcheService parcheService, IFileStorageService fileStorageService) : ControllerBase
    {
        protected readonly IParcheService _parcheService = parcheService;
        protected readonly IFileStorageService _fileStorageService = fileStorageService;

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
            try
            {
                // Validación manual para horarios
                if (parcheDto.Horarios != null)
                {
                    foreach (var horario in parcheDto.Horarios)
                    {
                        // Asegurarse de que TimeOnly esté correctamente formateado
                        if (horario.HoraInicio == default || horario.HoraFin == default)
                        {
                            return BadRequest("El formato de horaInicio y horaFin debe ser una cadena en formato 'HH:mm'");
                        }
                    }
                }
                
                IResult<ShowParcheDto> result = await _parcheService.CreateFromDto<ShowParcheDto, CreateParcheDto>(parcheDto);
                return result.ToHttpResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        
        [HttpPost("{id}/portada")]
        public async Task<ActionResult<ShowParcheDto>> SubirPortada(Guid id, IFormFile portada)
        {
            try
            {
                if (portada == null || portada.Length == 0)
                {
                    return BadRequest("La imagen de portada es requerida");
                }
                
                var parcheResult = await _parcheService.GetByIdAsync(id);
                if (parcheResult.Successful != true || parcheResult.Data == null)
                {
                    return NotFound($"No se encontró el parche con Id: {id}");
                }
                
                var parche = parcheResult.Data;
                
                IResult<string> portadaResult = await _fileStorageService.UploadFileAsync(portada);
                if (portadaResult.Successful != true)
                {
                    return BadRequest($"Error al subir la imagen de portada: {portadaResult.Message}");
                }
                
                UpdateParcheDto updateDto = new()
                {
                    Nombre = parche.Nombre,
                    Descripcion = parche.Descripcion,
                    Ubicacion = parche.Ubicacion,
                    Capacidad = parche.Capacidad,
                    EsPrivado = parche.EsPrivado,
                    Precio = parche.Precio,
                    PortadaUrl = portadaResult.Data,
                    Latitud = parche.Latitud,
                    Longitud = parche.Longitud,
                    Estado = parche.Estado,
                    FechaInicio = parche.FechaInicio,
                    FechaFin = parche.FechaFin
                };
                
                IResult<ShowParcheDto> result = await _parcheService.UpdateFromDto<ShowParcheDto, UpdateParcheDto>(id, updateDto);
                return result.ToHttpResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
        
        [HttpPost("{id}/imagenes")]
        public async Task<ActionResult<ShowParcheDto>> SubirImagenes(Guid id, IList<IFormFile> imagenes)
        {
            try
            {
                if (imagenes == null || imagenes.Count == 0)
                {
                    return BadRequest("Se requiere al menos una imagen");
                }
                
                var parcheResult = await _parcheService.GetByIdAsync(id);
                if (parcheResult.Successful != true || parcheResult.Data == null)
                {
                    return NotFound($"No se encontró el parche con Id: {id}");
                }
                
                int maxImagenes = Math.Min(imagenes.Count, 10);
                List<ParcheImagen> parcheImagenes = new();
                
                for (int i = 0; i < maxImagenes; i++)
                {
                    var imagen = imagenes[i];
                    if (imagen.Length > 0)
                    {
                        IResult<string> imagenResult = await _fileStorageService.UploadFileAsync(imagen);

                        if (imagenResult.Successful == true && imagenResult.Data != null)
                        {
                            parcheImagenes.Add(new ParcheImagen
                            {
                                ParcheId = id,
                                ImagenUrl = imagenResult.Data,
                                Orden = i + 1,
                                FechaCreacion = DateTime.UtcNow
                            });
                        }
                    }
                }
                
                if (parcheImagenes.Count == 0)
                {
                    return BadRequest("No se pudo procesar ninguna imagen");
                }
                
                bool success = await _parcheService.AddImagenes(id, parcheImagenes);
                if (!success)
                {
                    return StatusCode(500, "Error al guardar las imágenes en la base de datos");
                }
                
                IResult<ShowParcheDto> result = await _parcheService.GetProjectedByIdAsync<ShowParcheDto>(id);
                return result.ToHttpResponse();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
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
