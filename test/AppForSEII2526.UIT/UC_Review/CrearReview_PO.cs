using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Reviews
{
    public class CrearReview_PO : PageObject
    {
        // --- Localizadores Cabecera ---
        private By inputTitle = By.Id("Title");
        private By inputCountry = By.Id("Country");
        private By inputUserName = By.Id("UserName");
        private By btnSubmit = By.Id("Submit");

        // --- Localizadores de Errores (Aparecen debajo de los campos) ---
        private By validationMessages = By.CssSelector(".text-danger, .validation-message");

        public CrearReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarCabecera(string titulo, string pais, string nombre)
        {
            WaitForBeingVisible(inputTitle);

            _driver.FindElement(inputTitle).Clear();
            if (titulo != null) _driver.FindElement(inputTitle).SendKeys(titulo);

            _driver.FindElement(inputCountry).Clear();
            if (pais != null) _driver.FindElement(inputCountry).SendKeys(pais);

            _driver.FindElement(inputUserName).Clear();
            if (nombre != null) _driver.FindElement(inputUserName).SendKeys(nombre);
        }

        public void RellenarDetalleDispositivo(string nombreDispositivo, string puntuacion, string comentario)
        {
            // Localiza la fila de la tabla por el nombre del dispositivo
            string xpathRow = $"//tr[td[contains(., '{nombreDispositivo}')]]";
            IWebElement row = _driver.FindElement(By.XPath(xpathRow));

            // Puntuación (InputNumber)
            var inputScore = row.FindElement(By.CssSelector("input[type='number']"));
            inputScore.Clear();
            inputScore.SendKeys(puntuacion);

            // Comentario (InputTextArea)
            var inputComment = row.FindElement(By.TagName("textarea"));
            inputComment.Clear();
            if (comentario != null) inputComment.SendKeys(comentario);
        }

        public void ClickPublicarReseña()
        {
            WaitForBeingClickable(btnSubmit);
            IWebElement botón = _driver.FindElement(btnSubmit);

            try
            {
                // Intento 1: Desplazar la pantalla hasta el botón para que sea visible
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", botón);
                Thread.Sleep(500); // Pausa breve para que el scroll termine

                // Intento de clic normal
                botón.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Intento 2: Si algo lo bloquea (mensajes de error), forzamos el clic con JS
                _output.WriteLine("Clic normal interceptado, intentando clic por JavaScript...");
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botón);
            }

            Thread.Sleep(2000); // Espera para que aparezcan los errores o cambie la página
        }

        public bool ExisteMensajeDeError(string textoEsperado)
        {
            // 1. Espera técnica para que Blazor renderice el error tras el clic
            Thread.Sleep(1000);

            // 2. Buscamos en TODOS los posibles contenedores de error:
            // .text-danger (debajo de inputs), .validation-message (Blazor default) y .alert-danger (banner superior)
            var selectoresDeError = _driver.FindElements(By.CssSelector(".text-danger, .validation-message, .alert-danger"));

            foreach (var elemento in selectoresDeError)
            {
                if (elemento.Displayed)
                {
                    _output.WriteLine($"Texto de error detectado: {elemento.Text}");
                    if (elemento.Text.Contains(textoEsperado, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            _output.WriteLine($"ERROR: No se encontró '{textoEsperado}' en ningún elemento de error visible.");
            return false;
        }
    }
}

