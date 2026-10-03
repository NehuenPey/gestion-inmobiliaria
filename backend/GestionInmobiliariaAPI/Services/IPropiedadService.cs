using GestionInmobiliariaAPI.DTOs;

namespace GestionInmobiliariaAPI.Services
{
    public interface IPropiedadService
    {
        Task<(List<PropiedadDto> Propiedades, int Total)> ObtenerPropiedades(
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

        Task<PropiedadDto?> ObtenerPropiedadPorId(int id);

        Task<PropiedadDto> CrearPropiedad(CrearPropiedadDto dto);

        Task<PropiedadDto?> ActualizarPropiedad(
            int id,
            ActualizarPropiedadDto dto);

        Task<bool> EliminarPropiedad(int id);
    }
}