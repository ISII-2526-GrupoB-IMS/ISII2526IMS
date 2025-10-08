

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

        public Alquiler Alquiler;
        public AlquilarDispositivo()
        {
        }

        public AlquilarDispositivo(int idDispositivo, int idAlquiler, double precio, int cantidad, Alquiler alquiler)
        {
            IdDispositivo = idDispositivo;
            IdAlquiler = idAlquiler;
            Precio = precio;
            Cantidad = cantidad;
            Alquiler = alquiler;
        }

        public override bool Equals(object? obj)
        {
            return obj is AlquilarDispositivo dispositivo &&
                   IdDispositivo == dispositivo.IdDispositivo &&
                   IdAlquiler == dispositivo.IdAlquiler &&
                   Precio == dispositivo.Precio &&
                   Cantidad == dispositivo.Cantidad &&
                   EqualityComparer<Alquiler>.Default.Equals(Alquiler, dispositivo.Alquiler);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdDispositivo, IdAlquiler, Precio, Cantidad, Alquiler);
        }
    }
}
