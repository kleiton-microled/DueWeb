namespace Sistema.DUE.Web.Models
{
    public class DUEItemDetalhesAC
    {
        public DUEItemDetalhesAC()
        {

        }

        public DUEItemDetalhesAC(
            int idDetalheItem,
            string numero,
            string cNPJBeneficiario,
            string numeroItem,
            string nCMItem,
            decimal quantidadeUtilizada,
            decimal vMLEComCoberturaCambial,
            decimal vMLESemCoberturaCambial,
            string tipo,
            int exportadorBeneficiario)
        {
            IdDetalheItem = idDetalheItem;
            Numero = numero;
            CNPJBeneficiario = cNPJBeneficiario;
            NumeroItem = numeroItem;
            NCMItem = nCMItem;
            QuantidadeUtilizada = quantidadeUtilizada;
            VMLEComCoberturaCambial = vMLEComCoberturaCambial;
            VMLESemCoberturaCambial = vMLESemCoberturaCambial;
            TipoAC = tipo;
            ExportadorBeneficiario = exportadorBeneficiario;
        }

        public int Id { get; set; }
        public int IdDetalheItem { get; set; }
        public string TipoAC { get; set; }
        public int ExportadorBeneficiario { get; set; }
        public string Numero { get; set; }
        public string CNPJBeneficiario { get; set; }
        public string NumeroItem { get; set; }
        public string NCMItem { get; set; }
        public decimal QuantidadeUtilizada { get; set; }
        public decimal VMLEComCoberturaCambial { get; set; }
        public decimal VMLESemCoberturaCambial { get; set; }
    }
}