namespace Sistema.DUE.Web.DTO
{
    public class DUEItemDTO
    {
        public int Id { get; set; }
        public string DueId { get; set; }
        public string NroItem { get; set; }
        public string MotivoDispensaNF { get; set; }
        public string MotivoDispensaNFDescricao { get; set; }
        public string CondicaoVenda { get; set; }
        public string Exportador { get; set; }
        public string ExportadorDocumento { get; set; }
        public string ExportadorEndereco { get; set; }
        public string ExportadorUF { get; set; }
        public string ExportadorPais { get; set; }
        public string Importador { get; set; }
        public string ImportadorEndereco { get; set; }
        public string ImportadorPais { get; set; }
        public string NF { get; set; }
        public string ValorUnitVMLE { get; set; }
        public string ValorUnitVMCV { get; set; }
    }
}