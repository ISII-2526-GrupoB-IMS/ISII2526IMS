using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web
{
    // === DTOs Similares a los usados en tu API, necesarios para el State Container ===

    // Contiene la información obligatoria de la reseña general y los ítems.
    public class ReviewForCreateDTO
    {
        public string? Titulo { get; set; } // Obligatorio
        public string? Pais { get; set; } // Obligatorio
        public string? NombreCliente { get; set; } // Opcional
        public DateTimeOffset FechaDeReview { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<ItemReviewDTO> ItemsReview { get; set; } = new List<ItemReviewDTO>();
    }

    // Contiene los datos del dispositivo y los campos de entrada del usuario.
    public class ItemReviewDTO
    {
        public int IdDispositivo { get; set; }
        // Metadata para mostrar en el formulario de reseña
        public string? NombreDispositivo { get; set; }
        public string? Modelo { get; set; }
        public double Año { get; set; }
        // Campos de entrada obligatorios del usuario (Paso 5)
        public int Puntuacion { get; set; }
        public string? Comentario { get; set; }
    }
    // ==============================================================================


    public class ReviewStateContainer
    {
        // Almacena el objeto de reseña completo. Este estado se conserva entre componentes.
        public ReviewForCreateDTO Review { get; private set; }

        public ReviewStateContainer()
        {
            InitializeReview();
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // Inicializa el objeto ReviewForCreateDTO a su estado de inicio (llamado al inicio o tras un envío exitoso)
        public void InitializeReview()
        {
            Review = new ReviewForCreateDTO
            {
                Titulo = string.Empty,
                Pais = string.Empty,
                NombreCliente = string.Empty,
                ItemsReview = new List<ItemReviewDTO>()
            };
            NotifyStateChanged();
        }

        // Añade un dispositivo a la lista de ítems para reseñar si no existe.
        public void AddDispositivoForReview(DispositivoParaReviewDTO dispositivo)
        {
            // Buscamos si el dispositivo ya fue añadido por su ID para evitar duplicados
            if (!Review.ItemsReview.Any(ri => ri.IdDispositivo == dispositivo.Id))
            {
                // Creamos el ItemReviewDTO con la metadata necesaria
                Review.ItemsReview.Add(new ItemReviewDTO()
                {
                    IdDispositivo = dispositivo.Id,
                    NombreDispositivo = dispositivo.NombreDispositivo,
                    Modelo = dispositivo.Modelo?.NombreModelo ?? "N/A",
                    Año = dispositivo.Año,
                    Puntuacion = 0, // Valor inicial para el formulario (de 0 a 5)
                    Comentario = string.Empty,
                });
            }
            NotifyStateChanged();
        }

        // Elimina un dispositivo de la lista de ítems seleccionados
        public void RemoveDispositivoForReview(int dispositivoId)
        {
            var itemToRemove = Review.ItemsReview.FirstOrDefault(i => i.IdDispositivo == dispositivoId);
            if (itemToRemove != null)
            {
                Review.ItemsReview.Remove(itemToRemove);
                NotifyStateChanged();
            }
        }

        // Vacia la selección de dispositivos
        public void ClearReviewCart()
        {
            Review.ItemsReview.Clear();
            NotifyStateChanged();
        }

        // Propiedad de solo lectura para saber si el botón "Reseñar dispositivos" debe estar disponible.
        public bool HasDevicesSelected => Review.ItemsReview.Any();
    }
}
