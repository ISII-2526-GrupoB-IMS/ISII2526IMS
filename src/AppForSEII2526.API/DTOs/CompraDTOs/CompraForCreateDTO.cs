namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraForCreateDTO
    {
        public CompraForCreateDTO(string nombreUsuario, string apellidosUsuario, string direccionEntrega, DateTime fechaCompra, IList<CompraItemDTO> compraItems)
        {
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ApellidosUsuario = apellidosUsuario ?? throw new ArgumentNullException(nameof(apellidosUsuario));
            DireccionEntrega = direccionEntrega ?? throw new ArgumentNullException(nameof(direccionEntrega));
            FechaCompra = fechaCompra;
            CompraItems = compraItems ?? throw new ArgumentNullException(nameof(compraItems));
        }

        public CompraForCreateDTO()
        {
            CompraItems = new List<CompraItemDTO>();
        }

       

        public DateTime FechaCompra { get; set; }

       


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Dirección de Entrega")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "La dirección de entrega tiene que tener minimo 10 caracteres")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pon direccion de entrega")]
        public string DireccionEntrega { get; set; }

        [EmailAddress]
        [Required]
        public string NombreUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Pon nombre y apellidos")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Nombre y apellidos deben tener al menos 10 caracteres")]
        public string ApellidosUsuario { get; set; }

        public IList<CompraItemDTO> CompraItems { get; set; }
        [Required]
        

        

        [Display(Name = "Precio Total")]
        [JsonPropertyName("PrecioTotal")]
        public double PrecioTotal
        {
            get
            {
                return CompraItems.Sum(ri => ri.Precio * ri.Cantidad);
            }
        }

        [Display(Name = "Cantidad Total")]
        [JsonPropertyName("CantidadTotal")]
        public double CantidadTotal
        {
            get
            {
                return CompraItems.Sum(ri => 1 * ri.Cantidad);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is CompraForCreateDTO dTO &&
                   FechaCompra == dTO.FechaCompra &&
                   DireccionEntrega == dTO.DireccionEntrega &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   EqualityComparer<IList<CompraItemDTO>>.Default.Equals(CompraItems, dTO.CompraItems) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   CantidadTotal == dTO.CantidadTotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FechaCompra, DireccionEntrega, NombreUsuario, ApellidosUsuario, CompraItems, PrecioTotal, CantidadTotal);
        }
    }
}
