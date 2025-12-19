using AppForSEII2526.UIT.Shared; // Para UC_UIT
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    public class UC_Alquileres_UIT : UC_UIT
    {
        private SelectDispositivosAlquiler_PO _selectPO;
        private CreateAlquiler_PO _createPO; // PO Añadido para las pruebas de Create
        private DetalleAlquiler_PO _detallePO;
        private const string dispNombre1 = "Galaxy A54";
        private const string dispPrecioDia = "44,00 €"; // Precio base para validación
        public UC_Alquileres_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectDispositivosAlquiler_PO(_driver, _output);
            _createPO = new CreateAlquiler_PO(_driver, _output); // Inicializamos el PO de Create
            _detallePO = new DetalleAlquiler_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            //Perform_login("cliente1@test.com", "Password123!");
        }

        private void InitialStepsForAlquiler()
        {
            Precondition_perform_login();
            _driver.Navigate().GoToUrl(_URI + "Alquileres/SelectDispositivosAlquiler");
        }

        // =====================================================================
        // PRUEBAS DE SELECT (UC_Alq_3 a UC_Alq_11)
        // =====================================================================

        [Theory]
        // Caso UC2_4: Filtrar por Título (Precio vacío) -> Espera Oppo
        [InlineData("Oppo Find X5", "", "Oppo Find X5 Pro 256GB", "Oppo", "35,00 €")]
        // Caso UC2_5: Filtrar por Precio Máx (Nombre vacío) -> Espera Galaxy
        [InlineData("", "20", "Galaxy A54 5G 128GB", "Samsung", "20,00 €")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_3_4_FiltrarDispositivos(string filtroNombre, string filtroPrecio, string nombreEsperado, string marcaEsperada, string precioEsperado)
        {
            // Arrange
            InitialStepsForAlquiler();

            var expectedDispositivos = new List<string[]>
            {
                // Incluimos Nombre, Marca y Precio esperado en la validación
                new string[] { nombreEsperado, marcaEsperada, precioEsperado }
            };

            // Act
            _selectPO.SearchDispositivos(filtroNombre, filtroPrecio, null, null);

            // Assert
            Assert.True(_selectPO.CheckListOfDispositivos(expectedDispositivos),
                $"Error: La tabla no contiene la fila esperada: {nombreEsperado} | {marcaEsperada} | {precioEsperado}");
        }

        public static IEnumerable<object[]> TestCasesFor_ErrorInDates()
        {
            var allTests = new List<object[]>
            {
                // Caso válido (Solo para control, no debería dar error)
                new object[] { DateTime.Today.AddDays(2), DateTime.Today.AddDays(3), null },
                
                // UC2_7: Fecha inicio Ayer (-1) -> Fin Mañana (+2)
                new object[] { DateTime.Today.AddDays(-1), DateTime.Today.AddDays(2), "Las fechas de alquiler deben ser futuras." },

                // UC2_8: Fecha inicio Antier (-2) -> Fin Ayer (-1)
                new object[] { DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1), "Las fechas de alquiler deben ser futuras." },

                // UC2_9: Fecha inicio (+5) posterior a Fecha Fin (+2)
                new object[] { DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), "La fecha de fin debe ser posterior a la de inicio." }
            };
            return allTests;
        }

        // CASO DE PRUEBA 2: Error en Fechas (UC_Alq_6, 7, 8, 9)
        [Theory]
        [MemberData(nameof(TestCasesFor_ErrorInDates))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_6_7_8_9_ErrorFechas(DateTime from, DateTime to, string errorEsperado)
        {
            // Arrange
            InitialStepsForAlquiler();

            // Act
            _selectPO.SearchDispositivos("", "", from, to);

            // Assert
            Assert.True(_selectPO.CheckMessageError(errorEsperado),
                $"Error: Se esperaba el mensaje '{errorEsperado}' para el rango {from:dd/MM} - {to:dd/MM}, pero no apareció.");
        }

        // CASO DE PRUEBA 3: Carrito vacío (UC_Alq_10_11)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_10_11_CarritoVacio()
        {
            // Arrange
            InitialStepsForAlquiler();

            // Act
            _selectPO.SearchDispositivos(dispNombre1, "", null, null);
            _selectPO.AddDispositivoToCart(dispNombre1);
            _selectPO.RemoveDispositivoFromCart(dispNombre1);

            // Assert
            Assert.True(_selectPO.IsCrearReservaDisabledOrHidden(),
                "El botón de crear reserva debería estar oculto o deshabilitado si el carrito está vacío.");
        }

        // =====================================================================
        // PRUEBAS DE CREATE (UC_Create_12 a UC_Create_18)
        // =====================================================================

        // Método auxiliar para llevar al test hasta la pantalla de Create
        private void Precondition_GoToCreatePage()
        {
            // 1. Navegar a Select
            _driver.Navigate().GoToUrl(_URI + "Alquileres/SelectDispositivosAlquiler");

            // 2. Buscar y Añadir un dispositivo (Usamos dispNombre1 definido arriba)
            _selectPO.SearchDispositivos(dispNombre1, "", null, null);
            _selectPO.AddDispositivoToCart(dispNombre1);

            // 3. Ir a la pantalla Create
            _selectPO.ClickCrearReserva();
        }

        public static IEnumerable<object[]> TestCasesFor_FormErrors()
        {
            var allTests = new List<object[]>
            {
                // UC2_12: Nombre vacío
                new object[] { "", "Pérez García", "Calle Mayor 123", "The NombreUsuario field is required." },
                
                // UC2_13: Apellido corto 
                new object[] { "Juan", "Gómez", "Calle Mayor 123", "The field ApellidosUsuario must be a string with a minimum length of 10"  },

                // UC2_14: Apellido largo 
                new object[] { "Juan", new string('a', 51), "Calle Univ 1", "maximum length of 50" },

                // UC2_15 : Dirección vacía
                new object[] { "Juan", "Pérez García", "", "The DireccionEntrega field is required." },

                // UC2_16 : Dirección muy corta
                new object[] { "Juan", "Pérez García", "Calle", "minimum length of 10" },

                // UC2_17 : Dirección sin 'Calle' o 'Carretera'
                new object[] { "Juan", "Pérez García", "Mayor 123, Madrid", "Error de validación: Error en la dirección de envío. Por favor,introduce una dirección válida incluyendo las palabras Calle o Carretera." },
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_FormErrors))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Create_12_13_14_15_16_17(string nombre, string apellidos, string direccion, string errorEsperado)
        {
            // Arrange
            Precondition_GoToCreatePage();

            // Act
            _createPO.RellenarFormulario(nombre, apellidos, direccion, "TarjetaCredito");
            _createPO.ClickAlquilar();

            // Assert
            Assert.True(_createPO.HayMensajeDeError(errorEsperado),
                $"No se encontró el mensaje de error esperado: '{errorEsperado}'");
        }

        // CASO UC2_18: Error de Disponibilidad (Backend)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Create_18()
        {
            // Arrange
            // Navegamos directamente con fechas conflictivas (según tu imagen UC2_17)
            _driver.Navigate().GoToUrl(_URI + "Alquileres/SelectDispositivosAlquiler");

            // Ponemos fechas conflictivas donde sabemos que no hay stock (según setup de prueba)
            DateTime conflictFrom = DateTime.Today.AddDays(2);
            DateTime conflictTo = DateTime.Today.AddDays(4);

            // Buscamos el dispositivo problemático (ej. "Galaxy S23 Ultra" según imagen)
            string dispConflictivo = "Galaxy S23 Ultra";
            _selectPO.SearchDispositivos(dispConflictivo, "", conflictFrom, conflictTo);
            _selectPO.AddDispositivoToCart(dispConflictivo);
            _selectPO.ClickCrearReserva();

            // Rellenamos datos válidos para que salte el error de backend al final
            _createPO.RellenarFormulario("Juan", "Pérez García", "Calle Mayor 1", "TarjetaCredito");

            // Act
            _createPO.ClickAlquilar();

            // Assert
            string errorEsperado = " Error! Device with name'Galaxy S23 Ultra 512GB' is not available for being rented from ";
            Assert.True(_createPO.HayMensajeDeError(errorEsperado),
                "Se esperaba un error de disponibilidad al intentar alquilar.");
        }

        // =====================================================================
        // PRUEBAS DE DETALLE (UC_Detalle_1 a UC_Detalle_3)
        // =====================================================================

        [Theory]
        [InlineData("TarjetaCredito", "Galaxy A54")] // UC2_1
        [InlineData("PayPal", "Galaxy A54")]         // UC2_2
        [InlineData("Efectivo", "Galaxy A54")]       // UC2_3
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Detalle_1_2_3(string metodoPago, string dispositivoEsperado)
        {
            // 1. ARRANGE: Navegar y Seleccionar
            _driver.Navigate().GoToUrl(_URI + "Alquileres/SelectDispositivosAlquiler");

            // Fechas válidas (Mañana a Pasado mañana = 1 día)
            DateTime fechaInicio = DateTime.Today.AddDays(1);
            DateTime fechaFin = DateTime.Today.AddDays(3);

            _selectPO.SearchDispositivos(dispositivoEsperado, "", fechaInicio, fechaFin);
            _selectPO.AddDispositivoToCart(dispositivoEsperado);
            _selectPO.ClickCrearReserva();

            // 2. ACT: Rellenar Formulario Create y Alquilar
            string nombreUser = "Juan";
            string apellidosUser = "Pérez García";
            string direccionUser = "Calle Mayor 123";

            _createPO.RellenarFormulario(nombreUser, apellidosUser, direccionUser, metodoPago);
            _createPO.ClickAlquilar();

            // 3. ASSERT: Validar página de Detalle

            // Calculamos precio esperado: 2 día * 22€ = 44,00 €
            string precioTotalEsperado = dispPrecioDia;

            Assert.True(_detallePO.VerificarDetallesCabecera(
                $"{nombreUser} {apellidosUser}",
                direccionUser,
                metodoPago,
                precioTotalEsperado),
                "Los datos de la cabecera del detalle (Nombre, Dirección, Pago o Precio) son incorrectos.");

            Assert.True(_detallePO.VerificarDispositivoEnTabla(dispositivoEsperado),
                $"El dispositivo '{dispositivoEsperado}' no aparece en la tabla de detalles.");
        }


        // =====================================================================
        // PRUEBAS EXAMEN
        // =====================================================================

      
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_Examen()
        {
            InitialStepsForAlquiler();
            string item1_Nombre = "Galaxy A54 5G 256GB";    

            string item2_Nombre = "iPhone 13 256GB";

            Thread.Sleep(1000);


            _selectPO.AddDispositivoToCart(item1_Nombre);

            _selectPO.SearchDispositivos("iPhone 13", "", null, null);

            _selectPO.AddDispositivoToCart(item2_Nombre);
            Thread.Sleep(1000);


            _selectPO.ClickCrearReserva();


            Assert.True(_createPO.ContieneDispositivo(item2_Nombre), "El Item 2 debería estar en el resumen.");




            _createPO.RellenarFormulario("Juan", "Pérez García", "Calle Industria 55", "PayPal");
            _createPO.ClickAlquilar();


            Thread.Sleep(1000);

            Assert.True(_detallePO.VerificarDispositivoEnTabla(item2_Nombre), "El Item 2 alquilado debería estar en el detalle final.");
        }
    }

}
