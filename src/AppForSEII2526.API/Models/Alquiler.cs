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
    public DateTime FechaAlquiler {  get; set; }

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
    public IList<AlquilarDispositivo> DispositivosAlquilados { get; set; }


    public Alquiler()
    {
    }

    public Alquiler(int id, string direccionEntrega, TiposMetodoPago metodoPago, DateTime fechaAlquiler, DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, double precioTotal, IList<AlquilarDispositivo> dispositivosAlquilados)
    {
        Id = id;
        DireccionEntrega = direccionEntrega;
        MetodoPago = metodoPago;
        FechaAlquiler = fechaAlquiler;
        FechaAlquilerDesde = fechaAlquilerDesde;
        FechaAlquilerHasta = fechaAlquilerHasta;
        PrecioTotal = precioTotal;
        DispositivosAlquilados = dispositivosAlquilados;
    }

    public override bool Equals(object? obj)
    {
        return obj is Alquiler alquiler &&
               Id == alquiler.Id &&
               DireccionEntrega == alquiler.DireccionEntrega &&
               MetodoPago == alquiler.MetodoPago &&
               FechaAlquiler == alquiler.FechaAlquiler &&
               FechaAlquilerDesde == alquiler.FechaAlquilerDesde &&
               FechaAlquilerHasta == alquiler.FechaAlquilerHasta &&
               PrecioTotal == alquiler.PrecioTotal &&
               EqualityComparer<IList<AlquilarDispositivo>>.Default.Equals(DispositivosAlquilados, alquiler.DispositivosAlquilados);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, DireccionEntrega, MetodoPago, FechaAlquiler, FechaAlquilerDesde, FechaAlquilerHasta, PrecioTotal, DispositivosAlquilados);
    }
}
