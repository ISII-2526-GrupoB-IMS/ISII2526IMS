using static Alquiler;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO 
    {


        public AlquilerDetailDTO(int id, DateTime fechaAlquiler, string nombreUsuario, string apellidosUsuario,
            string direccionEntrega, TiposMetodoPago metodoPago, 
            DateTime fechaAlquilerdesde, DateTime fechaAlquilerHasta, IList<ItemAlquilerDTO> itemsAlquiler)
        {
            Id = id;
            FechaAlquiler = fechaAlquiler;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DireccionEntrega = direccionEntrega;
            MetodoPago = metodoPago;
            FechaAlquilerDesde = fechaAlquilerdesde;
            FechaAlquilerHasta = fechaAlquilerHasta;
            ItemsAlquiler = itemsAlquiler;
        }

        public int Id { get; set; }

        public DateTime FechaAlquiler { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must have at least 3 characters")]
        public string NombreUsuario { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Surname must have at least 3 characters")]
        public string ApellidosUsuario{ get; set; }


        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        public string DireccionEntrega { get; set; }

        [Required]
        public TiposMetodoPago MetodoPago { get; set; }

        public IList<ItemAlquilerDTO> ItemsAlquiler { get; set; }

        public DateTime FechaAlquilerDesde { get; set; }

        public DateTime FechaAlquilerHasta { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerDetailDTO dTO &&
                   Id == dTO.Id &&
                   FechaAlquiler == dTO.FechaAlquiler &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   DireccionEntrega == dTO.DireccionEntrega &&
                   MetodoPago == dTO.MetodoPago &&
                   EqualityComparer<IList<ItemAlquilerDTO>>.Default.Equals(ItemsAlquiler, dTO.ItemsAlquiler) &&
                   FechaAlquilerDesde == dTO.FechaAlquilerDesde &&
                   FechaAlquilerHasta == dTO.FechaAlquilerHasta;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, FechaAlquiler);
        }
    }
}