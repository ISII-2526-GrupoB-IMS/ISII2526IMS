using AppForSEII2526.API.Models;
using static Alquiler;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerDetailDTO 
    {


        public AlquilerDetailDTO(int id, DateTime fechaAlquiler, string nombreUsuario, string apellidosUsuario,
            string direccionEntrega, TiposMetodoPago metodoPago, 
            DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, IList<ItemAlquilerDTO> itemsAlquiler)
        {
            Id = id;
            FechaAlquiler = fechaAlquiler;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DireccionEntrega = direccionEntrega;
            MetodoPago = metodoPago;
            FechaAlquilerDesde = fechaAlquilerDesde;
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
                   FechaAlquiler.Day == dTO.FechaAlquiler.Day &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   DireccionEntrega == dTO.DireccionEntrega &&
                   MetodoPago == dTO.MetodoPago &&
                   ItemsAlquiler.SequenceEqual(dTO.ItemsAlquiler) &&
                   //La fecha se ajusta al mismo día para evitar errores por diferencias en horas/minutos/segundos
                   FechaAlquilerDesde.Day == dTO.FechaAlquilerDesde.Day &&
                   FechaAlquilerHasta.Day == dTO.FechaAlquilerHasta.Day;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, FechaAlquiler);
        }
    }
}