using PaParchar.Application.DTOs.Parche;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IParcheService : _IBaseService<Parche, Guid>
    {
        Task<IResult<ShowParcheDto>> CreateParche(CreateParcheDto parche);
        Task<IResult<ShowParcheDto>> UpdateParche(Guid id, UpdateParcheDto parcheDto);
        Task<IResult<ShowParcheDto>> GetParcheById(Guid id);
        Task<IResult<bool>> DeleteParche(Guid id);
    }
}
