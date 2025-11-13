using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.AlquilerDTOs;
using AppForSEII2526.API.DTOs.CompraDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.AlquileresController_test
{
    public class GetAlquileres_test : AppForSEII25264SqliteUT
    {
        public GetAlquileres_test() {
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

            // Crear Alquiler
            var alquiler = new Alquiler(
                user.DireccionDeEnvio,
                user.NombreUsuario,
                user.ApellidosUsuario,
                user,
                DateTime.Now.AddDays(1.0),
                DateTime.Now.AddDays(5.0),
                TiposMetodoPago.TarjetaCredito,
                new List<ItemAlquiler>()
            );


            // Crear items de alquiler
            var itemAlquiler1 = new ItemAlquiler(dispositivos[0].Id, dispositivos[0], alquiler.Id,alquiler, dispositivos[0].PrecioParaAlquiler, 2);

            var itemAlquiler2 = new ItemAlquiler(dispositivos[1].Id, dispositivos[1], alquiler.Id, alquiler, dispositivos[1].PrecioParaAlquiler, 1);

            alquiler.ItemsAlquiler.Add(itemAlquiler1);
            alquiler.ItemsAlquiler.Add (itemAlquiler2);

            alquiler.PrecioTotal = (dispositivos[0].PrecioParaAlquiler * 2 ) + (dispositivos[1].PrecioParaAlquiler*1 );
            

            _context.Users.Add(user);
            _context.Add(alquiler);
            _context.SaveChanges();

        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDetalleAlquilerNotFound_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;
            var controller = new AlquileresController(_context, logger);

            // Act
            var result = await controller.GetDetalleAlquiler(0);

            // Assert
            // Compruebo si no lo encuentra
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetDetalleAlquilerFound_Test()
        {
            // Arrange
            var mock = new Mock<ILogger<AlquileresController>>();
            ILogger<AlquileresController> logger = mock.Object;
            var controller = new AlquileresController(_context, logger);

            // Obtener la fecha de compra del contexto para la comparación
            var Alquilerdb = _context.Alquiler.First();
            var fechaAlquiler = Alquilerdb.FechaAlquiler;

            var alquilerEsperado = new AlquilerDetailDTO(
                Alquilerdb.Id,
                fechaAlquiler,
                "Juan",
                "Pérez García",
                "Calle Mayor 123, Madrid",
                TiposMetodoPago.TarjetaCredito,
                Alquilerdb.FechaAlquilerDesde,
                Alquilerdb.FechaAlquilerHasta,
                new List<ItemAlquilerDTO>()
            );


            alquilerEsperado.ItemsAlquiler.Add(new ItemAlquilerDTO(
                Alquilerdb.ItemsAlquiler[0].Dispositivo.Id,
                Alquilerdb.ItemsAlquiler[0].Dispositivo.Modelo.NombreModelo,
                Alquilerdb.ItemsAlquiler[0].Dispositivo.NombreDispositivo,
                Alquilerdb.ItemsAlquiler[0].Dispositivo.Marca,
                Alquilerdb.ItemsAlquiler[0].Dispositivo.PrecioParaAlquiler
            ));

            alquilerEsperado.ItemsAlquiler.Add(new ItemAlquilerDTO(
                Alquilerdb.ItemsAlquiler[1].Dispositivo.Id,
                Alquilerdb.ItemsAlquiler[1].Dispositivo.Modelo.NombreModelo,
                Alquilerdb.ItemsAlquiler[1].Dispositivo.NombreDispositivo,
                Alquilerdb.ItemsAlquiler[1].Dispositivo.Marca,
                Alquilerdb.ItemsAlquiler[1].Dispositivo.PrecioParaAlquiler
            ));

            // Act
            // Llamamos al System Under Test (SUT)
            var result = await controller.GetDetalleAlquiler(1);

            // Assert
            // Verificamos que el response type es OK y obtenemos la compra
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verificamos que el tipo retornado es igual al esperado
            var alquilerDTOActual = Assert.IsType<AlquilerDetailDTO>(okResult.Value);

            // Verificamos que los datos esperados y actuales son los mismos
            Assert.Equal(alquilerEsperado, alquilerDTOActual);
        }


    }
}
