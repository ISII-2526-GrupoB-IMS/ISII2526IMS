using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System.Collections.Immutable;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlquileresController> _logger;

        public AlquileresController(ApplicationDbContext context, ILogger<AlquileresController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(AlquilerDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetAlquiler(int id)
        {
            if (_context.Alquiler == null)
            {
                _logger.LogError("Error: Tabla alquiler no existe");
                return NotFound();
            }

            var alquiler = await _context.Alquiler
             .Where(r => r.Id == id)
                 .Include(r => r.ItemsAlquiler) //join table 
                    .ThenInclude(ia => ia.Dispositivo) //then join table Movies
                        .ThenInclude(disp => disp.Modelo) //then join table Genre

             .Select(r => new AlquilerDetailDTO(
                 r.Id, r.FechaAlquiler, r.ApplicationUser.NombreUsuario, r.ApplicationUser.ApellidosUsuario
                 , r.DireccionEntrega, r.MetodoPago, r.FechaAlquilerDesde, r.FechaAlquilerHasta, r.ItemsAlquiler
                        .Select(ia => new ItemAlquilerDTO(
                            ia.Dispositivo.Id,
                            ia.Dispositivo.Modelo.NombreModelo,
                            ia.Dispositivo.NombreDispositivo,
                            ia.Dispositivo.Marca,
                            ia.Dispositivo.PrecioParaAlquiler)).ToList()))
             .FirstOrDefaultAsync();


            if (alquiler == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(alquiler);
        }

        [HttpPost]
        [Route("[action]")]

        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(AlquilerForCreateDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CrearAlquiler(AlquilerForCreateDTO alquilerForCreate)
        {

            if (alquilerForCreate.FechaAlquilerDesde <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (alquilerForCreate.FechaAlquilerDesde >= alquilerForCreate.FechaAlquilerHasta)
                ModelState.AddModelError("RentalDateFrom&RentalDateTo", "Error! Your rental must end later than it starts");

            if (alquilerForCreate.ItemsAlquiler.Count == 0)
                ModelState.AddModelError("RentalItems", "Error! You must include at least one movie to be rented");

            //we must relate the Rental to the User
            var user = await _context.Users.FirstOrDefaultAsync(au => au.UserName == alquilerForCreate.NombreUsuario);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");


            var nombreDispositivo = alquilerForCreate.ItemsAlquiler.Select(ia => ia.NombreDispositivo).ToList<string>();

            var dispositivos = _context.Dispositivo.Include(d => d.ItemsAlquiler)
                .ThenInclude(ia => ia.Alquiler)

                //we must check that all the movies to be rented exist in the database 
                .Where(m => nombreDispositivo.Contains(m.NombreDispositivo))

                //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
                .Select(m => new
                {
                    m.Id,
                    m.NombreDispositivo,
                    m.CantidadParaAlquilar,
                    m.PrecioParaAlquiler,

                    //we count the number of rentalItems that are within the rental period 
                    NumeroDeDispositivosAlquilados = m.ItemsAlquiler.Count(ia => ia.Alquiler.FechaAlquilerDesde <= alquilerForCreate.FechaAlquilerHasta
                            && ia.Alquiler.FechaAlquilerHasta >= alquilerForCreate.FechaAlquilerDesde)
                })
                .ToList();



                //we must provide rental with the info to be saved in the database
                Alquiler alquiler = new Alquiler(alquilerForCreate.DireccionEntrega, alquilerForCreate.NombreUsuario,
                alquilerForCreate.ApellidosUsuario, user, alquilerForCreate.FechaAlquilerDesde, alquilerForCreate.FechaAlquilerHasta,
                alquilerForCreate.MetodoPago, new List<ItemAlquiler>());


                foreach (var item in alquilerForCreate.ItemsAlquiler)
                {
                    var dispositivo = dispositivos.FirstOrDefault(d => d.NombreDispositivo == item.NombreDispositivo);
                    //we must check that there is enough quantity to be rented in the database
                    if ((dispositivo == null) || (dispositivo.NumeroDeDispositivosAlquilados >= dispositivo.CantidadParaAlquilar))
                    {
                        ModelState.AddModelError("RentalItems", $"Error! Movie titled '{item.NombreDispositivo}' is not available for being rented from {alquilerForCreate.FechaAlquilerDesde.ToShortDateString()} to {alquilerForCreate.FechaAlquilerHasta.ToShortDateString()}");
                    }
                    else
                    {
                        // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                        alquiler.ItemsAlquiler.Add(new ItemAlquiler(dispositivo.Id, alquiler.Id, dispositivo.PrecioParaAlquiler));
                        item.PrecioParaAlquiler = dispositivo.PrecioParaAlquiler;
                    }
                }

                decimal numDays = (decimal)(alquiler.FechaAlquilerDesde- alquiler.FechaAlquilerHasta).TotalDays;
                alquiler.PrecioTotal = alquiler.ItemsAlquiler.Sum(ia => ia.Precio * (double) numDays);

                //if there is any problem because of the available quantity of movies or because any movie does not exist
                if (ModelState.ErrorCount > 0)
                {
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                _context.Add(alquiler);

                try
                {
                    //we store in the database both rental and its rentalitems
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(DateTime.Now + ":" + ex.Message);
                    //ModelState.AddModelError("Rental", $"Error! There was an error while saving your rental, please, try again later");
                    return Conflict("Error" + ex.Message);

                }

                var rentalDetail = new AlquilerForCreateDTO(alquiler.Id, alquiler.PrecioTotal,
                    alquiler.FechaAlquiler, alquiler.DireccionEntrega, alquiler.ApplicationUser.NombreUsuario, alquiler.ApplicationUser.NombreUsuario,
                    alquiler.FechaAlquilerDesde, alquiler.FechaAlquilerHasta, alquiler.MetodoPago,

                    alquiler.ApplicationUser.NombreUsuario!, alquilerForCreate.ItemsAlquiler);

                return CreatedAtAction("GetRental", new { id = alquiler.Id }, rentalDetail);


            }
    }

    }
