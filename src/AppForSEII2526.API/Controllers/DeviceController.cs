using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTOs.DispositivoDTOs;
namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ApplicationDbContext context, ILogger<DevicesController> logger)
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
        [ProducesResponseType(typeof(IList<DispositivoParaReseñarDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDispositivosParaAlquilar()
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
