using AppForSEII2526.API.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.ReseñaDTOs
{
    public class ReseñaDetailDTO
    {

        public ReseñaDetailDTO(string nombreUsuario, string pais, string titulo, DateTime fechaReseña, IList<ReseñaItemDTO> itemsReseña)
        {
            NombreUsuario = nombreUsuario;
            Pais = pais;
            Titulo = titulo;
            FechaReseña = fechaReseña;
            ItemsReseña = itemsReseña;

        }

        [StringLength(100, ErrorMessage = "El título no puede ser mayor de 100 caracteres")]
        [Display(Name = "Título de la reseña")]
        public string Titulo { get; set; }

        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres")]
        public string Pais { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string NombreUsuario { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la reseña")]
        public DateTime FechaReseña { get; set; }


        public IList<ReseñaItemDTO> ItemsReseña { get; set; }
    }
}