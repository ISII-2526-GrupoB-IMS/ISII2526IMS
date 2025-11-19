

namespace AppForSEII2526.API.Models
{
    public class Dispositivo
    {
        //Constructor vacío
        public Dispositivo()
        {
        }
        public Dispositivo(Modelo modelo, string marca, string color, string nombreDispositivo, double precioParaCompra, double precioParaAlquiler, int cantidadParaCompra, int cantidadParaAlquilar, double año)
        {
            Modelo = modelo;
            Marca = marca;
            Color = color;
            NombreDispositivo = nombreDispositivo;
            PrecioParaCompra = precioParaCompra;
            PrecioParaAlquiler = precioParaAlquiler;
            CantidadParaCompra = cantidadParaCompra;
            CantidadParaAlquilar = cantidadParaAlquilar;
            Año = año;
        }

        public Dispositivo(Modelo modelo, string marca, string color, double precioParaCompra, int cantidadParaCompra)
        {
            Modelo = modelo;
            Marca = marca;
            Color = color;
            PrecioParaCompra = precioParaCompra;
            CantidadParaCompra = cantidadParaCompra;
        }







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

        //PRECIO Alquiler
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
        [Display(Name = "Precio para Alquilar")]
        [Precision(10, 2)]
        public double PrecioParaAlquiler { get; set; }

        //CANTIDAD PARA COMPRA
        [Required]
        [Display(Name = "Cantidad para comprar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima para comprar es 1")]
        public int CantidadParaCompra { get; set; }

        //CANTIDAD PARA Alquilar
        [Required]
        [Display(Name = "Cantidad para alquilar")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima para alquilar es 1")]
        public int CantidadParaAlquilar{ get; set; }

        //AÑO
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Range(1998, 2025, ErrorMessage = "El año mínimo es 1998 y el máximo 2025")]
        [Display(Name = "Año")]
        public double Año { get; set; }

        //CALIDAD
        public TipoDeCalidad Calidad { get; set; }
        public enum TipoDeCalidad
        {
            Buena,
            Regular,
            Mala
        }

        public IList<ItemCompra> ItemsCompra { get; set; }

        public IList<ItemAlquiler> ItemsAlquiler { get; set; }
        public IList<ItemReseña> ItemsReseña { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Dispositivo dispositivo &&
                   Id == dispositivo.Id &&
                   EqualityComparer<Modelo>.Default.Equals(Modelo, dispositivo.Modelo) &&
                   Marca == dispositivo.Marca &&
                   Color == dispositivo.Color &&
                   NombreDispositivo == dispositivo.NombreDispositivo &&
                   PrecioParaCompra == dispositivo.PrecioParaCompra &&
                   PrecioParaAlquiler == dispositivo.PrecioParaAlquiler &&
                   CantidadParaCompra == dispositivo.CantidadParaCompra &&
                   CantidadParaAlquilar == dispositivo.CantidadParaAlquilar &&
                   Año == dispositivo.Año &&
                   Calidad == dispositivo.Calidad &&
                   EqualityComparer<IList<ItemCompra>>.Default.Equals(ItemsCompra, dispositivo.ItemsCompra) &&
                   EqualityComparer<IList<ItemAlquiler>>.Default.Equals(ItemsAlquiler, dispositivo.ItemsAlquiler) &&
                   EqualityComparer<IList<ItemReseña>>.Default.Equals(ItemsReseña, dispositivo.ItemsReseña);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(Modelo);
            hash.Add(Marca);
            hash.Add(Color);
            hash.Add(NombreDispositivo);
            hash.Add(PrecioParaCompra);
            hash.Add(PrecioParaAlquiler);
            hash.Add(CantidadParaCompra);
            hash.Add(CantidadParaAlquilar);
            hash.Add(Año);
            hash.Add(Calidad);
            hash.Add(ItemsCompra);
            hash.Add(ItemsAlquiler);
            hash.Add(ItemsReseña);
            return hash.ToHashCode();
        }
    }

}
