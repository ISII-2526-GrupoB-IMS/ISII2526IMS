namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewItemDTO
    {
        public ReviewItemDTO(string nombreDispositivo, string modelo, double año, int puntuacion, string comentario = "")
        {


            NombreDispositivo = nombreDispositivo;
            Modelo = modelo;
            Año = año;
            Puntuacion = puntuacion;
            Comentario = comentario;

        }


        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }
        public string Modelo { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        [Display(Name = "Puntuación")]
        public int Puntuacion { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "El comentario no puede ser mayor de 300 caracteres")]
        [Display(Name = "Comentario del dispositivo")]
        public string Comentario { get; set; }



        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&

                   NombreDispositivo == dTO.NombreDispositivo &&
                   Modelo == dTO.Modelo &&
                   Año == dTO.Año &&
                   Puntuacion == dTO.Puntuacion &&
                   Comentario == dTO.Comentario;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreDispositivo, Modelo, Año, Puntuacion, Comentario);
        }
    }
}
