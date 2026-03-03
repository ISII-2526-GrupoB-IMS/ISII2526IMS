using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Alquileres;
using AppForSEII2526.UIT.UC_Compras;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
             

             Assert.True(_selectPO.CheckMessageErrorNotAvaibleDispositivos(mensajeEsperado), "El mensaje de error debería ser visible en pantalla.");
 
            
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
            Assert.True(_selectPO.CheckPrecioTotal(precioEsperadoFinal));

        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_6Compra_Carrito_Vacio_Oculta_Tramitar()
        {
            //  ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            
            _selectPO.AddDispositivoToCart("iPhone");


            // ACT 
           
            _selectPO.VaciarCarrito();


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
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string mensajeError = "Por favor, introduce tu Nombre.";

            // ACT 

            crearCompraPO.EscribirNombre("");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");
            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_8_Apellidos_Vacio()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string mensajeError = "Por favor, introduce tus Apellidos.";

            // ACT 
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_9_Direccion_Vacio()
        {
            // ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string mensajeError = "Es obligatorio introducir una Dirección de entrega.";

            // ACT 
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_10_Usuario_No_Existe()
        {
            //  ARRANGE 
            InitialStepsForCompra();

            
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            string mensajeEsperado = "Atención: Ocurrió un error al procesar la compra: Bad Request Status: 400 Response:";
            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            //  ACT 
            crearCompraPO.EscribirNombre("x");
            crearCompraPO.EscribirApellidos("x");
            crearCompraPO.EscribirDireccion("x");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeEsperado), "El mensaje de error debería ser visible en pantalla.");

        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_11_Dispositivo_sin_stock()
        {
            // ARRANGE
            InitialStepsForCompra();

            
            string dispositivo = "iPhone 14 Pro 256";
            _selectPO.SearchDispositivos("iPhone", "");

            for (int i = 0; i < 15; i++)
            {
                _selectPO.AddDispositivoToCart(dispositivo);
            }

            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            string mensajeEsperado = "Atención: Ocurrió un error al procesar la compra: Bad Request Status: 400 Response:";

            // ACT 
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeEsperado), "El mensaje de error debería ser visible en pantalla.");



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

            string precioAntesDeIrse = "799,99 €";

            _selectPO.TramitarPedido();
           

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            //  ACT 

            crearCompraPO.ClickVolver();


            // ASSERT 


            Assert.True(_selectPO.CheckPrecioTotal(precioAntesDeIrse));


            
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_13_Nombre_Excede_Longitud()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string nombreLargo = new string('a', 51);
            string mensajeError = "El Nombre es demasiado largo (máximo 50 caracteres).";

            //ACT 
            crearCompraPO.EscribirNombre(nombreLargo);
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_14_Apellidos_Excede_Longitud()
        {
            // ARRANGE 
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            string apellidosLargos = new string('a', 71);
            string mensajeError = "Los Apellidos son demasiado largos (máximo 70 caracteres).";

            //  ACT 
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos(apellidosLargos);
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_15_Direccion_Excede_Longitud()
        {
            //  ARRANGE
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _selectPO.TramitarPedido();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            string direccionLarga = new string('a', 101);
            string mensajeError = "La Dirección es demasiado larga (máximo 100 caracteres).";

            // ACT 
            crearCompraPO.EscribirNombre("Nombre Válido");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion(direccionLarga);
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // ASSERT 
            Assert.True(crearCompraPO.CheckMessageErrorNotAvaibleMovies(mensajeError), "El mensaje de error debería ser visible en pantalla.");
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_1_Flujo_Basico() {


            //  ARRANGE 
            InitialStepsForCompra();
            string movil1 = "Oppo";
            string movil2 = "Iphone";

            string nombreEsperado = "Oppo Find X5";
            string marcaEsperada = "Oppo";
            string colorEsperado = "Azul";
            string precioEsperado = "799,99";
            string cantidadEsperada = "1";
            string descripcionEsperada = "Compra Web";

            string nombreEsperado2 = "iPhone 14 Pro";
            string marcaEsperada2 = "Apple";
            string colorEsperado2 = "Negro";
            string precioEsperado2 = "1.199,99";
            string cantidadEsperada2 = "1";
            string descripcionEsperada2 = "Compra Web";

            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movil1);
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone");
            _selectPO.TramitarPedido();


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

            string precioTotalEsperado = "1.999,98 €";
            string fechaEsperada = DateTime.Now.ToString("dd/MM/yyyy");

            Assert.True(_detallePO.VerificarDetallesCabecera(
                $"{nombreUser} {apellidosUser}",
                direccionUser,
                fechaEsperada,
                precioTotalEsperado),
                "Los datos de la cabecera del detalle (Nombre, Dirección, Pago o Precio) son incorrectos.");

            List<string[]> dispositivosEsperados = new List<string[]>
            {
                new string[] { nombreEsperado, marcaEsperada, colorEsperado, precioEsperado,cantidadEsperada,descripcionEsperada },
                new string[] { nombreEsperado2, marcaEsperada2, colorEsperado2, precioEsperado2,cantidadEsperada2,descripcionEsperada2 }
            };

            Assert.True(
                _detallePO.CheckListOfDispositivos(dispositivosEsperados),
                $"El dispositivo '{nombreEsperado}' no aparece en la tabla de detalles."
            );

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_16_Informacion_Carrito()
        {


            //  ARRANGE 
            InitialStepsForCompra();
            string movil1 = "Oppo";
            string movil2 = "Iphone";

            string nombreEsperado = "Oppo Find X5";
            string marcaEsperada = "Oppo";
            string colorEsperado = "Azul";
            string precioEsperado = "799,99";
            string cantidadEsperada = "1";
            string descripcionEsperada = "Compra Web";

            string nombreEsperado2 = "iPhone 14 Pro";
            string marcaEsperada2 = "Apple";
            string colorEsperado2 = "Negro";
            string precioEsperado2 = "1.199,99";
            string cantidadEsperada2 = "1";
            string descripcionEsperada2 = "Compra Web";

            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movil1);
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone");
            _selectPO.TramitarPedido();


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

           

            List<string[]> dispositivosEsperados = new List<string[]>
            {
                new string[] { nombreEsperado, colorEsperado, precioEsperado},
                new string[] { nombreEsperado2, colorEsperado2, precioEsperado2}
            };

            Assert.True(
                crearCompraPO.CheckListOfDispositivosEnCarrito(dispositivosEsperados)
            );



        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void Examen()
        {


            //  ARRANGE 
            InitialStepsForCompra();
            string nombre1 = "iPhone 14";

            string nombre2 = "Galaxy";
            string color2 = "Verde";

            string nombre3 = "iPhone 13";
            string marca = "Apple";
            string color = "Azul";
            string precioEsperado = "799,99";
            string cantidadEsperada = "1";
            string descripcionEsperada = "Compra Web";

            var crearCompraPO = new CrearCompra_PO(_driver, _output);
            var _detallePO = new DetalleCompra_PO(_driver, _output);



            string nombreUser = "Juan";
            string apellidosUser = "Pérez García";
            string direccionUser = "Calle Mayor 123";

            //  ACT 

            _selectPO.SearchDispositivos(nombre1, "");
            _selectPO.AddDispositivoToCart(nombre1);

            _selectPO.SearchDispositivos("", color2);
            _selectPO.AddDispositivoToCart(nombre2);

            _selectPO.SearchDispositivos(nombre3, "");
            _selectPO.AddDispositivoToCart(nombre3);

            _selectPO.RemoveDispositivoFromCart(nombre1);
            _selectPO.RemoveDispositivoFromCart(nombre2);

            _selectPO.TramitarPedido();


            crearCompraPO.EscribirNombre(nombreUser);
            crearCompraPO.EscribirApellidos(apellidosUser);
            crearCompraPO.EscribirDireccion(direccionUser);
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            string precioTotalEsperado = "799,99 €";
            string fechaEsperada = DateTime.Now.ToString("dd/MM/yyyy");

            Assert.True(_detallePO.VerificarDetallesCabecera(
               $"{nombreUser} {apellidosUser}",
               direccionUser,
               fechaEsperada,
               precioTotalEsperado),
               "Los datos de la cabecera del detalle (Nombre, Dirección, Pago o Precio) son incorrectos.");

            List<string[]> dispositivosEsperados = new List<string[]>
            {
                new string[] { nombre3, marca, color, precioEsperado,cantidadEsperada,descripcionEsperada }
            };

            Assert.True(
                _detallePO.CheckListOfDispositivos(dispositivosEsperados),
                $"El dispositivo '{nombre3}' no aparece en la tabla de detalles."
            );





        }













    }
}