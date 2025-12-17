using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ReviewController_test
{
    public class GetReview_test : AppForSEII2526.UT.AppForSEII25264SqliteUT
    {
        public GetReview_test()
        {
            // Crear modelos
            var modelos = new List<Modelo>()
            {
                new Modelo { NombreModelo = "iPhone 14 Pro" },
                new Modelo { NombreModelo = "Galaxy S23" }
            };

            // Crear dispositivos
            var dispositivos = new List<Dispositivo>() {
                new Dispositivo (modelos[0],"Apple","Negro","iPhone 14 Pro 256GB",1199.99,45.00,5,10,2023),
                new Dispositivo (modelos[1],"Samsung","Blanco","Galaxy S23 Ultra 512GB",1299.99,48.00,8,12,2023),
            };

            _context.AddRange(modelos);
            _context.AddRange(dispositivos);
            _context.SaveChanges();

            // Crear usuario
            var user = new ApplicationUser("1", "Juan", "España", "Calle Mayor 123, Madrid");
            user.Email = "juan.perez@email.com";
            user.UserName = user.Email;

            // Crear Review
            var Review = new Review(
                1,
                "Mis dispositivos",
                "España",
                DateTime.Now.Date,
                new List<ItemReview>(),
                user
            );

            // Crear ítems de Review
            var item1 = new ItemReview(
                "Excelente rendimiento",
                5,
                dispositivos[0],
                Review
            );

            var item2 = new ItemReview(
                "Muy bueno, pero algo pesado",
                4,
                dispositivos[1],
                Review
            );

            Review.ItemsReview.Add(item1);
            Review.ItemsReview.Add(item2);

            Review.CalificaciónGeneral = Review.ItemsReview.Average(i => i.Puntuacion);

            _context.Users.Add(user);
            _context.Review.Add(Review);
            _context.SaveChanges();
        }

        // --------------------------------------------------------------------
        // TEST 1: Review no encontrada
        // --------------------------------------------------------------------
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleReview_NotFound_Test()
        {
            var mock = new Mock<ILogger<ReviewController>>();
            var logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            var result = await controller.GetDetalleReview(0);

            Assert.IsType<NotFoundResult>(result);
        }

        // --------------------------------------------------------------------
        // TEST 2: Review encontrada correctamente
        // --------------------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleReview_Found_Test()
        {
            var mock = new Mock<ILogger<ReviewController>>();
            var logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            var ReviewEnDB = _context.Review.First();

            var fecha = ReviewEnDB.FechaReview;

            var expected = new ReviewDetailDTO(
                "Juan",
                "España",
                "Mis dispositivos",
                fecha,
                new List<ReviewItemDTO>()
            );

            expected.ItemsReview.Add(new ReviewItemDTO(
                ReviewEnDB.ItemsReview[0].Dispositivo.NombreDispositivo,
                ReviewEnDB.ItemsReview[0].Dispositivo.Modelo.NombreModelo,
                ReviewEnDB.ItemsReview[0].Dispositivo.Año,
                ReviewEnDB.ItemsReview[0].Puntuacion,
                ReviewEnDB.ItemsReview[0].Comentario
            ));

            expected.ItemsReview.Add(new ReviewItemDTO(
                ReviewEnDB.ItemsReview[1].Dispositivo.NombreDispositivo,
                ReviewEnDB.ItemsReview[1].Dispositivo.Modelo.NombreModelo,
                ReviewEnDB.ItemsReview[1].Dispositivo.Año,
                ReviewEnDB.ItemsReview[1].Puntuacion,
                ReviewEnDB.ItemsReview[1].Comentario
            ));

            var result = await controller.GetDetalleReview(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDTO = Assert.IsType<ReviewDetailDTO>(okResult.Value);

            Assert.Equal(expected.Titulo, actualDTO.Titulo);
            Assert.Equal(expected.Pais, actualDTO.Pais);
            Assert.Equal(expected.NombreUsuario, actualDTO.NombreUsuario);
            Assert.Equal(expected.FechaReview, actualDTO.FechaReview);

            Assert.Equal(expected.ItemsReview, actualDTO.ItemsReview);
            
        }
    }
}



