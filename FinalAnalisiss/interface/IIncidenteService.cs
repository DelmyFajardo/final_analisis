using FinalAnalisis.Models;

namespace FinalAnalisis.Services
{
     public interface IIncidenteService
    {
        Task<Incidente> CrearAsync(CrearIncidenteDto dto);
        Task<Incidente> AsignarOReasignarAsync(int id, int tecnicoId);
        Task<Incidente> ActualizarEstadoAsync(int id, ActualizarEstadoDto dto);
        Task<IEnumerable<Incidente>> ObtenerTodosAsync();
        Task<Incidente?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<HistorialEstado>> ObtenerHistorialAsync(int incidenteId);
        Task<object> ObtenerReporteConsolidadoAsync();
        Task VerificarYActivarEscalacionesAsync();
    }
}