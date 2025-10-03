using System;
using System.Runtime.CompilerServices;

public class Alquilar
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
    public DateTime FechaAlquiler {  get; set; }
    public DateTime FechaAlquilerDesde { get; set; }
    public DateTime FechaAlquilerHasta { get; set; }

    [Precision(10, 2)]
    public double PrecioTotal { get; set; }

    public IList<Alquilar> Alquileres;
    public Alquilar(int id, string direccionEntrega, string nombre, string apellidos, TiposMetodoPago metodoPago, DateTime fechaAlquiler, DateTime fechaAlquilerDesde, DateTime fechaAlquilerHasta, double precioTotal)
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
    }
    public override bool Equals(object? obj)
    {
        return obj is Alquilar alquilar &&
               Id == alquilar.Id &&
               DireccionEntrega == alquilar.DireccionEntrega &&
               Nombre == alquilar.Nombre &&
               Apellidos == alquilar.Apellidos &&
               MetodoPago == alquilar.MetodoPago &&
               FechaAlquiler == alquilar.FechaAlquiler &&
               FechaAlquilerDesde == alquilar.FechaAlquilerDesde &&
               FechaAlquilerHasta == alquilar.FechaAlquilerHasta &&
               PrecioTotal == alquilar.PrecioTotal;
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
        return hash.ToHashCode();
    }
}
