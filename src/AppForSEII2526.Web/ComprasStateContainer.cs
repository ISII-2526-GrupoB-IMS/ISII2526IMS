using AppForSEII2526.Web.API;
namespace AppForSEII2526.Web
{
    public class ComprasStateContainer
    {

        public CompraForCreateDTO Compra { get; private set; }

        public ComprasStateContainer()
        {
            ResetCompra();
        }
        public event Action? OnStateChange;
        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void ResetCompra()
        {
            
            Compra = new CompraForCreateDTO
            {
                NombreUsuario = string.Empty,
                ApellidosUsuario = string.Empty,
                DireccionDeEnvio = string.Empty,
                MetodoDePago = TiposMetodoPago.TarjetaCredito,
                Cantidad = 0,
                ItemsCompra = new List<CompraItemDTO>()
            };
            NotifyStateChanged();
        }

        public decimal TotalPrice
        {
            get
            {
                if (Compra.ItemsCompra == null) return 0;
                return (decimal)Compra.ItemsCompra.Sum(item => item.Precio * item.Cantidad);
            }
        }

        public void AñadirDispositivo(DispositivoParaComprarDTO dispositivo)
        {
           
            var itemExistente = Compra.ItemsCompra.FirstOrDefault(ic =>
                ic.Marca == dispositivo.Marca &&
                ic.Modelo == dispositivo.Modelo.NombreModelo && 
                ic.Color == dispositivo.Color
            );

            if (itemExistente != null)
            {
                itemExistente.Cantidad++;
            }
            else
            {
               
                var nuevoItem = new CompraItemDTO
                {
                    Marca = dispositivo.Marca,
                    Modelo = dispositivo.Modelo.NombreModelo, 
                    Color = dispositivo.Color,
                    Precio = dispositivo.PrecioParaCompra,
                    Cantidad = 1,
                    Descripcion = "Compra Web"
                };

                Compra.ItemsCompra.Add(nuevoItem);
            }

            ActualizarTotales();
            NotifyStateChanged();
        }

        public void EliminarCarrito()
        {
            Compra.ItemsCompra.Clear();
            ActualizarTotales();
            NotifyStateChanged();
        }

        public void QuitarItemParaComprar(CompraItemDTO item)
        {
            if (Compra.ItemsCompra.Contains(item))
            {
                Compra.ItemsCompra.Remove(item);
                ActualizarTotales();
                NotifyStateChanged();
            }
        }

        private void ActualizarTotales()
        {
            Compra.Cantidad = Compra.ItemsCompra.Sum(i => i.Cantidad);
        }
    }
}
