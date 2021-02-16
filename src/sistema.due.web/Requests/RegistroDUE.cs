using System;

namespace Sistema.DUE.Web.Requests
{
    [Serializable]
    public class RegistroDUE
    {
        public string DocumentoDeclarante { get; set; }
        public int MoedaId { get; set; }
        public string RUC { get; set; }
        public int SituacaoEspecial { get; set; }
        public int FormaExportacao { get; set; }
        public int ViaEspecialTransporte { get; set; }
        public int UnidadeDespachoId { get; set; }
        public int TipoRecintoAduaneiroDespacho { get; set; }
        public int RecintoAduaneiroDespachoId { get; set; }
        public string DocumentoResponsavelRecintoDespacho { get; set; }
        public string LatitudeRecintoDespacho { get; set; }
        public string LongitudeRecintoDespacho { get; set; }
        public string EnderecoRecintoDespacho { get; set; }
        public int UnidadeEmbarqueId { get; set; }
        public int TipoRecintoAduaneiroEmbarque { get; set; }
        public int RecintoAduaneiroEmbarqueId { get; set; }
        public string EnderecoReferenciaRecintoAduaneiroEmbarque { get; set; }
        public string InformacoesComplementares { get; set; }
    }
}