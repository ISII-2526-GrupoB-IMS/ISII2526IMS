using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class DetalleCompra_PO : PageObject
    {
        
        private By labelNameSurname = By.Id("NameSurname");
        private By labelAddress = By.Id("DeliveryAddress");
        private By fechaCompra = By.Id("FechaCompra");
        private By labelTotalPrice = By.Id("TotalPrice");
        private By tableMovies = By.Id("RentedMovies");


        
        public DetalleCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        

        public bool VerificarDetallesCabecera(string nombreCompleto, string direccion, string fecha, string precioTotal)
        {
            try
            {
                // Esperamos a que cargue la página de detalle
                WaitForBeingVisible(labelNameSurname);

                string actualName = _driver.FindElement(labelNameSurname).Text;
                string actualAddress = _driver.FindElement(labelAddress).Text;
                string actualFecha = _driver.FindElement(fechaCompra).Text;
                string actualPrice = _driver.FindElement(labelTotalPrice).Text;

                _output.WriteLine($"Detalle encontrado -> Nombre: {actualName}, Direccion: {actualAddress},Precio: {actualPrice}, Fecha: {actualFecha}");

                // Validamos
                bool checkName = actualName.Contains(nombreCompleto);
                bool checkAddr = actualAddress.Contains(direccion);
                bool checkPrice = actualPrice.Contains(precioTotal);
                bool checkFecha = actualFecha.Contains(fecha);

                _output.WriteLine($"DATOS EN LA WEB: {actualName} | {actualAddress} | {actualFecha} | {actualPrice}");
                _output.WriteLine($"DATOS ESPERADOS: {nombreCompleto} | {direccion} | {fecha} | {precioTotal}");

                return checkName && checkAddr && checkFecha && checkPrice;
            }
            catch (Exception ex)
            {
               _output.WriteLine($"Error verificando cabecera: {ex.Message}");
                return false;
            }
        }

        public bool VerificarDispositivoEnTabla(string nombreDispositivo)
        {
            try
            {
                WaitForBeingVisible(tableMovies);
                
                var filas = _driver.FindElements(By.CssSelector("#RentedMovies tbody tr"));

                foreach (var fila in filas)
                {
                    if (fila.Text.Contains(nombreDispositivo)) return true;
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

}
