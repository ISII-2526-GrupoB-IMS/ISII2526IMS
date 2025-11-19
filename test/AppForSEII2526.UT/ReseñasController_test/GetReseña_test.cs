using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReseñaDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppForSEII2526.UT.ReseñasController_test
{
    public class GetReseña_test : AppForSEII2526.UT.AppForSEII25264SqliteUT
    {
        public GetReseña_test()
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

            // Crear reseña
            var reseña = new Reseña(
                1,
                "Mis dispositivos",
                "España",
                DateTime.Now.Date,
                new List<ItemReseña>(),
                user
            );

            // Crear ítems de reseña
            var item1 = new ItemReseña(
                "Excelente rendimiento",
                5,
                dispositivos[0],
                reseña
            );

            var item2 = new ItemReseña(
                "Muy bueno, pero algo pesado",
                4,
                dispositivos[1],
                reseña
            );

            reseña.ItemsReseña.Add(item1);
            reseña.ItemsReseña.Add(item2);

            reseña.CalificaciónGeneral = reseña.ItemsReseña.Average(i => i.Puntuacion);

            _context.Users.Add(user);
            _context.Reseña.Add(reseña);
            _context.SaveChanges();
        }

        // --------------------------------------------------------------------
        // TEST 1: Reseña no encontrada
        // --------------------------------------------------------------------
        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleReseña_NotFound_Test()
        {
            var mock = new Mock<ILogger<ReseñasController>>();
            var logger = mock.Object;
            var controller = new ReseñasController(_context, logger);

            var result = await controller.GetDetalleReseña(0);

            Assert.IsType<NotFoundResult>(result);
        }

        // --------------------------------------------------------------------
        // TEST 2: Reseña encontrada correctamente
        // --------------------------------------------------------------------
        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleReseña_Found_Test()
        {
            var mock = new Mock<ILogger<ReseñasController>>();
            var logger = mock.Object;
            var controller = new ReseñasController(_context, logger);

            var reseñaEnDB = _context.Reseña.First();

            var fecha = reseñaEnDB.FechaReseña;

            var expected = new ReseñaDetailDTO(
                "Juan",
                "España",
                "Mis dispositivos",
                fecha,
                new List<ReseñaItemDTO>()
            );

            expected.ItemsReseña.Add(new ReseñaItemDTO(
                reseñaEnDB.ItemsReseña[0].Dispositivo.NombreDispositivo,
                reseñaEnDB.ItemsReseña[0].Dispositivo.Modelo.NombreModelo,
                reseñaEnDB.ItemsReseña[0].Dispositivo.Año,
                reseñaEnDB.ItemsReseña[0].Puntuacion,
                reseñaEnDB.ItemsReseña[0].Comentario
            ));

            expected.ItemsReseña.Add(new ReseñaItemDTO(
                reseñaEnDB.ItemsReseña[1].Dispositivo.NombreDispositivo,
                reseñaEnDB.ItemsReseña[1].Dispositivo.Modelo.NombreModelo,
                reseñaEnDB.ItemsReseña[1].Dispositivo.Año,
                reseñaEnDB.ItemsReseña[1].Puntuacion,
                reseñaEnDB.ItemsReseña[1].Comentario
            ));

            var result = await controller.GetDetalleReseña(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDTO = Assert.IsType<ReseñaDetailDTO>(okResult.Value);

            Assert.Equal(expected.Titulo, actualDTO.Titulo);
            Assert.Equal(expected.Pais, actualDTO.Pais);
            Assert.Equal(expected.NombreUsuario, actualDTO.NombreUsuario);
            Assert.Equal(expected.FechaReseña, actualDTO.FechaReseña);

            Assert.Equal(expected.ItemsReseña, actualDTO.ItemsReseña);
            
        }
    }
}



