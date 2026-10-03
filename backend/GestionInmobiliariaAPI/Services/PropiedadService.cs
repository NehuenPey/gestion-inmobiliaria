using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Models;
using GestionInmobiliariaAPI.Repositories;

namespace GestionInmobiliariaAPI.Services
{
    public class PropiedadService : IPropiedadService
    {
        private readonly IPropiedadRepository _propiedadRepository;

        public PropiedadService(
            IPropiedadRepository propiedadRepository)
        {
            _propiedadRepository = propiedadRepository;
        }

        public async Task<(List<PropiedadDto> Propiedades, int Total)> ObtenerPropiedades(
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
            string? orden)
        {
            var resultado = await _propiedadRepository.ObtenerPropiedades(
                localidad,
                operacion,
                tipo,
                precioMin,
                precioMax,
                ambientesMin,
                estado,
                page,
                pageSize,
                ordenarPor,
                orden
            );

            var propiedadesDto = resultado.Propiedades
                .Select(MapearADto)
                .ToList();

            return (propiedadesDto, resultado.Total);
        }

        public async Task<PropiedadDto?> ObtenerPropiedadPorId(int id)
        {
            var propiedad = await _propiedadRepository.ObtenerPorId(id);

            if (propiedad == null)
            {
                return null;
            }

            return MapearADto(propiedad);
        }

        public async Task<PropiedadDto> CrearPropiedad(
            CrearPropiedadDto dto)
        {
            var propiedad = new Propiedad
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                Tipo = dto.Tipo,
                Operacion = dto.Operacion,
                Precio = dto.Precio,
                Superficie = dto.Superficie,
                Ambientes = dto.Ambientes,
                Direccion = dto.Direccion,
                Localidad = dto.Localidad,
                Estado = dto.Estado
            };

            var propiedadCreada = await _propiedadRepository.Crear(propiedad);

            return MapearADto(propiedadCreada);
        }

        public async Task<PropiedadDto?> ActualizarPropiedad(
            int id,
            ActualizarPropiedadDto dto)
        {
            var propiedad = await _propiedadRepository.ObtenerPorId(id);

            if (propiedad == null)
            {
                return null;
            }

            ValidarCambioDeEstado(
                propiedad.Estado,
                dto.Estado
            );

            if (dto.Estado == "Vendida" &&
                !string.Equals(
                    dto.Operacion,
                    "Venta",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Una propiedad vendida debe tener la operación Venta."
                );
            }

            var propiedadActualizada =
                await _propiedadRepository.Actualizar(id, dto);

            if (propiedadActualizada == null)
            {
                return null;
            }

            return MapearADto(propiedadActualizada);
        }
        public async Task<bool> EliminarPropiedad(int id)
        {
            return await _propiedadRepository.Eliminar(id);
        }

        private void ValidarCambioDeEstado(
            string estadoActual,
            string nuevoEstado)
        {
            if (estadoActual == "Vendida")
            {
                throw new InvalidOperationException(
                    "Una propiedad vendida no puede ser modificada."
                );
            }

            if (estadoActual == "Reservada")
            {
                if (nuevoEstado != "Disponible" &&
                    nuevoEstado != "Vendida" &&
                    nuevoEstado != "Reservada")
                {
                    throw new InvalidOperationException(
                        "El cambio de estado no es válido para una propiedad reservada."
                    );
                }
            }

            if (estadoActual == "Disponible")
            {
                if (nuevoEstado != "Disponible" &&
                    nuevoEstado != "Reservada" &&
                    nuevoEstado != "Vendida")
                {
                    throw new InvalidOperationException(
                        "El cambio de estado no es válido para una propiedad disponible."
                    );
                }
            }
        }

        private PropiedadDto MapearADto(Propiedad propiedad)
        {
            return new PropiedadDto
            {
                Id = propiedad.Id,
                Titulo = propiedad.Titulo,
                Descripcion = propiedad.Descripcion,
                Tipo = propiedad.Tipo,
                Operacion = propiedad.Operacion,
                Precio = propiedad.Precio,
                Superficie = propiedad.Superficie,
                Ambientes = propiedad.Ambientes,
                Direccion = propiedad.Direccion,
                Localidad = propiedad.Localidad,
                Estado = propiedad.Estado
            };
        }
    }
}