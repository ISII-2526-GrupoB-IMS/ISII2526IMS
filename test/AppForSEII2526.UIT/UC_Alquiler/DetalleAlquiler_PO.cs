using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    public class DetalleAlquiler_PO : PageObject
    {
        // Localizadores
        private By labelNameSurname = By.Id("NameSurname");
        private By labelAddress = By.Id("DeliveryAddress");

      
        private By labelPayment = By.XPath("//tr[th[contains(text(),'Payment Method')]]/td");

        private By labelRentalPeriod = By.Id("RentalPeriod");
        private By labelTotalPrice = By.Id("TotalPrice");
        private By tableMovies = By.Id("RentedMovies");

        public DetalleAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool VerificarDetallesCabecera(string nombreCompleto, string direccion, string metodoPago, string precioTotal)
        {
            try
            {
                // Esperamos a que cargue la página de detalle
                WaitForBeingVisible(labelNameSurname);

                string actualName = _driver.FindElement(labelNameSurname).Text;
                string actualAddress = _driver.FindElement(labelAddress).Text;
                string actualPayment = _driver.FindElement(labelPayment).Text;
                string actualPrice = _driver.FindElement(labelTotalPrice).Text;

                _output.WriteLine($"Detalle encontrado -> Nombre: {actualName}, Pago: {actualPayment}, Total: {actualPrice}");

                // Validamos
                bool checkName = actualName.Contains(nombreCompleto);
                bool checkAddr = actualAddress.Contains(direccion);
                bool checkPay = actualPayment.Contains(metodoPago);
                bool checkPrice = actualPrice.Contains(precioTotal);

                return checkName && checkAddr && checkPay && checkPrice;
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
                // Buscamos en todas las filas de la tabla
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