using AppForSEII2526.UIT.Shared;
using AppForSEII2526.UIT.UC_Reviews;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Reviews
{
    public class UC_Reviews_UIT : UC_UIT
    {
        private SelectDispositivosReview_PO _selectPO;
        private CrearReview_PO _crearPO;
        private DetalleReview_PO _detallePO;

        public UC_Reviews_UIT(ITestOutputHelper output) : base(output)
        {
            _selectPO = new SelectDispositivosReview_PO(_driver, _output);
            _crearPO = new CrearReview_PO(_driver, _output);
            _detallePO = new DetalleReview_PO(_driver, _output);
        }

        private void InitialStepsForReview()
        {
            // Navegar a la URL del catálogo de reseñas
            _driver.Navigate().GoToUrl(_URI + "Reviews/SelectDispositivosReview");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_1_FlujoBasico_ReseñaExitosa()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();

            string marcaBusqueda = "Apple";
            string dispositivo1 = "iPhone 14 Pro 256GB";
            string dispositivo2 = "iPhone 14 Pro 512GB";

            // Datos del formulario
            string tituloReseña = "Excelente experiencia con iPhone";
            string paisReseña = "España";
            string nombreAutor = "Juan";
            string comentario = "Review para mis dispositivos favoritos";

            // --- 2. ACT ---

            // A. Buscar dispositivos por marca
            _selectPO.SearchDispositivos(marcaBusqueda, "");

            // B. Añadir dos dispositivos a la selección
            _selectPO.AddDispositivoToReview(dispositivo1);
            _selectPO.AddDispositivoToReview(dispositivo2);

            // C. Ir a la pantalla de Finalizar Reseña
            _selectPO.ClickReseñarDispositivos();

            // D. Rellenar cabecera y detalles de la tabla
            _crearPO.RellenarCabecera(tituloReseña, paisReseña, nombreAutor);
            _crearPO.RellenarDetalleDispositivo(dispositivo1, "5", comentario);
            _crearPO.RellenarDetalleDispositivo(dispositivo2, "4", comentario);

            // E. Publicar
            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---

            // Verificamos que estamos en la página de éxito
            Assert.True(_detallePO.VerificarExitoPublicacion(), "No se mostró el banner de éxito.");

            // Verificamos que el autor y ubicación coinciden
            Assert.True(_detallePO.VerificarDatosAutor(nombreAutor, paisReseña), "Los datos del autor no coinciden.");

            // Verificamos que los dispositivos aparecen valorados (con su calificacion X/5)
            Assert.True(_detallePO.VerificarDispositivoValorado(dispositivo1, "(5/5)", comentario));
            Assert.True(_detallePO.VerificarDispositivoValorado(dispositivo2, "(4/5)", comentario));

            // Verificamos que el registro está sincronizado
            Assert.True(_detallePO.VerificarSincronizacion(), "El registro no aparece como sincronizado.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_2_Error_Puntuacion_Fuera_Rango()
        {
            // --- ARRANGE ---
            InitialStepsForReview();
            _selectPO.SearchDispositivos("Apple", "");
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // --- ACT ---
            // Intentamos poner una puntuación de 0 (inválida)
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "0", "Review para probar error");
            _crearPO.ClickPublicarReseña();

            // --- ASSERT ---
            // Verificamos que sale el mensaje de error específico configurado en el DTO
            string mensajeEsperado = "must be between 1 and 5";
            Assert.True(_crearPO.ExisteMensajeDeError(mensajeEsperado),
                "Debería aparecer el error de validación por puntuación 0.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_3_GestionCarrito_Vaciar()
        {
            // --- ARRANGE ---
            InitialStepsForReview();
            _selectPO.SearchDispositivos("Apple", "");
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");

            // --- ACT ---
            _selectPO.VaciarCarrito();

            // --- ASSERT ---
            // Verificamos que el botón de reseñar se deshabilita o el contador vuelve a 0
            Assert.True(_selectPO.IsReseñarButtonDisabled(), "El botón de reseñar debería estar deshabilitado tras vaciar.");
        }

        [Theory]
        [InlineData("Apple", "2023", "iPhone 14 Pro 256GB")]
        [InlineData("Samsung", "2023", "Galaxy S23 Ultra 512GB")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_4_Filtrar_Marca_y_Año(string marca, string año, string nombreEsperado)
        {
            // --- ARRANGE ---
            InitialStepsForReview();

            var expected = new List<string[]> { new string[] { nombreEsperado, año } };

            // --- ACT ---
            _selectPO.SearchDispositivos(marca, año);

            // --- ASSERT ---
            Assert.True(_selectPO.CheckListOfDispositivos(expected),
                $"No se encontró el dispositivo {nombreEsperado} con los filtros {marca}/{año}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_5_Error_Comentario_Formato_Incorrecto()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            _crearPO.RellenarCabecera("Reseña Test", "España", "Juan");

            // --- 2. ACT ---
            // Ponemos un texto que NO cumple la expresión regular
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "pepe");

            // Forzamos el desenfoque del campo y clicamos en publicar
            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---
            // Buscamos el fragmento clave del mensaje definido en el DTO
            string mensajeEsperado = "debe empezar por Review para";

            bool errorDetectado = _crearPO.ExisteMensajeDeError(mensajeEsperado);

            Assert.True(errorDetectado, $"No se encontró el mensaje de error: '{mensajeEsperado}'. Asegúrate de haber añadido el RegularExpression al DTO.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_6_Error_Comentario_Vacio()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            _crearPO.RellenarCabecera("Reseña", "España", "Juan");

            // --- 2. ACT ---
            // Dejamos el comentario vacío (null o string vacío)
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "");

            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---
            // Verificamos el mensaje que aparece en el banner rojo superior según tu imagen
            string mensajeEsperado = "El comentario no puede estar vacío";

            Assert.True(_crearPO.ExisteMensajeDeError(mensajeEsperado),
                "No apareció el error de validación cuando el comentario está vacío.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_7_Error_Usuario_No_Registrado()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // Rellenamos el título y país correctamente
            _crearPO.RellenarCabecera("Mi Reseña", "España", "Augusto"); // "Augusto" no existe en el sistema

            // Rellenamos detalles válidos (puntuación 1 y comentario con formato correcto)
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "Review para mi móvil");

            // --- 2. ACT ---
            // Al publicar, el frontend enviará los datos, pero la API devolverá un error 400 o similar
            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---
            // Verificamos el mensaje exacto que sale en tu banner rojo superior
            string mensajeEsperado = "Usuario no registrado";

            Assert.True(_crearPO.ExisteMensajeDeError(mensajeEsperado),
                "No se detectó el error de servidor cuando el usuario no está registrado.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_8_Error_Pais_Vacio()
        {
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // Rellenamos todo menos el país
            _crearPO.RellenarCabecera("Título", "", "Juan");
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "Review para nada");

            _crearPO.ClickPublicarReseña();

            // Verificamos el error del banner superior según tu imagen
            Assert.True(_crearPO.ExisteMensajeDeError("El país es un campo obligatorio"));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_10_Error_Titulo_Vacio()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // Dejamos el título vacío, pero rellenamos el resto de la cabecera correctamente
            _crearPO.RellenarCabecera("", "España", "Juan");

            // Rellenamos el detalle correctamente para que el único error sea el título
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "5", "Review para mi móvil");

            // --- 2. ACT ---
            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---
            // Según las validaciones estándar de Blazor/DataAnnotations
            string mensajeEsperado = "El título es un campo obligatorio";
            // Nota: Si en tu DTO el mensaje es diferente (ej: "The Titulo field is required"), cámbialo aquí.

            Assert.True(_crearPO.ExisteMensajeDeError(mensajeEsperado),
                "No apareció el error de validación cuando el título de la reseña está vacío.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_11_Error_Titulo_Demasiado_Largo()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // --- 2. ACT ---
            // Generamos un título de 101 caracteres para forzar el error
            string tituloLargo = "Reseña" + new string('q', 110);
            _crearPO.RellenarCabecera(tituloLargo, "España", "Juan");
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "Review para nada");

            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---

            // Verificamos el mensaje técnico del DTO (que sale en rojo bajo el input y en el banner superior)
            bool errorLongitud = _crearPO.ExisteMensajeDeError("maximum length of 100");

            // Verificamos el mensaje genérico de tu método ShowValidationErrors (el banner rosado de tu imagen)
            bool errorGenerico = _crearPO.ExisteMensajeDeError("Hay errores en el formulario");

            Assert.True(errorLongitud, "No apareció el error técnico de 'maximum length of 100'.");
            Assert.True(errorGenerico, "No apareció el mensaje genérico 'Hay errores en el formulario'.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_12_Error_Pais_Excede_Longitud()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // --- 2. ACT ---
            // Generamos un nombre de país de más de 50 caracteres (según tu imagen image_778529.png)
            string paisLargo = "España" + new string('a', 55);

            _crearPO.RellenarCabecera("Reseña Válida", paisLargo, "Juan");
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "Review para nada");

            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---

            // Verificamos el mensaje técnico del DTO que aparece en rojo
            bool errorLongitudPais = _crearPO.ExisteMensajeDeError("The field Pais must be a string with a maximum length of 50");

            // Verificamos el mensaje genérico del banner rosado
            bool errorGenericoForm = _crearPO.ExisteMensajeDeError("Hay errores en el formulario");

            Assert.True(errorLongitudPais, "No se detectó el error técnico: 'maximum length of 50' para el país.");
            Assert.True(errorGenericoForm, "No apareció el banner genérico de errores en el formulario.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_13_Error_NombreAutor_Excede_Longitud()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // --- 2. ACT ---
            // Generamos un nombre de autor de más de 50 caracteres
            string nombreLargo = "Juan" + new string('x', 55);

            // Rellenamos el título y país correctamente, pero el nombre demasiado largo
            _crearPO.RellenarCabecera("Reseña Test", "España", nombreLargo);
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", "Review para mi móvil");

            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---

            // 1. Verificamos el mensaje técnico del DTO (nombre del campo según tu DTO, ej: NombreAutor o Nombre)
            bool errorLongitudNombre = _crearPO.ExisteMensajeDeError("maximum length of 50");

            // 2. Verificamos el mensaje genérico que sale en el banner rosado de tu código Blazor
            bool errorGenerico = _crearPO.ExisteMensajeDeError("Hay errores en el formulario");

            Assert.True(errorLongitudNombre, "No apareció el error de validación de longitud para el nombre del autor.");
            Assert.True(errorGenerico, "No apareció el banner genérico de 'Hay errores en el formulario'.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_14_Error_Comentario_Excede_Longitud()
        {
            // --- 1. ARRANGE ---
            InitialStepsForReview();
            _selectPO.AddDispositivoToReview("iPhone 14 Pro 256GB");
            _selectPO.ClickReseñarDispositivos();

            // --- 2. ACT ---
            // Generamos un comentario de más de 300 caracteres (según tu imagen image_77e5aa.png)
            string comentarioMuyLargo = "Review para " + new string('a', 305);

            _crearPO.RellenarCabecera("Reseña Válida", "España", "Juan");
            _crearPO.RellenarDetalleDispositivo("iPhone 14 Pro 256GB", "1", comentarioMuyLargo);

            _crearPO.ClickPublicarReseña();

            // --- 3. ASSERT ---


            // Verificamos el mensaje genérico del banner rosado
            bool errorGenericoForm = _crearPO.ExisteMensajeDeError("Error de validación del servidor");

            Assert.True(errorGenericoForm, "No apareció el banner genérico de errores en el formulario.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void CU_Rev_15_Examen_Sprint3()
        {
            
            InitialStepsForReview();
            Thread.Sleep(1000);

            string marcaBusqueda = "Apple";
            string dispositivo1 = "iPhone 14 Pro 256GB";
            string dispositivo2 = "iPhone 14 Pro 512GB";

            // Datos del formulario
            string tituloReseña = "Excelente experiencia con iPhone";
            string paisReseña = "España";
            string nombreAutor = "Juan";
            string comentario = "Review para el iPhone 14 Pro 512 GB: muy bueno";

            _selectPO.AddDispositivoToReview(dispositivo1);

            _selectPO.SearchDispositivos(marcaBusqueda, "");

            _selectPO.AddDispositivoToReview(dispositivo2);

            _selectPO.RemoveDispositivoFromReview(dispositivo1);

            _selectPO.ClickReseñarDispositivos();

            _crearPO.RellenarCabecera(tituloReseña, paisReseña, nombreAutor);

            _crearPO.RellenarDetalleDispositivo(dispositivo2, "4", comentario);

            _crearPO.ClickPublicarReseña();

            // Verificamos que estamos en la página de éxito
            Assert.True(_detallePO.VerificarExitoPublicacion(), "No se mostró el banner de éxito.");

            // Verificamos que el autor y ubicación coinciden
            Assert.True(_detallePO.VerificarDatosAutor(nombreAutor, paisReseña), "Los datos del autor no coinciden.");

            // Verificamos que los dispositivos aparecen valorados (con su calificacion X/5)
            Assert.True(_detallePO.VerificarDispositivoValorado(dispositivo2, "(4/5)", comentario));

            // Verificamos que el registro está sincronizado
            Assert.True(_detallePO.VerificarSincronizacion(), "El registro no aparece como sincronizado.");
        }
    }
}

