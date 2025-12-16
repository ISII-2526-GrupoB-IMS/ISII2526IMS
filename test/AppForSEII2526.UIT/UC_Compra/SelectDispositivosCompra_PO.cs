using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared; // Importante para heredar PageObject
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class SelectDispositivosCompra_PO : PageObject
    {
        // --- Localizadores (Adaptados a SelectDispositivosComprar.razor) ---

        // Filtros: Buscamos por el placeholder ya que no pusiste IDs (mala práctica, pero funcional)
        private By inputFiltroNombre = By.CssSelector("input[placeholder*='Nombre']");
        private By inputFiltroColor = By.CssSelector("input[placeholder*='Color']");

        // Botón Buscar
        private By buttonSearch = By.XPath("//button[contains(., 'Buscar')]");

        // Elementos de la lista (Ahora son CARDS, no filas de tabla)
        // Buscamos cualquier div que tenga la clase 'card' dentro de la columna
        private By cardDispositivo = By.CssSelector(".col .card");

        // Botones de acción generales
        private By buttonTramitar = By.XPath("//button[contains(., 'Tramitar Pedido')]");
        private By buttonVaciar = By.XPath("//button[contains(., 'Vaciar')]");

        // Mensajes de error/aviso (Alertas)
        private By alertMessage = By.CssSelector(".alert");

        public SelectDispositivosCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        // 1. Método de Búsqueda (Adaptado a tus filtros: Nombre y Color)
        public void SearchDispositivos(string nombre, string color)
        {
            // Esperar y limpiar nombre
            WaitForBeingClickable(inputFiltroNombre);
            _driver.FindElement(inputFiltroNombre).Clear();
            if (!string.IsNullOrEmpty(nombre))
            {
                _driver.FindElement(inputFiltroNombre).SendKeys(nombre);
            }

            // Esperar y limpiar color
            _driver.FindElement(inputFiltroColor).Clear();
            if (!string.IsNullOrEmpty(color))
            {
                _driver.FindElement(inputFiltroColor).SendKeys(color);
            }

            // Click Buscar
            _driver.FindElement(buttonSearch).Click();

            // Espera técnica para que Blazor regenere el DOM (las cards)
            Thread.Sleep(1000);
        }

        // 2. Verificar lista (Adaptado a CARDS en vez de TABLA)
        public bool CheckListOfDispositivos(List<string[]> expectedData)
        {
            // Obtenemos todas las tarjetas visibles
            var cards = _driver.FindElements(cardDispositivo);

            // Si esperamos datos y no hay tarjetas -> Fail
            if (cards.Count == 0 && expectedData.Count > 0) return false;

            foreach (var expected in expectedData)
            {
                bool found = false;
                string expectedNombre = expected[0];
                string expectedMarca = expected[1];
                string expectedPrecio = expected[2];

                foreach (var card in cards)
                {
                    // Dentro de la tarjeta, buscamos los elementos específicos por sus clases de Bootstrap
                    // Título h5 = Nombre
                    string actualNombre = card.FindElement(By.CssSelector(".card-title")).Text;
                    // Subtítulo h6 = Marca
                    string actualMarca = card.FindElement(By.CssSelector(".card-subtitle")).Text;
                    // Precio h3 = Precio
                    string actualPrecio = card.FindElement(By.CssSelector("h3.text-primary")).Text;

                    // Comprobamos coincidencia (usamos Contains para ser flexibles con espacios/formatos)
                    if (actualNombre.Contains(expectedNombre, StringComparison.OrdinalIgnoreCase) &&
                        actualMarca.Contains(expectedMarca, StringComparison.OrdinalIgnoreCase) &&
                        actualPrecio.Contains(expectedPrecio))
                    {
                        found = true;
                        break; // Encontrado, pasamos al siguiente esperado
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

        // 3. Añadir al carrito (Click en el botón dentro de la tarjeta específica)
        public void AddDispositivoToCart(string nombreDispositivo)
        {
            // XPath Avanzado:
            // 1. Busca un elemento con clase card-title que contenga el texto del nombre.
            // 2. Sube a los ancestros para encontrar la 'card' contenedora.
            // 3. Baja para encontrar el botón que contenga "Añadir".
            var xpathButton = $"//h5[contains(@class,'card-title') and contains(text(),'{nombreDispositivo}')]/ancestor::div[contains(@class,'card')]//button[contains(., 'Añadir')]";

            By btnAdd = By.XPath(xpathButton);

            WaitForBeingClickable(btnAdd);
            _driver.FindElement(btnAdd).Click();

            // Espera breve para que se actualice el estado del carrito
            Thread.Sleep(500);
        }

        // 4. Vaciar carrito
        public void VaciarCarrito()
        {
            if (!IsTramitarPedidoHidden()) // Solo si hay algo que vaciar
            {
                WaitForBeingClickable(buttonVaciar);
                _driver.FindElement(buttonVaciar).Click();
            }
        }

        // 5. Verificar estado del botón Tramitar (si está oculto es que el carro está vacío)
        private By btnTramitar = By.XPath("//button[contains(., 'Tramitar Pedido')]");

        public bool IsTramitarPedidoHidden()
        {
            // CAMBIO CLAVE: Usamos FindElements (plural)
            // Esto nos devuelve una lista. Si la lista está vacía (Count == 0),
            // significa que el botón NO EXISTE en la página (que es lo que queremos).

            // 1. Bajamos el tiempo de espera temporalmente para que no tarde mucho en decidir que no está
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

            var elementos = _driver.FindElements(btnTramitar);

            // 2. Restauramos el tiempo de espera normal (IMPORTANTE)
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10); // O el tiempo que tengas configurado

            // Si hay 0 elementos, es que está oculto/borrado -> true
            if (elementos.Count == 0) return true;

            // Si existe, comprobamos si es visible -> false si se ve, true si está oculto por CSS
            return !elementos[0].Displayed;
        }
        // Selector para el botón de eliminar (papelera o texto "Eliminar") dentro del carrito
        // Buscamos un botón que contenga "Eliminar" dentro de un <li> que contenga el nombre del móvil
        public void RemoveDispositivoFromCart(string nombreDispositivo)
        {
            // ERROR ANTERIOR: Buscaba texto 'Eliminar' que no existe.
            // SOLUCIÓN: Buscamos por la clase 'btn-remove' que añadimos en el Razor.

            // 1. Buscamos el <li> que contiene el nombre del móvil
            // 2. Dentro de ese <li>, buscamos el botón con la clase 'btn-remove'
            var xpathBoton = $"//li[contains(., '{nombreDispositivo}')]//button[contains(@class, 'btn-remove')]";

            By btnEliminar = By.XPath(xpathBoton);

            try
            {
                WaitForBeingClickable(btnEliminar);
                _driver.FindElement(btnEliminar).Click();

                // Esperamos un instante a que Blazor actualice la interfaz
                Thread.Sleep(1000);
            }
            catch (WebDriverTimeoutException)
            {
                // Este mensaje te ayudará si vuelve a fallar
                _output.WriteLine($"Error: No se encontró el botón de borrar (clase .btn-remove) para el móvil '{nombreDispositivo}'.");
                throw;
            }
        }

        // Selector del Precio Total (sidebar)
        // Buscamos el elemento <strong> que está dentro del div con el texto "Total (EUR)"
        private By totalPrecio = By.XPath("//div[contains(@class, 'card-footer')]//strong[contains(@class, 'text-primary') or contains(@class, 'h4') or contains(@class, 'h5')]");

        public string ObtenerPrecioTotal()
        {
            try
            {
                // Esperamos a que sea visible
                WaitForBeingVisible(totalPrecio);

                string texto = _driver.FindElement(totalPrecio).Text;

                // Depuración: Imprimimos lo que encuentra Selenium para que sepas qué pasa
                _output.WriteLine($"Precio encontrado en pantalla: '{texto}'");

                return texto;
            }
            catch (WebDriverTimeoutException)
            {
                _output.WriteLine("Error: No se encontró el elemento del precio total en el tiempo límite.");
                return "0,00 €";
            }
        }



    }
}