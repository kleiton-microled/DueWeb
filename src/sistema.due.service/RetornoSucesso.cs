using System.Xml.Serialization;

namespace Cargill.DUE.Service
{
    [XmlRoot(ElementName = "pucomexReturn")]
    public class RetornoSucesso
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }
        [XmlElement(ElementName = "due")]
        public string Due { get; set; }
        [XmlElement(ElementName = "ruc")]
        public string Ruc { get; set; }
        [XmlElement(ElementName = "chaveDeAcesso")]
        public string ChaveDeAcesso { get; set; }
        [XmlElement(ElementName = "date")]
        public string Date { get; set; }
        [XmlElement(ElementName = "cpf")]
        public string Cpf { get; set; }
    }
}