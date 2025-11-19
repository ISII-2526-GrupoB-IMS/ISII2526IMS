using AppForSEII2526.API.DTOs.ReseñaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReseñasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReseñasController> _logger;

        public ReseñasController(ApplicationDbContext context, ILogger<ReseñasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =====================================
        // Obtener detalle de una reseña
        // =====================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReseñaDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetalleReseña(int id)
        {
            if (_context.Reseña == null)
            {
                _logger.LogError("Error: La tabla Reseña no existe");
                return NotFound();
            }

            var reseña = await _context.Reseña
                .Include(r => r.ApplicationUser)
                .Include(r => r.ItemsReseña)
                    .ThenInclude(d => d.Dispositivo)
                        .ThenInclude(m => m.Modelo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reseña == null)
            {
                _logger.LogError($"Error: Reseña con id {id} no existe");
                return NotFound();
            }

            // Creamos el DTO de respuesta
            var reseñaDTO = new ReseñaDetailDTO(
                reseña.ApplicationUser.NombreUsuario,
                reseña.Pais,
                reseña.Titulo,
                reseña.FechaReseña,
                reseña.ItemsReseña
                    .Select(ir => new ReseñaItemDTO(
                        ir.Dispositivo.NombreDispositivo,
                        ir.Dispositivo.Modelo.NombreModelo,
                        ir.Dispositivo.Año,
                        ir.Puntuacion,
                        ir.Comentario
                    ))
                    .ToList()
            );

            return Ok(reseñaDTO);
        }


        // =====================================
        // Crear una reseña nueva
        // =====================================
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ReseñaDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CrearReseña(ReseñaForCreateDTO reseñaParaCrear)
        {
            // Validar que haya items en la reseña
            if (reseñaParaCrear.ItemsReseña == null || reseñaParaCrear.ItemsReseña.Count == 0)
            {
                ModelState.AddModelError("ItemsReseña", "Error. Debes reseñar al menos un dispositivo.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Buscar el usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.NombreUsuario == reseñaParaCrear.NombreUsuario);

            if (user == null)
            {
                ModelState.AddModelError("ReseñaUsuario", "Error! Usuario no registrado");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Crear la entidad de reseña (vacía al inicio)
            var reseña = new Reseña(
                 0, // EF Core lo asignará automáticamente
        reseñaParaCrear.Titulo,
        reseñaParaCrear.Pais,
        reseñaParaCrear.FechaReseña,
        new List<ItemReseña>(),
        applicationUser: user
            );

            // Procesar cada item de reseña
            foreach (var itemDTO in reseñaParaCrear.ItemsReseña)
            {
                // Buscar el dispositivo
                var dispositivo = await _context.Dispositivo
                    .Include(d => d.Modelo)
                    .FirstOrDefaultAsync(d =>
                        d.NombreDispositivo == itemDTO.NombreDispositivo &&
                        d.Modelo.NombreModelo == itemDTO.Modelo &&
                        d.Año == itemDTO.Año
                    );

                if (dispositivo == null)
                {
                    ModelState.AddModelError("DispositivoNoExiste",
                        $"Error! No se encontró el dispositivo: {itemDTO.NombreDispositivo} {itemDTO.Modelo} ({itemDTO.Año})");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Crear el ItemReseña
                var itemReseña = new ItemReseña(
                    itemDTO.Comentario,
                    itemDTO.Puntuacion,
                    dispositivo,
                    reseña 
                );

                reseña.ItemsReseña.Add(itemReseña);
            }

            // Log para debugging
            _logger.LogInformation($"Creando reseña con {reseña.ItemsReseña.Count} items");

            _context.Add(reseña);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? "No inner exception";
                _logger.LogError($"Error al guardar la reseña: {ex}");
                _logger.LogError($"Inner Exception: {inner}");
                return Conflict($"Error al guardar la reseña: {ex.Message}. Inner: {inner}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error general al guardar la reseña: {ex}");
                return Conflict("Error al guardar la reseña: " + ex.Message);
            }

            // Crear el DTO de respuesta
            var reseñaDetail = new ReseñaDetailDTO(
                reseña.ApplicationUser.NombreUsuario,
                reseña.Pais,
                reseña.Titulo,
                reseña.FechaReseña,
                reseñaParaCrear.ItemsReseña
            );

            return CreatedAtAction("GetDetalleReseña", new { id = reseña.Id }, reseñaDetail);
        }
    }
}
