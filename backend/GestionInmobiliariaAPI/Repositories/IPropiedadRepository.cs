using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Models;

namespace GestionInmobiliariaAPI.Repositories
{
    public interface IPropiedadRepository
    {
        Task<(List<Propiedad> Propiedades, int Total)> ObtenerPropiedades(
            string? localidad,
            string? operacion,
            string? tipo,
            decimal? precioMin,
            decimal? precioMax,
            int? ambientesMin,
            string? estado,
            int page,
            int pageSize,
            string? ordenarPor,
            string? orden);

        Task<Propiedad?> ObtenerPorId(int id);

        Task<Propiedad> Crear(Propiedad propiedad);

        Task<Propiedad?> Actualizar(
            int id,
            ActualizarPropiedadDto dto);

        Task<bool> Eliminar(int id);
    }
}