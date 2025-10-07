namespace AppForSEII2526.API.Models
{
    public class Compra
    {
        public Compra()
        {
        }

        public Compra(int id, string nombreUsuario, string apellidosUsuario, string direcciónDeEnvio, MetodosDePago metodoDePago, DateTime fechaCompra, IList<ItemCompra> itemsCompra)
        {
            Id = id;
            NombreUsuario = nombreUsuario;
            ApellidosUsuario = apellidosUsuario;
            DireccionDeEnvio = direcciónDeEnvio;
            MetodoDePago = metodoDePago;
            FechaCompra = fechaCompra;
            PrecioTotal = itemsCompra.Sum(ic => ic.Precio * ic.Cantidad);
            CantidadTotal = itemsCompra.Sum(ic => 1 * ic.Cantidad);

        }

        [Key]
        public int Id { get; set; }

        //NOMBRE USUARIO
        [Required]
        [StringLength(40, ErrorMessage = "El nombre del usuario no puede ser superior a 40 carecteres")]
        public string NombreUsuario { get; set; }

        //APELLIDOS USUARIO
        [StringLength(40, ErrorMessage = "Los apellidos del usuario no pueden ser superiores a 40 carecteres")]
        public string? ApellidosUsuario { get; set; }

        //DIRECCION
        [Required]
        [StringLength(100, ErrorMessage = "La direccion no puede ser superior a 100 carecteres")]
        public string DireccionDeEnvio { get; set; }

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
        public enum MetodosDePago
        {
            TarjetaCredito,
            PayPal,
            Efectivo
        }
        public override bool Equals(object? obj)
        {
            return obj is Compra compra &&
                   Id == compra.Id &&
                   NombreUsuario == compra.NombreUsuario &&
                   ApellidosUsuario == compra.ApellidosUsuario &&
                   DireccionDeEnvio == compra.DireccionDeEnvio &&
                   MetodoDePago == compra.MetodoDePago &&
                   FechaCompra == compra.FechaCompra &&
                   PrecioTotal == compra.PrecioTotal &&
                   CantidadTotal == compra.CantidadTotal &&
                   EqualityComparer<IList<ItemCompra>>.Default.Equals(ItemsCompra, compra.ItemsCompra);

        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(NombreUsuario);
            hash.Add(ApellidosUsuario);
            hash.Add(DireccionDeEnvio);
            hash.Add(MetodoDePago);
            hash.Add(FechaCompra);
            hash.Add(PrecioTotal);
            hash.Add(CantidadTotal);
            hash.Add(NombreUsuario);
            hash.Add(ItemsCompra);
            return hash.ToHashCode();
        }
    }
}

