using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Models
{
    [Index(nameof(NombreModelo), IsUnique = true)] //Nombre es UNIQUE
    public class Modelo
    {
        public Modelo()
        {
        }

        public Modelo(string nombreModelo)
        {
            NombreModelo = nombreModelo;
        }

        //ID Modleo
        [Key] //Clave Primaria
        public int Id { get; set; }

        //NOMBRE MODELO
        [Required]
        [StringLength(50, ErrorMessage = "El nombre del modelo no puede ser mayor de 50 caracteres")]
        public string NombreModelo { get; set; }


        public IList<Dispositivo> Dispositivos { get; set; } = new List<Dispositivo>();
    }
}
