using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AppForSEII2526.UT.ReviewController_test
{
    public class PostReview_test : AppForSEII25264SqliteUT
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

        public PostReview_test()
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
        public static IEnumerable<object[]> TestCasesPostReview()
        {
            // Caso 1: Sin items
            var ReviewSinItems = new ReviewForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                new List<ReviewItemDTO>()
            );

            // Caso 2: Usuario no existe
            var ReviewUsuarioNoExiste = new ReviewForCreateDTO(
                _titulo,
                _pais,
                "Pedro",
                new List<ReviewItemDTO>()
                {
                    new ReviewItemDTO("iPhone 14 Pro 256GB", "iPhone 14 Pro", 2023, 5, "Bueno")
                }
            );

            

            // Caso 3: Dispositivo no existe
            var ReviewDispositivoNoExiste = new ReviewForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                new List<ReviewItemDTO>()
                {
                    new ReviewItemDTO("ModeloInexistente", "Nada", 2020, 3, "Inexistente")
                }
            );

            // Caso 4: Comentario inválido
            var ReviewComentarioInvalido = new ReviewForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                new List<ReviewItemDTO>()
                {
                    new ReviewItemDTO("iPhone 14 Pro 256GB", "iPhone 14 Pro", 2023, 5, "Bueno")
                }
            );





            return new List<object[]>
            {
                new object[] { ReviewSinItems, "Error. Debes Reviewr al menos un dispositivo" },
                new object[] { ReviewUsuarioNoExiste, "Error! Usuario no registrado" },
                new object[] { ReviewDispositivoNoExiste, "Error! No se encontró el dispositivo" },
                new object[] { ReviewComentarioInvalido, "Error, el comentario de la Review: debe empezar por Review para" }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesPostReview))]
        public async Task CrearReview_Error_Test(ReviewForCreateDTO ReviewDTO, string errorEsperado)
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            // Act
            var result = await controller.CrearReview(ReviewDTO);

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
        public async Task CrearReview_Success_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            var items = new List<ReviewItemDTO>()
            {
                new ReviewItemDTO(_nombreDisp1, _modelo1, 2023, 5, "Review para valorar el iPhone 14 Pro"),
                new ReviewItemDTO(_nombreDisp2, _modelo2, 2023, 4, "Review para valorar el Galaxy S23 Ultra")
            };

            var ReviewDTO = new ReviewForCreateDTO(
                _titulo,
                _pais,
                _nombreUsuario,
                items
            );

            var fecha = DateTime.Now;

            var expected = new ReviewDetailDTO(
                _nombreUsuario,
                _pais,
                _titulo,
                fecha,
                items
            );

            // Act
            var result = await controller.CrearReview(ReviewDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actual = Assert.IsType<ReviewDetailDTO>(createdResult.Value);


            Assert.Equal( expected, actual );


            
        }
    }
}


