using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.DUE.Web.Models
{
    public class NotaFiscalConsultaCCT
    {
        public int Id { get; set; }

        public string TipoNF { get; set; }

        public string ChaveNF { get; set; }

        public int Item { get; set; }

        public string NumeroNF { get; set; }

        public string CnpjNF { get; set; }

        public decimal QuantidadeNF { get; set; }

        public string QtdeAverbada { get; set; }

        public string UnidadeNF { get; set; }

        public string NCM { get; set; }

        public string ChaveNFReferencia { get; set; }

        public string Arquivo { get; set; }

        public string DUE { get; set; }

        public int Usuario { get; set; }

        public string Login { get; set; }

        public DateTime? DataNF { get; set; }

        public string Memorando { get; set; }

        public string AnoMemorando { get; set; }

        public string Empresa { get; set; }

        public string Filial { get; set; }

        public string ChaveAcesso { get; set; }

        public string OBS { get; set; }

        public string DataEmissao { get; set; }

        public string SaldoCCT { get; set; }

        public decimal SaldoOutrasDUES { get; set; }

        public int DueId { get; set; }

        public string Status { get; set; }

        public string CodSituacao { get; set; }

        public decimal VMLE { get; set; }

        public decimal VMCV { get; set; }

        public int Enquadramento { get; set; }

        public string Recinto { get; set; }

        public string UnidadeReceita { get; set; }

        public string DataRegistro { get; set; }

        public string PesoEntradaCCT { get; set; }

        public string PesoAferido { get; set; }
    }
}