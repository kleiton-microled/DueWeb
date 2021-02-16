namespace Sistema.DUE.Web.Models
{
    public class DueItemNota
    {
        public int Id { get; set; }

        public int Item { get; set; }

        public string TipoNF { get; set; }

        public string ChaveNF { get; set; }

        public string NumeroNF { get; set; }

        public string CnpjNF { get; set; }

        public string QuantidadeNF { get; set; }

        public string UnidadeNF { get; set; }

        public string NCM { get; set; }

        public string DUE { get; set; }
    }
}