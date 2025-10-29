namespace AppForSEII2526.API.DTOs.DispositivoDTOs
{
    public class DispositivoParaAlquilarDTO
    {
        public DispositivoParaAlquilarDTO(int id, Modelo modelo, string nombreDispositivo, string marca, double año, string color)
        {
            Id = id;
            NombreDispositivo = nombreDispositivo;
            Marca = marca;
            Color = color;
            Año = año;     
            Modelo = modelo;
        }

        public int Id { get; set; }

        //MODELO

        public Modelo Modelo { get; set; }

        //MARCA

        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }


        //COLOR

        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres")]
        public string Color { get; set; }

        //NOMBRE DISPOSITIVO

        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }




    }
}