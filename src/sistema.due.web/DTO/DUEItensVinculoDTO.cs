namespace Sistema.DUE.Web.DTO
{
    public class DUEItensVinculoDTO
    {       
        public string Id { get; set; }

        public string Item { get; set; }

        public string DescricaoMercadoria { get; set; }

        public string QuantidadeUnidades { get; set; }

        public string PesoLiquidoTotal { get; set; }

        public string ValorMercadoriaLocalEmbarque { get; set; }

        public string ValorMercadoriaCondicaoVenda { get; set; }

        public string TotalEXP { get; set; }

        public string TotalFDL { get; set; }

        public string TotalREM_NFF { get; set; }

        public string NF { get; set; }
    }
}