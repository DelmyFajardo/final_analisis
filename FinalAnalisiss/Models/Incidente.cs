using System;

namespace FinalAnalisis.Models
{
    public enum SeveridadIncidente { Baja, Media, Alta, Critico, Urgente }
    public enum EstadoIncidente { Registrado, Asignado, EnProgreso, Resuelto, Cerrado }

    public class Incidente
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public SeveridadIncidente Severidad { get; set; }
        public EstadoIncidente Estado { get; set; } = EstadoIncidente.Registrado; 
        public int? TecnicoAsignadoId { get; set; }
        public string EspecialidadRequerida { get; set; } = string.Empty;
        public required string SitioRed { get; set; } 
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow; 
        public DateTime? UltimoCambioEstadoEn { get; set; } = DateTime.UtcNow;
        public bool Escalado { get; set; } = false; 
        public double? DuracionResolucionMinutos { get; set; } 
    }

    public class HistorialEstado
    {
        public int Id { get; set; }
        public int IncidenteId { get; set; }
        public EstadoIncidente EstadoAnterior { get; set; }
        public EstadoIncidente EstadoNuevo { get; set; }
        public DateTime CambiadoEn { get; set; } = DateTime.UtcNow; 
        public string Notas { get; set; } = string.Empty;
        public int? TecnicoId { get; set; }
    }

    public class CrearIncidenteDto
    {
        public required string Titulo { get; set; }
        public required string Descripcion { get; set; }
        public SeveridadIncidente Severidad { get; set; }
        public string EspecialidadRequerida { get; set; } = string.Empty;
        public required string SitioRed { get; set; }
    }

    public class AsignarTecnicoDto
    {
        public int TecnicoId { get; set; }
    }

    public class ActualizarEstadoDto
    {
        public EstadoIncidente NuevoEstado { get; set; }
        public string Notas { get; set; } = string.Empty;
    }
}