namespace AppForSEII2526.API.DTOs.AlquilerDTOs
{
    public class ItemAlquilerDTO
    {
        public ItemAlquilerDTO(int idDispositivo, string modelo, string nomberDispositivo,string marca, double precioParaAlquiler)
        {
            IdDispositivo = idDispositivo;
            Modelo = modelo;
            NombreDispositivo = nomberDispositivo;
            Marca = marca;
            PrecioParaAlquiler = precioParaAlquiler;
        }

        public int IdDispositivo { get; set; }


        public string Modelo { get; set; }


        public string Marca { get; set; }
        public string NombreDispositivo { get; set; }
        public double PrecioParaAlquiler { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ItemAlquilerDTO dTO &&
                   IdDispositivo == dTO.IdDispositivo &&
                   Modelo == dTO.Modelo &&
                   Marca == dTO.Marca &&
                   NombreDispositivo == dTO.NombreDispositivo &&
                   PrecioParaAlquiler == dTO.PrecioParaAlquiler;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdDispositivo, Modelo, Marca, PrecioParaAlquiler);
        }
    }
}
