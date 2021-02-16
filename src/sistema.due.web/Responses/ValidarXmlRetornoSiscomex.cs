using System.Xml.Serialization;

namespace Sistema.DUE.Web.Responses
{
    public class ValidarXmlRetornoSiscomex
    {
        public bool Sucesso { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public ValidarXmlRetornoCriticas[] Criticas { get; set; }
        public string DUE { get; set; }
        public string RUC { get; set; }
        public string ChaveDeAcesso { get; set; }
        public string XmlRetorno { get; set; }

        public string Debug { get; set; }
    }

    [XmlRoot(ElementName = "error")]
    public class ValidarXmlRetornoCriticas
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }
        [XmlElement(ElementName = "code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "tag")]
        public string Tag { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "severity")]
        public string Severity { get; set; }
        [XmlElement(ElementName = "detail")]
        public ValidarXmlDetail Detail { get; set; }
    }

    public class ValidarXmlDetail
    {
        [XmlElement(ElementName = "error")]
        public ValidarXmlRetornoCriticas[] Errors { get; set; }
    }

    [XmlRoot(ElementName = "pucomexReturn")]
    public class ValidarXmlRetornoSucesso
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