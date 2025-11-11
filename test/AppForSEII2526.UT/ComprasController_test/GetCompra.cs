using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.CompraDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
   public class GetCompra : AppForMovies.UT.AppForMovies4SqliteUT
   {
        public GetCompra()
        {

            //CREAR LOS MODELOS NECESARIOS PARA LAS PRUEBAS

            var modelos = new List<Modelo>()
            {
                new Modelo { NombreModelo = "iPhone 14 Pro" },
                new Modelo { NombreModelo = "Galaxy S23" }
            };

            //CREAR LOS DISPOSITIVOS NECESARIOS PARA LAS PRUEBAS

            var dispositivos = new List<Dispositivo>() {

                new Dispositivo (modelos[0],"Apple","Negro","iPhone 14 Pro 256GB",1199.99,45.00,5,10,2023),
                new Dispositivo (modelos[1],"Samsung","Blanco","Galaxy S23 Ultra 512GB",1299.99,48.00,8,12,2023),
               

            };

            _context.AddRange(modelos);
            _context.AddRange(dispositivos);
            _context.SaveChanges();

            // Crear usuario
            ApplicationUser user = new ApplicationUser(
                "1",
                "Juan",
                "Pérez García",
                "Calle Mayor 123, Madrid"
            );
            user.Email = "juan.perez@email.com";
            user.UserName = "juan.perez@email.com";

            // Crear compra
            var compra = new Compra(
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now,
                new List<ItemCompra>(),
                user
            );

            // Crear items de compra
            var itemCompra1 = new ItemCompra(dispositivos[0].Id, dispositivos[0].PrecioParaCompra, 2);
            itemCompra1.Descripcion = "iPhone 14 Pro 256GB, me encanta";

            var itemCompra2 = new ItemCompra(dispositivos[1].Id, dispositivos[1].PrecioParaCompra, 1);
            itemCompra2.Descripcion = "Galaxy S23 Ultra 512GB, es para mi padre";

            compra.ItemsCompra.Add(itemCompra1);
            compra.ItemsCompra.Add(itemCompra2);

            // Calcular totales
            compra.PrecioTotal = (dispositivos[0].PrecioParaCompra * 2) + (dispositivos[1].PrecioParaCompra * 1);
            compra.CantidadTotal = 3;

            _context.Users.Add(user);
            _context.Add(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleCompra_NotFound_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;
            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.GetDetalleCompra(0);

            // Assert
            // Verificamos que el response type es NotFound
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleCompra_Found_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;
            var controller = new ComprasController(_context, logger);

            // Obtener la fecha de compra del contexto para la comparación
            var compraEnDB = _context.Compra.First();
            var fechaCompra = compraEnDB.FechaCompra;

            var expectedCompra = new CompraDetailDTO("Juan", "Pérez García", "Calle Mayor 123, Madrid", fechaCompra, 3699.97, // (1199.99 * 2) + (1299.99 * 1)
                3,
                new List<CompraItemDTO>()
            );

            expectedCompra.ItemsCompra.Add(new CompraItemDTO("Apple", "iPhone 14 Pro", "Negro", 1199.99, 2, "iPhone 14 Pro 256GB, me encanta"));

            expectedCompra.ItemsCompra.Add(new CompraItemDTO("Samsung", "Galaxy S23", "Blanco", 1299.99, 1, "Galaxy S23 Ultra 512GB, es para mi padre"));

            // Act
            // Llamamos al System Under Test (SUT)
            var result = await controller.GetDetalleCompra(1);

            // Assert
            // Verificamos que el response type es OK y obtenemos la compra
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verificamos que el tipo retornado es igual al esperado
            var compraDTOActual = Assert.IsType<CompraDetailDTO>(okResult.Value);

            // Verificamos que los datos esperados y actuales son los mismos
            Assert.Equal(expectedCompra, compraDTOActual);
        }




    }
}
