using Microsoft.AspNetCore.Http;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class SubirImagenesHandler(IParcheService parcheService, IFileStorageService fileStorageService)
    {
        private readonly IParcheService _parcheService = parcheService;
        private readonly IFileStorageService _fileStorageService = fileStorageService;

        /// <summary>
        /// Sube y asocia imágenes adicionales a un parche.
        /// </summary>
        public async Task<IResult<ShowParcheDto>> Execute(Guid id, IList<IFormFile> imagenes)
        {
            if (imagenes == null || imagenes.Count == 0) return Result<ShowParcheDto>.Failure("Se requiere al menos una imagen");

            var parcheResult = await _parcheService.GetByIdAsync(id);
            if (parcheResult.Successful != true || parcheResult.Data == null) return Result<ShowParcheDto>.NotFound($"No se encontró el parche con Id: {id}");

            int maxImagenes = Math.Min(imagenes.Count, 10);
            var parcheImagenes = new List<ParcheImagen>();

            for (int i = 0; i < maxImagenes; i++)
            {
                var file = imagenes[i];
                if (file.Length > 0)
                {
                    var upload = await _fileStorageService.UploadFileAsync(file);
                    if (upload.Successful == true && upload.Data != null)
                    {
                        parcheImagenes.Add(new ParcheImagen
                        {
                            ParcheId = id,
                            ImagenUrl = upload.Data,
                            Orden = i + 1,
                            FechaCreacion = DateTime.UtcNow
                        });
                    }
                }
            }

            if (parcheImagenes.Count == 0) return Result<ShowParcheDto>.Failure("No se pudo procesar ninguna imagen");

            var added = await _parcheService.AddImagenes(id, parcheImagenes);
            if (!added) return Result<ShowParcheDto>.Failure("Error al guardar las imágenes en la base de datos");

            return await _parcheService.GetProjectedByIdAsync<ShowParcheDto>(id);
        }
    }
}

