using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DispositivoDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.DispositivosController_test
{
    public class GetDispositivosParaAlquilar_test : AppForSEII25264SqliteUT
    {
        public GetDispositivosParaAlquilar_test()
        {
            //CREAR LOS MODELOS NECESARIOS PARA LAS PRUEBAS
            var modelos = new List<Modelo>()
            {
                new Modelo { NombreModelo = "iPhone 14 Pro" },
                new Modelo { NombreModelo = "Galaxy S23" },
                new Modelo { NombreModelo = "Pixel 8" },
                new Modelo { NombreModelo = "Xiaomi 13" }
            };

            //CREAR LOS DISPOSITIVOS NECESARIOS PARA LAS PRUEBAS
            var dispositivos = new List<Dispositivo>()
            {
                new Dispositivo(modelos[0], "Apple", "Negro", "iPhone 14 Pro 256GB", 1199.99, 45.00, 5, 10, 2023),
                new Dispositivo(modelos[0], "Apple", "Plata", "iPhone 14 Pro 512GB", 1399.99, 50.00, 3, 8, 2023),
                new Dispositivo(modelos[1], "Samsung", "Negro", "Galaxy S23 Ultra 256GB", 1099.99, 42.00, 6, 15, 2023),
                new Dispositivo(modelos[1], "Samsung", "Blanco", "Galaxy S23 Ultra 512GB", 1299.99, 48.00, 4, 12, 2023),
                new Dispositivo(modelos[2], "Google", "Gris", "Pixel 8 Pro 128GB", 899.99, 38.00, 8, 16, 2024),
                new Dispositivo(modelos[3], "Xiaomi", "Verde", "Xiaomi 13 Pro 256GB", 999.99, 40.00, 7, 20, 2023),
                // Este dispositivo tiene CantidadParaAlquilar = 0, debería aparecer igual
                new Dispositivo(modelos[3], "Xiaomi", "Negro", "Xiaomi 13 Lite 128GB", 599.99, 25.00, 10, 0, 2023),
            };

            _context.AddRange(modelos);
            _context.AddRange(dispositivos);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDispositivosParaAlquilar_OK()
        {
            var testCases = new List<object[]>
            {
                // Filtros: nombreModelo, precioMaximo, cantidadEsperada
                // TC1: Sin filtros: devuelve todos los dispositivos (7)
                new object[] { null, null, 7 },

                // TC2: Filtro por nombre "iPhone": devuelve solo los iPhones (2)
                new object[] { "iPhone", null, 2 },

                // TC3: Filtro por precio máximo 40.00: dispositivos con precio <= 40 (3)
                new object[] { null, 40.00, 3 },

                // TC4: Filtro por nombre "Galaxy": devuelve los Galaxy (2)
                new object[] { "Galaxy", null, 2 },

                // TC5: Filtro por nombre "Pro" y precio máximo 45.00 (2)
                // Debería de dar solo los iphone pro
                new object[] { "Pro", 60.00, 2 },

                // TC6: Filtro por nombre que no existe: "Nokia" (0)
                new object[] { "Nokia", null, 0 },

                //// TC7: Filtro por precio muy bajo (10.00): no hay dispositivos (0)
                new object[] { null, 10.00, 0 },

                //// TC8: Filtro parcial por nombre "Pro" (2)
                new object[] { "Pro", null, 2 },

                //// TC9: Filtro por precio máximo 45.00 (5)
                new object[] { null, 45.00, 5 },

                //// TC10: Filtro por nombre "Xiaomi" (2)
                new object[] { "Xiaomi", null, 2 },

                //// TC11: Filtro por nombre "S23" y precio máximo 45.00 (1)
                new object[] { "S23", 45.00, 1 },

                //// TC12: Filtro por precio exacto 38.00 (2)
                new object[] { null, 38.00, 2 },

                // TC13: Filtro por nombre "Pixel" (1)
                new object[] { "Pixel", null, 1 },

                // TC14: Filtro por precio 50.00 (6)
                new object[] { null, 50.00, 7 }
            };

            return testCases;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDispositivosParaAlquilar_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDispositivosParaAlquilar_Filtros_Test(
            string? nombreModelo,
            double? precioMaximo,
            int cantidadEsperada)
        {
            // Arrange
            var controller = new DispositivoController(_context, null);

            // Act
            var result = await controller.GetDispositivosParaAlquilar(nombreModelo, precioMaximo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dispositivos = Assert.IsType<List<DispositivoParaAlquilarDTO>>(okResult.Value);

            Assert.Equal(cantidadEsperada, dispositivos.Count);
        }
    }
}