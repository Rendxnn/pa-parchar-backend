using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class ListParchesHandler(IParcheService parcheService)
    {
        private readonly IParcheService _parcheService = parcheService;

        /// <summary>
        /// Obtiene la lista de parches ordenados por nombre.
        /// </summary>
        public async Task<IResult<IEnumerable<ListParcheDto>>> Execute()
        {
            return await _parcheService.GetProjectedOrderedAsync<ListParcheDto, string>(x => x.Nombre, true);
        }
    }
}

