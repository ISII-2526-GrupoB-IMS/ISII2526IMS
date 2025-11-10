using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.CompraDTOs;

namespace AppForSEII2526.UT.ComprasController_test
{
    public class CrearCompra_Test : AppForSEII2526.UT.AppForSEII25264SqliteUT
    {
        // Definimos los datos para el test para facilitar su reutilización
        private const string _direccionEnvio = "Calle Mayor 123, Madrid";
        private const string _apellidosCliente = "Pérez García";
        private const string _nombreCliente = "Juan";
        private const string _nombreUsuarioCliente = "juan.perez@email.com";
        private const string _dispositivo1Nombre = "iPhone 14 Pro 256GB";
        private const string _dispositivo1Marca = "Apple";
        private const string _dispositivo1Modelo = "iPhone 14 Pro";
        private const string _dispositivo1Color = "Negro";
        private const string _dispositivo2Nombre = "Galaxy S23 Ultra 512GB";
        private const string _dispositivo2Marca = "Samsung";
        private const string _dispositivo2Modelo = "Galaxy S23";
        private const string _dispositivo2Color = "Blanco";

        public CrearCompra_Test()
        {
            var modelos = new List<Modelo>()
            {
                new Modelo { NombreModelo = _dispositivo1Modelo },
                new Modelo { NombreModelo = _dispositivo2Modelo },
            };

            var dispositivos = new List<Dispositivo>()
            {
                // Dispositivo con stock solo de 2 dispositivos 
                new Dispositivo
                {
                    NombreDispositivo = _dispositivo1Nombre,
                    Marca = _dispositivo1Marca,
                    Modelo = modelos[0],
                    Color = _dispositivo1Color,
                    PrecioParaCompra = 1199.99,
                    PrecioParaAlquiler = 45.00,
                    CantidadParaCompra = 2,
                    CantidadParaAlquilar = 5,
                    Año = 2023,
                    Calidad = Dispositivo.TipoDeCalidad.Buena
                },
                // Dispositivo con gran stock para comprar
                new Dispositivo
                {
                    NombreDispositivo = _dispositivo2Nombre,
                    Marca = _dispositivo2Marca,
                    Modelo = modelos[1],
                    Color = _dispositivo2Color,
                    PrecioParaCompra = 1299.99,
                    PrecioParaAlquiler = 48.00,
                    CantidadParaCompra = 100,
                    CantidadParaAlquilar = 8,
                    Año = 2023,
                    Calidad = Dispositivo.TipoDeCalidad.Buena
                },
            };

            
            _context.AddRange(modelos);
            _context.AddRange(dispositivos);

            ApplicationUser user = new ApplicationUser(
                "1",
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio
            );
            user.Email = _nombreUsuarioCliente;
            user.UserName = _nombreUsuarioCliente;
            _context.Add(user);

            _context.SaveChanges();

            // Crear una compra inicial en la BD
            var compra = new Compra(
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now,
                new List<ItemCompra>(),
                user
            );

            var itemCompra = new ItemCompra(dispositivos[0].Id, dispositivos[0].PrecioParaCompra, 1);
            itemCompra.Descripcion = "Mi dispositivo favorito";
            compra.ItemsCompra.Add(itemCompra);

            compra.PrecioTotal = dispositivos[0].PrecioParaCompra;
            compra.CantidadTotal = 1;

            _context.Add(compra);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CrearCompra()
        {
            // Compra sin items
            CompraForCreateDTO compraSinItems = new CompraForCreateDTO(
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                0,
                new List<CompraItemDTO>()
            );

            // Usuario no registrado
            IList<CompraItemDTO> itemsCompra = new List<CompraItemDTO>()
            {
                new CompraItemDTO(_dispositivo2Marca, _dispositivo2Modelo, _dispositivo2Color, 1299.99, 1, "Dispositivo de prueba")
            };

            CompraForCreateDTO usuarioNoRegistrado = new CompraForCreateDTO(
                "Carlos",
                "García López",
                _direccionEnvio,
                TiposMetodoPago.PayPal,
                1,
                itemsCompra
            );

            // Dispositivo sin stock suficiente (intentar comprar 5 cuando solo hay 2)
            IList<CompraItemDTO> itemsSinStock = new List<CompraItemDTO>()
            {
                new CompraItemDTO(_dispositivo1Marca, _dispositivo1Modelo, _dispositivo1Color, 1199.99, 5, "Muchas unidades")
            };

            CompraForCreateDTO compraSinStock = new CompraForCreateDTO(
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                5,
                itemsSinStock
            );

            // Dispositivo inexistente
            IList<CompraItemDTO> itemsDispositivoInexistente = new List<CompraItemDTO>()
            {
                new CompraItemDTO("Nokia", "3310", "Azul", 99.99, 1, "Dispositivo no existe")
            };

            CompraForCreateDTO dispositivoInexistente = new CompraForCreateDTO(
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio,
                TiposMetodoPago.Efectivo,
                1,
                itemsDispositivoInexistente
            );

            var allTests = new List<object[]>
            {
                // Error esperado por cada intento de compra
                new object[] { compraSinItems, "Error. Necesitas seleccionar al menos un dispositivo para ser comprado" },
                new object[] { usuarioNoRegistrado, "Error! Usuario no registrado" },
                new object[] { compraSinStock, "Error! No hay suficiente stock del dispositivo" },
                new object[] { dispositivoInexistente, "Error! No se encontró el dispositivo" },
            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CrearCompra))]
        public async Task CrearCompra_Error_Test(CompraForCreateDTO compraDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            // Verificamos que el response type es BadRequest y obtenemos el error retornado
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            // Verificamos que el mensaje de error esperado y actual son el mismo
            Assert.StartsWith(errorExpected, errorActual);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearCompra_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ComprasController>>();
            ILogger<ComprasController> logger = mock.Object;

            var controller = new ComprasController(_context, logger);

            DateTime fechaCompra = DateTime.Now;

            CompraForCreateDTO compraDTO = new CompraForCreateDTO(
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio,
                TiposMetodoPago.PayPal,
                2,
                new List<CompraItemDTO>()
            );

            compraDTO.ItemsCompra.Add(new CompraItemDTO(
                _dispositivo2Marca,
                _dispositivo2Modelo,
                _dispositivo2Color,
                1299.99,
                2,
                "Dispositivos para regalo"
            ));

            // El id es 2 porque ya hay otra compra en la base de datos
            CompraDetailDTO expectedCompraDetailDTO = new CompraDetailDTO(
                _nombreCliente,
                _apellidosCliente,
                _direccionEnvio,
                fechaCompra,
                2599.98, // 1299.99 * 2
                2,
                new List<CompraItemDTO>()
            );

            expectedCompraDetailDTO.ItemsCompra.Add(new CompraItemDTO(
                _dispositivo2Marca,
                _dispositivo2Modelo,
                _dispositivo2Color,
                1299.99,
                2,
                "Dispositivos para regalo"
            ));

            // Act
            var result = await controller.CrearCompra(compraDTO);

            // Assert
            // Verificamos que el response type es CreatedAtAction
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualCompraDetailDTO = Assert.IsType<CompraDetailDTO>(createdResult.Value);

            Assert.Equal(expectedCompraDetailDTO.NombreUsuario, actualCompraDetailDTO.NombreUsuario);
            Assert.Equal(expectedCompraDetailDTO.ApellidosUsuario, actualCompraDetailDTO.ApellidosUsuario);
            Assert.Equal(expectedCompraDetailDTO.DireccionDeEntrega, actualCompraDetailDTO.DireccionDeEntrega);
            Assert.Equal(expectedCompraDetailDTO.PrecioTotal, actualCompraDetailDTO.PrecioTotal);
            Assert.Equal(expectedCompraDetailDTO.CantidadTotal, actualCompraDetailDTO.CantidadTotal);
            Assert.Equal(expectedCompraDetailDTO.ItemsCompra.Count, actualCompraDetailDTO.ItemsCompra.Count);

            // Verificar items
            for (int i = 0; i < expectedCompraDetailDTO.ItemsCompra.Count; i++)
            {
                Assert.Equal(expectedCompraDetailDTO.ItemsCompra[i].Marca, actualCompraDetailDTO.ItemsCompra[i].Marca);
                Assert.Equal(expectedCompraDetailDTO.ItemsCompra[i].Modelo, actualCompraDetailDTO.ItemsCompra[i].Modelo);
                Assert.Equal(expectedCompraDetailDTO.ItemsCompra[i].Color, actualCompraDetailDTO.ItemsCompra[i].Color);
                Assert.Equal(expectedCompraDetailDTO.ItemsCompra[i].Precio, actualCompraDetailDTO.ItemsCompra[i].Precio);
                Assert.Equal(expectedCompraDetailDTO.ItemsCompra[i].Cantidad, actualCompraDetailDTO.ItemsCompra[i].Cantidad);
            }
        }

    }

}
