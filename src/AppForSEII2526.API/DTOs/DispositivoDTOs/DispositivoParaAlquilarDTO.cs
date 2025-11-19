

namespace AppForSEII2526.API.DTOs.DispositivoDTOs
{
    public class DispositivoParaAlquilarDTO
    {
        public DispositivoParaAlquilarDTO(int id, Modelo modelo, string nombreDispositivo, string marca, double año, string color, double precioParaAlquiler)
        {
            Id = id;
            Modelo = modelo;
            NombreDispositivo = nombreDispositivo;
            Marca = marca;
            Año = año;
            Color = color;
            PrecioParaAlquiler = precioParaAlquiler;
        }

        public int Id { get; set; }

        //MODELO
        [Required]

        public Modelo Modelo { get; set; }

        //MARCA

        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }


        //COLOR

        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres")]
        public string Color { get; set; }

        //NOMBRE DISPOSITIVO

        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }
        //PRECIO Alquiler

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
        [Display(Name = "Precio para Alquilar")]
        [Precision(10, 2)]
        public double PrecioParaAlquiler { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is DispositivoParaAlquilarDTO dTO &&
                   Id == dTO.Id &&
                   EqualityComparer<Modelo>.Default.Equals(Modelo, dTO.Modelo) &&
                   Marca == dTO.Marca &&
                   Color == dTO.Color &&
                   NombreDispositivo == dTO.NombreDispositivo &&
                   PrecioParaAlquiler == dTO.PrecioParaAlquiler &&
                   Año == dTO.Año;
        }
    }
}