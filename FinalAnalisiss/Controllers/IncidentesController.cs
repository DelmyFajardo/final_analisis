using Microsoft.AspNetCore.Mvc;
using FinalAnalisis.Models;
using FinalAnalisis.Services;

namespace FinalAnalisis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")] 
    public class IncidentesController : ControllerBase
    {
        private readonly IIncidenteService _service;

        public IncidentesController(IIncidenteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearIncidenteDto dto)
        {
            var resultado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Id }, resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() => Ok(await _service.ObtenerTodosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var incidente = await _service.ObtenerPorIdAsync(id);
            return incidente == null ? NotFound() : Ok(incidente);
        }

        [HttpPost("{id}/asignar")]
        public async Task<IActionResult> Asignar(int id, [FromBody] AsignarTecnicoDto dto)
        {
            try
            {
                var resultado = await _service.AsignarOReasignarAsync(id, dto.TecnicoId);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { Error = ex.Message }); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoDto dto)
        {
            try
            {
                var resultado = await _service.ActualizarEstadoAsync(id, dto);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex) { return BadRequest(new { Error = ex.Message }); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpGet("{id}/historial")]
        public async Task<IActionResult> ObtenerHistorial(int id) => Ok(await _service.ObtenerHistorialAsync(id));

        [HttpGet("reporte")]
        public async Task<IActionResult> ObtenerReporte() => Ok(await _service.ObtenerReporteConsolidadoAsync());

        [HttpPost("procesar-escalaciones")]
        public async Task<IActionResult> ProcesarEscalaciones()
        {
            await _service.VerificarYActivarEscalacionesAsync();
            return Ok(new { Mensaje = "Rutina de control de tiempos ejecutada con éxito." });
        }
    }
}