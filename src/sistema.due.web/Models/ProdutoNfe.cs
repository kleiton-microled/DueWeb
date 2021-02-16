namespace Sistema.DUE.Web.Models
{
    public class ProdutoNfe
    {
        public ProdutoNfe(
            int item,
            string descricaoProduto,
            string unidadeComercializada,
            decimal quantidadeComercializada,
            decimal quantidadeEstatistica,
            decimal valorProduto,
            string chaveNFE,
            string ncm,
            string cnpjEmitente,
            string razaoSocialEmitente,
            int numeroNF)
        {
            Item = item;
            DescricaoProduto = descricaoProduto;
            UnidadeComercializada = unidadeComercializada;
            QuantidadeComercializada = quantidadeComercializada;
            QuantidadeEstatistica = quantidadeEstatistica;
            ValorProduto = valorProduto;
            ChaveNFE = chaveNFE;
            NCM = ncm;
            CnpjEmitente = cnpjEmitente;
            RazaoSocialEmitente = razaoSocialEmitente;
            NumeroNF = numeroNF;
        }

        public int Item { get; set; }

        public string DescricaoProduto { get; set; }

        public string UnidadeComercializada { get; set; }

        public decimal QuantidadeComercializada { get; set; }

        public decimal QuantidadeEstatistica { get; set; }

        public decimal ValorProduto { get; set; }

        public string ChaveNFE { get; set; }

        public string NCM { get; set; }

        public string CnpjEmitente { get; set; }

        public string RazaoSocialEmitente { get; set; }

        public int NumeroNF { get; set; }
    }
}