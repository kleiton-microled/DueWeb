namespace Sistema.DUE.Web.Models
{
    public class DUEItemDetalhes
    {
        public DUEItemDetalhes()
        {

        }

        public DUEItemDetalhes(
            int dueItemId,
            int item,
            decimal valorMercadoriaLocalEmbarque,
            string paisDestino,
            decimal quantidadeEstatistica,
            string ncm,
            decimal valorMercadoriaCondicaoVenda,
            decimal quantidadeUnidades,
            decimal pesoLiquidoTotal,
            int enquadramento1Id,
            int enquadramento2Id,
            int enquadramento3Id,
            int enquadramento4Id,
            string descricaoComplementar,
            string padraoQualidade,
            string embarcadoEm,
            string tipo,
            string metodoProcessamento,
            string caracteristicaEspecial,
            string outraCaracteristicaEspecial,
            int embalagemFinal)
        {
            DUEItemId = dueItemId;
            Item = item;
            ValorMercadoriaLocalEmbarque = valorMercadoriaLocalEmbarque;
            PaisDestino = paisDestino;
            QuantidadeEstatistica = quantidadeEstatistica;
            NCM = ncm;
            ValorMercadoriaCondicaoVenda = valorMercadoriaCondicaoVenda;
            QuantidadeUnidades = quantidadeUnidades;
            PesoLiquidoTotal = pesoLiquidoTotal;
            Enquadramento1Id = enquadramento1Id;
            Enquadramento2Id = enquadramento2Id;
            Enquadramento3Id = enquadramento3Id;
            Enquadramento4Id = enquadramento4Id;
            DescricaoComplementar = descricaoComplementar;
            Attr_Padrao_Qualidade = padraoQualidade;
            Attr_Embarque_Em = embarcadoEm;
            Attr_Tipo = tipo;
            Attr_Metodo_Processamento = metodoProcessamento;
            Attr_Caracteristica_Especial = caracteristicaEspecial;
            Attr_Outra_Caracteristica_Especial = outraCaracteristicaEspecial;
            Attr_Embalagem_Final = embalagemFinal;
        }

        public DUEItemDetalhes(
            int dueItemId,
            int item,
            decimal valorMercadoriaLocalEmbarque,
            string paisDestino,
            decimal quantidadeEstatistica,
            int prioridadeCarga,
            int limite,
            string descricaoComplementar,
            string ncm,
            decimal valorMercadoriaCondicaoVenda,
            string descricaoMercadoria,
            decimal quantidadeUnidades,
            string descricaoUnidade,
            int codigoProduto,
            decimal pesoLiquidoTotal,
            int enquadramento1Id,
            int enquadramento2Id,
            int enquadramento3Id,
            int enquadramento4Id,
            decimal comissaoAgente,
            string padraoQualidade,
            string embarcadoEm,
            string tipo,
            string metodoProcessamento,
            string caracteristicaEspecial,
            string outraCaracteristicaEspecial,
            int embalagemFinal)
        {
            DUEItemId = dueItemId;
            Item = item;
            ValorMercadoriaLocalEmbarque = valorMercadoriaLocalEmbarque;
            PaisDestino = paisDestino;
            QuantidadeEstatistica = quantidadeEstatistica;
            PrioridadeCarga = prioridadeCarga;
            Limite = limite;
            DescricaoComplementar = descricaoComplementar;
            NCM = ncm;
            ValorMercadoriaCondicaoVenda = valorMercadoriaCondicaoVenda;
            DescricaoMercadoria = descricaoMercadoria;
            QuantidadeUnidades = quantidadeUnidades;
            DescricaoUnidade = descricaoUnidade;
            CodigoProduto = codigoProduto;
            PesoLiquidoTotal = pesoLiquidoTotal;
            Enquadramento1Id = enquadramento1Id;
            Enquadramento2Id = enquadramento2Id;
            Enquadramento3Id = enquadramento3Id;
            Enquadramento4Id = enquadramento4Id;
            ComissaoAgente = comissaoAgente;
            Attr_Padrao_Qualidade = padraoQualidade;
            Attr_Embarque_Em = embarcadoEm;
            Attr_Tipo = tipo;
            Attr_Metodo_Processamento = metodoProcessamento;
            Attr_Caracteristica_Especial = caracteristicaEspecial;
            Attr_Outra_Caracteristica_Especial = outraCaracteristicaEspecial;
            Attr_Embalagem_Final = embalagemFinal;
        }

        public DUEItemDetalhes(
            int dueItemId,
            int item,
            decimal valorMercadoriaLocalEmbarque,
            string paisDestino,
            decimal quantidadeEstatistica,
            int prioridadeCarga,
            int limite,
            string descricaoComplementar,
            string ncm,
            decimal valorMercadoriaCondicaoVenda,
            string descricaoMercadoria,
            decimal quantidadeUnidades,
            string descricaoUnidade,
            int codigoProduto,
            decimal pesoLiquidoTotal,
            int enquadramento1Id,
            int enquadramento2Id,
            int enquadramento3Id,
            int enquadramento4Id)
        {
            DUEItemId = dueItemId;
            Item = item;
            ValorMercadoriaLocalEmbarque = valorMercadoriaLocalEmbarque;
            PaisDestino = paisDestino;
            QuantidadeEstatistica = quantidadeEstatistica;
            PrioridadeCarga = prioridadeCarga;
            Limite = limite;
            DescricaoComplementar = descricaoComplementar;
            NCM = ncm;
            ValorMercadoriaCondicaoVenda = valorMercadoriaCondicaoVenda;
            DescricaoMercadoria = descricaoMercadoria;
            QuantidadeUnidades = quantidadeUnidades;
            DescricaoUnidade = descricaoUnidade;
            CodigoProduto = codigoProduto;
            PesoLiquidoTotal = pesoLiquidoTotal;
            Enquadramento1Id = enquadramento1Id;
            Enquadramento2Id = enquadramento2Id;
            Enquadramento3Id = enquadramento3Id;
            Enquadramento4Id = enquadramento4Id;
        }

        public int Id { get; set; }
        public int DUEItemId { get; set; }
        public int Item { get; set; }
        public decimal ValorMercadoriaLocalEmbarque { get; set; }
        public string PaisDestino { get; set; }
        public decimal QuantidadeEstatistica { get; set; }
        public int PrioridadeCarga { get; set; }
        public int Limite { get; set; }
        public string DescricaoComplementar { get; set; }
        public string NCM { get; set; }
        public decimal ValorMercadoriaCondicaoVenda { get; set; }
        public string DescricaoMercadoria { get; set; }
        public decimal QuantidadeUnidades { get; set; }
        public string DescricaoUnidade { get; set; }
        public int CodigoProduto { get; set; }
        public decimal PesoLiquidoTotal { get; set; }
        public decimal ComissaoAgente { get; set; }
        public int Enquadramento1Id { get; set; }
        public int Enquadramento2Id { get; set; }
        public int Enquadramento3Id { get; set; }
        public int Enquadramento4Id { get; set; }
        public string Attr_Padrao_Qualidade { get; set; }
        public string Attr_Embarque_Em { get; set; }
        public string Attr_Tipo { get; set; }
        public string Attr_Metodo_Processamento { get; set; }
        public string Attr_Caracteristica_Especial { get; set; }
        public string Attr_Outra_Caracteristica_Especial { get; set; }
        public int Attr_Embalagem_Final { get; set; }
    }
}