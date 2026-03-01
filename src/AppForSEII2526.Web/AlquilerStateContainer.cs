
using AppForSEII2526.Web.API;
namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        public AlquilerForCreateDTO Alquiler { get; private set; } = new AlquilerForCreateDTO()
        {
            ItemsAlquiler = new List<ItemAlquilerDTO>()
        };

        public decimal TotalPrice
        {
            get
            {
                int numberOfDays = (Alquiler.FechaAlquilerHasta - Alquiler.FechaAlquilerHasta).Days;
                return Convert.ToDecimal(Alquiler.ItemsAlquiler.Sum(ri => ri.PrecioParaAlquiler * numberOfDays));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddDispositivoForRental(DispositivoParaAlquilarDTO dispositivo)
        {
            if (!Alquiler.ItemsAlquiler.Any(ri => ri.IdDispositivo == dispositivo.Id))
                Alquiler.ItemsAlquiler.Add(new ItemAlquilerDTO()
                {
                    IdDispositivo = dispositivo.Id,
                    Modelo = dispositivo.Modelo.NombreModelo,
                    Marca = dispositivo.Marca,
                    NombreDispositivo = dispositivo.NombreDispositivo,
                    PrecioParaAlquiler = dispositivo.PrecioParaAlquiler,
                }
            );

        }

        public void RemoveDispositivoForRental(ItemAlquilerDTO item)
        {
            Alquiler.ItemsAlquiler.Remove(item);

        }

        public void ClearRentingCart()
        {
            Alquiler.ItemsAlquiler.Clear();

        }

        public void RentalProcessed()
        {
            Alquiler = new AlquilerForCreateDTO()
            {
                ItemsAlquiler = new List<ItemAlquilerDTO>()
            };
        }

    }
}


