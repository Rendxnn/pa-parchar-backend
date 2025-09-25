using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class GetParcheByIdHandler(IParcheService parcheService)
    {
        private readonly IParcheService _parcheService = parcheService;

        /// <summary>
        /// Obtiene un parche por su identificador proyectado a DTO.
        /// </summary>
        public async Task<IResult<ShowParcheDto>> Execute(Guid id)
        {
            return await _parcheService.GetProjectedByIdAsync<ShowParcheDto>(id);
        }
    }
}

