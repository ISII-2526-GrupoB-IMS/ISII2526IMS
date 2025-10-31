namespace AppForSEII2526.API.Models
{
    public class Compra
    {

        [Key]
        public int Id { get; set; }



        //MÉTODO DE PAGO
        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "Elija un método de pago")]
        public MetodoPago MetodoDePago { get; set; }
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

        private Compra()
        {
            ItemsCompra = new List<ItemCompra>();
        }

        public Compra(MetodoPago metodoDePago, DateTime fechaCompra, IList<ItemCompra> itemsCompra, ApplicationUser applicationUser)
        {
            MetodoDePago = metodoDePago;
            FechaCompra = fechaCompra;
            ItemsCompra = itemsCompra;
            ApplicationUser = applicationUser;
        }

        public enum MetodoPago
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }   






    }
}

