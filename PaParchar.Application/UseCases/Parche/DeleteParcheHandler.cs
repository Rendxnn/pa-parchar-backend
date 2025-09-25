using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class DeleteParcheHandler(IParcheService parcheService)
    {
        private readonly IParcheService _parcheService = parcheService;

        /// <summary>
        /// Elimina un parche por su identificador.
        /// </summary>
        public async Task<IResult<object>> Execute(Guid id)
        {
            return await _parcheService.DeleteAsync(id);
        }
    }
}

