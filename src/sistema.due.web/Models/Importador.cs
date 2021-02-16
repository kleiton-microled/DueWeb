namespace Sistema.DUE.Web.Models
{
    public class Importador
    {
        public Importador()
        {

        }

        public Importador(string descricao, string endereco, string pais)
        {
            Descricao = descricao;
            Endereco = endereco;
            Pais = pais;
        }

        public string Descricao { get; set; }
        public string Endereco { get; set; }
        public string Pais { get; set; }
    }
}