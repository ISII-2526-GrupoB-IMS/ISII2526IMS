

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IdDispositivo), nameof(IdReview))]
    public class ItemReview
    {
        public ItemReview() { }

        public ItemReview(string comentario, int puntuacion, Dispositivo dispositivo, Review review)
        {
            Comentario = comentario;
            Puntuacion = puntuacion;
            Dispositivo = dispositivo;
            Review = review; 
            IdReview = review.Id;
        }



        // COMENTARIO
        [Required(ErrorMessage = "Debe proporcionar un comentario para el dispositivo")]
        [StringLength(300, ErrorMessage = "El comentario no puede ser mayor de 300 caracteres")]
        [Display(Name = "Comentario del dispositivo")]
        public string Comentario { get; set; }

        // PUNTUACIÓN
        [Required(ErrorMessage = "Debe proporcionar una puntuación.")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        [Display(Name = "Puntuación")]
        public int Puntuacion { get; set; }

        // DISPOSITIVO Reviewdo
        [Required]
        public Dispositivo Dispositivo { get; set; }

        [Required]
        [ForeignKey("Dispositivo")]
        public int IdDispositivo { get; set; }

        // Review a la que pertenece este ítem
        [Required]
        public Review Review { get; set; }

        [Required]
        [ForeignKey("Review")]
        public int IdReview { get; set; }




    }
}

