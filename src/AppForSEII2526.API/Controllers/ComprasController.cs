using AppForSEII2526.API.DTOs.CompraDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetDetalleCompra(int id)
        {
            if (_context.Compra == null)
            {
                _logger.LogError("Error: La tabla Compra no existe");
                return NotFound();
            }

            var compra = await _context.Compra
             .Where(c => c.Id == id)
                 .Include(c => c.ApplicationUser) //join tabla ApplicationUser
                 .Include(c => c.ItemsCompra) //join table ItemCompra
                    .ThenInclude(d => d.Dispositivo) //then join tabla dispositivo
                        .ThenInclude(m => m.Modelo) //then join tabla Modelo     
             .Select(c => new CompraDetailDTO(c.ApplicationUser.NombreUsuario, c.ApplicationUser.ApellidosUsuario, c.ApplicationUser.DireccionDeEnvio, c.FechaCompra, c.PrecioTotal, c.CantidadTotal, c.ItemsCompra
                        .Select(ci => new CompraItemDTO(ci.Dispositivo.Marca, ci.Dispositivo.Modelo.NombreModelo, ci.Dispositivo.Color, ci.Dispositivo.PrecioParaCompra, c.ItemsCompra.Count, ci.Descripcion)).ToList<CompraItemDTO>()))
             .FirstOrDefaultAsync();


            if (compra == null)
            {
                _logger.LogError($"Error: Compra con {id} no existe");
                return NotFound();
            }


            return Ok(compra);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)] //OK si lo metemos en la base de datos
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)] // BadRequest cuando hay un error 
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)] //Conflict cuando hay un error al añadir a la base de datos
        public async Task<ActionResult> CreateCompra(CompraForCreateDTO compraForCreate)
        {
            if (compraForCreate.ItemsCompra.Count == 0) //comprobamos que he seleccionado algún dispositivo para comprar.
            {
                ModelState.AddModelError("ItemsCompra", "Error! Incluye al menos un coche para comprar");
            }

            var usuario = _context.ApplicationUser.FirstOrDefault(au => au.NombreUsuario == compraForCreate.NombreUsuario); //¿existe el usuario en la base de datos?
            if (usuario == null)
            {
                ModelState.AddModelError("CompraApplicationUser", "Error! NombreUsuario no registrado");
            }

            if (ModelState.ErrorCount > 0) //devuelve BadRequest si hay errores en el anteriore
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var dispostivosCompra = compraForCreate.ItemsCompra.Select(ic => ic.Modelo).ToList<string>();

            var dispositivos = _context.Dispositivo.Include(c => c.ItemsCompra)
                .ThenInclude(ic => ic.Compra)
                .Include(d => d.Modelo)
                .Where(d => dispostivosCompra.Contains(d.Modelo.NombreModelo))
                .Select(d => new
                {
                    d.Id,
                    d.Modelo.NombreModelo,
                    d.CantidadParaCompra,
                    d.PrecioParaCompra,
                    NumeroItemsCompra = d.ItemsCompra.Sum(ic => ic.Cantidad)
                })
                .ToList();

            Compra compra = new Compra(compraForCreate.MetodoDePago, DateTime.Now, new List<ItemCompra>(), usuario);
            compra.PrecioTotal = 0;

            foreach (var item in compraForCreate.ItemsCompra)
            {
                var dispositivo = dispositivos.FirstOrDefault(d => d.NombreModelo == item.Modelo);

                if (dispositivo == null)
                {
                    ModelState.AddModelError("ItemsCompra", $"Error! El dispositivo {item.Modelo} no se puede vender");
                }
                else if ((dispositivo.NumeroItemsCompra + item.Cantidad) > dispositivo.CantidadParaCompra)
                {
                    ModelState.AddModelError("ItemsCompra", $"Error! No hay suficientes unidades {item.Modelo}");
                }
                else
                {
                    compra.ItemsCompra.Add(new ItemCompra(dispositivo.Id, item.Cantidad, compra));
                    item.Precio = dispositivo.PrecioParaCompra;
                    compra.PrecioTotal += (dispositivo.PrecioParaCompra * item.Cantidad);
                }
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(compra);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Compra", $"Error! Error con la compra");
                return Conflict("Error" + ex.Message);
            }

            var compraDetail = new CompraDetailDTO(compra.ApplicationUser.NombreUsuario, compra.ApplicationUser.ApellidosUsuario, compra.ApplicationUser.DireccionDeEnvio, compra.FechaCompra, compra.PrecioTotal, compra.CantidadTotal, compraForCreate.ItemsCompra);

            return CreatedAtAction("GetDetalleCompra", new { id = compra.Id }, compraDetail);
        }



    }
}