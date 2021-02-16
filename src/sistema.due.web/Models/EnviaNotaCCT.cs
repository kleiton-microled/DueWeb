using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cargill.DUE.Web.Models
{
    public class EnviaNotaCCT
    {
        public string IdentificacaoRecepcao { get; set; }
        public string CnpjResp { get; set; }
        public string CodigoURF { get; set; }
        public string CodigoURA { get; set; }
        public string ChaveNFE { get; set; }
        public string CpfCnpjTransportador { get; set; }
        public string CpfCondutor { get; set; }
        public string PesoAferido { get; set; }
        public string ObservacoesGerais { get; set; }

        public EnviaNotaCCT(string identificacaoRecepcao, string cnpjResp, string codigoURF, string codigoURA, string chaveNFE, 
            string cpfCnpjTransportador, string cpfCondutor, string pesoAferido, string observacoesGerais)
        {
            IdentificacaoRecepcao = identificacaoRecepcao;
            CnpjResp = cnpjResp;
            CodigoURF = codigoURF;
            CodigoURA = codigoURA;
            ChaveNFE = chaveNFE;
            CpfCnpjTransportador = cpfCnpjTransportador;
            CpfCondutor = cpfCondutor;
            PesoAferido = pesoAferido;
            ObservacoesGerais = observacoesGerais;
        }
        public EnviaNotaCCT()
        {

        }
    }
}