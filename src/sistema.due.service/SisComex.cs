using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RestSharp;
using System.Net.Cache;

namespace Sistema.DUE.Service
{
    public class SisComex
    {
        private static string PerfilSiscomex = ConfigurationManager.AppSettings["PerfilSiscomex"].ToString();
        private static string UrlSiscomex = ConfigurationManager.AppSettings["UrlSiscomex"].ToString();

        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        private static Token Autenticar(string cpfCertificado)
        {
            var token = new Token();

            using (var handler = new WebRequestHandler())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                handler.ClientCertificates.Add(ObterCertificado(cpfCertificado));
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("Role-Type", PerfilSiscomex);

                    var request = new HttpRequestMessage(HttpMethod.Post, new Uri(UrlSiscomex + "/portal/api/autenticar"));
                    var response = client.SendAsync(request).Result;

                    response.EnsureSuccessStatusCode();

                    IEnumerable<string> valor;

                    if (response.Headers.TryGetValues("set-token", out valor))
                        token.SetToken = valor.FirstOrDefault();

                    if (response.Headers.TryGetValues("x-csrf-token", out valor))
                        token.CsrfToken = valor.FirstOrDefault();

                    if (response.Headers.TryGetValues("x-csrf-expiration", out valor))
                        token.CsrfExpiration = valor.FirstOrDefault();

                    return token;
                }
            }
        }

        private static Token ObterToken(string cpfCertificado)
        {
            var token = new Token();

            token = Autenticar(cpfCertificado);

            return token;
        }

        private static string CriarRequestGet(string url, IDictionary<string, string> headers, string certificado)
        {
            using (var handler = new WebRequestHandler())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                handler.ClientCertificates.Add(ObterCertificado(certificado));
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                using (var client = new HttpClient(handler))
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                    return client.GetStringAsync(new Uri(UrlSiscomex + url)).Result;
                }
            }
        }

        private static HttpResponseMessage CriarRequestPost(string url, IDictionary<string, string> headers, string xml, string cpfCertificado)
        {
            using (var handler = new WebRequestHandler())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                handler.ClientCertificates.Add(ObterCertificado(cpfCertificado));
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                    xml = xml.Replace("\r\n", string.Empty);

                    using (var stringContent = new StringContent(xml, Encoding.UTF8, "application/xml"))
                    {
                        return client.PostAsync(new Uri(UrlSiscomex + url), stringContent).Result;
                    }
                }
            }
        }

        private static HttpResponseMessage CriarRequestPut(string url, IDictionary<string, string> headers, string xml, string cpfCertificado)
        {
            using (var handler = new WebRequestHandler())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                handler.ClientCertificates.Add(ObterCertificado(cpfCertificado));
                ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = new TimeSpan(0, 15, 00);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);

                    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

                    xml = xml.Replace("\r\n", string.Empty);
                    xml = System.Text.RegularExpressions.Regex.Replace(xml, @"\t|\n|\r", "");
                    xml = xml.Replace("      ", " ");
                    xml = xml.Replace("    ", " ");
                    xml = xml.Replace("   ", " ");
                    xml = xml.Replace("  ", " ");
                    xml = xml.Replace(Environment.NewLine, "");

                    using (var stringContent = new StringContent(xml, Encoding.UTF8, "application/xml"))
                    {
                        return client.PutAsync(new Uri(string.Concat(UrlSiscomex, url)), stringContent).Result;
                    }
                }
            }
        }

        public static IEnumerable<X509Certificate2> ListarCertificadosInstalados(StoreLocation storeLocation)
        {
            var stores = new X509Store(StoreName.My, storeLocation);

            stores.Open(OpenFlags.ReadOnly);

            var certificadosInstalados = stores.Certificates;

            certificadosInstalados.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            certificadosInstalados.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

            stores.Close();

            var certificados = new List<X509Certificate2>();

            foreach (X509Certificate2 certificado in certificadosInstalados)
                yield return certificado;
        }

        private static X509Certificate2 ObterCertificado(string cpf)
        {
            var certificado = ListarCertificadosInstalados(StoreLocation.LocalMachine)
                .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));

            if (certificado == null)
            {
                certificado = ListarCertificadosInstalados(StoreLocation.CurrentUser)
                    .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));
            }

            if (certificado == null)
                throw new Exception($"Certificado Digital de CPF {cpf} não encontrado");

            return certificado;
        }

        private static Dictionary<string, string> ObterHeaders(Token token) => new Dictionary<string, string>
        {
            {"Authorization", token.SetToken},
            {"x-csrf-token", token.CsrfToken}
        };

        public static DueDadosResumidos ConsultarDadosResumidos(string numeroDUE, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/due/api/ext/due/consultarDadosResumidosDUE?numero={0}", numeroDUE), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    var dadosResumidos = JsonConvert.DeserializeObject<DueDadosResumidos>(response);

                    if (dadosResumidos != null)
                        return dadosResumidos;
                }
            }

            return null;
        }

        public static DueDadosCompletos ConsultarDadosCompletos(string numeroDUE, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/due/api/ext/due/numero-da-due/{0}", numeroDUE), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    var dadosResumidos = JsonConvert.DeserializeObject<DueDadosCompletos>(response);

                    if (dadosResumidos != null)
                        return dadosResumidos;
                }
            }

            return null;
        }

        public static DadosNotaPreACD ConsultarDadosNotaPreACD(string chave, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/cct/api/ext/deposito-carga/estoque-nota-fiscal/{0}", chave), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    var dadosNota = JsonConvert.DeserializeObject<DadosNotaPreACD>(response);

                    if (dadosNota != null)
                    {
                        if (dadosNota.estoqueNotasFiscais.Count == 0)
                        {
                            return new DadosNotaPreACD
                            {
                                sucesso = false,
                                Mensagem = dadosNota.ObterMensagem()
                            };
                        }

                        return new DadosNotaPreACD
                        {
                            sucesso = true,
                            Saldo = dadosNota.ObterSaldo()
                        };
                    }
                }
            }

            return null;
        }

        public static HttpResponseMessage EnviarDUESemNF(string xml, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                return CriarRequestPost("/due/api/ext/due", headers, xml, cpfCertificado);
            }

            return null;
        }

        public static HttpResponseMessage EnviarRetificacao(string xml, string due, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                return CriarRequestPut(string.Format("/due/api/ext/due/{0}", due), headers, xml, cpfCertificado);
            }

            return null;
        }
    }

    public class Token
    {
        public string SetToken { get; set; }

        public string CsrfToken { get; set; }

        public string CsrfExpiration { get; set; }

        public bool Valido() => SetToken?.Length > 0 && CsrfToken?.Length > 0
                && TimeSpan.FromMilliseconds(Convert.ToDouble(CsrfExpiration ?? "0")).TotalMinutes > 0;
    }

    public class CertificateWebClient : WebClient
    {
        private readonly X509Certificate2 certificate;

        public CertificateWebClient(X509Certificate2 cert)
        {
            certificate = cert;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate X509certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
            {
                return true;
            };

            request.ClientCertificates.Add(certificate);
            return request;
        }
    }
}