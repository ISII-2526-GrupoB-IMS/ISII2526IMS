using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class DetalleCompra_PO : PageObject
    {
        // --- 1. SELECTORES (Adaptados a tu HTML sin IDs) ---

        // El título h2: "¡Gracias por tu compra, Nombre Apellidos!"
        private By headerTitulo = By.TagName("h2");

        // Dirección: Buscamos el div que está al lado del texto "Dirección de envío:"
        // XPath: Busca un div que contenga el texto "Dirección...", sube al padre (row) y busca el div con clase 'fw-bold'
        private By lblDireccion = By.XPath("//div[contains(text(), 'Dirección de envío:')]/following-sibling::div");

        // Precio Total: Está dentro de un alert-success, es el segundo span (o el último)
        // XPath: Busca el div con clase alert-success y coge el span que contiene el precio (el último)
        private By lblPrecioTotal = By.XPath("//div[contains(@class, 'alert-success')]/span[last()]");

        // Botón "Volver a la Tienda"
        private By btnVolver = By.XPath("//button[contains(., 'Volver a la Tienda')]");


        // --- CONSTRUCTOR ---
        public DetalleCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // --- 2. MÉTODOS DE VALIDACIÓN ---

        public bool EstamosEnPaginaDetalle()
        {
            try
            {
                // Esperamos que el H2 sea visible
                WaitForBeingVisible(headerTitulo);
                // Tu URL tiene este formato: /Compras/DetalleCompra/{Id}
                return _driver.Url.Contains("/Compras/DetalleCompra/");
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public string ObtenerTextoTitulo()
        {
            // Devuelve: "¡Gracias por tu compra, Juan Perez!"
            WaitForBeingVisible(headerTitulo);
            string texto = _driver.FindElement(headerTitulo).Text;
            _output.WriteLine($"Título detectado: {texto}");
            return texto;
        }

        public string ObtenerDireccion()
        {
            WaitForBeingVisible(lblDireccion);
            return _driver.FindElement(lblDireccion).Text;
        }

        public string ObtenerPrecioTotal()
        {
            WaitForBeingVisible(lblPrecioTotal);
            string precio = _driver.FindElement(lblPrecioTotal).Text;
            _output.WriteLine($"Precio total detectado: {precio}");
            return precio;
        }

        // --- 3. MÉTODOS DE ACCIÓN ---

        public void ClickVolverALaTienda()
        {
            WaitForBeingClickable(btnVolver);
            _driver.FindElement(btnVolver).Click();
        }
    }

}
