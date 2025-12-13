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
        public bool CheckListOfDispositivos(List<string[]> expectedRows)
        {
            return CheckBodyTable(expectedRows, tableOfDispositivos);
        }

        // Verificar errores (Imitando CheckMessageError)
        public bool CheckMessageError(string errorMessage)
        {
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