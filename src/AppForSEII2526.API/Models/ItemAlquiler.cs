

using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IdDispositivo), nameof(IdAlquiler))]
    public class ItemAlquiler
    {

        [Required]
        int IdDispositivo { get; set; }
        [ForeignKey(nameof(IdDispositivo))]
        public Dispositivo Dispositivo { get; set; }

        [Required]
        int IdAlquiler { get; set; }

        [ForeignKey(nameof(IdAlquiler))]
        public Alquiler Alquiler { get; set; }


        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, double.MaxValue, ErrorMessage = "Precio mínimo es O,5")]
        [Display(Name = "Precio")]
        public double Precio { get; set; }

        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidd minima es 1")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }


        public ItemAlquiler()
        {
        }

        public ItemAlquiler(int idDispositivo, Dispositivo dispositivo, int idAlquiler, Alquiler alquiler, double precio, int cantidad)
        {
            IdDispositivo = idDispositivo;
            Dispositivo = dispositivo;
            IdAlquiler = idAlquiler;
            Alquiler = alquiler;
            Precio = precio;
            Cantidad = cantidad;
        }

        public ItemAlquiler(int idDispositivo, int idAlquiler, double precio)
        {
            IdDispositivo = idDispositivo;
            IdAlquiler = idAlquiler;
            Precio = precio;
        }

        public override bool Equals(object? obj)
        {
            return obj is ItemAlquiler dispositivo &&
                   IdDispositivo == dispositivo.IdDispositivo &&
                   EqualityComparer<Dispositivo>.Default.Equals(Dispositivo, dispositivo.Dispositivo) &&
                   IdAlquiler == dispositivo.IdAlquiler &&
                   EqualityComparer<Alquiler>.Default.Equals(Alquiler, dispositivo.Alquiler) &&
                   Precio == dispositivo.Precio &&
                   Cantidad == dispositivo.Cantidad;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdDispositivo, Dispositivo, IdAlquiler, Alquiler, Precio, Cantidad);
        }
    }
}
