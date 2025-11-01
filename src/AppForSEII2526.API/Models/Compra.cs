
namespace AppForSEII2526.API.Models
{
    public class Compra
    {

        [Key]
        public int Id { get; set; }
       
        public DateTime FechaCompra { get; set; }

        //PRECIO TOTAL
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, double.MaxValue, ErrorMessage = "Precio mínimo es O,5")]
        [Display(Name = "Precio Total")]
        public double PrecioTotal { get; set; }

        //CANTIDAD TOTAL
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidd minima es 1")]
        [Display(Name = "Cantidad Total")]
        public int CantidadTotal { get; set; }




        public IList<ItemCompra> ItemsCompra { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Column("MetodoDePago")]
        public TiposMetodoPago TiposMetodoPago { get; set; }
        private Compra()
        {
           
        }

        public Compra(TiposMetodoPago tiposMetodoPago, DateTime fechaCompra, IList<ItemCompra> itemsCompra, ApplicationUser applicationUser)
        {
            TiposMetodoPago = tiposMetodoPago;
            FechaCompra = fechaCompra;
            ItemsCompra = itemsCompra;
            ApplicationUser = applicationUser;
        }

        public override bool Equals(object? obj)
        {
            return obj is Compra compra &&
                   Id == compra.Id &&
                   FechaCompra == compra.FechaCompra &&
                   PrecioTotal == compra.PrecioTotal &&
                   CantidadTotal == compra.CantidadTotal &&
                   EqualityComparer<IList<ItemCompra>>.Default.Equals(ItemsCompra, compra.ItemsCompra) &&
                   EqualityComparer<ApplicationUser>.Default.Equals(ApplicationUser, compra.ApplicationUser) &&
                   TiposMetodoPago == compra.TiposMetodoPago;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FechaCompra, PrecioTotal, CantidadTotal, ItemsCompra, ApplicationUser, TiposMetodoPago);
        }
    }
}

