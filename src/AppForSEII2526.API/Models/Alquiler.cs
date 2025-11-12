using AppForSEII2526.API.DTOs.AlquilerDTOs;
using System;
using System.Runtime.CompilerServices;

public class Alquiler
{
    public int Id { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
    [Display(Name = "Dirección de entrega")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduce una dirección")]
    public string DireccionEntrega { get; set; }

    public TiposMetodoPago MetodoPago { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de alquiler")]
    public DateTime FechaAlquiler { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de alqulier desde")]
    public DateTime FechaAlquilerDesde { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
    [Required, DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Fecha de alquiler hasta")]
    public DateTime FechaAlquilerHasta { get; set; }

    [Required]
    [DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    [Range(0.5, float.MaxValue, ErrorMessage = "El precio mínimo es de 0,5 ")]
    [Display(Name = "Precio para compra")]
    [Precision(10, 2)]
    public double PrecioTotal { get; set; }

    public ApplicationUser ApplicationUser { get; set; }

    [Required]
    public IList<ItemAlquiler> ItemsAlquiler { get; set; }

    public Alquiler()
    {
        ItemsAlquiler = new List<ItemAlquiler>();
    }

    public Alquiler(string direccionEntrega, string nombreUsuario, string apellidosUsuario,
        ApplicationUser? user, DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta,
        TiposMetodoPago metodoPago, List<ItemAlquiler> itemsAlquilers)
    {
        DireccionEntrega = direccionEntrega;
        ApplicationUser = user;  // Asignar a la propiedad
        FechaAlquiler = DateTime.Now;  // Inicializar la fecha de alquiler
        FechaAlquilerDesde = fechaAlquilerDesde;
        FechaAlquilerHasta = fechaAlquilerHasta;
        MetodoPago = metodoPago;  // Asignar a la propiedad
        ItemsAlquiler = itemsAlquilers;  // Asignar a la propiedad
    }

    public override bool Equals(object? obj)
    {
        return obj is Alquiler alquiler &&
               Id == alquiler.Id &&
               DireccionEntrega == alquiler.DireccionEntrega &&
               MetodoPago == alquiler.MetodoPago &&
               FechaAlquiler.Day == alquiler.FechaAlquiler.Day &&
               FechaAlquilerDesde.Day == alquiler.FechaAlquilerDesde.Day &&
               FechaAlquilerHasta.Day == alquiler.FechaAlquilerHasta.Day &&
               PrecioTotal == alquiler.PrecioTotal &&
               EqualityComparer<IList<ItemAlquiler>>.Default.Equals(ItemsAlquiler, alquiler.ItemsAlquiler);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, DireccionEntrega, MetodoPago, FechaAlquiler, FechaAlquilerDesde, FechaAlquilerHasta, PrecioTotal, ItemsAlquiler);
    }
}