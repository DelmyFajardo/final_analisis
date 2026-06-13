using Microsoft.EntityFrameworkCore;
using FinalAnalisis.Data;
using FinalAnalisis.Models;

namespace FinalAnalisis.Services
{


    public class IncidenteService : IIncidenteService
    {
        private readonly FinalAnalisisContext _context;

        public IncidenteService(FinalAnalisisContext context)
        {
            _context = context;
        }

        public async Task<Incidente> CrearAsync(CrearIncidenteDto dto)
        {
            var incidente = new Incidente
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Severidad = dto.Severidad,
                EspecialidadRequerida = dto.EspecialidadRequerida,
                SitioRed = dto.SitioRed,
                Estado = EstadoIncidente.Registrado,
                CreadoEn = DateTime.UtcNow
            };

            _context.Incidentes.Add(incidente);
            await _context.SaveChangesAsync();
            return incidente;
        }

        public async Task<Incidente> AsignarOReasignarAsync(int id, int tecnicoId)
        {
            var incidente = await _context.Incidentes.FindAsync(id);
            if (incidente == null) throw new KeyNotFoundException("Incidente no localizado.");
            if (incidente.Estado == EstadoIncidente.Cerrado) throw new InvalidOperationException("No se puede modificar un ticket Cerrado.");

            var cantidadActivos = await _context.Incidentes.CountAsync(i => 
                i.TecnicoAsignadoId == tecnicoId && 
                (i.Estado == EstadoIncidente.Asignado || i.Estado == EstadoIncidente.EnProgreso));

            if (cantidadActivos >= 3)
            {
                throw new InvalidOperationException("El técnico ya cuenta con el límite máximo de 3 incidentes activos.");
            }

            var estadoAnterior = incidente.Estado;
            int? tecnicoAnteriorId = incidente.TecnicoAsignadoId;

            incidente.TecnicoAsignadoId = tecnicoId;

            if (incidente.Estado == EstadoIncidente.Registrado)
            {
                incidente.Estado = EstadoIncidente.Asignado;
                incidente.UltimoCambioEstadoEn = DateTime.UtcNow;
            }

            _context.HistorialEstados.Add(new HistorialEstado
            {
                IncidenteId = incidente.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = incidente.Estado,
                Notas = tecnicoAnteriorId == null ? $"Asignado al técnico #{tecnicoId}" : $"Reasignación. Salió técnico #{tecnicoAnteriorId} e ingresó técnico #{tecnicoId}.",
                TecnicoId = tecnicoId
            });

            await _context.SaveChangesAsync();
            return incidente;
        }

        public async Task<Incidente> ActualizarEstadoAsync(int id, ActualizarEstadoDto dto)
        {
            var incidente = await _context.Incidentes.FindAsync(id);
            if (incidente == null) throw new KeyNotFoundException("Incidente no localizado.");
            if (incidente.Estado == EstadoIncidente.Cerrado) throw new InvalidOperationException("Un incidente no puede cambiar cuando ya está Cerrado.");

            bool esValido = incidente.Estado switch
            {
                EstadoIncidente.Registrado => dto.NuevoEstado == EstadoIncidente.Asignado,
                EstadoIncidente.Asignado => dto.NuevoEstado == EstadoIncidente.EnProgreso,
                EstadoIncidente.EnProgreso => dto.NuevoEstado == EstadoIncidente.Resuelto,
                EstadoIncidente.Resuelto => dto.NuevoEstado == EstadoIncidente.Cerrado,
                _ => false
            };

            if (!esValido)
            {
                throw new InvalidOperationException($"Transición inválida desde {incidente.Estado} hacia {dto.NuevoEstado}.");
            }

            var estadoAnterior = incidente.Estado;
            incidente.Estado = dto.NuevoEstado;
            incidente.UltimoCambioEstadoEn = DateTime.UtcNow;

            if (incidente.Estado == EstadoIncidente.Resuelto)
            {
                incidente.DuracionResolucionMinutos = (incidente.UltimoCambioEstadoEn.Value - incidente.CreadoEn).TotalMinutes;
            }

            _context.HistorialEstados.Add(new HistorialEstado
            {
                IncidenteId = incidente.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = incidente.Estado,
                Notas = dto.Notas
            });

            await _context.SaveChangesAsync();
            return incidente;
        }

        public async Task<IEnumerable<Incidente>> ObtenerTodosAsync() => await _context.Incidentes.ToListAsync();

        public async Task<Incidente?> ObtenerPorIdAsync(int id) => await _context.Incidentes.FindAsync(id);

        public async Task<IEnumerable<HistorialEstado>> ObtenerHistorialAsync(int incidenteId) => 
            await _context.HistorialEstados.Where(h => h.IncidenteId == incidenteId).OrderBy(h => h.CambiadoEn).ToListAsync();

        public async Task<object> ObtenerReporteConsolidadoAsync()
        {
            var incidentes = await _context.Incidentes.ToListAsync();
            return new 
            {
                ReportePorEstado = incidentes.GroupBy(i => i.Estado).Select(g => new { Elemento = g.Key.ToString(), Total = g.Count() }),
                ReportePorSeveridad = incidentes.GroupBy(i => i.Severidad).Select(g => new { Elemento = g.Key.ToString(), Total = g.Count() }),
                ReportePorTecnico = incidentes.GroupBy(i => i.TecnicoAsignadoId).Select(g => new { Elemento = g.Key == null ? "Sin Asignar" : $"Técnico #{g.Key}", Total = g.Count() })
            };
        }

        public async Task VerificarYActivarEscalacionesAsync()
        {
            var tiempoLimite = DateTime.UtcNow.AddHours(-2);
            var incidentesVencidos = await _context.Incidentes
                .Where(i => i.Estado == EstadoIncidente.Registrado && 
                            !i.Escalado && 
                            (i.Severidad == SeveridadIncidente.Critico || i.Severidad == SeveridadIncidente.Urgente) && 
                            i.CreadoEn <= tiempoLimite)
                .ToListAsync();

            foreach (var ticket in incidentesVencidos)
            {
                ticket.Escalado = true;
                _context.HistorialEstados.Add(new HistorialEstado
                {
                    IncidenteId = ticket.Id,
                    EstadoAnterior = ticket.Estado,
                    EstadoNuevo = ticket.Estado,
                    Notas = "Escalación automática por exceder el tiempo máximo de 2 horas en estado Registrado."
                });
            }

            if (incidentesVencidos.Count > 0) await _context.SaveChangesAsync();
        }
    }
}