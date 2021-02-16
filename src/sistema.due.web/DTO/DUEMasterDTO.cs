namespace Sistema.DUE.Web.DTO
{
    public class DUEMasterDTO
    {
        public string Id { get; set; }
        public string DUE { get; set; }
        public string DocumentoDeclarante { get; set; }
        public string MoedaNegociacao { get; set; }
        public string RUC { get; set; }
        public string SituacaoEspecial { get; set; }
        public string FormaExportacao { get; set; }
        public string ViaEspecialTransporte { get; set; }
        public string UnidadeDespachoId { get; set; }
        public string UnidadeDespacho { get; set; }
        public string TipoRecintoAduaneiroDespacho { get; set; }
        public string RecintoAduaneiroDespachoId { get; set; }
        public string RecintoAduaneiroDespacho { get; set; }
        public string DocumentoResponsavelRecintoDespacho { get; set; }
        public string UnidadeEmbarqueId { get; set; }
        public string UnidadeEmbarque { get; set; }
        public string TipoRecintoAduaneiroEmbarque { get; set; }
        public string RecintoAduaneiroEmbarqueId { get; set; }
        public string RecintoAduaneiroEmbarque { get; set; }
        public string DataCadastro { get; set; }
        public string Login { get; set; }
        public string EnviadoSiscomex { get; set; }
        public string RetificadoSiscomex { get; set; }

        public string ImportadoSiscomex { get; set; }
        public string ChaveAcesso { get; set; }
        public string SituacaoDUE { get; set; }
        public string DueImportada { get; set; }
        public string Completa { get; set; }
        public string CriadoPorNF { get; set; }
    }
}