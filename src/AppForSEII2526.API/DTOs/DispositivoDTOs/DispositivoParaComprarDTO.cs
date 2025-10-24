namespace AppForSEII2526.API.DTOs.DispositivoDTOs
{
    public class DispositivoParaComprarDTO
    {
        public DispositivoParaComprarDTO(int id, string nombreDispositivo, string marca, Modelo modelo, string color, double precioParaCompra)
        {
            Id = id;
            NombreDispositivo = nombreDispositivo;
            Marca = marca;
            Modelo = modelo;
            Color = color;
            PrecioParaCompra = precioParaCompra;
        }

        public int Id { get; set; }

        //NOMBRE DISPOSITIVO
        [Required]
        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }

        //MARCA
        [Required]
        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }

        //MODELO
        [Required]
        public Modelo Modelo { get; set; }

        //COLOR
        [Required]
        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres")]
        public string Color { get; set; }
        //PRECIO COMPRA
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
        [Display(Name = "Precio para compra")]
        [Precision(10, 2)]
        public double PrecioParaCompra { get; set; }
    }
}
