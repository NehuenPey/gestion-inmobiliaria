namespace GestionInmobiliariaAPI.DTOs
{
    public static class PropiedadValidation
    {
        public static readonly string[] TiposPermitidos =
        {
            "Casa",
            "Departamento",
            "PH",
            "Terreno",
            "Local",
            "Oficina"
        };

        public static readonly string[] OperacionesPermitidas =
        {
            "Venta",
            "Alquiler"
        };

        public static readonly string[] EstadosPermitidos =
        {
            "Disponible",
            "Reservada",
            "Vendida"
        };
    }
}