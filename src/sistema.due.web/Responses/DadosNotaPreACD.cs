using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistema.DUE.Web.Responses
{
    public class EstoquePreACDNotaFiscalItem
    {
        public int item { get; set; }
        public double saldo { get; set; }
    }

    public class EstoquePreACDNotaFiscal
    {
        public string numero { get; set; }
        public string urf { get; set; }
        public string recinto { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime? registro { get; set; }
        //public long? registro { get; set; }
        public string responsavel { get; set; }
        public double? pesoAferido { get; set; }
        public string motivoNaoPesagem { get; set; }
        public List<EstoquePreACDNotaFiscalItem> itens { get; set; }
    }

    public class EstoquePreACD
    {
        public string codigo { get; set; }
        public string mensagem { get; set; }
    }

    public class DadosNotaPreACD
    {
        public DadosNotaPreACD()
        {
            mensagens = new List<EstoquePreACD>();
        }

        public List<EstoquePreACDNotaFiscal> estoqueNotasFiscais { get; set; }

        public List<EstoquePreACD> mensagens { get; set; }

        public string Mensagem { get; set; }

        public double Saldo { get; set; }

        public string Registro { get; set; }

        public string Recinto { get; set; }

        public string UnidadeReceita { get; set; }

        public string ResponsavelIdentificacao { get; set; }

        public int Item { get; set; }

        public double? PesoAferido { get; set; }

        public string MotivoNaoPesagem { get; set; }

        public bool Sucesso { get; set; }

        public string ObterMensagem()
            => mensagens.Select(c => c.mensagem).FirstOrDefault();

        public double ObterSaldo() => estoqueNotasFiscais
            .Select(c => c.itens.Select(d => d.saldo).FirstOrDefault())
            .FirstOrDefault();

        public double? ObterPesoAferido() => estoqueNotasFiscais
            .Sum(c => c.pesoAferido);

        public string ObterMotivoNaoPesagem() 
            => estoqueNotasFiscais.Select(c => c.motivoNaoPesagem).FirstOrDefault();
    }
}