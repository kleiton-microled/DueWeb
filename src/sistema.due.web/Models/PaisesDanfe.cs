namespace Sistema.DUE.Web.Models
{
    public class PaisesDanfe
    {
        public PaisesDanfe(string codigoSPED, string descricao, string codigoSiscomex, string sigla1, string sigla2)
        {
            CodigoSPED = codigoSPED;
            Descricao = descricao;
            CodigoSiscomex = codigoSiscomex;
            Sigla1 = sigla1;
            Sigla2 = sigla2;
        }

        public string CodigoSPED { get; set; }

        public string Descricao { get; set; }

        public string CodigoSiscomex { get; set; }

        public string Sigla1 { get; set; }

        public string Sigla2 { get; set; }
    }
}