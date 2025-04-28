using PaParchar.Domain.Entities;

namespace PaParchar.Application.Interfaces.Services
{
    public interface IParcheService : _IBaseService<Parche, Guid>
    {
        Task<bool> AddImagenes(Guid parcheId, IEnumerable<ParcheImagen> imagenes);
    }
}
