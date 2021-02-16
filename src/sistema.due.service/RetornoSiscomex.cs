using System.Collections.Generic;

namespace Cargill.DUE.Service
{
    public class RetornoSiscomex
    {
        public bool Sucesso { get; set; }
        public string Message { get; set; }
        public RetornoCriticas[] Criticas { get; set; }
        public string DUE { get; set; }
        public string RUC { get; set; }
        public string ChaveDeAcesso { get; set; }
        public string XmlRetorno { get; set; }
    }
}