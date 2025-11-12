using NuGet.DependencyResolver;



namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(DispositivoId), nameof(CompraId))]
    public class ItemCompra
    {
        private ItemCompra() { }

        public ItemCompra(int dispositivoId, double precio, int cantidad)
        {
            DispositivoId = dispositivoId;
            Precio = precio;
            Cantidad = cantidad;
        }

        // DESCRIPCIÓN
        [Required]
        [StringLength(150, ErrorMessage = "La descripción no puede ser superior a 150 caracteres.")]
        public string? Descripcion { get; set; }

        // FK: DISPOSITIVO
        [Required]
        public int DispositivoId { get; set; }
        public Dispositivo Dispositivo { get; set; } = null!;

        // FK: COMPRA
        [Required]
        public int CompraId { get; set; }
        public Compra Compra { get; set; } = null!;

        // PRECIO 
        [Required]
        
        [Range(0.5, double.MaxValue, ErrorMessage = "El precio mínimo es de 0,5.")]
        [Precision(10, 2)]
        public double Precio { get; set; }

        // CANTIDAD
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debes proporcionar una cantidad válida mayor que 1.")]
        public int Cantidad { get; set; }
    }
}
