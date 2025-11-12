using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DispositivoDTOs;

namespace AppForSEII2526.UT.DispositivosController_test
{
    public class GetDispositivosParaCompra_test : AppForSEII2526.UT.AppForSEII25264SqliteUT
    {
        public GetDispositivosParaCompra_test()
        {
            // === MODELOS ===
            var modelos = new List<Modelo>
            {
                new Modelo { Id = 1, NombreModelo = "iPhone 14 Pro" },
                new Modelo { Id = 2, NombreModelo = "iPhone 13" },
                new Modelo { Id = 3, NombreModelo = "Galaxy S23 Ultra" },
                new Modelo { Id = 4, NombreModelo = "Galaxy A54" },
                new Modelo { Id = 5, NombreModelo = "Xiaomi 13 Pro" },
                new Modelo { Id = 6, NombreModelo = "Pixel 7 Pro" },
                new Modelo { Id = 7, NombreModelo = "OnePlus 11" },
                new Modelo { Id = 8, NombreModelo = "Huawei P60 Pro" },
                new Modelo { Id = 9, NombreModelo = "Oppo Find X5" },
                new Modelo { Id = 10, NombreModelo = "Realme GT3" }
            };

            // === DISPOSITIVOS ===
            var dispositivos = new List<Dispositivo>
            {
                // iPhone
                new Dispositivo(modelos[0], "Apple", "Negro", "iPhone 14 Pro 256GB", 1199.99, 45.00, 5, 10, 2023),
                new Dispositivo(modelos[0], "Apple", "Plata", "iPhone 14 Pro 512GB", 1399.99, 50.00, 3, 8, 2023),
                new Dispositivo(modelos[0], "Apple", "Morado", "iPhone 14 Pro 128GB", 1099.99, 42.00, 7, 12, 2023),
                new Dispositivo(modelos[1], "Apple", "Azul", "iPhone 13 256GB", 799.99, 35.00, 10, 15, 2022),
                new Dispositivo(modelos[1], "Apple", "Rosa", "iPhone 13 128GB", 699.99, 30.00, 12, 20, 2022),

                // Samsung
                new Dispositivo(modelos[2], "Samsung", "Verde", "Galaxy S23 Ultra 512GB", 1299.99, 48.00, 4, 12, 2023),
                new Dispositivo(modelos[2], "Samsung", "Negro", "Galaxy S23 Ultra 256GB", 1099.99, 42.00, 6, 15, 2023),
                new Dispositivo(modelos[3], "Samsung", "Blanco", "Galaxy A54 5G 256GB", 449.99, 22.00, 15, 25, 2023),
                new Dispositivo(modelos[3], "Samsung", "Negro", "Galaxy A54 5G 128GB", 399.99, 20.00, 18, 30, 2023),

                // Xiaomi
                new Dispositivo(modelos[4], "Xiaomi", "Negro", "Xiaomi 13 Pro 256GB", 999.99, 40.00, 8, 18, 2023),
                new Dispositivo(modelos[4], "Xiaomi", "Blanco", "Xiaomi 13 Pro 512GB", 1099.99, 45.00, 5, 12, 2023),

                // Google
                new Dispositivo(modelos[5], "Google", "Blanco", "Pixel 7 Pro 256GB", 899.99, 38.00, 6, 14, 2022),
                new Dispositivo(modelos[5], "Google", "Negro", "Pixel 7 Pro 128GB", 799.99, 35.00, 8, 16, 2022),

                // OnePlus
                new Dispositivo(modelos[6], "OnePlus", "Verde", "OnePlus 11 5G 256GB", 849.99, 37.00, 7, 15, 2023),

                // Huawei
                new Dispositivo(modelos[7], "Huawei", "Dorado", "Huawei P60 Pro 256GB", 949.99, 40.00, 5, 10, 2023),

                // Oppo
                new Dispositivo(modelos[8], "Oppo", "Azul", "Oppo Find X5 Pro 256GB", 799.99, 35.00, 6, 12, 2022),

                // Realme
                new Dispositivo(modelos[9], "Realme", "Negro", "Realme GT3 240W 256GB", 649.99, 28.00, 10, 20, 2023),
                new Dispositivo(modelos[9], "Realme", "Blanco", "Realme GT3 240W 128GB", 599.99, 25.00, 12, 22, 2023)
            };

            _context.AddRange(modelos);
            _context.AddRange(dispositivos);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDispositivosParaComprar_OK()
        {
            var testCases = new List<object[]>
            {
                // 1. Sin filtros: deben aparecer todos los dispositivos con CantidadParaCompra > 0 (18)
                new object[] { null, null, 18 },

                // 2. Filtro por nombre “iPhone”
                new object[] { "iPhone", null, 5 },

                // 3. Filtro por nombre “Galaxy”
                new object[] { "Galaxy", null, 4 },

                // 4. Filtro por nombre “Pixel”
                new object[] { "Pixel", null, 2 },

                // 5. Filtro por color “Negro”
                new object[] { null, "Negro", 6 },

                // 6. Filtro por nombre “Xiaomi”
                new object[] { "Xiaomi", null, 2 },

                // 7. Filtro por nombre “Realme”
                new object[] { "Realme", null, 2 },

                // 8. Filtro por nombre “Huawei”
                new object[] { "Huawei", null, 1 },

                // 9. Filtro por nombre “OnePlus”
                new object[] { "OnePlus", null, 1 },

                // 10. Filtro inexistente
                new object[] { "Nokia", null, 0 }
            };

            return testCases;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDispositivosParaComprar_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDispositivosParaComprar_Filtros_Test(
            string? filtroNombre,
            string? filtroColor,
            int cantidadEsperada)
        {
            // Arrange
            var controller = new DispositivoController(_context, null);

            // Act
            var result = await controller.GetDispositivosParaComprar(filtroNombre, filtroColor);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var dispositivos = Assert.IsType<List<DispositivoParaComprarDTO>>(okResult.Value);

            Assert.Equal(cantidadEsperada, dispositivos.Count);
        }

    }

}










    

