
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class ParcheService : _BaseService<Parche, Guid>, IParcheService
    {
        public ParcheService(_IBaseRepository<Parche, Guid> repository) : base(repository)
        {
        }

        public async Task<IResult<IEnumerable<ListParcheDto>>> GetAllProjected()
        {
            IEnumerable<ListParcheDto> a = await base._repository.GetProjectedOrderedAsync<ListParcheDto, DateTime>(orderBy: p => p.FechaParche, ascending: true, predicate: null);

            return await Result<IEnumerable<ListParcheDto>>.SuccessAsync(a);
        }
    }
}
