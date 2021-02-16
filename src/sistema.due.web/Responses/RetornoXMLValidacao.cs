using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.DUE.Web.Responses
{
    public class RetornoXMLValidacao
    {
        public bool Sucesso { get; set; }

        public string Debug { get; set; }

        public List<string> Mensagens { get; set; } = new List<string>();

        public void AdicionarMensagem(string mensagem) => Mensagens.Add(mensagem);

        public string XmlRetorno { get; set; }

        public string NumeroDUE { get; set; }
    }
}
