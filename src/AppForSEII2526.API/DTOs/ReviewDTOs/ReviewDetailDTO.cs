using AppForSEII2526.API.DTOs.CompraDTOs;
using AppForSEII2526.API.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {

        public ReviewDetailDTO(string nombreUsuario, string pais, string titulo, DateTime fechaReview, IList<ReviewItemDTO> itemsReview)
        {
            NombreUsuario = nombreUsuario;
            Pais = pais;
            Titulo = titulo;
            FechaReview = fechaReview;
            ItemsReview = itemsReview;

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
        public DateTime FechaReview { get; set; }


        public IList<ReviewItemDTO> ItemsReview { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not ReviewDetailDTO dTO)
                return false;

            
            bool fechas= Math.Abs((FechaReview - dTO.FechaReview).TotalSeconds) < 1;

            return
                NombreUsuario == dTO.NombreUsuario &&
                Pais == dTO.Pais &&
                Titulo == dTO.Titulo &&
                fechas &&
                ItemsReview.SequenceEqual(dTO.ItemsReview);
        }
    }
}