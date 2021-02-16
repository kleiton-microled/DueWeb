namespace Sistema.DUE.Web.Models
{
    public class DUEItem
    {
        public DUEItem()
        {

        }

        public DUEItem(int dueId,int motivoDispensaNF, string condicaoVenda)
        {
            DueId = dueId;            
            MotivoDispensaNF = motivoDispensaNF;
            CondicaoVenda = condicaoVenda;

            Exportador = new Exportador();
            Importador = new Importador();
        }

        public DUEItem(int dueId, string nf)
        {
            DueId = dueId;            
            NF = nf;

            Exportador = new Exportador();
            Importador = new Importador();
        }

        public int Id { get; set; }

        public int DueId { get; set; }

        public Exportador Exportador { get; set; }

        public Importador Importador { get; set; }

        public int MotivoDispensaNF { get; set; }

        public string CondicaoVenda { get; set; }

        public string NF { get; set; }

        public decimal ValorUnitVMLE { get; set; }

        public decimal ValorUnitVMCV { get; set; }

        public void AdicionarExportador(Exportador exportador)
        {
            if (exportador != null)
                Exportador = exportador;
        }

        public void AdicionarImportador(Importador importador)
        {
            if (importador != null)
                Importador = importador;
        }
    }    
}