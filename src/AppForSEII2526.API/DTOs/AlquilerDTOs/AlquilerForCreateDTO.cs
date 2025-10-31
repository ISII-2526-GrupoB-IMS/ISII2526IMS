using static Alquiler;

namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class AlquilerForCreateDTO
    {
        private int id;
        private double precioTotal;
        private DateTime fechaAlquiler;
        private string nombreUsuario1;
        private string nombreUsuario2;
        private string v;

        public AlquilerForCreateDTO(string nombreUsuario, string apellidosUsuario, string direccionEntrega, TiposMetodoPago metodoPago, 
            DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, IList<ItemAlquilerDTO> itemsAlquiler)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            DireccionEntrega = direccionEntrega ?? throw new ArgumentNullException(nameof(direccionEntrega));
            MetodoPago = metodoPago ;
            FechaAlquilerDesde = fechaAlquilerDesde;
            FechaAlquilerHasta = fechaAlquilerHasta;
            ItemsAlquiler = itemsAlquiler ?? throw new ArgumentNullException(nameof(itemsAlquiler));
        }

        public AlquilerForCreateDTO()
        {
            ItemsAlquiler = new List<ItemAlquilerDTO>();
        }

        public AlquilerForCreateDTO(int id, double precioTotal, DateTime fechaAlquiler, string direccionEntrega, string nombreUsuario1, string nombreUsuario2, DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, TiposMetodoPago metodoPago, string v, IList<ItemAlquilerDTO> itemsAlquiler)
        {
            this.id = id;
            this.precioTotal = precioTotal;
            this.fechaAlquiler = fechaAlquiler;
            DireccionEntrega = direccionEntrega;
            this.nombreUsuario1 = nombreUsuario1;
            this.nombreUsuario2 = nombreUsuario2;
            FechaAlquilerDesde = fechaAlquilerDesde;
            FechaAlquilerHasta = fechaAlquilerHasta;
            this.MetodoPago = metodoPago;
            this.v = v;
            ItemsAlquiler = itemsAlquiler;
        }

        public DateTime FechaAlquilerDesde { get; set; }

        public DateTime FechaAlquilerHasta { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Dirección de entrega")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "La dirección de usuario debe tener al menos 10 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Porvafor, introduzca dirección de entrega")]
        public string DireccionEntrega { get; set; }

        [EmailAddress]
        [Required]
        public string NombreUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Porvafor, Inserte nombre y apellidos")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Nombre y apellidos deben tener al menos 10 caracteres")]
        public string ApellidosUsuario { get; set; }

        public IList<ItemAlquilerDTO> ItemsAlquiler { get; set; }
        [Required]
        public TiposMetodoPago MetodoPago { get; set; }

        private int NumeroDeDias
        {
            get
            {
                return (FechaAlquilerHasta - FechaAlquilerDesde).Days;
            }
        }

        [Display(Name = "Precio total")]
        [JsonPropertyName("TotalPrice")]
        public double PrecioTotal
        {
            get
            {
                return ItemsAlquiler.Sum(ri => ri.PrecioParaAlquiler * NumeroDeDias);
            }
        }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is AlquilerForCreateDTO dTO &&
                   CompareDate(FechaAlquilerDesde, dTO.FechaAlquilerDesde) &&
                   CompareDate(FechaAlquilerHasta, dTO.FechaAlquilerHasta) &&
                   DireccionEntrega == dTO.DireccionEntrega &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   ItemsAlquiler.SequenceEqual(dTO.ItemsAlquiler) &&
                   MetodoPago == dTO.MetodoPago &&
                   PrecioTotal == dTO.PrecioTotal;
        }
    }
}
