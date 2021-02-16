using System;
using System.Xml.Serialization;

namespace Sistema.DUE.Web.Responses
{

    public class ErroSiscomexResponse
    {
        public string message { get; set; }
        public string tag { get; set; }
        public DateTime date { get; set; }
        public int status { get; set; }
        public string severity { get; set; }
        public ErroSiscomexResponseInfo info { get; set; }
    }

    public class ErroSiscomexResponseInfo
    {
        public string mnemonico { get; set; }
        public string sistema { get; set; }
        public string ambiente { get; set; }
        public string visao { get; set; }
        public string usuario { get; set; }
        public string url { get; set; }
    }

}