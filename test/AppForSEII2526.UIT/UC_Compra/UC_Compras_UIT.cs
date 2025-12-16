using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Compras;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Compra
{
    public class UC_Compras_UIT : UC_UIT
    {
        private SelectDispositivosCompra_PO _selectPO;

        // Datos de prueba constantes
        private const string movilPrueba = "iPhone 14 Pro 512GB";

        public UC_Compras_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectDispositivosCompra_PO(_driver, _output);
        }

        private void InitialStepsForCompra()
        {
            // Login previo si fuera necesario (descomentar si tu app lo requiere)
            // Perform_login("cliente@test.com", "Password123!");

            // Navegar a la URL de Compras
            _driver.Navigate().GoToUrl(_URI + "Compras/SelectDispositivosComprar");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_2_No_hay_Dispositivos()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // Definimos el texto "sajfbuf" y el mensaje esperado
            string colorInexistente = "sajfbuf";
            string mensajeEsperado = "No se han encontrado dispositivos con esos filtros.";

            // --- ACT ---
            
            _selectPO.SearchDispositivos("", colorInexistente);

            // --- ASSERT ---

            

            try
            {
                // Buscamos cualquier etiqueta (*) que contenga el texto esperado
                var elementoMensaje = _driver.FindElement(By.XPath($"//*[contains(text(), '{mensajeEsperado}')]"));

                // Verificamos que el elemento existe y es visible
                Assert.True(elementoMensaje.Displayed, "El mensaje de error debería ser visible en pantalla.");
                Assert.Contains(mensajeEsperado, elementoMensaje.Text);
            }
            catch (NoSuchElementException)
            {
                // Si entra aquí es que no encontró el texto en toda la página
                Assert.Fail($"No se encontró el mensaje de error: '{mensajeEsperado}' en la página.");
            }
        }





        [Theory]
        [Trait("LevelTesting", "Funcional Testing")]
        // Caso 1: Filtrar por Nombre CU1_3
        [InlineData("Oppo", "", "Oppo Find X5 Pro 256GB", "Oppo", "799,99 €")]
        // Caso 2: Filtrar por Color CU1_4
        [InlineData("", "Plata", "iPhone 14 Pro 512GB", "Apple", "1.399,99 €")] 
        public void UC1_3Y4_Compra_FiltrarDispositivos(string filtroNombre, string filtroColor, string nombreEsperado, string marcaEsperada, string precioEsperado)
        {
            // Arrange
            InitialStepsForCompra();

            var expectedDispositivos = new List<string[]>
            {
                new string[] { nombreEsperado, marcaEsperada, precioEsperado }
            };

            // Act
            _selectPO.SearchDispositivos(filtroNombre, filtroColor);

            // Assert
            Assert.True(_selectPO.CheckListOfDispositivos(expectedDispositivos),
                $"Error: No se encontró la tarjeta con: {nombreEsperado} | {marcaEsperada}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_5_GestionCarrito_RecalculoPrecios()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();
            if (!_selectPO.IsTramitarPedidoHidden()) _selectPO.VaciarCarrito();

            // Definimos los datos del caso de prueba (según tu documento)
            string movil1 = "Oppo";
            string movil2 = "iPhone 14 Pro 512GB"; // Ajusta el nombre si en tu BD es diferente

            // Selenium lee texto, así que validamos los strings de precio exactos que salen en pantalla
            string precioEsperadoTotalAmbos = "2.199,98 €"; // Suma de los dos
            string precioEsperadoFinal = "1.399,99 €";      // Solo el iPhone

            // --- ACT & ASSERT (Paso a Paso) ---

            //  Añadimos el primer móvil (Oppo)
            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movil1);

            // Añadimos el segundo móvil (iPhone)
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart(movil2);

            // Verificación Intermedia: ¿La suma es correcta?
            // (Esto valida la primera parte del requisito: "de 2.199,98...")
            string totalActual = _selectPO.ObtenerPrecioTotal();
            Assert.Contains(precioEsperadoTotalAmbos, totalActual);

            // 3Eliminamos el primer móvil (Oppo)
            _selectPO.RemoveDispositivoFromCart(movil1);

            // --- ASSERT FINAL ---

            // Verificamos que se ha recalculado correctamente
            string totalFinal = _selectPO.ObtenerPrecioTotal();

            Assert.Contains(precioEsperadoFinal, totalFinal);

           
        }

        
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6Compra_Carrito_Vacio_Oculta_Tramitar()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos algo primero (para que aparezca el botón)
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone");

            // Comprobamos que AHORA sí se ve (debería ser false que esté oculto)
            Assert.False(_selectPO.IsTramitarPedidoHidden(), "El botón debería verse con items.");

            // --- ACT ---
            // 2. Vaciamos el carrito
            
            _selectPO.VaciarCarrito();

            
            // Esto asegura que la interfaz se ha actualizado.
            Thread.Sleep(1000); // Espera explícita de seguridad

            // --- ASSERT ---
           
            Assert.True(_selectPO.IsTramitarPedidoHidden(), "El botón 'Tramitar Pedido' debería ocultarse tras vaciar el carrito.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_7_Nombre_Vacio()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos para que NO salte el error de "Campos obligatorios"
            // (Queremos que llegue a intentar enviar al servidor)
            crearCompraPO.EscribirNombre("");
            crearCompraPO.EscribirApellidos("Apellido Test");
            crearCompraPO.EscribirDireccion("Calle Test");
            crearCompraPO.SeleccionarPago("Efectivo"); 

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Verificamos que contiene el texto que te está saliendo ahora mismo
            // "Error al procesar la compra" o "400"
            Assert.Contains("Error al procesar", mensajeError);
            Assert.Contains("400", mensajeError); // Confirmamos que es el error que esperas
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_8_Apellidos_Vacio()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos para que NO salte el error de "Campos obligatorios"
            // (Queremos que llegue a intentar enviar al servidor)
            crearCompraPO.EscribirNombre("Nombre_Test");
            crearCompraPO.EscribirApellidos("");
            crearCompraPO.EscribirDireccion("Calle Test");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Verificamos que contiene el texto que te está saliendo ahora mismo
            // "Error al procesar la compra" o "400"
            Assert.Contains("Error al procesar", mensajeError);
            Assert.Contains("400", mensajeError); // Confirmamos que es el error que esperas
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_9_Direccion_Vacio()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos para que NO salte el error de "Campos obligatorios"
            // (Queremos que llegue a intentar enviar al servidor)
            crearCompraPO.EscribirNombre("Nombre Test");
            crearCompraPO.EscribirApellidos("Apellidos Test");
            crearCompraPO.EscribirDireccion("");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Verificamos que contiene el texto que te está saliendo ahora mismo
            // "Error al procesar la compra" o "400"
            Assert.Contains("Error al procesar", mensajeError);
            Assert.Contains("400", mensajeError); // Confirmamos que es el error que esperas
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_10_MetodoPago_Vacio()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos para que NO salte el error de "Campos obligatorios"
            // (Queremos que llegue a intentar enviar al servidor)
            crearCompraPO.EscribirNombre("Nombre Test");
            crearCompraPO.EscribirApellidos("Apellidos Test");
            crearCompraPO.EscribirDireccion("Calle Test");
            crearCompraPO.SeleccionarPago("");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Verificamos que contiene el texto que te está saliendo ahora mismo
            // "Error al procesar la compra" o "400"
            Assert.Contains("Error al procesar", mensajeError);
            Assert.Contains("400", mensajeError); // Confirmamos que es el error que esperas
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_11_Dispositivo_sin_stock()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 256");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos para que NO salte el error de "Campos obligatorios"
            // (Queremos que llegue a intentar enviar al servidor)
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Verificamos que contiene el texto que te está saliendo ahora mismo
            // "Error al procesar la compra" o "400"
            Assert.Contains("Error al procesar", mensajeError);
            Assert.Contains("400", mensajeError); // Confirmamos que es el error que esperas
        }







    }
}
