using Microsoft.AspNetCore.Http;
using PaParchar.Utils.Results;
using System.Threading.Tasks;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        IResult<string> UploadFile(IFormFile formFile);
        Task<IResult<string>> UploadFileAsync(IFormFile formFile);
    }
}
