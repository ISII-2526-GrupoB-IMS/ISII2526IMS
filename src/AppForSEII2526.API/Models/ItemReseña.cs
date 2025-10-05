using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Models
{
    public class ItemReseña
    {
        public ItemReseña() { }

        public ItemReseña(string comentario, int puntuacion, Dispositivo dispositivo, Reseña reseña)
        {
            Comentario = comentario;
            Puntuacion = puntuacion;
            Dispositivo = dispositivo;
            IdDispositivo = dispositivo.Id;
            Reseña = reseña;
            IdReseña = reseña.Id;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "El comentario no puede superar los 300 caracteres")]
        public string Comentario { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        public int Puntuacion { get; set; }

        [Required]
        public Dispositivo Dispositivo { get; set; }

        [Required]
        [ForeignKey("Dispositivo")]
        public int IdDispositivo { get; set; }

        [Required]
        public Reseña Reseña { get; set; }

        [Required]
        [ForeignKey("Reseña")]
        public int IdReseña { get; set; }

    }
