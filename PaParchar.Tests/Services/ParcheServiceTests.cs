using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using PaParchar.Application.DTOs.Parche;
using PaParchar.Application.Interfaces.Repositories;
using PaParchar.Domain.Entities;
using PaParchar.Infrastructure.Services;

namespace PaParchar.Tests.Services
{
    public class ParcheServiceTests
    {
        private readonly Mock<_IBaseRepository<Parche, Guid>> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ParcheService _parcheService;

        public ParcheServiceTests()
        {
            _mockRepository = new Mock<_IBaseRepository<Parche, Guid>>();
            _mockMapper = new Mock<IMapper>();
            _parcheService = new ParcheService(_mockRepository.Object, _mockMapper.Object);
        }

        #region CreateParche

        [Fact]
        public async Task CreateParche_ConDatosValidos_RetornaExito()
        {
            // Arrange
            var createParcheDto = new CreateParcheDto
            {
                Nombre = "Parche de Prueba",
                Descripcion = "Descripción de prueba",
                FechaInicio = DateTime.UtcNow.AddDays(1),
                FechaFin = DateTime.UtcNow.AddDays(2),
                Horarios = new List<CreateParcheHorarioDto>
                {
                    new CreateParcheHorarioDto
                    {
                        Dia = "L",
                        HoraInicio = new TimeOnly(14, 0),
                        HoraFin = new TimeOnly(16, 0)
                    }
                }
            };

            var mappedParche = new Parche
            {
                Id = Guid.NewGuid(),
                Nombre = createParcheDto.Nombre,
                Descripcion = createParcheDto.Descripcion,
                FechaInicio = createParcheDto.FechaInicio,
                FechaFin = createParcheDto.FechaFin,
                Horarios = new List<ParcheHorario>()
            };

            var showParcheDto = new ShowParcheDto
            {
                ParcheId = mappedParche.Id,
                Nombre = mappedParche.Nombre,
                Descripcion = mappedParche.Descripcion,
                FechaInicio = mappedParche.FechaInicio,
                FechaFin = mappedParche.FechaFin
            };

            _mockMapper.Setup(m => m.Map<Parche>(It.IsAny<CreateParcheDto>())).Returns(mappedParche);
            _mockMapper.Setup(m => m.Map<ParcheHorario>(It.IsAny<CreateParcheHorarioDto>())).Returns(new ParcheHorario());
            _mockMapper.Setup(m => m.Map<ShowParcheDto>(It.IsAny<Parche>())).Returns(showParcheDto);
            
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Parche>())).Returns(Task.FromResult(mappedParche));
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _parcheService.CreateParche(createParcheDto);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal(mappedParche.Id, result.Data.ParcheId);
            Assert.Equal(mappedParche.Nombre, result.Data.Nombre);
            
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Parche>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateParche_CuandoOcurreExcepcion_RetornaError()
        {
            // Arrange
            var createParcheDto = new CreateParcheDto
            {
                Nombre = "Parche de Prueba",
                FechaInicio = DateTime.UtcNow.AddDays(1),
                FechaFin = DateTime.UtcNow.AddDays(2)
            };

            _mockMapper.Setup(m => m.Map<Parche>(It.IsAny<CreateParcheDto>()))
                       .Throws(new Exception("Error de mapeo"));

            // Act
            var result = await _parcheService.CreateParche(createParcheDto);

            // Assert
            Assert.False(result.Successful);
            Assert.Equal("Error creando el parche", result.Message);
            Assert.Equal("Error de mapeo", result.ExceptionMessage);
            
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Parche>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region GetParcheById

        [Fact]
        public async Task GetParcheById_CuandoExisteParche_RetornaParche()
        {
            // Arrange
            var parcheId = Guid.NewGuid();
            var parche = new Parche
            {
                Id = parcheId,
                Nombre = "Parche Existente",
                Descripcion = "Descripción del parche",
                FechaInicio = DateTime.UtcNow.AddDays(1),
                FechaFin = DateTime.UtcNow.AddDays(2)
            };

            var showParcheDto = new ShowParcheDto
            {
                ParcheId = parcheId,
                Nombre = parche.Nombre,
                Descripcion = parche.Descripcion,
                FechaInicio = parche.FechaInicio,
                FechaFin = parche.FechaFin
            };

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(parche));
            _mockMapper.Setup(m => m.Map<ShowParcheDto>(parche))
                      .Returns(showParcheDto);

            // Act
            var result = await _parcheService.GetParcheById(parcheId);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal(parcheId, result.Data.ParcheId);
            Assert.Equal(parche.Nombre, result.Data.Nombre);
            
            _mockRepository.Verify(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"), Times.Once);
        }

        [Fact]
        public async Task GetParcheById_CuandoNoExisteParche_RetornaNotFound()
        {
            // Arrange
            var parcheId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(null));

            // Act
            var result = await _parcheService.GetParcheById(parcheId);

            // Assert
            Assert.False(result.Successful);
            Assert.False(result.Found);
            Assert.Contains($"No se encontró el parche con Id: {parcheId}", result.Message);
            
            _mockRepository.Verify(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"), Times.Once);
        }

        #endregion

        #region UpdateParche
        
        [Fact(Skip = "Esta prueba requiere una revisión más detallada")]
        public async Task UpdateParche_CuandoExisteParche_ActualizaYRetornaParche()
        {
            // Arrange
            var parcheId = Guid.NewGuid();
            var updateParcheDto = new UpdateParcheDto
            {
                Nombre = "Parche Actualizado",
                Descripcion = "Descripción actualizada",
                FechaInicio = DateTime.UtcNow.AddDays(2),
                FechaFin = DateTime.UtcNow.AddDays(3),
                Horarios = new List<CreateParcheHorarioDto>
                {
                    new CreateParcheHorarioDto
                    {
                        Dia = "M",
                        HoraInicio = new TimeOnly(15, 0),
                        HoraFin = new TimeOnly(17, 0)
                    }
                }
            };

            var existingParche = new Parche
            {
                Id = parcheId,
                Nombre = "Parche Original",
                Descripcion = "Descripción original",
                FechaInicio = DateTime.UtcNow.AddDays(1),
                FechaFin = DateTime.UtcNow.AddDays(2),
                Horarios = new List<ParcheHorario>()
            };

            var updatedParche = new Parche
            {
                Id = parcheId,
                Nombre = updateParcheDto.Nombre,
                Descripcion = updateParcheDto.Descripcion,
                FechaInicio = updateParcheDto.FechaInicio,
                FechaFin = updateParcheDto.FechaFin,
                Horarios = new List<ParcheHorario>()
            };

            var showParcheDto = new ShowParcheDto
            {
                ParcheId = parcheId,
                Nombre = updatedParche.Nombre,
                Descripcion = updatedParche.Descripcion,
                FechaInicio = updatedParche.FechaInicio,
                FechaFin = updatedParche.FechaFin,
                Horarios = new List<CreateParcheHorarioDto>()
            };

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(existingParche));
            
            _mockMapper.Setup(m => m.Map(updateParcheDto, existingParche))
                      .Callback<UpdateParcheDto, Parche>((src, dest) => {
                          dest.Nombre = src.Nombre;
                          dest.Descripcion = src.Descripcion;
                          dest.FechaInicio = src.FechaInicio;
                          dest.FechaFin = src.FechaFin;
                          dest.UltimaActualizacion = DateTime.UtcNow;
                      });
            
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Parche>()))
                          .Returns(Task.CompletedTask);
            
            _mockRepository.Setup(r => r.SaveChangesAsync())
                          .Returns(Task.CompletedTask);
            
            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(updatedParche));
            
            _mockMapper.Setup(m => m.Map<ShowParcheDto>(It.IsAny<Parche>()))
                      .Returns(showParcheDto);

            // Mock para el context y DbSet
            var mockContext = new Mock<DbContext>();
            var mockSet = new Mock<DbSet<ParcheHorario>>();

            // Setup para el context
            mockContext.Setup(c => c.Set<ParcheHorario>()).Returns(mockSet.Object);
            _mockRepository.Setup(r => r.GetContext()).Returns(mockContext.Object);

            // Act
            var result = await _parcheService.UpdateParche(parcheId, updateParcheDto);

            // Assert
            Assert.True(result.Successful);
            Assert.Equal(parcheId, result.Data.ParcheId);
            Assert.Equal(updateParcheDto.Nombre, result.Data.Nombre);
            Assert.Equal(updateParcheDto.Descripcion, result.Data.Descripcion);
            Assert.Equal("Parche actualizado exitosamente", result.Message);
            
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Parche>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateParche_CuandoNoExisteParche_RetornaNotFound()
        {
            // Arrange
            var parcheId = Guid.NewGuid();
            var updateParcheDto = new UpdateParcheDto
            {
                Nombre = "Parche Actualizado",
                Descripcion = "Descripción actualizada",
                FechaInicio = DateTime.UtcNow.AddDays(2),
                FechaFin = DateTime.UtcNow.AddDays(3)
            };

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(null));

            // Act
            var result = await _parcheService.UpdateParche(parcheId, updateParcheDto);

            // Assert
            Assert.False(result.Successful);
            Assert.False(result.Found);
            Assert.Contains($"No se encontró el parche con Id: {parcheId}", result.Message);
            
            _mockRepository.Verify(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Parche>()), Times.Never);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region DeleteParche

        [Fact]
        public async Task DeleteParche_CuandoExisteParche_EliminaYRetornaTrue()
        {
            // Arrange
            var parcheId = Guid.NewGuid();
            var existingParche = new Parche
            {
                Id = parcheId,
                Nombre = "Parche a Eliminar",
                FechaInicio = DateTime.UtcNow.AddDays(1),
                FechaFin = DateTime.UtcNow.AddDays(2),
                Horarios = new List<ParcheHorario>()
            };

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(existingParche));
            _mockRepository.Setup(r => r.DeleteAsync(parcheId))
                          .Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync())
                          .Returns(Task.CompletedTask);

            // Mocking context setup para horarios
            var mockContext = new Mock<DbContext>();
            var mockSet = new Mock<DbSet<ParcheHorario>>();
            mockContext.Setup(c => c.Set<ParcheHorario>()).Returns(mockSet.Object);
            _mockRepository.Setup(r => r.GetContext()).Returns(mockContext.Object);

            // Act
            var result = await _parcheService.DeleteParche(parcheId);

            // Assert
            Assert.True(result.Successful);
            Assert.True(result.Data);
            Assert.Equal("Parche eliminado exitosamente", result.Message);
            
            _mockRepository.Verify(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(parcheId), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteParche_CuandoNoExisteParche_RetornaNotFound()
        {
            // Arrange
            var parcheId = Guid.NewGuid();

            _mockRepository.Setup(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"))
                          .Returns(Task.FromResult<Parche?>(null));

            // Act
            var result = await _parcheService.DeleteParche(parcheId);

            // Assert
            Assert.False(result.Successful);
            Assert.False(result.Found);
            Assert.Contains($"No se encontró el parche con Id: {parcheId}", result.Message);
            
            _mockRepository.Verify(r => r.GetByIdWithIncludeAsync(parcheId, "Horarios"), Times.Once);
            _mockRepository.Verify(r => r.DeleteAsync(parcheId), Times.Never);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        #endregion
    }
} 