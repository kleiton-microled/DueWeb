using System;

namespace Sistema.DUE.Web.Models
{
    public class DUEItemDetalhesACNF
    {
        public DUEItemDetalhesACNF()
        {

        }

        public DUEItemDetalhesACNF(int idAcd, string chaveNumNFAC, decimal quantidadeNFAC, DateTime dataEmissaoNFAC, decimal valorNFAC)
        {
            IdAcd = idAcd;
            ChaveNota = chaveNumNFAC;
            Quantidade = quantidadeNFAC;
            DataEmissao = dataEmissaoNFAC;
            Valor = valorNFAC;
        }

        public int Id { get; set; }

        public int IdAcd { get; set; }

        public string ChaveNota { get; set; }

        public decimal Quantidade { get; set; }

        public DateTime DataEmissao { get; set; }

        public decimal Valor { get; set; }
    }
}