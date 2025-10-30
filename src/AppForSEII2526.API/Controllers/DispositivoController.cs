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
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ComputeDivision(decimal op1, decimal op2)
        {
            if (op2 == 0)
            {
                _logger.LogError($"{DateTime.Now} Exception: op2=0, division by 0");
                return BadRequest("op2 must be different from 0");
            }
            decimal result = decimal.Round(op1 / op2, 2);
            return Ok(result);
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
        public async Task<ActionResult> GetDispositivosParaAlquilar()
        {
            var dispositivos = await _context.Dispositivo
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
    }
}
