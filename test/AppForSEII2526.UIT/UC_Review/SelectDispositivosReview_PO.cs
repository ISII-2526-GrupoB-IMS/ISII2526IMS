using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Reviews
{
    public class SelectDispositivosReview_PO : PageObject
    {
        // --- Localizadores Basados en tu Interfaz (Cards) ---

        // Filtros
        private By inputMarca = By.CssSelector("input[placeholder*='Marca']");
        private By inputAño = By.CssSelector("input[placeholder*='Año']");
        private By buttonSearch = By.XPath("//button[contains(., 'Buscar')]");

        // Elementos del Catálogo (Cards)
        private By cardDispositivo = By.CssSelector(".card"); // Cada tarjeta de dispositivo

        // Carrito (Sidebar)
        private By buttonReseñarDispositivos = By.XPath("//button[contains(., 'Reseñar Dispositivos')]");
        private By buttonVaciarCarrito = By.XPath("//button[contains(., 'Vaciar Carrito')]");

        // Mensajes de Error
        private By errorShownBy = By.Id("ErrorsShown");

        public SelectDispositivosReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // --- Acciones de Búsqueda ---
        public void SearchDispositivos(string marca, string año)
        {
            WaitForBeingClickable(inputMarca);
            _driver.FindElement(inputMarca).Clear();
            if (!string.IsNullOrEmpty(marca)) _driver.FindElement(inputMarca).SendKeys(marca);

            _driver.FindElement(inputAño).Clear();
            if (!string.IsNullOrEmpty(año)) _driver.FindElement(inputAño).SendKeys(año);

            _driver.FindElement(buttonSearch).Click();
            Thread.Sleep(1000); // Espera para renderizado de Blazor
        }

        // --- Verificación de Cards en el Catálogo ---
        public bool CheckListOfDispositivos(List<string[]> expectedData)
        {
            var cards = _driver.FindElements(cardDispositivo);
            if (cards.Count == 0 && expectedData.Count > 0) return false;

            foreach (var expected in expectedData)
            {
                bool found = false;
                string expectedNombre = expected[0]; // Nombre del dispositivo
                string expectedAño = expected[1];    // Año

                foreach (var card in cards)
                {
                    string cardText = card.Text;
                    // Verificamos que la card contenga el nombre y el año
                    if (cardText.Contains(expectedNombre, StringComparison.OrdinalIgnoreCase) &&
                        cardText.Contains(expectedAño))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _output.WriteLine($"No se encontró CARD para: {expectedNombre} ({expectedAño})");
                    return false;
                }
            }
            return true;
        }

        // --- Gestión de Selección (Botones en las Cards) ---
        public void AddDispositivoToReview(string nombreDispositivo)
        {
            // Busca la card que contiene el nombre y hace click en su botón "Añadir"
            var xpathButton = $"//div[contains(@class,'card') and contains(.,'{nombreDispositivo}')]//button[contains(., 'Añadir')]";
            By btnAdd = By.XPath(xpathButton);

            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();
            Thread.Sleep(500);
        }

        public void RemoveDispositivoFromReview(string nombreDispositivo)
        {
            // Busca el botón "Quitar de la Selección" en la card correspondiente
            var xpathButton = $"//div[contains(@class,'card') and contains(.,'{nombreDispositivo}')]//button[contains(., 'Quitar')]";
            By btnRemove = By.XPath(xpathButton);

            WaitForBeingClickable(btnRemove);
            _driver.FindElement(btnRemove).Click();
            Thread.Sleep(500);
        }

        // --- Acciones del Carrito (Sidebar) ---
        public void VaciarCarrito()
        {
            WaitForBeingClickable(buttonVaciarCarrito);
            _driver.FindElement(buttonVaciarCarrito).Click();
            Thread.Sleep(500);
        }

        public void ClickReseñarDispositivos()
        {
            WaitForBeingClickable(buttonReseñarDispositivos);
            _driver.FindElement(buttonReseñarDispositivos).Click();
        }

        public bool IsReseñarButtonDisabled()
        {
            // 1. Bajamos el tiempo de espera implícito para que el test no se quede "congelado" 
            // buscando un botón que sabemos que puede no estar.
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            try
            {
                var elementos = _driver.FindElements(buttonReseñarDispositivos);

                // Si la lista está vacía, el botón ha desaparecido (está oculto por un @if en Blazor)
                if (elementos.Count == 0)
                {
                    _output.WriteLine("El botón 'Reseñar' no existe en el DOM (Carrito vacío).");
                    return true;
                }

                // Si existe, comprobamos si está deshabilitado visualmente o por atributo
                bool isDisabled = !elementos[0].Enabled || !elementos[0].Displayed;
                return isDisabled;
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                // 2. IMPORTANTE: Restauramos el tiempo de espera normal para el resto del test
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
        }
    }
}