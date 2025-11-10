using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraForCreateDTO
    {

        public CompraForCreateDTO(string nombreUsuario, string apellidosUsuario, string direccionDeEnvio, TiposMetodoPago metodoDePago, int cantidad, IList<CompraItemDTO> itemsCompra)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            DireccionDeEnvio = direccionDeEnvio ?? throw new ArgumentNullException(nameof(direccionDeEnvio));
            MetodoDePago = metodoDePago;
            Cantidad = cantidad;
            ItemsCompra = itemsCompra ?? throw new ArgumentNullException(nameof(itemsCompra));
        }

        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string NombreUsuario { get; set; }

        [StringLength(70, ErrorMessage = "Los apellidos no pueden tener mas de 70 caracteres")]
        public string ApellidosUsuario { get; set; }

        [StringLength(100, ErrorMessage = "La direccion no puede tener mas de 100 caracteres")]
        public string DireccionDeEnvio { get; set; }

        public DateTime FechaCompra
        {
            get { return DateTime.Now; }
        }

        public TiposMetodoPago MetodoDePago { get; set; }

        public int Cantidad { get; set; }

        public IList<CompraItemDTO> ItemsCompra { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraForCreateDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   DireccionDeEnvio == dTO.DireccionDeEnvio &&
                   FechaCompra == dTO.FechaCompra &&
                   MetodoDePago == dTO.MetodoDePago &&
                   Cantidad == dTO.Cantidad &&
                   EqualityComparer<IList<CompraItemDTO>>.Default.Equals(ItemsCompra, dTO.ItemsCompra);
        }
    }
}