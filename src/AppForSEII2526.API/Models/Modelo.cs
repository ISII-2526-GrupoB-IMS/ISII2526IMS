using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Models
{
    public class Modelo
    {


        //ID Modleo
        [Key] //Clave Primaria
        public int Id { get; set; }

        //NOMBRE MODELO
        [Required]
        [StringLength(50, ErrorMessage = "El nombre del modelo no puede ser mayor de 50 caracteres")]
        public string NombreModelo { get; set; }


        public IList<Dispositivo> Dispositivos { get; set; }
    }
}
