
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IdDispositivo), nameof(IdReseña))]
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

        // DISPOSITIVO reseñado
        [Required]
        public Dispositivo Dispositivo { get; set; }

        [Required]
        [ForeignKey("Dispositivo")]
        public int IdDispositivo { get; set; }

        // RESEÑA a la que pertenece este ítem
        [Required]
        public Reseña Reseña { get; set; }

        [Required]
        [ForeignKey("Reseña")]
        public int IdReseña { get; set; }

     

       
    }
}

