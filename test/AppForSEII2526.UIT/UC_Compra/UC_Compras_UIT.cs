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
        public void CU1_1_FlujoBasico_CompraExitosa()
        {
            // --- 1. ARRANGE (Preparación) ---
            InitialStepsForCompra();

            // Datos de prueba: Qué móvil compramos y quién lo compra
            // NOTA: Asegúrate de que este móvil existe en tu base de datos de prueba
            string movilAComprar = "Oppo";
            string precioEsperado = "799,99 €"; // El precio exacto que sale en pantalla

            // Datos del cliente para el formulario
            string nombreCliente = "David";
            string apellidosCliente = "Gómez Fernández";
            string direccionCliente = "Paseo de la Castellana 100, Madrid";
            string metodoPago = "Efectivo";

            // --- 2. ACT (Ejecución) ---

            // A. Buscar y Añadir al Carrito (Usando tu PO de Selección)
            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movilAComprar);

            // B. Ir a la pantalla de pago
            // (Hacemos clic en el botón Tramitar Pedido)
            // Nota: Si este botón tarda en aparecer, _driver.FindElement esperará implícitamente
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            // C. Rellenar el formulario de compra (Usando tu PO de Crear Compra)
            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            crearCompraPO.EscribirNombre(nombreCliente);
            crearCompraPO.EscribirApellidos(apellidosCliente);
            crearCompraPO.EscribirDireccion(direccionCliente);
            crearCompraPO.SeleccionarPago(metodoPago);

            // D. Confirmar la compra
            crearCompraPO.ClickConfirmar();

            // --- 3. ASSERT (Verificación) ---

            // Instanciamos el PO de Detalle (el que acabamos de arreglar con XPath)
            var detallePO = new DetalleCompra_PO(_driver, _output);

            // Verificación 1: ¿Hemos llegado a la página de detalles?
            Assert.True(detallePO.EstamosEnPaginaDetalle(),
                "Error: No se ha redirigido a la página de DetalleCompra tras confirmar.");

            // Verificación 2: ¿El título contiene el nombre del usuario?
            // El título es: "¡Gracias por tu compra, Estudiante Aprobado!"
            string tituloFinal = detallePO.ObtenerTextoTitulo();
            Assert.Contains(nombreCliente, tituloFinal);
            Assert.Contains(apellidosCliente, tituloFinal);

            // Verificación 3: ¿La dirección es la correcta?
            string direccionFinal = detallePO.ObtenerDireccion();
            Assert.Equal(direccionCliente, direccionFinal);

            // Verificación 4: ¿El precio total es correcto?
            string precioFinal = detallePO.ObtenerPrecioTotal();
            // Usamos Contains por si hay símbolos de moneda o espacios extraños
            Assert.Contains(precioEsperado, precioFinal);

           
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
            // 2. Dejamos el Nombre vacío pero rellenamos lo demás
            crearCompraPO.EscribirNombre("");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos (Aquí saltará la validación manual del Razor, NO va al servidor)
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos el mensaje de validación manual
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // El mensaje debe coincidir con el que pusimos en el Razor:
            // if (string.IsNullOrWhiteSpace(CompraState.Compra.NombreUsuario)) mensajeError = "Por favor, introduce tu Nombre.";
            Assert.Contains("Por favor, introduce tu Nombre", mensajeError);
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
            // 2. Dejamos Apellidos vacíos
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("");
            crearCompraPO.EscribirDireccion("Calle Mayor 123, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Validamos el mensaje específico de apellidos
            Assert.Contains("Por favor, introduce tus Apellidos", mensajeError);
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
            // 2. Dejamos Dirección vacía
            crearCompraPO.EscribirNombre("Juan");
            crearCompraPO.EscribirApellidos("Pérez García");
            crearCompraPO.EscribirDireccion("");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // Validamos el mensaje específico de dirección
            Assert.Contains("Es obligatorio introducir una Dirección de entrega", mensajeError);
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_10_Usuario_No_Existe()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos datos (usamos "x" para simular datos rápidos pero "válidos" para el cliente)
            crearCompraPO.EscribirNombre("x");
            crearCompraPO.EscribirApellidos("x");
            crearCompraPO.EscribirDireccion("x");
            crearCompraPO.SeleccionarPago("Efectivo");

            // 3. Confirmamos
            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            // 4. Verificamos que aparece la alerta de error del servidor
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();

            _output.WriteLine($"Mensaje encontrado: {mensajeError}");

            // CORRECCIÓN: Usamos 'error' en minúscula para coincidir con "Ocurrió un error..."
            // O convertimos todo a minúsculas para evitar problemas futuros:
            Assert.Contains("error al procesar", mensajeError.ToLower());

            // Verificamos el código de estado
            Assert.Contains("400", mensajeError);
        }
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_11_Dispositivo_sin_stock()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // 1. Añadimos producto hasta agotar stock (11 veces)
            // Nota: Podrías usar un bucle for aquí para que quede más limpio, pero así funciona igual.
            string dispositivo = "iPhone 14 Pro 256";
            _selectPO.SearchDispositivos("iPhone", "");

            for (int i = 0; i < 11; i++)
            {
                _selectPO.AddDispositivoToCart(dispositivo);
            }

            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- ACT ---
            // 2. Rellenamos los campos correctamente
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

            // CORRECCIÓN: Usamos "error" en minúscula porque la frase es "Ocurrió un error..."
            Assert.Contains("error al procesar", mensajeError);

            // Verificamos el código 400
            Assert.Contains("400", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_12_Volver_Desde_CrearCompra_Mantiene_Carrito()
        {
            // --- 1. ARRANGE (Preparar el escenario) ---
            InitialStepsForCompra();

            // Elegimos un móvil para añadir al carrito
            string movilPrueba = "Oppo";

            // Lo buscamos y lo añadimos
            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movilPrueba);

            // Guardamos el precio total actual para compararlo luego (ej: "1.399,99 €")
            string precioAntesDeIrse = _selectPO.ObtenerPrecioTotal();

            // Navegamos hacia la pantalla de Crear Compra (Tramitar Pedido)
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            // Verificamos brevemente que hemos cambiado de pantalla (opcional, pero buena práctica)
            // Instanciamos el PO de Crear Compra solo para tener acceso al botón Volver
            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // --- 2. ACT (La acción principal: Pulsar Volver) ---

            // Hacemos clic en "Volver"
            crearCompraPO.ClickVolver();

            // --- 3. ASSERT (Verificaciones) ---

            // A. Verificar que hemos regresado a la URL de Selección
            // La URL debería contener "SelectDispositivosComprar" o la ruta base de compras
            Assert.Contains("SelectDispositivosComprar", _driver.Url);

            // B. Verificar que el carrito NO se ha vaciado
            // Comprobamos que el precio total sigue siendo el mismo que antes de irnos
            string precioAlVolver = _selectPO.ObtenerPrecioTotal();

            Assert.Equal(precioAntesDeIrse, precioAlVolver);

            // C. Verificar visualmente que el ítem sigue ahí (buscando el texto en el carrito)
            // Esto asume que el nombre del móvil es visible en la zona del carrito
            var cuerpoPagina = _driver.FindElement(By.TagName("body")).Text;
            Assert.Contains(movilPrueba, cuerpoPagina);

            
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_13_Nombre_Excede_Longitud()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            // Preparar carrito e ir al checkout
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // Generamos un nombre de 51 caracteres (el límite es 50)
            string nombreLargo = new string('a', 51);

            // --- ACT ---
            crearCompraPO.EscribirNombre(nombreLargo);
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 50 caracteres", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_14_Apellidos_Excede_Longitud()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // Generamos apellidos de 71 caracteres (el límite es 70)
            string apellidosLargos = new string('a', 71);

            // --- ACT ---
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos(apellidosLargos);
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 70 caracteres", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_15_Direccion_Excede_Longitud()
        {
            // --- ARRANGE ---
            InitialStepsForCompra();

            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart("iPhone 14 Pro 512GB");
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            // Generamos dirección de 101 caracteres (el límite es 100)
            string direccionLarga = new string('a', 101);

            // --- ACT ---
            crearCompraPO.EscribirNombre("Nombre Válido");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion(direccionLarga);
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            // --- ASSERT ---
            string mensajeError = crearCompraPO.ObtenerMensajeAlertaGeneral();
            _output.WriteLine($"Mensaje: {mensajeError}");

            Assert.Contains("máximo 100 caracteres", mensajeError);
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU1_examen()
        {
            
            InitialStepsForCompra();

            // Datos de prueba
            string movilNombre = "Oppo";       
            string movilColor = "iPhone";
            string movilNombre2 = "Galaxy";
            string nombreCompletoMovil2 = "iPhone 14 Pro 256GB"; 

            

            // Filtrar por NOMBRE y añadir al carrito
            _selectPO.SearchDispositivos(movilNombre, "");
            _selectPO.AddDispositivoToCart(movilNombre);

            // Filtrar por COLOR y añadir al carrito
            _selectPO.SearchDispositivos("", "Negro");
            
            _selectPO.AddDispositivoToCart(nombreCompletoMovil2);

            // Filtrar por NOMBRE y añadir al carrito
            _selectPO.SearchDispositivos(movilNombre2, "");
            _selectPO.AddDispositivoToCart(movilNombre2);

            // Eliminar el PRIMER dispositivo (El Oppo)
            _selectPO.RemoveDispositivoFromCart(movilNombre);

            Thread.Sleep(1000);
            // Ir a Crear Compra 
            _driver.FindElement(By.XPath("//button[contains(., 'Tramitar Pedido')]")).Click();

            var crearCompraPO = new CrearCompra_PO(_driver, _output);

            //RELLENAR EL FORMULARIO
            crearCompraPO.EscribirNombre("David");
            crearCompraPO.EscribirApellidos("Gómez Fernández");
            crearCompraPO.EscribirDireccion("Paseo de la Castellana 100, Madrid");
            crearCompraPO.SeleccionarPago("Efectivo");

            crearCompraPO.ClickConfirmar();

            

            // Comprobamos que llegamos a la página de detalle
            var detallePO = new DetalleCompra_PO(_driver, _output);
            Assert.True(detallePO.EstamosEnPaginaDetalle(), "La compra debería haberse completado correctamente tras el flujo complejo.");


            
        }
        

        









    }
}