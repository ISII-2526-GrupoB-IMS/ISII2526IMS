using System;
using System.Runtime.CompilerServices;

public class Alquiler
{


    public int Id { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
    [Display(Name = "Dirección de entrega")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduce una dirección")]
    public string DireccionEntrega { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
    [Display(Name = "Nombre")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduce un nombre")]
    public string Nombre { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
    [Display(Name = "Apellido")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Porfavor, introduce un apellido")]
    public string Apellidos { get; set; }
    
    public TiposMetodoPago MetodoPago { get; set; }
    public enum TiposMetodoPago
    {
        TarjetaCredito,
        PayPal,
        Efectivo
    }

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

    [Required]
    public AlquilarDispositivo alquilarDispositivo { get; set; }


    public Alquiler()
    {
    }

    public Alquiler(int id, string direccionEntrega, string nombre, string apellidos, TiposMetodoPago metodoPago, DateTime fechaAlquiler, DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, double precioTotal, AlquilarDispositivo alquilarDispositivo)
    {
        Id = id;
        DireccionEntrega = direccionEntrega;
        Nombre = nombre;
        Apellidos = apellidos;
        MetodoPago = metodoPago;
        FechaAlquiler = fechaAlquiler;
        FechaAlquilerDesde = fechaAlquilerDesde;
        FechaAlquilerHasta = fechaAlquilerHasta;
        PrecioTotal = precioTotal;
        this.alquilarDispositivo = alquilarDispositivo;
    }

    public override bool Equals(object? obj)
    {
        return obj is Alquiler alquiler &&
               Id == alquiler.Id &&
               DireccionEntrega == alquiler.DireccionEntrega &&
               Nombre == alquiler.Nombre &&
               Apellidos == alquiler.Apellidos &&
               MetodoPago == alquiler.MetodoPago &&
               FechaAlquiler == alquiler.FechaAlquiler &&
               FechaAlquilerDesde == alquiler.FechaAlquilerDesde &&
               FechaAlquilerHasta == alquiler.FechaAlquilerHasta &&
               PrecioTotal == alquiler.PrecioTotal &&
               EqualityComparer<AlquilarDispositivo>.Default.Equals(alquilarDispositivo, alquiler.alquilarDispositivo);
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(Id);
        hash.Add(DireccionEntrega);
        hash.Add(Nombre);
        hash.Add(Apellidos);
        hash.Add(MetodoPago);
        hash.Add(FechaAlquiler);
        hash.Add(FechaAlquilerDesde);
        hash.Add(FechaAlquilerHasta);
        hash.Add(PrecioTotal);
        hash.Add(alquilarDispositivo);
        return hash.ToHashCode();
    }
}
