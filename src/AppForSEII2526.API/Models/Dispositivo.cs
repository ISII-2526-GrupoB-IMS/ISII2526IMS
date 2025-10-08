
namespace AppForSEII2526.API.Models
{
    public class Dispositivo
    {
        //ID Dispositivo
        [Key]
        public int Id { get; set; }

        //MODELO
        [Required]
        public Modelo Modelo { get; set; }

        //MARCA
        [Required]
        [StringLength(50, ErrorMessage = "La marca no puede ser mayor de 50 caracteres")]
        public string Marca { get; set; }

        //COLOR
        [Required]
        [StringLength(20, ErrorMessage = "El color no puede ser mayor de 20 caracteres")]
        public string Color { get; set; }

        //NOMBRE DISPOSITIVO
        [Required]
        [StringLength(50, ErrorMessage = "El nombre del dispositivo no puede ser mayor de 50 caracteres")]
        public string NombreDispositivo { get; set; }

        //PRECIO COMPRA
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
        [Display(Name = "Precio para compra")]
        [Precision(10, 2)]
        public double PrecioParaCompra { get; set; }

        //CANTIDAD PARA COMPRA
        [Required]
        [Display(Name = "Cantidad para comprar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima para comprar es 1")]
        public int CantidadParaCompra { get; set; }

        //AÑO
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }

        public IList<ItemCompra> ItemsCompra { get; set; }

        public IList<AlquilarDispositivo> DispositivosAlquilados { get; set; }

    }

}
