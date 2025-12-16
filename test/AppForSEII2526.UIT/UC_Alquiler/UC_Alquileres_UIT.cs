using AppForSEII2526.UIT.Shared; // Para UC_UIT
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Alquileres
{
    public class UC_Alquileres_UIT : UC_UIT
    {
        private SelectDispositivosAlquiler_PO _selectPO;

        private const string dispNombre1 = "Galaxy A54";


        public UC_Alquileres_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectDispositivosAlquiler_PO(_driver, _output);
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
            // IMPORTANTE: Aquí usamos las variables que vienen del InlineData, no valores fijos.
            // Si filtroPrecio viene vacío (caso Oppo), buscará sin restricción de precio.
            _selectPO.SearchDispositivos(filtroNombre, filtroPrecio, null, null);

            // Assert
            Assert.True(_selectPO.CheckListOfDispositivos(expectedDispositivos),
                $"Error: La tabla no contiene la fila esperada: {nombreEsperado} | {marcaEsperada} | {precioEsperado}");
        }


        public static IEnumerable<object[]> TestCasesFor_ErrorInDates()
        {
            var allTests = new List<object[]>
            {
                // UC2_7: Fecha inicio Ayer (-1) -> Fin Mañana (+2)
                // Error esperado: "Las fechas de alquiler deben ser futuras."
                new object[] {
                    DateTime.Today.AddDays(2),
                    DateTime.Today.AddDays(3),
                    null    
                },
                new object[] {
                    DateTime.Today.AddDays(-1),
                    DateTime.Today.AddDays(2),
                    "Las fechas de alquiler deben ser futuras."
                },

                // UC2_8: Fecha inicio Antier (-2) -> Fin Ayer (-1)
                // Error esperado: "Las fechas de alquiler deben ser futuras."
                new object[] {
                    DateTime.Today.AddDays(-2),
                    DateTime.Today.AddDays(-1),
                    "Las fechas de alquiler deben ser futuras."
                },

                // UC2_9: Fecha inicio (+5) posterior a Fecha Fin (+2)
                // Error esperado: "La fecha de fin debe ser posterior a la de inicio."
                new object[] {
                    DateTime.Today.AddDays(5),
                    DateTime.Today.AddDays(2),
                    "La fecha de fin debe ser posterior a la de inicio."
                }
            };

            return allTests;
        }

        // CASO DE PRUEBA 2: Error en Fechas (UC_Alq_2)
        [Theory]
        [MemberData(nameof(TestCasesFor_ErrorInDates))]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_6_7_8_9_ErrorFechas(DateTime from, DateTime to, string errorEsperado)
        {
            // Arrange
            InitialStepsForAlquiler();

            // Act
            // Pasamos filtros vacíos y las fechas del caso de prueba
            _selectPO.SearchDispositivos("", "", from, to);

            // Assert
            Assert.True(_selectPO.CheckMessageError(errorEsperado),
                $"Error: Se esperaba el mensaje '{errorEsperado}' para el rango {from:dd/MM} - {to:dd/MM}, pero no apareció.");
        }

        // CASO DE PRUEBA 3: Carrito vacío (UC_Alq_4 según tu Word)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_10_11_CarritoVacio()
        {
            // Arrange
            InitialStepsForAlquiler();

            // Act
            // Añadimos y quitamos para dejarlo vacío explícitamente (o si ya entra vacío)
            _selectPO.SearchDispositivos(dispNombre1, "", null, null);
            _selectPO.AddDispositivoToCart(dispNombre1);
            _selectPO.RemoveDispositivoFromCart(dispNombre1);

            // Assert
            Assert.True(_selectPO.IsCrearReservaDisabledOrHidden(),
                "El botón de crear reserva debería estar oculto o deshabilitado si el carrito está vacío.");
        }
    }
}