using Microsoft.AspNetCore.Http;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class GCPBucketService : IFileStorageService
    {
        public IResult<string> UploadFile(IFormFile formFile)
        {
            throw new NotImplementedException();
        }
    }
}
