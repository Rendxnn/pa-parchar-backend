using Microsoft.AspNetCore.Http;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class SubirPortadaHandler(IParcheService parcheService, IFileStorageService fileStorageService)
    {
        private readonly IParcheService _parcheService = parcheService;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        /// <summary>
        /// Sube y asigna la imagen de portada para un parche.
        /// </summary>
        public async Task<IResult<ShowParcheDto>> Execute(Guid id, IFormFile portada)
        {
            if (portada == null || portada.Length == 0) return Result<ShowParcheDto>.Failure("La imagen de portada es requerida");

            var parcheResult = await _parcheService.GetByIdAsync(id);
            if (parcheResult.Successful != true || parcheResult.Data == null) return Result<ShowParcheDto>.NotFound($"No se encontró el parche con Id: {id}");

            var uploadResult = await _fileStorageService.UploadFileAsync(portada);
            if (uploadResult.Successful != true || string.IsNullOrWhiteSpace(uploadResult.Data)) return Result<ShowParcheDto>.Failure("Error al subir la imagen de portada");

            var entity = parcheResult.Data;
            var updateDto = new UpdateParcheDto
            {
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion,
                Ubicacion = entity.Ubicacion,
                Capacidad = entity.Capacidad,
                EsPrivado = entity.EsPrivado,
                Precio = entity.Precio,
                PortadaUrl = uploadResult.Data,
                Latitud = entity.Latitud,
                Longitud = entity.Longitud,
                Estado = entity.Estado,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin
            };

            return await _parcheService.UpdateFromDto<ShowParcheDto, UpdateParcheDto>(id, updateDto);
        }
    }
}

