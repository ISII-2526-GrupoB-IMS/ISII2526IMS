using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReseñaDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AppForSEII2526.UT.ReseñasController_test
{
    public class PostReseñas_test : AppForSEII25264SqliteUT
    {
        private const string _usuarioEmail = "juan.perez@email.com";
        private const string _nombreUsuario = "Juan";
        private const string _apellidosUsuario = "Pérez García";
        private const string _pais = "España";
        private const string _titulo = "Mis dispositivos";

        private const string _modelo1 = "iPhone 14 Pro";
        private const string _nombreDisp1 = "iPhone 14 Pro 256GB";

        private const string _modelo2 = "Galaxy S23 Ultra";
        private const string _nombreDisp2 = "Galaxy S23 Ultra 512GB";

        public PostReseñas_test()
        {
            // Crear modelos
            var modelos = new List<Modelo>()
            {
                new Modelo(_modelo1),
                new Modelo(_modelo2)
            };

            // Crear dispositivos 
            var dispositivos = new List<Dispositivo>()
            {
                new Dispositivo(modelos[0], "Apple", "Negro", _nombreDisp1, 1199.99, 45.00, 5, 10, 2023),
                new Dispositivo(modelos[1], "Samsung", "Verde", _nombreDisp2, 1299.99, 48.00, 4, 12, 2023)
            };

            // Usuario
            var user = new ApplicationUser("user-001", _nombreUsuario, _apellidosUsuario, _usuarioEmail)
            {
                DireccionDeEnvio = "Calle Mayor 123, Madrid"
            };

            _context.Modelo.AddRange(modelos);
            _context.Dispositivo.AddRange(dispositivos);
            _context.ApplicationUser.Add(user);
            _context.SaveChanges();
        }

        // ----------------------------- TEST DE ERROR -----------------------------
        public static IEnumerable<object[]> TestCasesPostReseña()
        {
            // Caso 1: Sin items
            var reseñaSinItems = new ReseñaForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                new List<ReseñaItemDTO>()
            );

            // Caso 2: Usuario no existe
            var reseñaUsuarioNoExiste = new ReseñaForCreateDTO(
                _titulo,
                _pais,
                "Pedro",
                new List<ReseñaItemDTO>()
                {
                    new ReseñaItemDTO("iPhone 14 Pro 256GB", "iPhone 14 Pro", 2023, 5, "Bueno")
                }
            );

            // Caso 3: Dispositivo no existe
            var reseñaDispositivoNoExiste = new ReseñaForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                new List<ReseñaItemDTO>()
                {
                    new ReseñaItemDTO("ModeloInexistente", "Nada", 2020, 3, "Inexistente")
                }
            );

            return new List<object[]>
            {
                new object[] { reseñaSinItems, "Error. Debes reseñar al menos un dispositivo" },
                new object[] { reseñaUsuarioNoExiste, "Error! Usuario no registrado" },
                new object[] { reseñaDispositivoNoExiste, "Error! No se encontró el dispositivo" }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesPostReseña))]
        public async Task CrearReseña_Error_Test(ReseñaForCreateDTO reseñaDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ReseñasController>>();
            ILogger<ReseñasController> logger = mock.Object;
            var controller = new ReseñasController(_context, logger);

            // Act
            var result = await controller.CrearReseña(reseñaDTO);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(badResult.Value);

            var errorActual = problem.Errors.First().Value[0];

            Assert.StartsWith(errorEsperado, errorActual);
        }

        // ------------------------------- TEST DE ÉXITO -------------------------------
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CrearReseña_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReseñasController>>();
            ILogger<ReseñasController> logger = mock.Object;
            var controller = new ReseñasController(_context, logger);

            var items = new List<ReseñaItemDTO>()
            {
                new ReseñaItemDTO(_nombreDisp1, _modelo1, 2023, 5, "Excelente rendimiento"),
                new ReseñaItemDTO(_nombreDisp2, _modelo2, 2023, 4, "Muy buen dispositivo")
            };

            var reseñaDTO = new ReseñaForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                items
            );

            var fecha = DateTime.Now;

            var expected = new ReseñaDetailDTO(
                _nombreUsuario,
                _pais,
                _titulo,
                fecha,
                items
            );

            // Act
            var result = await controller.CrearReseña(reseñaDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<ReseñaDetailDTO>(createdResult.Value);


            Assert.Equal(expected.NombreUsuario, actual.NombreUsuario);
            Assert.Equal(expected.Pais, actual.Pais);
            Assert.Equal(expected.Titulo, actual.Titulo);
            Assert.Equal(expected.ItemsReseña, actual.ItemsReseña);
            Assert.Equal(expected.FechaReseña, actual.FechaReseña, TimeSpan.FromSeconds(1));


            
        }
    }
}


