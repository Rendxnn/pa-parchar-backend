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
                
                newParche.FechaCreacion = DateTime.UtcNow;
                newParche.FechaInicio = newParche.FechaInicio.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(newParche.FechaInicio, DateTimeKind.Utc) 
                    : newParche.FechaInicio.ToUniversalTime();
                newParche.FechaFin = newParche.FechaFin.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(newParche.FechaFin, DateTimeKind.Utc) 
                    : newParche.FechaFin.ToUniversalTime();
                
                if (parcheDto.Horarios != null && parcheDto.Horarios.Any())
                {
                    newParche.Horarios = new List<ParcheHorario>();
                    foreach (CreateParcheHorarioDto horarioDto in parcheDto.Horarios)
                    {
                        ParcheHorario horario = mapper.Map<ParcheHorario>(horarioDto);
                        newParche.Horarios.Add(horario);
                    }
                }

                Parche created = await _repository.AddAsync(newParche);

                await _repository.SaveChangesAsync();

                ShowParcheDto result = mapper.Map<ShowParcheDto>(created);

                return await Result<ShowParcheDto>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return await Result<ShowParcheDto>.FailureAsync("Error creando el parche", ex.Message);
            }
        }
        
        public async Task<IResult<ShowParcheDto>> UpdateParche(Guid id, UpdateParcheDto parcheDto)
        {
            try
            {
                Parche? existingParche = await _repository.GetByIdWithIncludeAsync(id, "Horarios");
                if (existingParche == null)
                {
                    return await Result<ShowParcheDto>.NotFoundAsync($"No se encontró el parche con Id: {id}");
                }
                
                List<ParcheHorario> existingHorarios = existingParche.Horarios?.ToList() ?? new List<ParcheHorario>();
                
                mapper.Map(parcheDto, existingParche);
                
                existingParche.Id = id;
                
                existingParche.UltimaActualizacion = DateTime.UtcNow;
                
                existingParche.FechaInicio = existingParche.FechaInicio.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(existingParche.FechaInicio, DateTimeKind.Utc) 
                    : existingParche.FechaInicio.ToUniversalTime();
                existingParche.FechaFin = existingParche.FechaFin.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(existingParche.FechaFin, DateTimeKind.Utc) 
                    : existingParche.FechaFin.ToUniversalTime();
                
                if (parcheDto.Horarios != null)
                {
                    if (existingParche.Horarios == null)
                    {
                        existingParche.Horarios = new List<ParcheHorario>();
                    }
                    else
                    {
                        existingParche.Horarios.Clear();
                    }
                    
                    List<ParcheHorario> horariosToKeep = new List<ParcheHorario>();
                    
                    foreach (CreateParcheHorarioDto horarioDto in parcheDto.Horarios)
                    {
                        ParcheHorario? existingHorario = existingHorarios.FirstOrDefault(h => 
                            h.Dia == horarioDto.Dia && 
                            h.HoraInicio.Equals(horarioDto.HoraInicio) && 
                            h.HoraFin.Equals(horarioDto.HoraFin));
                        
                        if (existingHorario != null)
                        {
                            horariosToKeep.Add(existingHorario);
                        }
                        else
                        {
                            ParcheHorario nuevoHorario = mapper.Map<ParcheHorario>(horarioDto);
                            nuevoHorario.ParcheId = id;
                            horariosToKeep.Add(nuevoHorario);
                        }
                    }
                    
                    foreach (ParcheHorario horarioToRemove in existingHorarios.Except(horariosToKeep).ToList())
                    {
                        _repository.GetContext().Set<ParcheHorario>().Remove(horarioToRemove);
                    }
                    
                    foreach (ParcheHorario horario in horariosToKeep)
                    {
                        existingParche.Horarios.Add(horario);
                    }
                }
                
                await _repository.UpdateAsync(existingParche);
                await _repository.SaveChangesAsync();
                
                Parche? updatedParche = await _repository.GetByIdWithIncludeAsync(existingParche.Id, "Horarios");
                
                ShowParcheDto result = mapper.Map<ShowParcheDto>(updatedParche);
                return await Result<ShowParcheDto>.SuccessAsync(result, "Parche actualizado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await Result<ShowParcheDto>.FailureAsync("Error actualizando el parche", ex.Message);
            }
        }
        
        public async Task<IResult<ShowParcheDto>> GetParcheById(Guid id)
        {
            try
            {
                Parche? parche = await _repository.GetByIdWithIncludeAsync(id, "Horarios");
                
                if (parche == null)
                {
                    return await Result<ShowParcheDto>.NotFoundAsync($"No se encontró el parche con Id: {id}");
                }
                
                ShowParcheDto result = mapper.Map<ShowParcheDto>(parche);
                return await Result<ShowParcheDto>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await Result<ShowParcheDto>.FailureAsync($"Error obteniendo el parche con Id: {id}", ex.Message);
            }
        }
        
        public async Task<IResult<bool>> DeleteParche(Guid id)
        {
            try
            {
                Parche? existingParche = await _repository.GetByIdWithIncludeAsync(id, "Horarios");
                if (existingParche == null)
                {
                    return await Result<bool>.NotFoundAsync($"No se encontró el parche con Id: {id}");
                }
                
                if (existingParche.Horarios != null)
                {
                    foreach (ParcheHorario horario in existingParche.Horarios.ToList())
                    {
                        _repository.GetContext().Set<ParcheHorario>().Remove(horario);
                    }
                }
                
                await _repository.DeleteAsync(id);
                await _repository.SaveChangesAsync();
                
                return await Result<bool>.SuccessAsync(true, "Parche eliminado exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return await Result<bool>.FailureAsync($"Error eliminando el parche con Id: {id}", ex.Message);
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
