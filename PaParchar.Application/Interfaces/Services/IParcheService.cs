
using PaParchar.Application.DTOs.Parche;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IParcheService : _IBaseService<Parche, Guid>
    {
        Task<IResult<IEnumerable<ListParcheDto>>> GetAllProjected();
        Task<IResult<ShowParcheDto>> CreateParche(CreateParcheDto parche);
    }
}
