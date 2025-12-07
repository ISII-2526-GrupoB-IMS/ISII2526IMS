
using AppForSEII2526.Web.API;
namespace AppForSEII2526.Web
{
    public class AlquilerStateContainer
    {
        public AlquilerForCreateDTO Alquiler { get; private set; } = new AlquilerForCreateDTO()
        {
            ItemsAlquiler = new List<ItemAlquilerDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
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
            //before adding a movie we checked whether it has been already added
            if (!Alquiler.ItemsAlquiler.Any(ri => ri.IdDispositivo == dispositivo.Id))
                //we add it if it is not in the list
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

        //to delete movies from the list of selected movies
        public void RemoveDispositivoForRental(ItemAlquilerDTO item)
        {
            Alquiler.ItemsAlquiler.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Alquiler.ItemsAlquiler.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void RentalProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Alquiler = new AlquilerForCreateDTO()
            {
                ItemsAlquiler = new List<ItemAlquilerDTO>()
            };
        }

    }
}


