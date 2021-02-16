using System;

namespace Sistema.DUE.Web.DTO
{
    public class NFRemessaPosACD
    {
        public int Id { get; set; }

        public string DataDUE { get; set; }
        public string DUE { get; set; }
        public string RUC { get; set; }
        public string ChaveDUE { get; set; }
        public string DeclaranteCnpj { get; set; }
        public string DeclaranteNome { get; set; }
        public string UltimoEvento { get; set; }
        public string DataUltimoEvento { get; set; }
        public string DataAverbacao { get; set; }
        public string ItemDUE { get; set; }
        public string NCM { get; set; }
        public string ItemNF { get; set; }
        public string CFOP { get; set; }
        public string DESCRICAO { get; set; }
        public string TipoNF { get; set; }
        public string ChaveNF { get; set; }
        public string NUMERO { get; set; }
        public string MODELO { get; set; }
        public string SERIE { get; set; }
        public string UF { get; set; }
        public string CNPJEmitente { get; set; }
        public string EventoAverbacao { get; set; }
        public string VMLE { get; set; }
        public string ValorEmReais { get; set; }
        public string VMCV { get; set; }
        public string IMPORTADOR { get; set; }
        public string ImportadorEndereco { get; set; }
        public string ImportadorPais { get; set; }
        public string PaisDestino { get; set; }
        public string Unidade { get; set; }
        public string PesoLiquidoTotal { get; set; }
        public string Moeda { get; set; }
        public string INCOTERM { get; set; }
        public string InformacoesComplementares { get; set; }
        public string UnidadeRFB { get; set; }
        public string DescricaoUnidadeRFB { get; set; }
        public string RecintoDespacho { get; set; }
        public string DescricaoRecintoDespacho { get; set; }
        public string QuantidadeNF { get; set; }
        public string QuantidadeAverbada { get; set; }

        public string SessaoHash { get; set; }
        public DateTime DataConsulta { get; set; }

        public string CamposRemessa { get; set; }
        public int UsuarioId { get; set; }

        public bool DadosSISCOMEX { get; set; }
    }
}