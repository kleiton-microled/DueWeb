using System.Collections.Generic;

namespace Sistema.DUE.Web.Responses
{
    public class DadosDUE
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public List<ListaDadosDUE> ListaDadosDUE { get; set; }
    }

    public class ListaDadosDUE
    {
        public string title { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
        public string method { get; set; }
        public string type { get; set; }
    }
}