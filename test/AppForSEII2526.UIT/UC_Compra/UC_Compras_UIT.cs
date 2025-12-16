using AppForSEII2526.UIT.Shared;
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





        [Theory]
        [Trait("LevelTesting", "Funcional Testing")]
        // Caso 1: Filtrar por Nombre CU1_3
        [InlineData("Oppo", "", "Oppo Find X5 Pro 256GB", "Oppo", "799,99 €")]
        // Caso 2: Filtrar por Color CU1_4
        [InlineData("", "Plata", "iPhone 14 Pro 512GB", "Apple", "1.399,99 €")] 
        public void UC_Compra_FiltrarDispositivos(string filtroNombre, string filtroColor, string nombreEsperado, string marcaEsperada, string precioEsperado)
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

            // 1. Añadimos el primer móvil (Oppo)
            _selectPO.SearchDispositivos("Oppo", "");
            _selectPO.AddDispositivoToCart(movil1);

            // 2. Añadimos el segundo móvil (iPhone)
            _selectPO.SearchDispositivos("iPhone", "");
            _selectPO.AddDispositivoToCart(movil2);

            // Verificación Intermedia: ¿La suma es correcta?
            // (Esto valida la primera parte del requisito: "de 2.199,98...")
            string totalActual = _selectPO.ObtenerPrecioTotal();
            Assert.Contains(precioEsperadoTotalAmbos, totalActual);

            // 3. Eliminamos el primer móvil (Oppo)
            _selectPO.RemoveDispositivoFromCart(movil1);

            // --- ASSERT FINAL ---

            // 4. Verificamos que se ha recalculado correctamente
            string totalFinal = _selectPO.ObtenerPrecioTotal();

            Assert.Contains(precioEsperadoFinal, totalFinal);

           
        }

        //CU1_6
        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_Compra_Carrito_Vacio_Oculta_Tramitar()
        {
            // Arrange
            InitialStepsForCompra();

            // Act & Assert 1: Al entrar, el carrito debería estar vacío (botón oculto)
            // Nota: Asumimos que es una sesión nueva o limpia
            bool botonOcultoAlInicio = _selectPO.IsTramitarPedidoHidden();

            // Si por alguna razón hay cosas, lo vaciamos
            if (!botonOcultoAlInicio)
            {
                _selectPO.VaciarCarrito();
                botonOcultoAlInicio = _selectPO.IsTramitarPedidoHidden();
            }

            Assert.True(botonOcultoAlInicio, "El botón 'Tramitar Pedido' debería estar oculto con el carrito vacío.");

            // Act 2: Añadimos un producto
            // Primero buscamos sin filtros para asegurar que sale algo
            _selectPO.SearchDispositivos("", "");
            _selectPO.AddDispositivoToCart(movilPrueba); 

            // Assert 2: El botón debe aparecer
            Assert.False(_selectPO.IsTramitarPedidoHidden(), "El botón 'Tramitar Pedido' debería aparecer tras añadir un producto.");

            // Act 3: Vaciamos el carrito
            _selectPO.VaciarCarrito();

            // Assert 3: El botón debe desaparecer de nuevo
            Assert.True(_selectPO.IsTramitarPedidoHidden(), "El botón 'Tramitar Pedido' debería ocultarse tras vaciar el carrito.");
        }
    }
}
