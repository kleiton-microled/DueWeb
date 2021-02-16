namespace Sistema.DUE.Web.Models
{
    public class EstoquePreACDPesos
    {
        private string _pesoEntradaCCT;
        private string _pesoAferido;
        private string _motivoNaoPesagem;

        public string PesoEntradaCCT
        {
            get
            {
                if (!string.IsNullOrEmpty(_pesoEntradaCCT))
                {
                    return _pesoEntradaCCT.Replace(".", ",");
                }

                return string.Empty;
            }
            set
            {
                _pesoEntradaCCT = value;
            }
        }

        public string PesoAferido
        {
            get
            {
                if (!string.IsNullOrEmpty(_pesoAferido))
                {
                    return _pesoAferido.Replace(".", ",");
                }

                return string.Empty;
            }
            set
            {
                _pesoAferido = value;
            }
        }

        public string MotivoNaoPesagem { get; set; }
    }
}