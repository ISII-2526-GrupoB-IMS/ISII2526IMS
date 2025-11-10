using AppForSEII2526.API.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraDetailDTO
    {

        public CompraDetailDTO(string nombreUsuario, string apellidosUsuario, string direccionDeEntrega, DateTime fechaCompra, double precioTotal, int cantidadTotal, IList<CompraItemDTO> itemsCompra)
        {
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DireccionDeEntrega = direccionDeEntrega;
            FechaCompra = fechaCompra;
            PrecioTotal = precioTotal;
            CantidadTotal = cantidadTotal;
            ItemsCompra = itemsCompra;

        }

        //NOMBRE USUARIO

        [StringLength(40, ErrorMessage = "El nombre del usuario no puede ser superior a 40 carecteres")]
        public string NombreUsuario { get; set; }

        //APELLIDOS 
        [StringLength(40, ErrorMessage = "Los apellidos del usuario no pueden ser superiores a 40 carecteres")]
        public string ApellidosUsuario { get; set; }

        //DIRECCION
        [Required]
        [StringLength(100, ErrorMessage = "La direccion no puede ser superior a 100 carecteres")]
        public string DireccionDeEntrega { get; set; }

        //FECHA DE COMPRA
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de compra")]
        public DateTime FechaCompra { get; set; }

        //PRECIO TOTAL
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, double.MaxValue, ErrorMessage = "Precio mínimo es O,5")]
        [Display(Name = "Precio Total")]
        [Precision(10, 2)]
        public double PrecioTotal { get; set; }

        //CANTIDAD TOTAL
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidd minima es 1")]
        [Display(Name = "Cantidad Total")]
        public int CantidadTotal { get; set; }

        public IList<CompraItemDTO> ItemsCompra { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraDetailDTO dTO &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   DireccionDeEntrega == dTO.DireccionDeEntrega &&
                   FechaCompra == dTO.FechaCompra &&
                   PrecioTotal == dTO.PrecioTotal &&
                   CantidadTotal == dTO.CantidadTotal &&
                   ItemsCompra.SequenceEqual(dTO.ItemsCompra);
        }
    }
}
