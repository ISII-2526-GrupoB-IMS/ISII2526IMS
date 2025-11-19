using AppForSEII2526.API.DTOs.DispositivoDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispositivoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DispositivoController> _logger;

        public DispositivoController(ApplicationDbContext context,
            ILogger<DispositivoController> logger)
        {
            _context = context;
            _logger = logger;
        }
        


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DispositivoParaComprarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDispositivosParaComprar(string? filtroNombre, string? filtroColor)
        {
            var dispositivos = await _context.Dispositivo
                .Where(d => (d.NombreDispositivo.Contains(filtroNombre) || filtroNombre == null) && (d.Color.Contains(filtroColor) || filtroColor == null ))
                .Select(d => new DispositivoParaComprarDTO(
                    d.Id,
                    d.NombreDispositivo,
                    d.Marca,
                    d.Modelo,
                    d.Color,
                    d.PrecioParaCompra
                ))
                .ToListAsync();
            return Ok(dispositivos);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DispositivoParaAlquilarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDispositivosParaAlquilar(string? nombreModelo = null, double? precioMaximo = null)
        {
            var dispositivos = await _context.Dispositivo
                .Include(d => d.Modelo)
                .Where(d => ((d.Modelo.NombreModelo.Contains(nombreModelo)) || (nombreModelo == null))
                    && ((d.PrecioParaAlquiler <= precioMaximo) || (precioMaximo == null)))
                .OrderBy(d => d.Modelo.NombreModelo)
                .Select(d => new DispositivoParaAlquilarDTO(
                    d.Id,
                    d.Modelo,
                    d.NombreDispositivo,
                    d.Marca,
                    d.Año,
                    d.Color,
                    d.PrecioParaAlquiler
                ))
                .ToListAsync();

            return Ok(dispositivos);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DispositivoParaReseñarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDispositivosParaReseñar()
        {
            var dispositivos = await _context.Dispositivo
                .Select(d => new DispositivoParaReseñarDTO(
                    d.Id,
                    d.NombreDispositivo,
                    d.Marca,
                    d.Color,
                    d.Año,
                    d.Modelo
                    ))
                .ToListAsync();

            return Ok(dispositivos);
        }
    }
}
