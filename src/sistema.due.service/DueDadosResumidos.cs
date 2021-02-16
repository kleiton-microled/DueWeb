namespace Cargill.DUE.Service
{
    public class DueDadosResumidos
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
        public object coordenadasDespacho { get; set; }
        public DeclaranteDadosResumidos declarante { get; set; }
        public ExportadorDadosResumidos[] exportadores { get; set; }
        public int[] situacaoCarga { get; set; }
    }

    public class DeclaranteDadosResumidos
    {
        public string numero { get; set; }
        public string tipo { get; set; }
    }

    public class ExportadorDadosResumidos
    {
        public string numero { get; set; }
        public string tipo { get; set; }
    }
}