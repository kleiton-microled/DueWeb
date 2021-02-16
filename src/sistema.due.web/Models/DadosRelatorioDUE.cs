using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema.DUE.Web.Models
{
    public class DadosRelatorioDUE
    {        
        public string Due { get; set; }
        public string ExportadorCnpj { get; set; }
        public string UltimoEvento { get; set; }
        public DateTime? DataDue { get; set; }
        public DateTime? DataEmbarque { get; set; }
        public DateTime? DataAverbacao { get; set; }
        public string Canal { get; set; }
        public int Item { get; set; }
        public string VMLE { get; set; }
        public string CpfDue { get; set; }
        public string NCM { get; set; }
        public string PesoLiquidoTotal { get; set; }
    }
}