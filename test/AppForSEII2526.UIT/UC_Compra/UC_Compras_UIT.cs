using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Alquileres;
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


        public UC_Compras_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectDispositivosCompra_PO(_driver, _output);
            
        }

        private void InitialStepsForCompra()
        {
            
            _driver.Navigate().GoToUrl(_URI + "Compras/SelectDispositivosComprar");
        }

        // PRUEBAS DEL SELECT DISPOSITIVOS COMPRAR

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_2_No_hay_Dispositivos()
        {
            // ARRANGE
            InitialStepsForCompra();

            // Definimos el texto "sajfbuf" y el mensaje esperado
            string colorInexistente = "sajfbuf";
            string mensajeEsperado = "No se han encontrado dispositivos con esos filtros.";

            // ACT

            _selectPO.SearchDispositivos("", colorInexistente);

            // ASSERT 
             

             Assert.True(_selectPO.CheckMessageErrorNotAvaibleMovies(mensajeEsperado), "El mensaje de error debería ser visible en pantalla.");
 
            
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
            
            // ARRANGE
            InitialStepsForCompra();
          
            string movil1 = "Oppo";
            string movil2 = "iPhone 14 Pro 512GB"; 

            string precioEsperadoTotalAmbos = "2.199,98 €"; 
            string precioEsperadoFinal = "1.399,99 €";      

            // ACT 

            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movil1);

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart(movil2);

            _selectPO.RemoveDispositivoFromCart(movil1);

            // ASSERT 

            string totalFinal = _selectPO.ObtenerPrecioTotal();

            Assert.Equal(precioEsperadoFinal, totalFinal);

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6Compra_Carrito_Vacio_Oculta_Tramitar()
        {
            //  ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            Thread.Sleep(4000);
            _selectPO.AddDispositivoToCart("iPhone");


            // ACT 
           
            _selectPO.VaciarCarrito();

            Thread.Sleep(4000); 

            // ASSERT 

            Assert.True(_selectPO.IsTramitarPedidoHidden(), "El botón 'Tramitar Pedido' debería ocultarse tras vaciar el carrito.");
        }


        //PRUEBAS DEL CREAR COMPRA

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_7_Nombre_Vacio()
        {
            //  ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // ACT 
            
            crearCompraPO.EscribirNombre("");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");
            crearCompraPO.ClickConfirmar();

            // ASSERT 
            
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            Assert.Contains("Por favor, introduce tu Nombre", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_8_Apellidos_Vacio()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // ACT 
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            //  ASSERT
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            Assert.Contains("Por favor, introduce tus Apellidos", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_9_Direccion_Vacio()
        {
            // ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // ACT 
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            //ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            Assert.Contains("Es obligatorio introducir una Dirección de entrega", mensajeError);
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_10_Usuario_No_Existe()
        {
            //  ARRANGE 
            InitialStepsForCompra();

            
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            //  ACT 
            crearCompraPO.EscribirNombre("x");
            crearCompraPO.EscribirApellidos("x");
            crearCompraPO.EscribirDireccion("x");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            Assert.Contains("error al procesar", mensajeError.ToLower());

            Assert.Contains("400", mensajeError);
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_11_Dispositivo_sin_stock()
        {
            // ARRANGE
            InitialStepsForCompra();

            
            string dispositivo = "iPhone 14 Pro 256";
            _selectPO.SearchDispositivos("iPhone", "");

            for (int i = 0; i < 11; i++)
            {
                _selectPO.AddDispositivoToCart(dispositivo);
            }

            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // ACT 
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            Assert.Contains("error al procesar", mensajeError);
           
            Assert.Contains("400", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_12_Volver_Desde_CrearCompra_Mantiene_Carrito()
        {
            // ARRANGE 
            InitialStepsForCompra();

            string movilPrueba = "Oppo";

            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movilPrueba);

            string precioAntesDeIrse = _selectPO.ObtenerPrecioTotal();

       
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            //  ACT 

            crearCompraPO.ClickVolver();

            // ASSERT 

            string precioAlVolver = _selectPO.ObtenerPrecioTotal();

            Assert.Equal(precioAntesDeIrse, precioAlVolver);


            
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_13_Nombre_Excede_Longitud()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string nombreLargo = new string('a', 51);

            //ACT 
            crearCompraPO.EscribirNombre(nombreLargo);
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 50 caracteres", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_14_Apellidos_Excede_Longitud()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string apellidosLargos = new string('a', 71);

            //  ACT 
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos(apellidosLargos);
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            //  ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 70 caracteres", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_15_Direccion_Excede_Longitud()
        {
            //  ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            string direccionLarga = new string('a', 101);

            // ACT 
            crearCompraPO.EscribirNombre("Nombre Válido");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion(direccionLarga);
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 100 caracteres", mensajeError);
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_1_Flujo_Basico() {


            //  ARRANGE 
            InitialStepsForCompra();
            string movil1 = "Real";
            _selectPO.SearchDispositivos("Real", "");
            _selectPO.AddDispositivoToCart(movil1);
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();


            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            var _detallePO = new DetalleCompra_PO(_driver, _output);

            string nombreUser = "Juan";
            string apellidosUser = "Pérez García";
            string direccionUser = "Calle Mayor 123";

            //  ACT 
            crearCompraPO.EscribirNombre(nombreUser);
            crearCompraPO.EscribirApellidos(apellidosUser);
            crearCompraPO.EscribirDireccion(direccionUser);
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            string precioTotalEsperado = "649,99 €";
            string fechaEsperada = DateTime.Now.ToString("dd/MM/yyyy");

            Assert.True(_detallePO.VerificarDetallesCabecera(
                $"{nombreUser} {apellidosUser}",
                direccionUser,
                fechaEsperada,
                precioTotalEsperado),
                "Los datos de la cabecera del detalle (Nombre, Dirección, Pago o Precio) son incorrectos.");

            Assert.True(_detallePO.VerificarDispositivoEnTabla(movil1),
               $"El dispositivo '{movil1}' no aparece en la tabla de detalles.");


        }













    }
}