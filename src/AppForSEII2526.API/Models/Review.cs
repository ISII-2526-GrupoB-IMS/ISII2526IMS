
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AppForSEII2526.API.Models
{
    public class Review
    {
        public Review() { }

        public Review(int id, string titulo, string pais, DateTime fechaReview, IList<ItemReview> itemsReview, ApplicationUser applicationUser)
        {
            Id = id;
            Titulo = titulo;
            Pais = pais;
           
            FechaReview = fechaReview;
            ItemsReview = itemsReview ?? new List<ItemReview>();

            // ✅ Calcular CalificaciónGeneral solo si hay elementos
            CalificaciónGeneral = (ItemsReview != null && ItemsReview.Any())
                ? ItemsReview.Average(item => item.Puntuacion)
                : 0;
            ItemsReview = itemsReview;
            ApplicationUser = applicationUser;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El título no puede ser mayor de 100 caracteres")]
        [Display(Name = "Título de la Review")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres")]
        public string Pais { get; set; }

        

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la Review")]
        public DateTime FechaReview { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación general debe estar entre 1 y 5")]
        [Display(Name = "Calificación general")]
        public double CalificaciónGeneral { get; set; }

        public IList<ItemReview> ItemsReview { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Review Review &&
                   Id == Review.Id &&
                   Titulo == Review.Titulo &&
                   Pais == Review.Pais &&
                   FechaReview == Review.FechaReview &&
                   CalificaciónGeneral == Review.CalificaciónGeneral;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titulo);
            hash.Add(Pais);
            hash.Add(FechaReview);
            hash.Add(CalificaciónGeneral);
            return hash.ToHashCode();
        }
    }
}

