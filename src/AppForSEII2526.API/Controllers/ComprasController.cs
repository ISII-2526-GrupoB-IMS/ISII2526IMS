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
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(CompraDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CrearCompra(CompraForCreateDTO compraParaCrear)
        {
            // Validar que hay items en la compra
            if (compraParaCrear.ItemsCompra == null || compraParaCrear.ItemsCompra.Count == 0)
            {
                ModelState.AddModelError("ItemsCompra", "Error. Necesitas seleccionar al menos un dispositivo para ser comprado.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Buscar el usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.NombreUsuario == compraParaCrear.NombreUsuario
                                       && u.ApellidosUsuario == compraParaCrear.ApellidosUsuario);

            if (user == null)
            {
                ModelState.AddModelError("CompraApplicationUser", "Error! Usuario no registrado");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Crear la compra (sin ItemsCompra inicialmente)
            var compra = new Compra(
                compraParaCrear.MetodoDePago,
                compraParaCrear.FechaCompra,
                new List<ItemCompra>(),
                user
            );

            double precioTotal = 0;
            int cantidadTotal = 0;

            // Procesar cada item de la compra
            foreach (var itemDTO in compraParaCrear.ItemsCompra)
            {
                // Buscar el dispositivo por marca, modelo y color
                var dispositivo = await _context.Dispositivo
                    .Include(d => d.Modelo)
                    .Where(d => d.Marca == itemDTO.Marca
                             && d.Modelo.NombreModelo == itemDTO.Modelo
                             && d.Color == itemDTO.Color)
                    .FirstOrDefaultAsync();

                if (dispositivo == null)
                {
                    ModelState.AddModelError("DispositivoNoExiste",
                        $"Error! No se encontró el dispositivo: Marca='{itemDTO.Marca}', Modelo='{itemDTO.Modelo}', Color='{itemDTO.Color}'");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Validar stock disponible
                if (dispositivo.CantidadParaCompra < itemDTO.Cantidad)
                {
                    ModelState.AddModelError("DispositivoNoDisponible",
                        $"Error! No hay suficiente stock del dispositivo '{itemDTO.Modelo}'. Disponible: {dispositivo.CantidadParaCompra}, Solicitado: {itemDTO.Cantidad}");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Actualizar la cantidad disponible del dispositivo
                dispositivo.CantidadParaCompra -= itemDTO.Cantidad;

                // Crear el ItemCompra - SOLO con el constructor básico
                var itemCompra = new ItemCompra(
                    dispositivo.Id,
                    dispositivo.PrecioParaCompra,
                    itemDTO.Cantidad
                );

                // NO establecer manualmente IdDispositivo ni Dispositivo
                // Entity Framework lo hará automáticamente por la relación

                // Asignar solo la descripción
                itemCompra.Descripcion = string.IsNullOrEmpty(itemDTO.Descripcion)
                    ? $"{dispositivo.Marca} {dispositivo.Modelo.NombreModelo} - {dispositivo.Color}"
                    : itemDTO.Descripcion;

                // Agregar el item a la compra
                compra.ItemsCompra.Add(itemCompra);

                // Calcular totales
                precioTotal += dispositivo.PrecioParaCompra * itemDTO.Cantidad;
                cantidadTotal += itemDTO.Cantidad;

                // Actualizar el precio en el DTO para la respuesta
                itemDTO.Precio = dispositivo.PrecioParaCompra;
            }

            // Establecer los totales en la compra
            compra.PrecioTotal = precioTotal;
            compra.CantidadTotal = cantidadTotal;

            // Verificar si hay errores de validación
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Log para debugging
            _logger.LogInformation($"Creando compra con {compra.ItemsCompra.Count} items");
            foreach (var item in compra.ItemsCompra)
            {
                _logger.LogInformation($"ItemCompra: DispositivoId={item.IdDispositivo}, Cantidad={item.Cantidad}, Precio={item.Precio}");
            }

            // Agregar la compra al contexto
            _context.Add(compra);

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException?.Message ?? "No inner exception";
                _logger.LogError($"{DateTime.Now}: {ex.ToString()}");
                _logger.LogError($"Inner Exception: {innerException}");
                return Conflict($"Error al guardar la compra: {ex.Message}. Inner: {innerException}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}: {ex.ToString()}");
                return Conflict("Error al guardar la compra: " + ex.Message);
            }

            // Crear el DTO de respuesta
            var compraDetail = new CompraDetailDTO(
                compra.ApplicationUser.NombreUsuario,
                compra.ApplicationUser.ApellidosUsuario,
                compra.ApplicationUser.DireccionDeEnvio,
                compra.FechaCompra,
                compra.PrecioTotal,
                compra.CantidadTotal,
                compraParaCrear.ItemsCompra
            );

            return CreatedAtAction("GetDetalleCompra", new { id = compra.Id }, compraDetail);
        }


    }
}