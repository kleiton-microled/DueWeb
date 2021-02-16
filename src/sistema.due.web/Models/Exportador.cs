namespace Sistema.DUE.Web.Models
{
    public class Exportador
    {
        public Exportador()
        {

        }

        public Exportador(string descricao, string documento, string endereco, string uF, string pais)
        {
            Descricao = descricao;
            Documento = documento;
            Endereco = endereco;
            UF = uF;
            Pais = pais;
        }

        public string Descricao { get; set; }
        public string Documento { get; set; }
        public string Endereco { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }
    }
}