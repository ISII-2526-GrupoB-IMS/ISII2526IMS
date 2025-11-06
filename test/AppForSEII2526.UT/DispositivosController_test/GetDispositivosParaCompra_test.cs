using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DispositivoDTOs;

namespace AppForSEII2526.UT.DispositivosController_test
{
    public class GetDispositivosParaCompra_test : AppForMovies.UT.AppForMovies4SqliteUT
    {
        public GetDispositivosParaCompra_test()
        {

            //CREAR LOS MODELOS NECESARIOS PARA LAS PRUEBAS

            var modelos = new List<Modelo>()
            {
                new Modelo { NombreModelo = "iPhone 14 Pro" },
                new Modelo { NombreModelo = "Galaxy S23" },
                new Modelo { NombreModelo = "Pixel 8" }
            };

            //CREAR LOS DISPOSITIVOS NECESARIOS PARA LAS PRUEBAS

            var dispositivos = new List<Dispositivo>() {

                new Dispositivo (modelos[0],"Apple","Negro","iPhone 14 Pro 256GB",1199.99,45.00,5,10,2023),
                new Dispositivo (modelos[0],"Apple","Plata","iPhone 14 Pro 512GB",1399.99,50.00,3,8,2023),
                new Dispositivo (modelos[1],"Samsung","Negro","Galaxy S23 Ultra 256GB",1099.99,42.00,6,15,2023),
                new Dispositivo (modelos[1],"Samsung","Blanco","Galaxy S23 Ultra 512GB",1299.99,48.00,4,12,2023),
                new Dispositivo (modelos[2],"Google","Gris","Pixel 8 Pro 128GB",899.99,38.00,8,16,2024),
                // Este dispositivo tiene CantidadParaCompra = 0, no debería aparecer en compras
                new Dispositivo (modelos[2],"OnePlus","Verde","OnePlus 11 5G 256GB",849.99,37.00,0,5,2023),

            };
            _context.AddRange(modelos);
            _context.AddRange(dispositivos);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetDispositivosParaComprar_OK()
        {

            var dispositivoDTOs = new List<DispositivoParaComprarDTO>()
            {
                new DispositivoParaComprarDTO(1, "iPhone 14 Pro 256GB", "Apple",
                    new Modelo { Id = 1, NombreModelo = "iPhone 14 Pro" }, "Negro", 1199.99),
                new DispositivoParaComprarDTO(2, "iPhone 14 Pro 512GB", "Apple",
                    new Modelo { Id = 1, NombreModelo = "iPhone 14 Pro" }, "Plata", 1399.99),
                new DispositivoParaComprarDTO(3, "Galaxy S23 Ultra 256GB", "Samsung",
                    new Modelo { Id = 2, NombreModelo = "Galaxy S23" }, "Negro", 1099.99),
                new DispositivoParaComprarDTO(4, "Galaxy S23 Ultra 512GB", "Samsung",
                    new Modelo { Id = 2, NombreModelo = "Galaxy S23" }, "Blanco", 1299.99),
                new DispositivoParaComprarDTO(5, "Pixel 8 Pro 128GB", "Google",
                    new Modelo { Id = 3, NombreModelo = "Pixel 8" }, "Gris", 899.99)
            };


            // TC1: Sin filtros: devuelve todos menos OnePlus que tiene cantidad para compra =0
            var dispositivosTC1 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[0], dispositivoDTOs[1], dispositivoDTOs[2],
                dispositivoDTOs[3], dispositivoDTOs[4]
            };

            // TC2: Filtro por nombre "iPhone": devuelve solo los iPhones
            var dispositivosTC2 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[0], dispositivoDTOs[1]
            };

            // TC3: Filtro por color "Negro": devuelve los dispositivos negros
            var dispositivosTC3 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[0], dispositivoDTOs[2]
            };

            // TC4: Filtro por nombre "Galaxy":devuelve los Galaxy
            var dispositivosTC4 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[2], dispositivoDTOs[3]
            };

            // TC5: Filtro por nombre "Galaxy" y color "Negro"
            var dispositivosTC5 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[2]
            };

            // TC6: Filtro por nombre que no existe: "Nokia"
            var dispositivosTC6 = new List<DispositivoParaComprarDTO>();

            // TC7: Filtro por color que no existe en nuestros dispositivos 
            var dispositivosTC7 = new List<DispositivoParaComprarDTO>();

            // TC8: Filtro parcial, es decir, sin completar la palabra ("Pro")
            var dispositivosTC8 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[0], dispositivoDTOs[1], dispositivoDTOs[4]
            };

            // TC9: Filtro por color "Plata": devuelve los dispositivos plpata
            var dispositivosTC9 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[1]
            };

            // TC10: Filtro por nombre "Pixel"
            var dispositivosTC10 = new List<DispositivoParaComprarDTO>()
            {
                dispositivoDTOs[4]
            };

            var allTests = new List<object[]>
            {
                // Filtros: filtroNombre, filtroColor, DispositivosEsperados
                new object[] { null, null, dispositivosTC1 },                    // Sin filtros
                new object[] { "iPhone", null, dispositivosTC2 },                // Solo nombre: iPhone
                new object[] { null, "Negro", dispositivosTC3 },                 // Solo color: Negro
                new object[] { "Galaxy", null, dispositivosTC4 },                // Solo nombre: Galaxy
                new object[] { "Galaxy", "Negro", dispositivosTC5 },             // Ambos filtros
                new object[] { "Nokia", null, dispositivosTC6 },                 // Nombre inexistente
                new object[] { null, "Rojo", dispositivosTC7 },                  // Color inexistente
                new object[] { "Pro", null, dispositivosTC8 },                   // Búsqueda parcial
                new object[] { null, "Plata", dispositivosTC9 },                 // Color: Plata
                new object[] { "Pixel", null, dispositivosTC10 },                // Nombre: Pixel
            };

            return allTests;


        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetDispositivosParaComprar_OK))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetDispositivosParaComprar_Filtros_Test(
           string? filtroNombre,
           string? filtroColor,
           IList<DispositivoParaComprarDTO> dispositivosEsperados)
        {
            // Arrange

            var controller = new DispositivoController(_context, null);

            // Act
            var result = await controller.GetDispositivosParaComprar(filtroNombre, filtroColor);

            // Assert
            // Verificamos que el response type es OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            // Obtenemos la lista de dispositivos
            var dispositivoDTOsActual = Assert.IsType<List<DispositivoParaComprarDTO>>(okResult.Value);

            //Comprobamos que el esperado y el actual son iguales

            for (int i = 0; i < dispositivosEsperados.Count; i++)
            {
                Assert.Equal(dispositivosEsperados[i].Id, dispositivoDTOsActual[i].Id);
                Assert.Equal(dispositivosEsperados[i].NombreDispositivo, dispositivoDTOsActual[i].NombreDispositivo);
                Assert.Equal(dispositivosEsperados[i].Marca, dispositivoDTOsActual[i].Marca);
                Assert.Equal(dispositivosEsperados[i].Color, dispositivoDTOsActual[i].Color);
                Assert.Equal(dispositivosEsperados[i].PrecioParaCompra, dispositivoDTOsActual[i].PrecioParaCompra);
                Assert.Equal(dispositivosEsperados[i].Modelo.Id, dispositivoDTOsActual[i].Modelo.Id);
                Assert.Equal(dispositivosEsperados[i].Modelo.NombreModelo, dispositivoDTOsActual[i].Modelo.NombreModelo);
            }



        }

    }

}










    

