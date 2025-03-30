using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Extensions;
using PaParchar.Infrastructure.Options;
using PaParchar.Utils.Results;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PaParchar.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly FileStorageOptions _options;

        public FileController(
            IFileStorageService fileStorageService,
            IOptions<FileStorageOptions> options)
        {
            _fileStorageService = fileStorageService;
            _options = options.Value;
        }

        /// <summary>
        /// Sube una imagen al almacenamiento en la nube
        /// </summary>
        /// <param name="file">Archivo a subir (debe ser una imagen: JPG, PNG, GIF o WEBP)</param>
        /// <returns>URL de la imagen subida</returns>
        /// <response code="200">Retorna la URL del archivo subido correctamente</response>
        /// <response code="400">Si el archivo no cumple con las validaciones</response>
        /// <response code="500">Si ocurre un error en el servidor</response>
        [HttpPost("upload")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10MB limite total de la solicitud
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            try
            {
                // Validación básica
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No se ha proporcionado ningún archivo");
                }

                // Validar tamaño
                if (file.Length > _options.MaxFileSize)
                {
                    return BadRequest($"El archivo es demasiado grande. El tamaño máximo permitido es {_options.MaxFileSize / 1024 / 1024}MB");
                }

                // Validar tipo de archivo
                string contentType = file.ContentType.ToLower();
                if (!_options.AllowedMimeTypes.Contains(contentType))
                {
                    return BadRequest($"Tipo de archivo no permitido. Solo se permiten: {string.Join(", ", _options.AllowedMimeTypes)}");
                }

                // Validar extensión
                string extension = Path.GetExtension(file.FileName).ToLower();
                if (string.IsNullOrEmpty(extension) || !_options.AllowedExtensions.Contains(extension))
                {
                    return BadRequest($"Extensión de archivo no válida. Solo se permiten: {string.Join(", ", _options.AllowedExtensions)}");
                }

                // Subir archivo
                IResult<string> result = await _fileStorageService.UploadFileAsync(file);

                if (!result.Successful.HasValue || !result.Successful.Value)
                {
                    return StatusCode(500, new { message = "Error al subir el archivo", error = result.ExceptionMessage });
                }

                return Ok(new { url = result.Data, message = "Archivo subido correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado al procesar el archivo", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene información sobre la configuración de archivos permitidos
        /// </summary>
        /// <returns>Información sobre límites y tipos permitidos</returns>
        [HttpGet("info")]
        [ProducesResponseType(200)]
        public ActionResult<object> GetFileStorageInfo()
        {
            return Ok(new
            {
                maxFileSizeMB = _options.MaxFileSize / 1024 / 1024,
                allowedTypes = _options.AllowedMimeTypes,
                allowedExtensions = _options.AllowedExtensions,
                folderPrefix = _options.FolderPrefix
            });
        }
    }
} 