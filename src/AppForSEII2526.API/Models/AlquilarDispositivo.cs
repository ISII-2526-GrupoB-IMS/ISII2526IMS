
namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IdDispositivo), nameof(IdAlquiler))]
    public class AlquilarDispositivo
    {

        [Required]
        int IdDispositivo { get; set; }
        [Required]
        int IdAlquiler { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, double.MaxValue, ErrorMessage = "Precio mínimo es O,5")]
        [Display(Name = "Precio")]
        double Precio { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidd minima es 1")]
        [Display(Name = "Cantidad")]
        int Cantidad { get; set; }

        public IList<Alquiler> Alquileres;
        public AlquilarDispositivo()
        {
        }

        public AlquilarDispositivo(int idDispositivo, double precio, int cantidad, int idAlquiler)
        {
            IdDispositivo = idDispositivo;
            Precio = precio;
            Cantidad = cantidad;
            IdAlquiler = idAlquiler;
        }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarDispositivo dispositivo &&
                   IdDispositivo == dispositivo.IdDispositivo &&
                   Precio == dispositivo.Precio &&
                   Cantidad == dispositivo.Cantidad &&
                   IdAlquiler == dispositivo.IdAlquiler;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdDispositivo, Precio, Cantidad, IdAlquiler);
        }
    }
}
