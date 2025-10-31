using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO( string nombreUsuario, string apellidosUsuario, string direccionDeEntrega, Compra.MetodoPago metodoDePago, int cantidad, IList<CompraItemDTO> itemsCompra) 
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(NombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(ApellidosUsuario));
            DireccionDeEntrega = direccionDeEntrega ?? throw new ArgumentNullException(nameof(DireccionDeEntrega));
            MetodoDePago = metodoDePago;
            Cantidad = cantidad;
            ItemsCompra = itemsCompra ?? throw new ArgumentNullException(nameof(ItemsCompra));
        }

        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string NombreUsuario { get; set; }

        [StringLength(70, ErrorMessage = "Los apellidos no pueden tener mas de 70 caracteres")]
        public string ApellidosUsuario { get; set; }

        [StringLength(100, ErrorMessage = "La direccion no puede tener mas de 100 caracteres")]
        public string DireccionDeEntrega { get; set; }

        public Compra.MetodoPago MetodoDePago { get; set; }

        public int Cantidad { get; set; }
        public IList<CompraItemDTO> ItemsCompra { get; set; }

    }
}
