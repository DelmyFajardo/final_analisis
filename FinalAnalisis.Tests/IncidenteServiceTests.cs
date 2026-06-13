using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalAnalisis.Data;
using FinalAnalisis.Models;
using FinalAnalisis.Services;
using Xunit;

namespace FinalAnalisis.Tests
{
    public class IncidenteServiceTests
    {
        private FinalAnalisisContext ObtenerContextoEnMemoria()
        {
            var opciones = new DbContextOptionsBuilder<FinalAnalisisContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            return new FinalAnalisisContext(opciones);
        }

        [Fact]
        public async Task CrearAsync_DebeInicializarIncidenteCorrectamente()
        {
            // Arrange
            var contexto = ObtenerContextoEnMemoria();
            var servicio = new IncidenteService(contexto);
            var dto = new CrearIncidenteDto
            {
                Titulo = "Enlace Caído",
                Descripcion = "Falla de fibra óptica",
                Severidad = SeveridadIncidente.Alta,
                SitioRed = "Sitio Norte - Cobán"
            };

            // Act
            var resultado = await servicio.CrearAsync(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(EstadoIncidente.Registrado, resultado.Estado);
            Assert.Equal("Sitio Norte - Cobán", resultado.SitioRed);
            Assert.False(resultado.Escalado);
            Assert.True(resultado.CreadoEn <= DateTime.UtcNow);
        }

        [Fact]
        public async Task AsignarOReasignarAsync_DebeFallar_SiTecnicoYaTieneTresIncidentesActivos()
        {
            // Arrange
            var contexto = ObtenerContextoEnMemoria();
            var servicio = new IncidenteService(contexto);
            int idTecnicoSaturado = 7;

            for (int i = 0; i < 3; i++)
            {
                contexto.Incidentes.Add(new Incidente
                {
                    Titulo = $"Incidente Activo {i}",
                    Descripcion = "Soporte",
                    SitioRed = "Sitio Central",
                    TecnicoAsignadoId = idTecnicoSaturado,
                    Estado = EstadoIncidente.EnProgreso
                });
            }
            await contexto.SaveChangesAsync();

            var nuevoIncidente = await servicio.CrearAsync(new CrearIncidenteDto
            {
                Titulo = "Cuarto Incidente",
                Descripcion = "No debería permitir asignarse",
                Severidad = SeveridadIncidente.Baja,
                SitioRed = "Sitio Sur"
            });

            var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                servicio.AsignarOReasignarAsync(nuevoIncidente.Id, idTecnicoSaturado)
            );

            Assert.Contains("límite máximo de 3 incidentes activos", excepcion.Message);
        }

        [Fact]
        public async Task ActualizarEstadoAsync_DebeRespetarFlujoUnidireccionalEstricto()
        {
            // Arrange
            var contexto = ObtenerContextoEnMemoria();
            var servicio = new IncidenteService(contexto);
            
            var incidente = await servicio.CrearAsync(new CrearIncidenteDto
            {
                Titulo = "Ticket de Prueba",
                Descripcion = "Prueba de estados",
                Severidad = SeveridadIncidente.Media,
                SitioRed = "Sitio Oriente"
            });

            var dtoInvalido = new ActualizarEstadoDto 
            { 
                NuevoEstado = EstadoIncidente.EnProgreso, 
                Notas = "Salto de estado prohibido" 
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                servicio.ActualizarEstadoAsync(incidente.Id, dtoInvalido)
            );
        }

        [Fact]
        public async Task VerificarYActivarEscalacionesAsync_DebeEscalarTicket_SiPasanMasDeDosHoras()
        {
            // Arrange
            var contexto = ObtenerContextoEnMemoria();
            var servicio = new IncidenteService(contexto);

            var incidenteAntiguo = new Incidente
            {
                Titulo = "Corte Crítico Antiguo",
                Descripcion = "Nadie lo atendió",
                Severidad = SeveridadIncidente.Critico,
                Estado = EstadoIncidente.Registrado,
                SitioRed = "Sitio Occidente",
                CreadoEn = DateTime.UtcNow.AddHours(-3) 
            };

            contexto.Incidentes.Add(incidenteAntiguo);
            await contexto.SaveChangesAsync();

          
            await servicio.VerificarYActivarEscalacionesAsync();

            // Assert
            var incidenteValidado = await contexto.Incidentes.FindAsync(incidenteAntiguo.Id);
            Assert.NotNull(incidenteValidado);
            Assert.True(incidenteValidado.Escalado); 
        }

        [Fact]
        public async Task ActualizarEstadoAsync_DebeCalcularMinutosDeSLA_AlResolver()
        {
            // Arrange
            var contexto = ObtenerContextoEnMemoria();
            var servicio = new IncidenteService(contexto);

            var incidente = new Incidente
            {
                Titulo = "Ticket para SLA",
                Descripcion = "Medición de minutos",
                Estado = EstadoIncidente.EnProgreso, 
                SitioRed = "Sitio Central",
                CreadoEn = DateTime.UtcNow.AddMinutes(-30) 
            };
            contexto.Incidentes.Add(incidente);
            await contexto.SaveChangesAsync();

            var dtoResuelto = new ActualizarEstadoDto 
            { 
                NuevoEstado = EstadoIncidente.Resuelto, 
                Notas = "Solucionado exitosamente" 
            };

            var resultado = await servicio.ActualizarEstadoAsync(incidente.Id, dtoResuelto);

            Assert.NotNull(resultado.DuracionResolucionMinutos);
            Assert.True(resultado.DuracionResolucionMinutos >= 29); 
        }
    }
}