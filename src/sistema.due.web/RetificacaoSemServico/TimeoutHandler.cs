using System;
using System.Net.Http;

namespace Sistema.DUE.Web.RetificacaoSemServico
{
    public class TimeoutHandler : DelegatingHandler
    {
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(100);       
    }
}
