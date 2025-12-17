using AppForSEII2526.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO
    {
        public ReviewForCreateDTO(string titulo, string pais, string nombreUsuario, IList<ReviewItemDTO> itemsReview)
        {  
            Titulo = titulo ?? throw new ArgumentNullException(nameof(titulo));
            Pais = pais ?? throw new ArgumentNullException(nameof(pais));
            NombreUsuario = nombreUsuario ?? throw new ArgumentNullException(nameof(nombreUsuario));
            ItemsReview = itemsReview ?? throw new ArgumentNullException(nameof(itemsReview));
        }
        [Required(ErrorMessage = "El título es un campo obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede ser mayor de 100 caracteres")]
        [Display(Name = "Título de la Review")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El país es un campo obligatorio")]
        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres")]
        public string Pais { get; set; }

        [StringLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
        public string NombreUsuario { get; set; }

        public DateTime FechaReview { get; set; } = DateTime.Now;

        [Required]
        public IList<ReviewItemDTO> ItemsReview { get; set; }
    }
   
    }