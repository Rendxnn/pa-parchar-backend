using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Options;
using PaParchar.Utils.Results;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PaParchar.Infrastructure.Services
{
    public class GCPBucketService : IFileStorageService
    {
        private readonly string _projectId;
        private readonly string _bucketName;
        private readonly string _credentialPath;
        private readonly StorageClient _storageClient;
        private readonly FileStorageOptions _fileOptions;

        public GCPBucketService(
            IConfiguration configuration, 
            IOptions<FileStorageOptions> fileOptions)
        {
            _projectId = configuration["GCPStorage:ProjectId"];
            _bucketName = configuration["GCPStorage:BucketName"];
            _credentialPath = configuration["GCPStorage:CredentialPath"];
            _fileOptions = fileOptions.Value;

            // Validar la configuración
            if (string.IsNullOrEmpty(_projectId) || string.IsNullOrEmpty(_bucketName))
            {
                throw new ArgumentException("La configuración de GCP Storage es inválida. Verifica tus archivos appsettings.json.");
            }

            // Inicializar el cliente de Storage con las credenciales
            if (!string.IsNullOrEmpty(_credentialPath) && File.Exists(_credentialPath))
            {
                GoogleCredential credential = GoogleCredential.FromFile(_credentialPath);
                _storageClient = StorageClient.Create(credential);
            }
            else
            {
                // En entornos de desarrollo o donde ya estén configuradas las credenciales por defecto
                _storageClient = StorageClient.Create();
            }
        }

        public IResult<string> UploadFile(IFormFile formFile)
        {
            // Implementar una versión sincrónica que llama a la versión asincrónica
            return UploadFileAsync(formFile).GetAwaiter().GetResult();
        }

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

                string contentType = formFile.ContentType.ToLower();
                bool isValidMimeType = false;
                foreach (var allowedType in _fileOptions.AllowedMimeTypes)
                {
                    if (contentType == allowedType)
                    {
                        isValidMimeType = true;
                        break;
                    }
                }

                if (!isValidMimeType)
                {
                    return new Result<string>
                    {
                        Successful = false,
                        Message = "Tipo de archivo no permitido",
                        Data = null
                    };
                }

                string fileExtension = Path.GetExtension(formFile.FileName).ToLower();
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                
                string objectName;
                if (_fileOptions.UseDateFolders)
                {
                    objectName = $"{_fileOptions.FolderPrefix}/{DateTime.UtcNow:yyyy-MM-dd}/{uniqueFileName}";
                }
                else
                {
                    objectName = $"{_fileOptions.FolderPrefix}/{uniqueFileName}";
                }

                using (var stream = formFile.OpenReadStream())
                {
                    var uploadOptions = new UploadObjectOptions
                    {
                        PredefinedAcl = PredefinedObjectAcl.PublicRead
                    };

                    var uploadedObject = await _storageClient.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: contentType,
                        source: stream,
                        options: uploadOptions);
                }

                // Construir la URL pública del archivo
                string fileUrl = $"https://storage.googleapis.com/{_bucketName}/{objectName}";

                return new Result<string>
                {
                    Successful = true,
                    Message = "Archivo subido correctamente",
                    Data = fileUrl
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
