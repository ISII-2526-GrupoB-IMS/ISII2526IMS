using System;
using System.Collections.Generic;
using System.Linq;
using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web.Services
{
    public class AlquileresStateContainer
    {
        private readonly List<AlquilerForCreateDTO> _alquileres = new();

        public event Action? OnChange;

        public IReadOnlyList<AlquilerForCreateDTO> Alquileres => _alquileres.AsReadOnly();

        public void AgregarAlquiler(AlquilerForCreateDTO alquiler)
        {
            if (alquiler == null) return;
            _alquileres.Add(alquiler);
            NotifyStateChanged();
        }

        public void Vaciar()
        {
            _alquileres.Clear();
            NotifyStateChanged();
        }

        public AlquilerForCreateDTO? GetPorIndex(int index)
        {
            if (index < 0 || index >= _alquileres.Count) return null;
            return _alquileres[index];
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}