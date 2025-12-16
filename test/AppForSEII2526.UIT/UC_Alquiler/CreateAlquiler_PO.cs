using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    public class CreateAlquiler_PO : PageObject
    {
        // --- Localizadores ---
        private By inputName = By.Id("Name");
        private By inputSurname = By.Id("Surname");
        private By inputAddress = By.Id("DeliveryAddress");
        private By selectPayment = By.Id("PaymentMethod");
        private By btnSubmit = By.Id("Submit");

        // Botón "Modify items"
        private By btnModifyItems = By.XPath("//button[contains(text(), 'Modify items')]");

        // Tabla de items en la vista Create
        private By tableRows = By.CssSelector("table tbody tr");

        // --- Localizadores de Errores Específicos ---
        // 1. Alertas generales (ValidationSummary y div de error custom)
        private By alertError = By.CssSelector(".alert.alert-danger");
        // 2. Mensajes de validación de campo individuales (Blazor usa esta clase por defecto)
        private By fieldValidation = By.CssSelector(".validation-message");

        public CreateAlquiler_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void RellenarFormulario(string nombre, string apellidos, string direccion, string metodoPago)
        {
            WaitForBeingVisible(inputName);

            if (nombre != null)
            {
                _driver.FindElement(inputName).Clear();
                _driver.FindElement(inputName).SendKeys(nombre);
            }

            if (apellidos != null)
            {
                _driver.FindElement(inputSurname).Clear();
                _driver.FindElement(inputSurname).SendKeys(apellidos);
            }

            if (direccion != null)
            {
                _driver.FindElement(inputAddress).Clear();
                _driver.FindElement(inputAddress).SendKeys(direccion);
            }

            if (!string.IsNullOrEmpty(metodoPago))
            {
                var paymentElement = _driver.FindElement(selectPayment);
                var select = new SelectElement(paymentElement);
                select.SelectByText(metodoPago);
            }
        }

        public void ClickAlquilar()
        {

            WaitForBeingClickable(btnSubmit);
            

            _driver.FindElement(btnSubmit).Click();
            Thread.Sleep(2000);
        }

        public void ClickModifyItems()
        {
            WaitForBeingClickable(btnModifyItems);
            _driver.FindElement(btnModifyItems).Click();
        }

        public void ConfirmarEnModal()
        {
            Thread.Sleep(500); // Pequeña espera para animación del modal
            By btnConfirmar = By.XPath("//div[contains(@class, 'modal')]//button[contains(@class, 'btn-primary')]");
            WaitForBeingClickable(btnConfirmar);
            _driver.FindElement(btnConfirmar).Click();
        }

        // MÉTODO ACTUALIZADO: Busca solo en los contenedores de error
        public bool HayMensajeDeError(string textoEsperado)
        {
            try
            {
                // Lista de selectores donde pueden aparecer los errores
                var errorLocators = new List<By> { alertError, fieldValidation };
                bool encontrado = false;

                // Intentamos esperar brevemente a que aparezca algún error (opcional, pero recomendado si el test va rápido)
                try
                {
                    // Esperamos a que aparezca al menos una alerta O un mensaje de validación
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                    wait.Until(d => d.FindElements(alertError).Count > 0 || d.FindElements(fieldValidation).Count > 0);
                }
                catch (WebDriverTimeoutException)
                {
                    // Si no aparece nada en 2 segundos, seguimos para comprobar si ya estaba ahí o no hay nada
                }

                foreach (var locator in errorLocators)
                {
                    var elementos = _driver.FindElements(locator);
                    foreach (var elemento in elementos)
                    {
                        // Verificamos que sea visible y contenga el texto
                        if (elemento.Displayed && elemento.Text.Contains(textoEsperado))
                        {
                            // _output.WriteLine($"Error encontrado en {locator}: {elemento.Text}"); // Debug
                            encontrado = true;
                            break;
                        }
                    }
                    if (encontrado) break;
                }

                return encontrado;
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Excepción buscando errores: {ex.Message}");
                return false;
            }
        }

        public bool ContieneDispositivo(string nombreDispositivo)
        {
            try
            {
                var filas = _driver.FindElements(tableRows);
                foreach (var fila in filas)
                {
                    if (fila.Text.Contains(nombreDispositivo)) return true;
                }
                return false;
            }
            catch (NoSuchElementException) { return false; }
        }
    }
}