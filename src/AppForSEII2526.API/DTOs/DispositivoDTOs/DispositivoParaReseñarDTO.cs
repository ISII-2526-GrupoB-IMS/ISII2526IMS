namespace AppForSEII2526.API.DTOs.DispositivoDTOs
{
    public class DispositivoParaReseñarDTO
    {
        public DispositivoParaReseñarDTO(int id, string nombreDispositivo, string marca, string color, double año, Modelo modelo)
        {
            Id = id;
            NombreDispositivo = nombreDispositivo;
            Marca = marca;
            Color = color;
            Año = año;
            Modelo = modelo;
        }

        //ID Dispositivo
        [Key]
        public int Id { get; set; }

        //NOMBRE DISPOSITIVO
        [Required]
        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }

        //MARCA
        [Required]
        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }

        //COLOR
        [Required]
        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres")]
        public string Color { get; set; }

        //AÑO
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }

        //MODELO
        [Required]
        public Modelo Modelo { get; set; }

        
    }
}