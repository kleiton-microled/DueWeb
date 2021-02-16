namespace Sistema.DUE.Web.Models
{
    public class Pais
    {
        public Pais()
        {

        }

        public Pais(int id, string descricao, string sigla)
        {
            Id = id;
            Descricao = descricao;
            Sigla = sigla;
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
    }
}