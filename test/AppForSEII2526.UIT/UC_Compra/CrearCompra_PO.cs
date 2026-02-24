using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Compras
{
    public class CrearCompra_PO : PageObject
    {
        
        private By inputNombre = By.Id("InputNombre");
        private By inputApellidos = By.Id("InputApellidos");
        private By inputDireccion = By.Id("InputDireccion");
        private By inputPago = By.Id("InputPago"); 
        private By btnConfirmar = By.XPath("//button[@type='submit']");
        private By btnVolver = By.XPath("//a[contains(., 'Volver')]");
        private By alertaGeneral = By.CssSelector("div.alert.alert-danger");
        private By mensajesErrorCampo = By.CssSelector(".validation-message");



        public CrearCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }


        

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
                
                try
                {
                    select.SelectByText(metodoPago);
                }
                catch (NoSuchElementException)
                {
                   
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


        
        public string ObtenerMensajeAlertaGeneral()
        {
            try
            {
                
                WaitForBeingVisible(alertaGeneral);
                string texto = _driver.FindElement(alertaGeneral).Text;

               
                _output.WriteLine($"Alerta detectada: {texto}");
                return texto;
            }
            catch (WebDriverTimeoutException)
            {
                
                return "";
            }
        }

        public bool EsVisibleAlertaError()
        {
            try
            {
               
                var elementos = _driver.FindElements(alertaGeneral);
                return elementos.Count > 0 && elementos[0].Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckMessageErrorNotAvaibleMovies(string expectedError)
        {
            return ObtenerMensajeAlertaGeneral().Contains(expectedError);

        }



        public void LimpiarFormulario()
        {
            EscribirNombre("");
            EscribirApellidos("");
            EscribirDireccion("");
        }
    }
}