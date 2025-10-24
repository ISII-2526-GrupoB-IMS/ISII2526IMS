
namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraItemDTO
    {
        public CompraItemDTO(int dispositivoId, string marca, string modelo, string color, double precio, int cantidad ,string descripcion = "")
        {
            DispositivoId = dispositivoId;
            Marca = marca;
            Modelo = modelo;
            Color = color;
            Precio = precio;
            Cantidad = cantidad;
            Descripcion = descripcion;

        }

        public int DispositivoId { get; set; }


        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }

        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dTO &&
                   DispositivoId == dTO.DispositivoId &&
                   Marca == dTO.Marca &&
                   Modelo == dTO.Modelo &&
                   Color == dTO.Color &&
                   Precio == dTO.Precio &&
                   Cantidad == dTO.Cantidad &&
                   Descripcion == dTO.Descripcion;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DispositivoId, Marca, Modelo, Color, Precio, Cantidad, Descripcion);
        }
    }
}
