using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Options;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly FileStorageOptions _fileOptions;
        private readonly string _baseDirectory;

        public LocalFileStorageService(IConfiguration configuration, IOptions<FileStorageOptions> fileOptions)
        {
            _fileOptions = fileOptions.Value;
            _baseDirectory = configuration["LocalStorage:BaseDirectory"] ?? "Storage";
        }

        /// <summary>
        /// Sube un archivo a almacenamiento local.
        /// </summary>
        public IResult<string> UploadFile(IFormFile formFile)
        {
            return UploadFileAsync(formFile).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Sube un archivo a almacenamiento local de forma asíncrona.
        /// </summary>
        public async Task<IResult<string>> UploadFileAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return new Result<string>
                {
                    Successful = false,
                    Message = "No se ha proporcionado ningún archivo",
                    Data = null
                };
            }

            try
            {
                if (formFile.Length > _fileOptions.MaxFileSize)
                {
                    return new Result<string>
                    {
                        Successful = false,
                        Message = $"El archivo es demasiado grande. Tamaño máximo: {_fileOptions.MaxFileSize / 1024 / 1024}MB",
                        Data = null
                    };
                }

                var mime = formFile.ContentType.ToLower();
                var allowedMime = _fileOptions.AllowedMimeTypes.Contains(mime);
                if (!allowedMime)
                {
                    return new Result<string>
                    {
                        Successful = false,
                        Message = "Tipo de archivo no permitido",
                        Data = null
                    };
                }

                var ext = Path.GetExtension(formFile.FileName).ToLower();
                var allowedExt = _fileOptions.AllowedExtensions.Contains(ext);
                if (!allowedExt)
                {
                    return new Result<string>
                    {
                        Successful = false,
                        Message = "Extensión de archivo no permitida",
                        Data = null
                    };
                }

                var uniqueName = $"{Guid.NewGuid()}{ext}";
                var relativeFolder = _fileOptions.UseDateFolders ? Path.Combine(_fileOptions.FolderPrefix, DateTime.UtcNow.ToString("yyyy-MM-dd")) : _fileOptions.FolderPrefix;
                var targetFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _baseDirectory, relativeFolder);
                Directory.CreateDirectory(targetFolder);

                var fullPath = Path.Combine(targetFolder, uniqueName);
                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await formFile.CopyToAsync(stream);
                }

                var relativePath = Path.Combine(_baseDirectory, relativeFolder, uniqueName).Replace("\\", "/");

                return new Result<string>
                {
                    Successful = true,
                    Message = "Archivo subido correctamente",
                    Data = relativePath
                };
            }
            catch (Exception ex)
            {
                return new Result<string>
                {
                    Successful = false,
                    Message = "Error al subir el archivo",
                    ExceptionMessage = ex.Message,
                    Data = null
                };
            }
        }
    }
}

