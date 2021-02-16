using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.DUE.Web.Models
{
    public class ConsultaNfs
    {
        public int Id { get; set; }

        public string DataRegistro { get; set; }

        public string ChaveNF { get; set; }

        public string SaldoCCT { get; set; }

        public string Observacoes { get; set; }

        public string Recinto { get; set; }

        public string UnidadeReceita { get; set; }

        public string Item { get; set; }

        public string PesoAferido { get; set; }
        public string PesoEntradaCCT { get; set; }

        public string DUE { get; set; }

        public string QtdeAverbada { get; set; }
    }
}