namespace GestionInmobiliariaAPI.DTOs
{
    public class RespuestaPaginadaDto<T>
    {
        public int Pagina { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalPropiedades { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Resultados { get; set; } = new();
    }
}