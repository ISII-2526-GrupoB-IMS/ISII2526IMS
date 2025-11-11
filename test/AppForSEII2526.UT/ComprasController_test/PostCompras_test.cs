using AppForSEII2526.API.Controllers; 
using AppForSEII2526.API.DTOs.CompraDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class PostCompras_test : AppForSEII25264SqliteUT
    {
        private const string _usuarioEmail = "cliente1@test.com";
        private const string _nombreUsuario = "Juan";
        private const string _apellidosUsuario = "García López";
        private const string _direccionEnvio = "Avenida Libertad 45, Barcelona";

        private const string _marcaValida = "Samsung";
        private const string _modeloValido = "Galaxy S24 Ultra";
        private const string _colorValido = "Negro";

        private const string _marcaSinStock = "Sony";
        private const string _modeloSinStock = "Xperia 1 VI";
        private const string _colorSinStock = "Morado";

        public PostCompras_test()
        {
            // Datos base: Modelos
            var modelos = new List<Modelo>
            {
                new Modelo("Galaxy S24 Ultra"),
                new Modelo("Xperia 1 VI")
            };

            // Datos base: Dispositivos (uno con stock y otro sin stock)
            var dispositivos = new List<Dispositivo>
            {
                new Dispositivo(modelos[0], _marcaValida, _colorValido, "Galaxy S24 Ultra 512GB", 1399.99, 59.99, 15, 5, 2024),
                new Dispositivo(modelos[1], _marcaSinStock, _colorSinStock, "Xperia 1 VI 256GB", 1299.00, 52.99, 0, 3, 2024)
            };

            // Datos base: Usuario
            var user = new ApplicationUser("user-cliente-001", _nombreUsuario, _apellidosUsuario, _usuarioEmail)
            {
                DireccionDeEnvio = _direccionEnvio
            };

            // Datos base: Compra
            var compra = new Compra(
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now,
                new List<ItemCompra>(),
                user
            );

            _context.Modelo.AddRange(modelos);
            _context.Dispositivo.AddRange(dispositivos);
            _context.ApplicationUser.AddRange(user);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_CreateCompra(){

            // sin items
            var compraSinItems = new CompraForCreateDTO(_nombreUsuario, _apellidosUsuario, _direccionEnvio, TiposMetodoPago.TarjetaCredito, 1, // cantidad
            new List<CompraItemDTO>() // vacío
            );

            // item válido
            var itemsValidos = new List<CompraItemDTO>()
            {
        new CompraItemDTO(_marcaValida, _modeloValido, _colorValido, 1399.99, 1, "Compra válida de prueba")
            };

            //Caso 1:usuario no existe
            var compraUsuarioNoExiste = new CompraForCreateDTO("Pedro", "Martínez","Calle Falsa 123, Madrid", TiposMetodoPago.TarjetaCredito,1,itemsValidos);

            //Caso 2: dispositivo no existe

            var compraDispositivoNoExiste = new CompraForCreateDTO(_nombreUsuario, _apellidosUsuario, _direccionEnvio, TiposMetodoPago.TarjetaCredito, 1,
                new List<CompraItemDTO>()
                {
                    new CompraItemDTO("Apple", "iPhone 20 Pro Max", "Blanco", 4399.00, 1, "No existe en BD")
                }
            );

            //Caso 3: dispositivo sin stock

            var compraDispositivoSinStock = new CompraForCreateDTO(_nombreUsuario, _apellidosUsuario, _direccionEnvio, TiposMetodoPago.TarjetaCredito, 1,
                 new List<CompraItemDTO>()
                 {
                    new CompraItemDTO(_marcaSinStock, _modeloSinStock, _colorSinStock, 1299.00, 1, "Sin stock disponible")
                 }
            );

            var allTests = new List<object[]>
            {
                 new object[] { compraSinItems, "Error. Necesitas seleccionar al menos un dispositivo para ser comprado." },
                 new object[] { compraUsuarioNoExiste, "Error! Usuario no registrado" },
                 new object[] { compraDispositivoNoExiste, "Error! No se encontró el dispositivo" },
                 new object[] { compraDispositivoSinStock, "Error! No hay suficiente stock del dispositivo" },
            };

            return allTests;

        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateCompra))]
        public async Task CrearCompra_Error_Test(CompraForCreateDTO compraDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            // Verificamos que el resultado sea BadRequest
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            // Obtenemos el mensaje de error devuelto
            var errorActual = problemDetails.Errors.First().Value[0];

            // Comparamos que empiece con el esperado (por si contiene detalles adicionales)
            Assert.StartsWith(errorEsperado, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearCompra_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            // Creamos una compra válida para el usuario que existe en el contexto
            var compraDTO = new CompraForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                2, // cantidad total
                new List<CompraItemDTO>()
                {
            new CompraItemDTO(_marcaValida, _modeloValido, _colorValido, 1399.99, 2, "Compra válida de Galaxy S24 Ultra")
                }
            );

            // DTO esperado de respuesta (CompraDetailDTO)
            var expectedCompraDetailDTO = new CompraDetailDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                DateTime.Now,       // fecha compra
                2799.98,            // 1399.99 * 2
                2,                  // cantidad total
                compraDTO.ItemsCompra
            );

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);


            Assert.Equal(expectedCompraDetailDTO, actualCompraDetailDTO);

            
        }
    }
}