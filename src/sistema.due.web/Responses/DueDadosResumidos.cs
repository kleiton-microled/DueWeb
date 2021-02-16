namespace Sistema.DUE.Web.Responses
{
    public class ConsultaDueDadosResumidos
    {
        public string numeroDUE { get; set; }
        public string numeroRUC { get; set; }
        public int situacaoDUE { get; set; }
        public string dataSituacaoDUE { get; set; }
        public int indicadorBloqueio { get; set; }
        public int controleAdministrativo { get; set; }
        public string uaEmbarque { get; set; }
        public string uaDespacho { get; set; }
        public object responsavelUADespacho { get; set; }
        public string codigoRecintoAduaneiroDespacho { get; set; }
        public string codigoRecintoAduaneiroEmbarque { get; set; }
        public object latitudeDespacho { get; set; }
        public object longitudeDespacho { get; set; }
        public int[] situacaoCarga { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public ConsultaDueDadosResumidosDeclarante declarante { get; set; }
    }

    public class ConsultaDueDadosResumidosDeclarante
    {
        public string numero { get; set; }
    }
}