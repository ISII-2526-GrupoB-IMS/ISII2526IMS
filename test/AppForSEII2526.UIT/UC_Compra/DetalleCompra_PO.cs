using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class DetalleCompra_PO : PageObject
    {
        
        private By labelNameSurname = By.Id("NameSurname");
        private By labelAddress = By.Id("DeliveryAddress");
        private By fechaCompra = By.Id("FechaCompra");
        private By labelTotalPrice = By.Id("TotalPrice");
        private By tableMovies = By.Id("RentedMovies");


        
        public DetalleCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        

        public bool VerificarDetallesCabecera(string nombreCompleto, string direccion, string fecha, string precioTotal)
        {
            try
            {
                // Esperamos a que cargue la página de detalle
                WaitForBeingVisible(labelNameSurname);

                string actualName = _driver.FindElement(labelNameSurname).Text;
                string actualAddress = _driver.FindElement(labelAddress).Text;
                string actualFecha = _driver.FindElement(fechaCompra).Text;
                string actualPrice = _driver.FindElement(labelTotalPrice).Text;

                _output.WriteLine($"Detalle encontrado -> Nombre: {actualName}, Direccion: {actualAddress},Precio: {actualPrice}, Fecha: {actualFecha}");

                // Validamos
                bool checkName = actualName.Contains(nombreCompleto);
                bool checkAddr = actualAddress.Contains(direccion);
                bool checkPrice = actualPrice.Contains(precioTotal);
                bool checkFecha = actualFecha.Contains(fecha);

                _output.WriteLine($"DATOS EN LA WEB: {actualName} | {actualAddress} | {actualFecha} | {actualPrice}");
                _output.WriteLine($"DATOS ESPERADOS: {nombreCompleto} | {direccion} | {fecha} | {precioTotal}");

                return checkName && checkAddr && checkFecha && checkPrice;
            }
            catch (Exception ex)
            {
               _output.WriteLine($"Error verificando cabecera: {ex.Message}");
                return false;
            }
        }

        public bool CheckListOfDispositivos(List<string[]> expectedData)
        {
            WaitForBeingVisible(tableMovies);

            CultureInfo culturaES = new CultureInfo("es-ES");

            var filas = _driver.FindElements(By.CssSelector("#RentedMovies tbody tr"));

            if (filas.Count == 0 && expectedData.Count > 0) return false;

            foreach (var expected in expectedData)
            {
                bool found = false;

                string expectedNombre = expected[0];
                string expectedMarca = expected[1];
                string expectedColor = expected[2];
                string expectedPrecio = Math.Round(
                    decimal.Parse(expected[3], culturaES), 2)
                    .ToString("F2", culturaES);

                foreach (var fila in filas)
                {
                    var columnas = fila.FindElements(By.TagName("td"));

                    string actualNombre = columnas[0].Text.Trim();
                    string actualMarca = columnas[1].Text.Trim();
                    string actualColor = columnas[2].Text.Trim();

                    string precioTexto = columnas[3].Text.Replace("€", "").Trim();

                    string actualPrecio = Math.Round(
                        decimal.Parse(precioTexto, culturaES), 2)
                        .ToString("F2", culturaES);

                    _output.WriteLine($"DATOS EN LA WEB: {actualNombre} | {actualMarca} | {actualColor} | {actualPrecio}");
                    _output.WriteLine($"DATOS ESPERADOS: {expectedNombre} | {expectedMarca} | {expectedColor} | {expectedPrecio}");

                    if (actualNombre.Contains(expectedNombre, StringComparison.OrdinalIgnoreCase) &&
                        actualMarca.Contains(expectedMarca, StringComparison.OrdinalIgnoreCase) &&
                        actualColor.Contains(expectedColor, StringComparison.OrdinalIgnoreCase) &&
                        actualPrecio.Contains(expectedPrecio))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _output.WriteLine($"No se encontró FILA para: {expectedNombre} | {expectedMarca} | {expectedColor} | {expectedPrecio}");
                    return false;
                }
            }

            return true;
        }





        public bool CheckListOfDispositivos1(string nombre, string marca, string color, string precio)
        {
            WaitForBeingVisible(tableMovies);

            CultureInfo culturaES = new CultureInfo("es-ES");

            var filas = _driver.FindElements(By.CssSelector("#RentedMovies tbody tr"));

            // 👉 Normalizamos el precio esperado a 2 decimales (STRING)
            decimal precioDecimalEsperado = Math.Round(decimal.Parse(precio, culturaES), 2);
            string precioEsperado = precioDecimalEsperado.ToString("F2", culturaES);

            foreach (var fila in filas)
            {
                var columnas = fila.FindElements(By.TagName("td"));

                string nombreTabla = columnas[0].Text.Trim();
                string marcaTabla = columnas[1].Text.Trim();
                string colorTabla = columnas[2].Text.Trim();
                string precioTexto = columnas[3].Text.Replace("€", "").Trim(); //quita €

                // Redondeamos  2 decimales
                decimal precioDecimalTabla = Math.Round(decimal.Parse(precioTexto, culturaES), 2);
                string precioTabla = precioDecimalTabla.ToString("F2", culturaES);

                _output.WriteLine($"DATOS EN LA WEB: {nombreTabla} | {marcaTabla} | {colorTabla} | {precioTabla}");
                _output.WriteLine($"DATOS ESPERADOS: {nombre} | {marca} | {color} | {precio}");

                if (nombreTabla == nombre &&
                    marcaTabla == marca &&
                    colorTabla == color &&
                    precioTabla == precioEsperado)
                {
                    return true;
                }
            }

            return false;
        }

        
        public bool VerificarDispositivoEnTabla(string nombreDispositivo)
        {
            try
            {
                WaitForBeingVisible(tableMovies);
                
                var filas = _driver.FindElements(By.CssSelector("#RentedMovies tbody tr"));

                foreach (var fila in filas)
                {
                    if (fila.Text.Contains(nombreDispositivo)) return true;
                }
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

}
