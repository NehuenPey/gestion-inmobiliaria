using System.ComponentModel.DataAnnotations;

namespace GestionInmobiliariaAPI.DTOs
{
    public class CrearPropiedadDto : IValidatableObject
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de propiedad es obligatorio")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La operación es obligatoria")]
        public string Operacion { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "La superficie debe ser mayor a 0")]
        public double Superficie { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La propiedad debe tener al menos 1 ambiente")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La localidad es obligatoria")]
        public string Localidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = string.Empty;


        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (!PropiedadValidation.TiposPermitidos
                .Contains(Tipo, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "El tipo de propiedad no es válido.",
                    new[] { nameof(Tipo) }
                );
            }

            if (!PropiedadValidation.OperacionesPermitidas
                .Contains(Operacion, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "La operación no es válida.",
                    new[] { nameof(Operacion) }
                );
            }

            if (!PropiedadValidation.EstadosPermitidos
                .Contains(Estado, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "El estado no es válido.",
                    new[] { nameof(Estado) }
                );
            }
        }
    }
}