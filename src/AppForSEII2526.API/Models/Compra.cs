namespace AppForSEII2526.API.Models
{
    public class Compra
    {

        [Key]
        public int Id { get; set; }



        //MÉTODO DE PAGO
        [Display(Name = "Método de pago")]
        [Required(ErrorMessage = "Elija un método de pago")]
        public MetodosDePago MetodoDePago { get; set; }

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
        public double PrecioTotal { get; set; }

        //CANTIDAD TOTAL
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidd minima es 1")]
        [Display(Name = "Cantidad Total")]
        public int CantidadTotal { get; set; }




        public IList<ItemCompra> ItemsCompra { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public enum MetodosDePago
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }



    }
}

