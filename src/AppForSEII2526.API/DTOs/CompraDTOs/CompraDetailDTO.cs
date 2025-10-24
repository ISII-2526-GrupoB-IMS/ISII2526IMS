using AppForSEII2526.API.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.CompraDTOs
{
    public class CompraDetailDTO : CompraForCreateDTO
    {
       

        public CompraDetailDTO(int id, string nombreUsuario, string apellidosUsuario, string direccionEntrega, DateTime fechaCompra, IList<CompraItemDTO> compraItems)
           : base(nombreUsuario,
                  apellidosUsuario,
                  direccionEntrega,
                  fechaCompra,
                  compraItems)
        {
            Id = id;
        }
        public int Id { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CompraDetailDTO dTO &&
                   base.Equals(obj) &&
                   FechaCompra == dTO.FechaCompra &&
                   DireccionEntrega == dTO.DireccionEntrega &&
                   NombreUsuario == dTO.NombreUsuario &&
                   ApellidosUsuario == dTO.ApellidosUsuario &&
                   EqualityComparer<IList<CompraItemDTO>>.Default.Equals(CompraItems, dTO.CompraItems) &&
                   PrecioTotal == dTO.PrecioTotal &&
                   Id == dTO.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), FechaCompra, DireccionEntrega, NombreUsuario, ApellidosUsuario, CompraItems, PrecioTotal, Id);
        }
    }

}
