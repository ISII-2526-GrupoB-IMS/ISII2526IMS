using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReseñaDTOs
{
    public class ReseñaForCreateDTO
    {
        public ReseñaForCreateDTO(string titulo, string pais, string nombreUsuario, IList<ReseñaItemDTO> itemsReseña)
        {  
            Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
            Pais = pais ?? throw new ArgumentNullException(nameof(pais));
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ItemsReseña = itemsReseña ?? throw new ArgumentNullException(nameof(itemsReseña));
        }


        [StringLength(100, ErrorMessage = "El título no puede ser mayor de 100 caracteres")]
        [Display(Name = "Título de la reseña")]
        public string Titulo { get; set; }

        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres")]
        public string Pais { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string NombreUsuario { get; set; }

        public DateTime FechaReseña
        {
            get { return DateTime.Now; }
        }

        
        public IList<ReseñaItemDTO> ItemsReseña { get; set; }
    }
}