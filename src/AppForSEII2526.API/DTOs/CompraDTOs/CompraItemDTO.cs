
namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraItemDTO
    {
        public CompraItemDTO(string marca, string modelo, string color, double precio, int cantidad ,string descripcion = "")
        {
            
            Marca = marca;
            Modelo = modelo;
            Color = color;
            Precio = precio;
            Cantidad = cantidad;
            Descripcion = descripcion;

        }

        
        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }
        public string Modelo { get; set; }

        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres ni menor que 1 ", MinimumLength = 1)]
        public string Color { get; set; }

        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   
                   Marca == dTO.Marca &&
                   Modelo == dTO.Modelo &&
                   Color == dTO.Color &&
                   Precio == dTO.Precio &&
                   Cantidad == dTO.Cantidad &&
                   Descripcion == dTO.Descripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Marca, Modelo, Color, Precio, Cantidad, Descripcion);
        }
    }
}
