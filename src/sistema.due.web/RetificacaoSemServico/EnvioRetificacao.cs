using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Sistema.DUE.Web.ServicoSiscomex;

namespace Sistema.DUE.Web.RetificacaoSemServico
{
    public class EnvioRetificacao
    {
        private static string PerfilSiscomex = ConfigurationManager.AppSettings["PerfilSiscomex"].ToString();
        private static string UrlSiscomex = ConfigurationManager.AppSettings["UrlSiscomexEnvio"].ToString();
        private static HttpClient Client = new HttpClient();
        public static async Task<RetornoSiscomex> EnviarSemServico(string xml, string due, string cpfCertificado)
        {
            try
            {
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    var token = new Token();

                    token = Autenticar(cpfCertificado);

                    if (token != null)
                    {
                        var headers = ObterHeaders(token);

                        var url = string.Format("/due/api/ext/due/{0}", due);



                        using (var handler = new WebRequestHandler())
                        {
                            handler.UseProxy = false;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                            handler.ClientCertificates.Add(ObterCertificado(cpfCertificado));
                            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

                            using (var client = new HttpClient(handler))
                            {
                                // client.Timeout = new TimeSpan(0, 15, 00);
                                client.Timeout = new TimeSpan(25, 0, 0);

                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                                foreach (var header in headers)
                                    client.DefaultRequestHeaders.Add(header.Key, header.Value);

                                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                                client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");

                                using (var stringContent = new StringContent(xml, Encoding.UTF8, "application/xml"))
                                {
                                    HttpResponseMessage retorno = await client
                                .PutAsync(new Uri(string.Concat(UrlSiscomex, url)), stringContent)
                                .ConfigureAwait(false);
                                    response = retorno;
                                }
                            }
                        }
                    }

                    var retornoSiscomex = new ServicoSiscomex.RetornoSiscomex();

                    var retornoResponse = response.Content.ReadAsStringAsync().Result;

                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Unescape(retornoResponse))))
                    {
                        var reader = new StreamReader(memoryStream, Encoding.UTF8);

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            try
                            {
                                var xmlSerializer = new XmlSerializer(typeof(RetornoCriticas));
                                var criticas = (ServicoSiscomex.RetornoCriticas)xmlSerializer.Deserialize(reader);

                                retornoSiscomex.Sucesso = false;
                                retornoSiscomex.Warnings = false;
                                retornoSiscomex.Message = criticas.message;
                                retornoSiscomex.Criticas = criticas.detail;
                                retornoSiscomex.XmlRetorno = retornoResponse;
                            }
                            catch (Exception ex)
                            {
                                retornoSiscomex.Sucesso = false;
                                retornoSiscomex.Message = "Erro: " + retornoResponse;
                                retornoSiscomex.XmlRetorno = retornoResponse;
                            }
                        }
                        else
                        {
                            if (retornoResponse.Contains("warnings"))
                            {
                                var xmlSerializer = new XmlSerializer(typeof(PucomexReturn));
                                var warnings = (PucomexReturn)xmlSerializer.Deserialize(reader);

                                retornoSiscomex.Sucesso = true;
                                retornoSiscomex.Warnings = true;
                                retornoSiscomex.Message = string.Join("<br />", warnings.Warnings.Warning);
                            }
                            else
                            {
                                var xmlSerializer = new XmlSerializer(typeof(RetornoSucesso));
                                var sucesso = (RetornoSucesso)xmlSerializer.Deserialize(reader);

                                retornoSiscomex.Sucesso = true;
                                retornoSiscomex.Warnings = false;
                                retornoSiscomex.Message = sucesso.Message;
                                retornoSiscomex.DUE = sucesso.Due;
                                retornoSiscomex.RUC = sucesso.Ruc;
                            }
                        }
                    }

                    return retornoSiscomex;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new RetornoSiscomex
                {
                    Sucesso = false,
                    Message = ex.Message
                };
            }
        }

        
        private static Dictionary<string, string> ObterHeaders(Token token) => new Dictionary<string, string>
        {
            {"Authorization", token.SetToken},
            {"x-csrf-token", token.CsrfToken}
        };

        private static Token Autenticar(string cpfCertificado)
        {
            var token = new Token();

            using (var handler = new WebRequestHandler())
            {
                handler.UseProxy = false;
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

        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
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

    }

    public class Token
    {
        public string SetToken { get; set; }

        public string CsrfToken { get; set; }

        public string CsrfExpiration { get; set; }

        public bool Valido() => SetToken?.Length > 0 && CsrfToken?.Length > 0
                && TimeSpan.FromMilliseconds(Convert.ToDouble(CsrfExpiration ?? "0")).TotalMinutes > 0;
    }
}






