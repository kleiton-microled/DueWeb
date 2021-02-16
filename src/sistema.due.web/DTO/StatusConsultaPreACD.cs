using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.DUE.Web.DTO
{
    public class StatusConsultaPreACD
    {
        public string GUID { get; set; }

        public int UsuarioId { get; set; }

        public int TotalRegistros { get; set; }

        public int TotalRegistrosProcessados { get; set; }

        public string Status { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
