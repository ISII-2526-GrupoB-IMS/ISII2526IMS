using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquileresController_test
{
    public class PostAlquileres_test : AppForSEII25264SqliteUT
    {
        private const string _usuarioEmail = "cliente1@test.com";
        private const string _nombreUsuario = "Juan";
        private const string _apellidosUsuario = "García López";
        private const string _direccionEnvio = "Calle Libertad 45, Barcelona";

        private const string _marcaValida = "Samsung";
        private const string _modeloValido = "Galaxy S24 Ultra";
        private const string _colorValido = "Negro";
        private const string _nombreDispositivo = "Galaxy S24 Ultra 512GB";
        private const string _marcaSinStock = "Sony";
        private const string _modeloSinStock = "Xperia 1 VI";
        private const string _colorSinStock = "Morado";

        public PostAlquileres_test()
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
                new Dispositivo(modelos[1], _marcaSinStock, _colorSinStock, "Xperia 1 VI 256GB", 1299.00, 52.99, 0, 0, 2024)
            };

            // Datos base: Usuario
            var user = new ApplicationUser("user-cliente-001", _nombreUsuario, _apellidosUsuario, _usuarioEmail)
            {
                DireccionDeEnvio = _direccionEnvio
            };

            _context.Modelo.AddRange(modelos);
            _context.Dispositivo.AddRange(dispositivos);
            _context.ApplicationUser.Add(user);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesPostAlquiler()
        {
            var itemsValidos = new List<ItemAlquilerDTO>()
            {
                new ItemAlquilerDTO(1, _modeloValido, _nombreDispositivo, _marcaValida, 59.99)
            };

            // CASO 1: Sin items
            var alquilerSinItems = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(1.0),
                DateTime.Now.AddDays(5.0),
                new List<ItemAlquilerDTO>() // vacío
            );

            // CASO 2: Usuario no existente
            var alquilerUsuarioNoExiste = new AlquilerForCreateDTO(
                "Pedro",
                "Martínez",
                "Calle Falsa 123, Madrid",
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(1.0),
                DateTime.Now.AddDays(5.0),
                itemsValidos
            );

            // CASO 3: Dispositivo no existente
            var alquilerDispositivoNoExiste = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(1.0),
                DateTime.Now.AddDays(5.0),
                new List<ItemAlquilerDTO>()
                {
                    new ItemAlquilerDTO(999, "No existe en BD", "No existe en BD", "No existe en BD", 9999.99)
                }
            );

            // CASO 4: Dispositivo sin stock
            var alquilerDispositivoSinStock = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(1.0),
                DateTime.Now.AddDays(5.0),
                new List<ItemAlquilerDTO>()
                {
                    new ItemAlquilerDTO(2, _modeloSinStock, "Xperia 1 VI 256GB", _marcaSinStock, 52.99)
                }
            );

            // CASO 5: Fecha de inicio <= hoy (debe ser futura)
            var alquilerFechaInicioInvalida = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Today, // Hoy o antes
                DateTime.Now.AddDays(5.0),
                itemsValidos
            );

            // CASO 6: Fecha de inicio >= fecha de fin
            var alquilerFechasInvalidas = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(10.0), // Después de la fecha de fin
                DateTime.Now.AddDays(5.0),  // Antes de la fecha de inicio
                itemsValidos
            );

            // CASO 7: Dirección no contiene ni Calle ni Carretera
            var alquilerDireccionInvalida = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                "avenida de la libertad",
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(5.0), 
                DateTime.Now.AddDays(10.0),  
                itemsValidos
            );
            // CASO 8: Dirección nula
            var alquilerDireccionNula = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                null,
                TiposMetodoPago.TarjetaCredito,
                DateTime.Now.AddDays(5.0),
                DateTime.Now.AddDays(10.0),
                itemsValidos
            );

            var allTests = new List<object[]>
            {
                // Caso 1: sin items
                new object[] { alquilerSinItems, "Error! You must include at least one device to be rented" },

                // Caso 2: usuario no existe
                new object[] { alquilerUsuarioNoExiste, "Error! UserName is not registered" },

                // Caso 3: dispositivo no existe
                new object[] { alquilerDispositivoNoExiste, "Error! Device with name" },

                // Caso 4: sin stock
                new object[] { alquilerDispositivoSinStock, "Error! Device with name" },

                // Caso 5: fecha de inicio <= hoy
                new object[] { alquilerFechaInicioInvalida, "Error! Your rental date must start later than today" },

                // Caso 6: fecha inicio >= fecha fin
                new object[] { alquilerFechasInvalidas, "Error! Your rental must end later than it starts" },

                // CASO 7: Dirección no contiene ni Calle ni Carretera
                new object[] { alquilerDireccionInvalida, "Error en la dirección de envío. Por favor,introduce una dirección válida incluyendo las palabras Calle o Carretera" },

                // CASO 8: Dirección nula
                new object[] { alquilerDireccionNula, "Error en la dirección de envío. Por favor,introduce una dirección válida incluyendo las palabras Calle o Carretera" },

            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesPostAlquiler))]
        public async Task CrearAlquilerError_Test(AlquilerForCreateDTO alquilerDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;

            var controller = new AlquileresController(_context, logger);

            // Act
            var result = await controller.CrearAlquiler(alquilerDTO);

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
        public async Task CrearAlquiler_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;
            var controller = new AlquileresController(_context, logger);

            // Creamos un alquiler válido para el usuario que existe en el contexto
            var fechaDesde = DateTime.Now.AddDays(2);
            var fechaHasta = DateTime.Now.AddDays(7);
            var numeroDias = (fechaHasta - fechaDesde).Days;

            var alquilerDTO = new AlquilerForCreateDTO(
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                fechaDesde,
                fechaHasta,
                new List<ItemAlquilerDTO>()
                {
            new ItemAlquilerDTO(1, _modeloValido, _nombreDispositivo, _marcaValida, 59.99)
                }
            );

            // DTO esperado de respuesta (AlquilerForCreateDTO)
            var expectedAlquilerDTO = new AlquilerDetailDTO(
                1,
                DateTime.Now,
                _nombreUsuario,
                _apellidosUsuario,
                _direccionEnvio,
                TiposMetodoPago.TarjetaCredito,
                fechaDesde,
                fechaHasta,
                alquilerDTO.ItemsAlquiler
            );

            // Act
            var result = await controller.CrearAlquiler(alquilerDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualAlquilerDTO = Assert.IsType<AlquilerDetailDTO>(createdResult.Value);

            Assert.Equal(expectedAlquilerDTO, actualAlquilerDTO);
        }
    }
}