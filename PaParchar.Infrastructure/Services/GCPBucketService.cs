using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Infrastructure.Options;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class GCPBucketService : IFileStorageService
    {
        private readonly string _bucketName;
        private readonly string _credentialPath;
        private readonly StorageClient _storageClient;
        private readonly FileStorageOptions _fileOptions;

        public GCPBucketService(
            IConfiguration configuration, 
            IOptions<FileStorageOptions> fileOptions)
        {
            _bucketName = configuration["GoogleCloudStorage:BucketName"] ?? throw new Exception("Error obteniendo el nombre del bucket");
            _credentialPath = configuration["GoogleCloudStorage:CredentialsFilePath"] ?? throw new Exception("Error obteniendo la ruta del archivo de credenciales");
            _fileOptions = fileOptions.Value;

            if (string.IsNullOrEmpty(_bucketName))
            {
                throw new ArgumentException("La configuración de GoogleCloudStorage es inválida. Verifica el BucketName en tu archivo appsettings.json.");
            }

            if (!string.IsNullOrEmpty(_credentialPath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _credentialPath);
                
                if (!File.Exists(fullPath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo de credenciales en: {fullPath}");
                }
                
                GoogleCredential credential = GoogleCredential.FromFile(fullPath);
                _storageClient = StorageClient.Create(credential);
            }
            else
            {
                _storageClient = StorageClient.Create();
            }
        }

        public IResult<string> UploadFile(IFormFile formFile)
        {
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
                    var uploadOptions = new UploadObjectOptions();

                    var uploadedObject = await _storageClient.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: contentType,
                        source: stream,
                        options: uploadOptions);
                }

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
