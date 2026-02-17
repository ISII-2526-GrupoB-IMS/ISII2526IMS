using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared; 
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class SelectDispositivosCompra_PO : PageObject
    {
        private By inputFiltroNombre = By.CssSelector("input[placeholder*='Nombre']");
        private By inputFiltroColor = By.CssSelector("input[placeholder*='Color']");
        private By buttonSearch = By.XPath("//button[contains(., 'Buscar')]");
        private By cardDispositivo = By.CssSelector(".col .card");
        private By buttonVaciar = By.XPath("//button[contains(., 'Vaciar')]");
        private By btnTramitar = By.XPath("//button[contains(., 'Tramitar Pedido')]");
        private By alertMessage = By.CssSelector(".alert");


        private IWebElement _rentButton() => _driver.FindElement(btnTramitar);
        public SelectDispositivosCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
       
        public void SearchDispositivos(string nombre, string color)
        {
            // Esperar y borrar nombre
            WaitForBeingClickable(inputFiltroNombre);
            _driver.FindElement(inputFiltroNombre).Clear();
            if (!string.IsNullOrEmpty(nombre))
            {
                _driver.FindElement(inputFiltroNombre).SendKeys(nombre);
            }

            // Esperar y borrar color
            _driver.FindElement(inputFiltroColor).Clear();
            if (!string.IsNullOrEmpty(color))
            {
                _driver.FindElement(inputFiltroColor).SendKeys(color);
            }

            
            _driver.FindElement(buttonSearch).Click();

            
            Thread.Sleep(1000);
        }

        
        public bool CheckListOfDispositivos(List<string[]> expectedData)
        {
            var cards = _driver.FindElements(cardDispositivo);

            
            if (cards.Count == 0 && expectedData.Count > 0) return false;

            foreach (var expected in expectedData)
            {
                bool found = false;
                string expectedNombre = expected[0];
                string expectedMarca = expected[1];
                string expectedPrecio = expected[2];

                foreach (var card in cards)
                {
                    
                    string actualNombre = card.FindElement(By.CssSelector(".card-title")).Text;
                    string actualMarca = card.FindElement(By.CssSelector(".card-subtitle")).Text;
                    string actualPrecio = card.FindElement(By.CssSelector("h3.text-primary")).Text;

                    if (actualNombre.Contains(expectedNombre, StringComparison.OrdinalIgnoreCase) &&
                        actualMarca.Contains(expectedMarca, StringComparison.OrdinalIgnoreCase) &&
                        actualPrecio.Contains(expectedPrecio))
                    {
                        found = true;
                        break; 
                    }
                }

                if (!found)
                {
                    _output.WriteLine($"No se encontró TARJETA para: {expectedNombre} | {expectedMarca} | {expectedPrecio}");
                    return false;
                }
            }
            return true;
        }

        
        public void AddDispositivoToCart(string nombreDispositivo)
        {
            
            var xpathButton = $"//h5[contains(@class,'card-title') and contains(text(),'{nombreDispositivo}')]/ancestor::div[contains(@class,'card')]//button[contains(., 'Añadir')]";

            By btnAdd = By.XPath(xpathButton);

            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();

            Thread.Sleep(500);
        }

       
        public void VaciarCarrito()
        {
            if (!IsTramitarPedidoHidden())
            {
                WaitForBeingClickable(buttonVaciar);
                _driver.FindElement(buttonVaciar).Click();
            }
        }

        

        public bool IsTramitarPedidoHidden()
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            var elementos = _driver.FindElements(btnTramitar);

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); 
            if (elementos.Count == 0) return true;

         
            return !elementos[0].Displayed;
        }
        
        public void RemoveDispositivoFromCart(string nombreDispositivo)
        {
            
            var xpathBoton = $"//li[contains(., '{nombreDispositivo}')]//button[contains(@class, 'btn-remove')]";

            By btnEliminar = By.XPath(xpathBoton);

            try
            {
                WaitForBeingClickable(btnEliminar);
                _driver.FindElement(btnEliminar).Click();

                
                Thread.Sleep(1000);
            }
            catch (WebDriverTimeoutException)
            {
               
                _output.WriteLine($"Error: No se encontró el botón de borrar (clase .btn-remove) para el móvil '{nombreDispositivo}'.");
                throw;
            }
        }

        
        private By totalPrecio = By.XPath("//div[contains(@class, 'card-footer')]//strong[contains(@class, 'text-primary') or contains(@class, 'h4') or contains(@class, 'h5')]");

        public string ObtenerPrecioTotal()
        {
            try
            {
                
                WaitForBeingVisible(totalPrecio);

                string texto = _driver.FindElement(totalPrecio).Text;

                _output.WriteLine($"Precio encontrado en pantalla: '{texto}'");

                return texto;
            }
            catch (WebDriverTimeoutException)
            {
                _output.WriteLine("Error: No se encontró el elemento del precio total en el tiempo límite.");
                return "0,00 €";
            }
        }

        public bool CheckRentMoviesDisabled()
        {
            //we return true if the button is disabled
            return !(_rentButton().Enabled);
        }

        public bool CheckMessageErrorNotAvaibleMovies(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);

        }



    }
}