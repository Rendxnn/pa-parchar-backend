using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Utils.Results;

namespace PaParchar.Application.UseCases.Parche
{
    public class UpdateParcheHandler(IParcheService parcheService)
    {
        private readonly IParcheService _parcheService = parcheService;

        /// <summary>
        /// Actualiza un parche a partir del DTO de actualización.
        /// </summary>
        public async Task<IResult<ShowParcheDto>> Execute(Guid id, UpdateParcheDto parcheDto)
        {
            if (parcheDto.Horarios != null)
            {
                foreach (var h in parcheDto.Horarios)
                {
                    if (h.HoraInicio == default || h.HoraFin == default) return Result<ShowParcheDto>.Failure("El formato de horaInicio y horaFin debe ser 'HH:mm'");
                }
            }

            if (parcheDto.FechaInicio >= parcheDto.FechaFin) return Result<ShowParcheDto>.Failure("La fecha de inicio debe ser menor a la fecha de fin");

            return await _parcheService.UpdateFromDto<ShowParcheDto, UpdateParcheDto>(id, parcheDto);
        }
    }
}

