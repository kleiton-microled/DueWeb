using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sistema.DUE.Web.RetificacaoSemServico
{
    [XmlRoot(ElementName = "warnings")]
    public class Warnings
    {
        [XmlElement(ElementName = "warning")]
        public List<string> Warning { get; set; }
    }

    [XmlRoot(ElementName = "pucomexReturn")]
    public class PucomexReturn
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }
        [XmlElement(ElementName = "due")]
        public string Due { get; set; }
        [XmlElement(ElementName = "ruc")]
        public string Ruc { get; set; }
        [XmlElement(ElementName = "date")]
        public string Date { get; set; }
        [XmlElement(ElementName = "cpf")]
        public string Cpf { get; set; }
        [XmlElement(ElementName = "warnings")]
        public Warnings Warnings { get; set; }
    }
}
