namespace Sistema.DUE.Web.Models
{
    public class DUEItemDetalhesLPCO
    {
        public DUEItemDetalhesLPCO()
        {

        }

        public DUEItemDetalhesLPCO(
            int idDetalheItem, 
            string numero)
        {
            IdDetalheItem = idDetalheItem;
            Numero = numero;
        }

        public int Id { get; set; }

        public int IdDetalheItem { get; set; }

        public string Numero { get; set; }     
    }
}

