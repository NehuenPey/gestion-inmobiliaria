using GestionInmobiliariaAPI.Data;
using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionInmobiliariaAPI.Repositories
{
    public class PropiedadRepository : IPropiedadRepository
    {
        private readonly ApplicationDbContext _context;

        public PropiedadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Propiedad> Propiedades, int Total)> ObtenerPropiedades(
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
            var consulta = _context.Propiedades.AsQueryable();

            if (!string.IsNullOrWhiteSpace(localidad))
            {
                consulta = consulta.Where(p => p.Localidad == localidad);
            }

            if (!string.IsNullOrWhiteSpace(operacion))
            {
                consulta = consulta.Where(p => p.Operacion == operacion);
            }

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                consulta = consulta.Where(p => p.Tipo == tipo);
            }

            if (precioMin.HasValue)
            {
                consulta = consulta.Where(p => p.Precio >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                consulta = consulta.Where(p => p.Precio <= precioMax.Value);
            }

            if (ambientesMin.HasValue)
            {
                consulta = consulta.Where(p => p.Ambientes >= ambientesMin.Value);
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                consulta = consulta.Where(p => p.Estado == estado);
            }

            // Ordenamiento
            if (ordenarPor == "precio")
            {
                if (orden == "desc")
                {
                    consulta = consulta.OrderByDescending(p => p.Precio);
                }
                else
                {
                    consulta = consulta.OrderBy(p => p.Precio);
                }
            }
            else if (ordenarPor == "superficie")
            {
                if (orden == "desc")
                {
                    consulta = consulta.OrderByDescending(p => p.Superficie);
                }
                else
                {
                    consulta = consulta.OrderBy(p => p.Superficie);
                }
            }
            else if (ordenarPor == "ambientes")
            {
                if (orden == "desc")
                {
                    consulta = consulta.OrderByDescending(p => p.Ambientes);
                }
                else
                {
                    consulta = consulta.OrderBy(p => p.Ambientes);
                }
            }
            else
            {
                consulta = consulta.OrderBy(p => p.Id);
            }

            // Obtener cantidad total antes de aplicar la paginación
            var total = await consulta.CountAsync();

            // Aplicar paginación
            var propiedades = await consulta
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (propiedades, total);
        }

        public async Task<Propiedad?> ObtenerPorId(int id)
        {
            return await _context.Propiedades
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Propiedad> Crear(Propiedad propiedad)
        {
            _context.Propiedades.Add(propiedad);

            await _context.SaveChangesAsync();

            return propiedad;
        }

        public async Task<Propiedad?> Actualizar(
            int id,
            ActualizarPropiedadDto dto)
        {
            var propiedadExistente = await _context.Propiedades
                .FirstOrDefaultAsync(p => p.Id == id);

            if (propiedadExistente == null)
            {
                return null;
            }

            propiedadExistente.Titulo = dto.Titulo;
            propiedadExistente.Descripcion = dto.Descripcion;
            propiedadExistente.Tipo = dto.Tipo;
            propiedadExistente.Operacion = dto.Operacion;
            propiedadExistente.Precio = dto.Precio;
            propiedadExistente.Superficie = dto.Superficie;
            propiedadExistente.Ambientes = dto.Ambientes;
            propiedadExistente.Direccion = dto.Direccion;
            propiedadExistente.Localidad = dto.Localidad;
            propiedadExistente.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return propiedadExistente;
        }

        public async Task<bool> Eliminar(int id)
        {
            var propiedad = await _context.Propiedades
                .FirstOrDefaultAsync(p => p.Id == id);

            if (propiedad == null)
            {
                return false;
            }

            _context.Propiedades.Remove(propiedad);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}