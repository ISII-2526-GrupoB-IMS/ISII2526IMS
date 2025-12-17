using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Reviews
{
    public class DetalleReview_PO : PageObject
    {
        // --- Localizadores Basados en tu Interfaz ---

        // Banner de éxito superior
        private By bannerExito = By.XPath("//div[contains(@class, 'alert-success') or contains(., '¡Reseña Publicada con Éxito!')]");
        private By idSistema = By.XPath("//span[contains(text(), 'ID de Sistema')]");

        // Datos de la Cabecera (Sección Azul/Gris)
        private By lblAutor = By.XPath("//div[contains(text(), 'Autor:') or contains(., 'Autor:')]");
        private By lblUbicacion = By.XPath("//div[contains(text(), 'Ubicación:') or contains(., 'Ubicación:')]");
        private By lblFecha = By.XPath("//div[contains(text(), 'Fecha:') or contains(., 'Fecha:')]");
        private By lblEstadoSincro = By.XPath("//span[contains(text(), 'Sincronizado con Base de Datos')]");

        // Tabla de Dispositivos Valorados
        private By tableValorados = By.XPath("//table[contains(., 'Dispositivo / Modelo')]");
        private By rowsDispositivos = By.CssSelector("table tbody tr");

        // Botón de navegación
        private By btnVolver = By.XPath("//button[contains(., 'Volver al Catálogo')]");

        public DetalleReview_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // --- Métodos de Validación ---

        public bool VerificarExitoPublicacion()
        {
            try
            {
                WaitForBeingVisible(bannerExito);
                _output.WriteLine("Banner de éxito detectado correctamente.");
                return _driver.FindElement(bannerExito).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                _output.WriteLine("Error: No se mostró el banner de éxito de la reseña.");
                return false;
            }
        }

        public bool VerificarSincronizacion()
        {
            try
            {
                return _driver.FindElement(lblEstadoSincro).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool VerificarDatosAutor(string autorEsperado, string ubicacionEsperada)
        {
            try
            {
                string textoAutor = _driver.FindElement(lblAutor).Text;
                string textoUbi = _driver.FindElement(lblUbicacion).Text;

                _output.WriteLine($"Cabecera detectada -> Autor: {textoAutor}, Ubicación: {textoUbi}");

                return textoAutor.Contains(autorEsperado) && textoUbi.Contains(ubicacionEsperada);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error validando autor: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifica que un dispositivo aparezca en la tabla con su calificación y comentario.
        /// </summary>
        public bool VerificarDispositivoValorado(string nombreDispositivo, string calificacionEsperada, string comentarioEsperado)
        {
            try
            {
                WaitForBeingVisible(tableValorados);
                var filas = _driver.FindElements(rowsDispositivos);

                foreach (var fila in filas)
                {
                    string contenidoFila = fila.Text;
                    if (contenidoFila.Contains(nombreDispositivo))
                    {
                        bool califOk = contenidoFila.Contains(calificacionEsperada);
                        bool comentarioOk = contenidoFila.Contains(comentarioEsperado);

                        _output.WriteLine($"Fila encontrada para {nombreDispositivo}. Calificación OK: {califOk}, Comentario OK: {comentarioOk}");
                        return califOk && comentarioOk;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // --- Métodos de Acción ---

        public void ClickVolverAlCatalogo()
        {
            WaitForBeingClickable(btnVolver);
            _driver.FindElement(btnVolver).Click();
        }
    }
}

