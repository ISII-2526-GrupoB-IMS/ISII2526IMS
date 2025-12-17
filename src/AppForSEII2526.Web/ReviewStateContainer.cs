using AppForSEII2526.Web.API;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {
        public ReviewForCreateDTO Reseña { get; private set; } = new ReviewForCreateDTO() { ItemsReview = new List<ReviewItemDTO>() };

        // NUEVA PROPIEDAD: Para guardar el resultado de la API y mostrarlo en el detalle
        public ReviewDetailDTO? UltimaReseñaProcesada { get; set; }

        public event Action? OnStateChange;
        private void NotifyStateChanged() => OnStateChange?.Invoke();

        // Añade un dispositivo a la lista de reseñas pendientes
        public void AddDispositivoForReview(DispositivoParaReviewDTO dispositivo)
        {
            // Asegúrate de que la validación coincida con los campos de tu Clave Primaria (Nombre + Modelo)
            bool yaExiste = Reseña.ItemsReview.Any(ri =>
                ri.NombreDispositivo == dispositivo.NombreDispositivo &&
                ri.Modelo == dispositivo.Modelo.NombreModelo);

            if (!yaExiste)
            {
                Reseña.ItemsReview.Add(new ReviewItemDTO()
                {
                    NombreDispositivo = dispositivo.NombreDispositivo,
                    Modelo = dispositivo.Modelo.NombreModelo,
                    Año = dispositivo.Año,
                    Puntuacion = 0,
                    Comentario = ""
                });
                NotifyStateChanged();
            }
        }

        // Elimina un dispositivo específico de la lista
        public void RemoveDispositivoForReview(ReviewItemDTO item)
        {
            if (Reseña.ItemsReview.Contains(item))
            {
                Reseña.ItemsReview.Remove(item);
                NotifyStateChanged();
            }
        }

        // Vacía la cesta de dispositivos para reseñar
        public void ClearReviewCart()
        {
            Reseña.ItemsReview.Clear();
            NotifyStateChanged();
        }

        // Reinicia el estado después de que la reseña ha sido procesada con éxito por la API
        public void ReviewProcessed(ReviewDetailDTO detalle)
        {
            UltimaReseñaProcesada = detalle;
            // Reseteo total del objeto para la siguiente reseña
            Reseña = new ReviewForCreateDTO();
            Reseña.ItemsReview = new List<ReviewItemDTO>();
            NotifyStateChanged();
        }
    }
}
