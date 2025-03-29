using AutoMapper;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Application.Interfaces.Services;
using PaParchar.Domain.Entities;
using PaParchar.Utils.Results;

namespace PaParchar.Infrastructure.Services
{
    public class ParcheService : _BaseService<Parche, Guid>, IParcheService
    {
        protected readonly IMapper mapper;

        public ParcheService(_IBaseRepository<Parche, Guid> repository, IMapper mapper) : base(repository)
        {
            this.mapper = mapper;
        }

        public async Task<IResult<ShowParcheDto>> CreateParche(CreateParcheDto parcheDto)
        {
            try
            {
                Parche newParche = mapper.Map<Parche>(parcheDto);
                
                if (parcheDto.Horarios != null && parcheDto.Horarios.Any())
                {
                    newParche.Horarios = new List<ParcheHorario>();
                    foreach (var horarioDto in parcheDto.Horarios)
                    {
                        var horario = mapper.Map<ParcheHorario>(horarioDto);
                        newParche.Horarios.Add(horario);
                    }
                }

                Parche created = await _repository.AddAsync(newParche);

                ShowParcheDto result = mapper.Map<ShowParcheDto>(created);

                return await Result<ShowParcheDto>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return await Result<ShowParcheDto>.FailureAsync("Error creando el parche", ex.Message);
            }
        }

        public async Task<IResult<IEnumerable<ListParcheDto>>> GetAllProjected()
        {
            try
            {
                IEnumerable<ListParcheDto> result = await _repository.GetProjectedOrderedAsync<ListParcheDto, DateTime>(orderBy: p => p.FechaInicio, ascending: true, predicate: null);

                return await Result<IEnumerable<ListParcheDto>>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

                return await Result<IEnumerable<ListParcheDto>>.FailureAsync("Error obteniendo los parches", ex.Message);
            }
        }
    }
}
