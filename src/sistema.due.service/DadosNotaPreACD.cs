using System;
using System.Collections.Generic;
using System.Linq;

namespace Cargill.DUE.Service
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
        public DateTime registro { get; set; }
        public string responsavel { get; set; }
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

        public bool sucesso { get; set; }

        public List<EstoquePreACDNotaFiscal> estoqueNotasFiscais { get; set; }

        public List<EstoquePreACD> mensagens { get; set; }

        public string Mensagem { get; set; }

        public double Saldo { get; set; }

        public string ObterMensagem() 
            => mensagens.Select(c => c.mensagem).FirstOrDefault();

        public double ObterSaldo() => estoqueNotasFiscais
            .Select(c => c.itens.Select(d => d.saldo).FirstOrDefault())
            .FirstOrDefault();
    }
}