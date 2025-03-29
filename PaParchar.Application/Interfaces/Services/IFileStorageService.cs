
using Microsoft.AspNetCore.Http;
using PaParchar.Utils.Results;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        IResult<string> UploadFile(IFormFile formFile);
    }
}
