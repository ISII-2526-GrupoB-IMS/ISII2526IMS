using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTOs.CompraDTOs;
namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprasController> _logger;

        public ComprasController(ApplicationDbContext context, ILogger<ComprasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompra(int id)
        {
            var compradto = await _context.Compra
             .Where(c => c.Id == id)
             .Include(c => c.ItemsCompra) 
                    .ThenInclude(ic => ic.Dispositivo) 
                        .ThenInclude(v => v.Modelo) 
             .Select(c => new CompraDetailDTO(c.Id, c.ApplicationUser.NombreUsuario, c.ApplicationUser.ApellidosUsuario, c.FechaCompra, c.DireccionEntrega,
                        .


            if (compradto == null)
            {
                _logger.LogError($"Error: Compra con id {id} no existe");
                return NotFound();
            }

            return Ok(compradto);


        }

    }
}
}
