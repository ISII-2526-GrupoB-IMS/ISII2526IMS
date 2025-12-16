using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; // Necesario para el Select (Dropdown)
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    public class CreateAlquiler_PO : PageObject
    {
        // Localizadores (Basados en tus IDs del Razor)
        private By inputName = By.Id("Name");
        private By inputSurname = By.Id("Surname");
        private By inputAddress = By.Id("DeliveryAddress");
        private By selectPayment = By.Id("PaymentMethod");
        private By btnSubmit = By.Id("Submit");

        // El mensaje de error general (div alert-danger)
        private By alertError = By.CssSelector(".alert.alert-danger");

        public CreateAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarFormulario(string nombre, string apellidos, string direccion, string metodoPago)
        {
            WaitForBeingVisible(inputName);

            // Nombre
            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(nombre);

            // Apellidos
            _driver.FindElement(inputSurname).Clear();
            _driver.FindElement(inputSurname).SendKeys(apellidos);

            // Dirección
            _driver.FindElement(inputAddress).Clear();
            _driver.FindElement(inputAddress).SendKeys(direccion);

            // Método de Pago (Dropdown)
            // Si te da error aquí, asegúrate de tener el paquete NuGet 'Selenium.Support'
            var paymentElement = _driver.FindElement(selectPayment);
            var select = new SelectElement(paymentElement);
            select.SelectByText(metodoPago);
        }

        public void ClickAlquilar()
        {
            WaitForBeingClickable(btnSubmit);
            _driver.FindElement(btnSubmit).Click();

            // Esperamos un momento para que se procese (validación o envío)
            // Si hay un Dialogo de confirmación (Modal), habría que manejarlo aquí.
            // Según tu código Razor, hay un Dialogo (DialogIsOpen), así que al dar click
            // se abre el modal, y luego hay que confirmar en el modal.
        }

        // Método extra para manejar tu Modal de confirmación "DialogIsOpen"
        public void ConfirmarEnModal()
        {
            // Buscamos el botón de "Yes" o "OK" del componente Dialog
            // Asumo que el botón de confirmar tiene una clase tipo btn-primary dentro del modal
            // O busca por texto "Yes" / "Save"
            By btnConfirmarModal = By.XPath("//div[@class='modal-footer']//button[contains(@class, 'btn-primary')]");

            WaitForBeingClickable(btnConfirmarModal);
            _driver.FindElement(btnConfirmarModal).Click();
        }

        public bool HayMensajeDeError(string textoEsperado)
        {
            try
            {
                WaitForBeingVisible(alertError);
                var mensaje = _driver.FindElement(alertError).Text;
                _output.WriteLine($"Error encontrado: {mensaje}");
                return mensaje.Contains(textoEsperado);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}