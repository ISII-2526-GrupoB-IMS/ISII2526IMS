using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared; // Importante para heredar PageObject
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    // Heredamos de PageObject tal cual lo hacen ellas
    public class SelectDispositivosAlquiler_PO : PageObject
    {
        // --- Localizadores (Adaptados a tu HTML) ---
        // Como no tienes IDs en los inputs, uso CSS/XPath específicos
        private By inputNombre = By.CssSelector("input[placeholder='ej. iPhone']");
        private By inputPrecio = By.CssSelector("input[type='number']"); // El input de precio
        private By buttonSearch = By.XPath("//button[contains(text(),'Buscar')]");

        // Fechas: Blazor a veces pone type="date", buscamos por orden o clase
        private By inputFrom = By.XPath("(//input[@type='date'])[1]"); // Primer calendario
        private By inputTo = By.XPath("(//input[@type='date'])[2]");   // Segundo calendario

        private By tableOfDispositivos = By.TagName("table");
        private By errorShownBy = By.Id("ErrorsShown"); // Este sí tenía ID en tu código
        private By buttonCrearReserva = By.XPath("//button[contains(text(),'Crear reserva')]");

        // Constructor que pasa los datos a la base
        public SelectDispositivosAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Método Search (Imitando SearchMovies)
        public void SearchDispositivos(string nombre, string precioMax, DateTime? from, DateTime? to)
        {
            // Esperamos que el input sea visible/clickable
            WaitForBeingClickable(inputNombre);

            // Nombre
            _driver.FindElement(inputNombre).Clear();
            _driver.FindElement(inputNombre).SendKeys(nombre);

            // Precio (Si no es nulo/vacío)
            if (!string.IsNullOrEmpty(precioMax))
            {
                _driver.FindElement(inputPrecio).Clear();
                _driver.FindElement(inputPrecio).SendKeys(precioMax);
            }

            // Fechas: Usamos tu método helper 'InputDateInDatePicker' que tienes en PageObject.cs
            // Ellas usaban SendKeys, pero tu método es más robusto para fechas complejas.
            if (from.HasValue)
                InputDateInDatePicker(inputFrom, from.Value);

            if (to.HasValue)
                InputDateInDatePicker(inputTo, to.Value);

            // Click Buscar
            _driver.FindElement(buttonSearch).Click();

            // Espera implícita para que la tabla refresque (opcional, pero recomendada)
            Thread.Sleep(1000);
        }

        // Verificar la tabla (Imitando CheckListOfMovies)
        // En SelectDispositivosAlquiler_PO.cs

        public bool CheckListOfDispositivos(List<string[]> expectedRows)
        {
            // 1. Obtenemos todas las filas del cuerpo de la tabla
            var rows = _driver.FindElements(By.CssSelector("table tbody tr"));

            // 2. Si no hay filas y esperamos alguna, devolvemos false
            if (rows.Count == 0 && expectedRows.Count > 0) return false;

            // 3. Iteramos por cada dispositivo que ESPERAMOS encontrar
            foreach (var expected in expectedRows)
            {
                bool found = false;
                string expectedNombre = expected[0];
                string expectedMarca = expected[1];
                string expectedPrecio = expected[2];

                // Buscamos en las filas visibles de la web
                foreach (var row in rows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    // Aseguramos que la fila tenga suficientes columnas (al menos 5 según tu imagen)
                    if (cells.Count < 5) continue;

                    // Extraemos SOLO lo que nos interesa: Índices 0 (Nombre), 1 (Marca) y 4 (Precio)
                    // Usamos .Trim() para limpiar espacios extra
                    string actualNombre = cells[0].Text.Trim();
                    string actualMarca = cells[1].Text.Trim();
                    string actualPrecio = cells[4].Text.Trim();

                    // Comparamos (Contains para ser flexible o Equals para exactitud)
                    if (actualNombre.Contains(expectedNombre) &&
                        actualMarca.Contains(expectedMarca) &&
                        actualPrecio.Contains(expectedPrecio))
                    {
                        found = true;
                        break; // Encontramos este dispositivo, pasamos al siguiente esperado
                    }
                }

                // Si terminamos de revisar todas las filas y no encontramos el esperado: Falso.
                if (!found)
                {
                    _output.WriteLine($"No se encontró fila para: {expectedNombre} | {expectedMarca} | {expectedPrecio}");
                    return false;
                }
            }

            return true;
        }

        // Verificar errores (Imitando CheckMessageError)
        public bool CheckMessageError(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                // Si no esperamos error, verificamos que NO haya mensaje
                var elements = _driver.FindElements(errorShownBy);
                return elements.Count == 0 || string.IsNullOrEmpty(elements[0].Text);
            }
            WaitForBeingVisible(errorShownBy);
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"Actual Message shown: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        // Añadir al carrito (Imitando AddMovieToRentingCart)
        public void AddDispositivoToCart(string nombreDispositivo)
        {
            // Buscamos el botón "Añadir" que esté en la misma fila que el nombre del dispositivo
            By btnAddSpecific = By.XPath($"//tr[td[contains(text(), '{nombreDispositivo}')]]//button[contains(text(), 'Añadir')]");

            WaitForBeingClickable(btnAddSpecific);
            _driver.FindElement(btnAddSpecific).Click();
        }

        // Quitar del carrito (Imitando RemoveMovieFromRentingCart)
        public void RemoveDispositivoFromCart(string nombreDispositivo)
        {
            // Buscamos el botón "Quitar" en la fila correspondiente
            By btnRemoveSpecific = By.XPath($"//tr[td[contains(text(), '{nombreDispositivo}')]]//button[contains(text(), 'Quitar')]");

            WaitForBeingClickable(btnRemoveSpecific);
            _driver.FindElement(btnRemoveSpecific).Click();
        }

        // Verificar botón reserva (Imitando RentingNotAvailable o similar)
        public bool IsCrearReservaDisabledOrHidden()
        {
            try
            {
                // Si el panel del carrito está oculto (hidden), el botón no es interactuable
                // En tu código: <div class="col-4" hidden="@hideCart">
                var boton = _driver.FindElement(buttonCrearReserva);
                return !boton.Displayed || !boton.Enabled;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        public void ClickCrearReserva()
        {
            WaitForBeingClickable(buttonCrearReserva);
            _driver.FindElement(buttonCrearReserva).Click();
        }
    }
}