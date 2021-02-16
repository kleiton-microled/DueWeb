using System.Xml.Serialization;

namespace Cargill.DUE.Service
{
    [XmlRoot(ElementName = "error")]
    public class RetornoCriticas
    {
        [XmlElement(ElementName = "message")]
        public string Message { get; set; }
        [XmlElement(ElementName = "tag")]
        public string Tag { get; set; }
        [XmlElement(ElementName = "status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "severity")]
        public string Severity { get; set; }
        [XmlElement(ElementName = "detail")]
        public Detail Detail { get; set; }
    }

    public class Detail
    {
        [XmlElement(ElementName = "error")]
        public RetornoCriticas[] Errors { get; set; }
    }
}