using AutoMapper;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;

namespace PaParchar.Infrastructure.Services
{
    public class ParcheService : _BaseService<Parche, Guid>, IParcheService
    {
        public ParcheService(_IBaseRepository<Parche, Guid> repository, IMapper mapper) : base(repository, mapper)
        {
        }
        
        public async Task<bool> AddImagenes(Guid parcheId, IEnumerable<ParcheImagen> imagenes)
        {
            try
            {
                var parche = await _repository.GetByIdAsync(parcheId);
                if (parche == null)
                {
                    return false;
                }
                
                var dbContext = _repository.GetContext();
                var imagenesDbSet = dbContext.Set<ParcheImagen>();
                
                foreach (var imagen in imagenes)
                {
                    await imagenesDbSet.AddAsync(imagen);
                }
                
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
