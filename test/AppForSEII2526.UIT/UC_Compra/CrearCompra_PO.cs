using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Compras
{
    public class CrearCompra_PO : PageObject
    {
        // --- 1. SELECTORES (Basados en los IDs que pusimos en el Razor) ---

        // Inputs del Formulario
        private By inputNombre = By.Id("InputNombre");
        private By inputApellidos = By.Id("InputApellidos");
        private By inputDireccion = By.Id("InputDireccion");
        private By inputPago = By.Id("InputPago"); // El <select>

        // Botones
        // Buscamos el botón de tipo submit (Confirmar)
        private By btnConfirmar = By.XPath("//button[@type='submit']");

        // Buscamos el enlace que contiene el texto "Volver"
        private By btnVolver = By.XPath("//a[contains(., 'Volver')]");

        // Mensajes de Error / Feedback
        // La alerta roja grande que sale arriba del todo
        private By alertaGeneral = By.CssSelector("div.alert.alert-danger");

        // Mensajes de validación pequeñitos debajo de cada input (por si acaso los necesitas)
        private By mensajesErrorCampo = By.CssSelector(".validation-message");


        // --- CONSTRUCTOR ---
        public CrearCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        // --- 2. MÉTODOS DE ACCIÓN (Escribir y Clicar) ---

        public void EscribirNombre(string nombre)
        {
            WaitForBeingVisible(inputNombre);
            _driver.FindElement(inputNombre).Clear();
            if (!string.IsNullOrEmpty(nombre))
            {
                _driver.FindElement(inputNombre).SendKeys(nombre);
            }
        }

        public void EscribirApellidos(string apellidos)
        {
            WaitForBeingVisible(inputApellidos);
            _driver.FindElement(inputApellidos).Clear();
            if (!string.IsNullOrEmpty(apellidos))
            {
                _driver.FindElement(inputApellidos).SendKeys(apellidos);
            }
        }

        public void EscribirDireccion(string direccion)
        {
            WaitForBeingVisible(inputDireccion);
            _driver.FindElement(inputDireccion).Clear();
            if (!string.IsNullOrEmpty(direccion))
            {
                _driver.FindElement(inputDireccion).SendKeys(direccion);
            }
        }

        public void SeleccionarPago(string metodoPago)
        {
            WaitForBeingVisible(inputPago);
            if (!string.IsNullOrEmpty(metodoPago))
            {
                var select = new SelectElement(_driver.FindElement(inputPago));
                // Intenta seleccionar por texto visible (ej. "Tarjeta", "Paypal")
                try
                {
                    select.SelectByText(metodoPago);
                }
                catch (NoSuchElementException)
                {
                    // Si falla por texto, intenta ver si coincide parcialmente o informa
                    _output.WriteLine($"Advertencia: No se encontró la opción exacta '{metodoPago}'.");
                }
            }
        }

        public void ClickConfirmar()
        {
            WaitForBeingClickable(btnConfirmar);
            _driver.FindElement(btnConfirmar).Click();
        }

        public void ClickVolver()
        {
            WaitForBeingClickable(btnVolver);
            _driver.FindElement(btnVolver).Click();
        }


        // --- 3. MÉTODOS DE VALIDACIÓN (Leer lo que pasa en pantalla) ---

        /// <summary>
        /// Obtiene el texto de la alerta roja grande superior.
        /// Útil para leer "Hay que rellenar campos obligatorios" o "Error 400 Bad Request".
        /// </summary>
        public string ObtenerMensajeAlertaGeneral()
        {
            try
            {
                // Esperamos un poco a que aparezca la alerta (Blazor tarda unos ms)
                WaitForBeingVisible(alertaGeneral);
                string texto = _driver.FindElement(alertaGeneral).Text;

                // Imprimimos en la consola de test para depurar
                _output.WriteLine($"Alerta detectada: {texto}");
                return texto;
            }
            catch (WebDriverTimeoutException)
            {
                // Si no aparece nada tras el tiempo de espera, devolvemos vacío
                return "";
            }
        }

        /// <summary>
        /// Devuelve true si la alerta de error está visible en pantalla.
        /// </summary>
        public bool EsVisibleAlertaError()
        {
            try
            {
                // Usamos FindElements (plural) para comprobar existencia sin lanzar excepción
                var elementos = _driver.FindElements(alertaGeneral);
                return elementos.Count > 0 && elementos[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Método auxiliar para limpiar todos los campos rápidamente.
        /// </summary>
        public void LimpiarFormulario()
        {
            EscribirNombre("");
            EscribirApellidos("");
            EscribirDireccion("");
        }
    }
}