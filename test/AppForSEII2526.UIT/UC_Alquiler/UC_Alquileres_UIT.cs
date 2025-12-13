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
        private const string dispMarca1 = "Samsung";
        private const string dispPrecio1 = "20,00 €";

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
        [InlineData("Galaxy", "Galaxy A54 5G 128GB", "Samsung", "20,00 €")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_1_FiltrarDispositivos(string filtroBusqueda, string nombreEsperado, string marcaEsperada, string precioEsperado)
        {
            // Arrange
            InitialStepsForAlquiler();

                    var expectedDispositivos = new List<string[]>
            {
                // Selenium concatena las columnas con espacios.
                // Basta con verificar las primeras columnas clave.
                new string[] { nombreEsperado, marcaEsperada }
            };

            // Act
            // Pasamos el filtro de búsqueda ("Galaxy")
            _selectPO.SearchDispositivos(filtroBusqueda, "20,00€", null, null);

            // Assert
            Assert.True(_selectPO.CheckListOfDispositivos(expectedDispositivos),
                $"Error: Se esperaba 1 fila que comenzara con '{nombreEsperado} {marcaEsperada}', pero la tabla no coincide.");
        }

        // CASO DE PRUEBA 2: Error en Fechas (UC_Alq_2)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_2_ErrorFechas()
        {
            // Arrange
            InitialStepsForAlquiler();

            DateTime from = DateTime.Today.AddDays(5);
            DateTime to = DateTime.Today.AddDays(2);
            string errorEsperado = "La fecha de fin debe ser posterior a la de inicio";

            // Act
            _selectPO.SearchDispositivos("", "", from, to);

            // Assert
            Assert.True(_selectPO.CheckMessageError(errorEsperado),
                $"El mensaje de error esperado '{errorEsperado}' no apareció.");
        }

        // CASO DE PRUEBA 3: Carrito vacío (UC_Alq_4 según tu Word)
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Alq_3_CarritoVacio()
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