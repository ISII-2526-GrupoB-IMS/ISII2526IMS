
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AppForSEII2526.API.Models
{
    public class Reseña
    {
        public Reseña() { }

        public Reseña(int id, string titulo, string pais, DateTime fechaReseña, IList<ItemReseña> itemsReseña, ApplicationUser applicationUser)
        {
            Id = id;
            Titulo = titulo;
            Pais = pais;
           
            FechaReseña = fechaReseña;
            ItemsReseña = itemsReseña ?? new List<ItemReseña>();

            // ✅ Calcular CalificaciónGeneral solo si hay elementos
            CalificaciónGeneral = (ItemsReseña != null && ItemsReseña.Any())
                ? ItemsReseña.Average(item => item.Puntuacion)
                : 0;
            ItemsReseña = itemsReseña;
            ApplicationUser = applicationUser;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El título no puede ser mayor de 100 caracteres")]
        [Display(Name = "Título de la reseña")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El país no puede superar los 50 caracteres")]
        public string Pais { get; set; }

        

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de la reseña")]
        public DateTime FechaReseña { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación general debe estar entre 1 y 5")]
        [Display(Name = "Calificación general")]
        public double CalificaciónGeneral { get; set; }

        public IList<ItemReseña> ItemsReseña { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Reseña reseña &&
                   Id == reseña.Id &&
                   Titulo == reseña.Titulo &&
                   Pais == reseña.Pais &&
                   FechaReseña == reseña.FechaReseña &&
                   CalificaciónGeneral == reseña.CalificaciónGeneral;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Titulo);
            hash.Add(Pais);
            hash.Add(FechaReseña);
            hash.Add(CalificaciónGeneral);
            return hash.ToHashCode();
        }
    }
}

