namespace Sistema.DUE.Web.Models
{
    public class UnidadeDespacho
    {
        public string Id { get; set; }
        public int TipoRecinto { get; set; }
        public string RecintoAduaneiroId { get; set; }
        public string DocumentoResponsavel { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Endereco { get; set; }
    }
}