using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // =====================================
        // Obtener detalle de una Review
        // =====================================
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetDetalleReview(int id)
        {
            if (_context.Review == null)
            {
                _logger.LogError("Error: La tabla Review no existe");
                return NotFound();
            }

            var Review = await _context.Review
                .Include(r => r.ApplicationUser)
                .Include(r => r.ItemsReview)
                    .ThenInclude(d => d.Dispositivo)
                        .ThenInclude(m => m.Modelo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Review == null)
            {
                _logger.LogError($"Error: Review con id {id} no existe");
                return NotFound();
            }

            // Creamos el DTO de respuesta
            var ReviewDTO = new ReviewDetailDTO(
                Review.ApplicationUser.NombreUsuario,
                Review.Pais,
                Review.Titulo,
                Review.FechaReview,
                Review.ItemsReview
                    .Select(ir => new ReviewItemDTO(
                        ir.Dispositivo.NombreDispositivo,
                        ir.Dispositivo.Modelo.NombreModelo,
                        ir.Dispositivo.Año,
                        ir.Puntuacion,
                        ir.Comentario
                    ))
                    .ToList()
            );

            return Ok(ReviewDTO);
        }


        // =====================================
        // Crear una Review nueva
        // =====================================
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CrearReview(ReviewForCreateDTO ReviewParaCrear)
        {
            // Validar que haya items en la Review
            if (ReviewParaCrear.ItemsReview == null || ReviewParaCrear.ItemsReview.Count == 0)
            {
                ModelState.AddModelError("ItemsReview", "Error. Debes Reviewr al menos un dispositivo.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Buscar el usuario
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.NombreUsuario == ReviewParaCrear.NombreUsuario);

            if (user == null)
            {
                ModelState.AddModelError("ReviewUsuario", "Error! Usuario no registrado");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // Crear la entidad de Review (vacía al inicio)
            var Review = new Review(
                 0, // EF Core lo asignará automáticamente
        ReviewParaCrear.Titulo,
        ReviewParaCrear.Pais,
        ReviewParaCrear.FechaReview,
        new List<ItemReview>(),
        applicationUser: user
            );

            // Procesar cada item de Review
            foreach (var itemDTO in ReviewParaCrear.ItemsReview)
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

               

                if (itemDTO.Comentario != null && !itemDTO.Comentario.StartsWith("Review para"))
                {
                    ModelState.AddModelError("ComentarioInvalido", "Error, el comentario de la Review: debe empezar por Review para");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                // Crear el ItemReview
                var itemReview = new ItemReview(
                    itemDTO.Comentario,
                    itemDTO.Puntuacion,
                    dispositivo,
                    Review 
                );

                Review.ItemsReview.Add(itemReview);
            }

            // Log para debugging
            _logger.LogInformation($"Creando Review con {Review.ItemsReview.Count} items");

            _context.Add(Review);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var inner = ex.InnerException?.Message ?? "No inner exception";
                _logger.LogError($"Error al guardar la Review: {ex}");
                _logger.LogError($"Inner Exception: {inner}");
                return Conflict($"Error al guardar la Review: {ex.Message}. Inner: {inner}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error general al guardar la Review: {ex}");
                return Conflict("Error al guardar la Review: " + ex.Message);
            }

            // Crear el DTO de respuesta
            var ReviewDetail = new ReviewDetailDTO(
                Review.ApplicationUser.NombreUsuario,
                Review.Pais,
                Review.Titulo,
                Review.FechaReview,
                ReviewParaCrear.ItemsReview
            );

            return CreatedAtAction("GetDetalleReview", new { id = Review.Id }, ReviewDetail);
        }
    }
}
