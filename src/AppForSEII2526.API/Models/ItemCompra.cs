using NuGet.DependencyResolver;

namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(IdDispositivo), nameof(IdCompra))]
    public class ItemCompra
    {

        //DESCRIPCIÓN
        [Required]
        [StringLength(150, ErrorMessage = "La descripción no puede ser superior a 150 carecteres")]
        public string? Descripcion { get; set; }

        //Dispositivo
        [Required]
        public Dispositivo Dispositivo { get; set; }
        //Id Dispositivo
        [Required]
        public int IdDispositivo { get; set; }

        //COMPRA
        [Required]
        public Compra Compra { get; set; }
        //Id Compra
        [Required]
        public int IdCompra { get; set; }

        //PRECIO 
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
        [Display(Name = "Precio ")]
        [Precision(10, 2)]
        public double Precio { get; set; }

        //CANTIDAD
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debes proporcionar una cantidad válida mayor de 1")]
        public int Cantidad { get; set; }


    }

}
