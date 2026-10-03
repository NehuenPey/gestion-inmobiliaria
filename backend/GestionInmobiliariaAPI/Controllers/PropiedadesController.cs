using GestionInmobiliariaAPI.DTOs;
using GestionInmobiliariaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionInmobiliariaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadService _propiedadService;

        public PropiedadesController(IPropiedadService propiedadService)
        {
            _propiedadService = propiedadService;
        }

        // GET: api/propiedades
        [HttpGet]
        [EndpointSummary("Obtiene propiedades inmobiliarias")]
        [EndpointDescription("Obtiene un listado paginado de propiedades. Permite filtrar por localidad, operación, tipo, precio, ambientes y estado. También permite ordenar los resultados por precio, superficie o ambientes.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPropiedades(
                    string? localidad,
                    string? operacion,
                    string? tipo,
                    decimal? precioMin,
                    decimal? precioMax,
                    int? ambientesMin,
                    string? estado,
                    int page = 1,
                    int pageSize = 10,
                    string? ordenarPor = null,
                    string? orden = "asc")
        {
            if (page < 1)
            {
                return BadRequest(new
                {
                    mensaje = "El número de página debe ser mayor o igual a 1."
                });
            }

            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest(new
                {
                    mensaje = "El tamaño de página debe estar entre 1 y 50."
                });
            }

            var resultado = await _propiedadService.ObtenerPropiedades(
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

            var totalPaginas = (int)Math.Ceiling(
                (double)resultado.Total / pageSize
            );

            var respuesta = new RespuestaPaginadaDto<PropiedadDto>
            {
                Pagina = page,
                TamanoPagina = pageSize,
                TotalPropiedades = resultado.Total,
                TotalPaginas = totalPaginas,
                Resultados = resultado.Propiedades
            };

            return Ok(respuesta);
        }

        // GET: api/propiedades/1
        [HttpGet("{id}")]
        [EndpointSummary("Obtiene una propiedad por ID")]
        [EndpointDescription("Obtiene los datos completos de una propiedad inmobiliaria a partir de su identificador.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPropiedad(int id)
        {
            var propiedad = await _propiedadService
                .ObtenerPropiedadPorId(id);

            if (propiedad == null)
            {
                return NotFound(new
                {
                    mensaje = "Propiedad no encontrada"
                });
            }

            return Ok(propiedad);
        }

        // POST: api/propiedades
        [HttpPost]
        [EndpointSummary("Crea una nueva propiedad")]
        [EndpointDescription("Crea una propiedad inmobiliaria utilizando los datos proporcionados y devuelve la propiedad creada.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CrearPropiedad(CrearPropiedadDto dto)
        {
            var propiedad = await _propiedadService
                .CrearPropiedad(dto);

            return CreatedAtAction(
                nameof(GetPropiedad),
                new { id = propiedad.Id },
                propiedad
            );
        }

        // PUT: api/propiedades/1
        [HttpPut("{id}")]
        [EndpointSummary("Actualiza una propiedad")]
        [EndpointDescription("Actualiza los datos de una propiedad existente. Se aplican las reglas de negocio relacionadas con los estados Disponible, Reservada y Vendida.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarPropiedad(
       int id,
       ActualizarPropiedadDto dto)
        {
            try
            {
                var propiedad = await _propiedadService
                    .ActualizarPropiedad(id, dto);

                if (propiedad == null)
                {
                    return NotFound(new
                    {
                        mensaje = "Propiedad no encontrada"
                    });
                }

                return Ok(propiedad);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message
                });
            }
        }

        // DELETE: api/propiedades/1
        [HttpDelete("{id}")]
        [EndpointSummary("Elimina una propiedad")]
        [EndpointDescription("Elimina una propiedad inmobiliaria existente a partir de su identificador.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPropiedad(int id)
        {
            var eliminada = await _propiedadService
                .EliminarPropiedad(id);

            if (!eliminada)
            {
                return NotFound(new
                {
                    mensaje = "Propiedad no encontrada"
                });
            }

            return Ok(new
            {
                mensaje = "Propiedad eliminada correctamente"
            });
        }
    }
}