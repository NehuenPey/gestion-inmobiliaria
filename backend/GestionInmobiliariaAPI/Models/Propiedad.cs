namespace GestionInmobiliariaAPI.Models
{
    public class Propiedad
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public string Tipo { get; set; } = string.Empty;

        public string Operacion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public double Superficie { get; set; }

        public int Ambientes { get; set; }

        public string Direccion { get; set; } = string.Empty;

        public string Localidad { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
}