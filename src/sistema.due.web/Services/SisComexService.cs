using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Sistema.DUE.Web.Services
{
    public class SisComexService
    {
        private static string PerfilSiscomex = ConfigurationManager.AppSettings["PerfilSiscomex"].ToString();
        private static string UrlSiscomex = ConfigurationManager.AppSettings["UrlSiscomexConsultas"].ToString();

        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        public static Token Autenticar(string cpfCertificado)
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
                    //Task.Delay(1000);
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

        public static Token ObterToken(string cpfCertificado)
        {
            var token = new Token();
            try
            {
                token = Autenticar(cpfCertificado);
            }
            catch (Exception)
            {
                try
                {
                    token = Autenticar(cpfCertificado);
                }
                catch (Exception)
                {
                    token = Autenticar(cpfCertificado);
                }
            }

            return token;
        }

        public static string CriarRequestGet(string url, IDictionary<string, string> headers, string certificado)
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

                    try
                    {
                        var retorno = client.GetAsync(new Uri(UrlSiscomex + url)).Result;

                        if (retorno.IsSuccessStatusCode)
                        {
                            return retorno.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            var msg = retorno.Content.ReadAsStringAsync().Result;

                            if (!string.IsNullOrEmpty(msg))
                            {
                                try
                                {
                                    var jsonObj = JsonConvert.DeserializeObject<ErroSiscomexResponse>(msg);

                                    if (jsonObj != null)
                                    {
                                        return jsonObj.message;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (retorno.StatusCode == HttpStatusCode.NotFound)
                                        return "DUE não encontrada!";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    return string.Empty;
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

        public static X509Certificate2 ObterCertificado(string cpf)
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

        public static Dictionary<string, string> ObterHeaders(Token token) => new Dictionary<string, string>
        {
            {"Authorization", token.SetToken},
            {"x-csrf-token", token.CsrfToken}
        };

        public static DadosNotaPreACD ConsultarDadosNotaPreACD(string chave, int item, string cpfCertificado)
        {
            Token token = null;

            if (HttpContext.Current.Session["TOKEN"] == null)
            {
                token = ObterToken(cpfCertificado);
            }
            else
            {
                token = (Token)HttpContext.Current.Session["TOKEN"];
            }

            if (token != null)
            {
                HttpContext.Current.Session["TOKEN"] = token;

                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/cct/api/ext/deposito-carga/estoque-nota-fiscal/{0}", chave), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    var dadosNota = JsonConvert.DeserializeObject<DadosNotaPreACD>(response);

                    if (dadosNota != null)
                    {
                        var nota = dadosNota.estoqueNotasFiscais
                            .SelectMany(c => c.itens
                            .Where(d => d.item == item))
                            .FirstOrDefault();

                        if (dadosNota.estoqueNotasFiscais.Count == 0)
                        {
                            return new DadosNotaPreACD
                            {
                                Sucesso = false,
                                Mensagem = dadosNota.ObterMensagem()
                            };
                        }

                        return new DadosNotaPreACD
                        {
                            Sucesso = true,
                            Saldo = nota?.saldo ?? 0,
                            Recinto = dadosNota.Recinto
                        };
                    }
                }
            }

            return null;
        }


        public static ConsultaDueDadosResumidos ObterDetalhesDUE(string due, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/due/api/ext/due/consultarDadosResumidosDUE?numero={0}", due), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    ConsultaDueDadosResumidos dadosDUE = new ConsultaDueDadosResumidos();

                    try
                    {
                        dadosDUE = JsonConvert.DeserializeObject<ConsultaDueDadosResumidos>(response);

                        if (dadosDUE != null)
                        {
                            var obj = new ConsultaDueDadosResumidos
                            {
                                Sucesso = true,
                                numeroDUE = due,
                                situacaoDUE = dadosDUE.situacaoDUE,
                                Mensagem = ""
                            };

                            DateTime result;

                            if (DateTime.TryParse(dadosDUE.dataSituacaoDUE, out result))
                            {
                                obj.dataSituacaoDUE = result.ToString("dd/MM/yyyy HH:mm");
                            }
                            else
                            {
                                obj.dataSituacaoDUE = dadosDUE.dataSituacaoDUE;
                            }

                            return obj;
                        }
                        else
                        {
                            return new ConsultaDueDadosResumidos
                            {
                                Sucesso = false,
                                numeroDUE = due,
                                Mensagem = "DUE não encontrada (Siscomex)"
                            };
                        }
                    }
                    catch (Exception)
                    {
                        return new ConsultaDueDadosResumidos
                        {
                            Sucesso = false,
                            numeroDUE = due,
                            Mensagem = "Portal Microled: Falha ao obter os dados da DUE - Detalhes: " + response
                        };
                    }
                }
            }

            return new ConsultaDueDadosResumidos
            {
                Sucesso = false,
                numeroDUE = due
            };
        }

        public static DueDadosCompletos ObterDUECompleta(string due, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = CriarRequestGet(string.Format("/due/api/ext/due/numero-da-due/{0}", due), headers, cpfCertificado);

                if (!string.IsNullOrEmpty(response))
                {
                    if (response.ToLower().Contains("cpf logado não representa o cnpj do declarante") || response.Contains("DUE não encontrada!"))
                    {
                        var objErro = new DueDadosCompletos();
                        objErro.Sucesso = false;
                        objErro.Mensagem = response;

                        return objErro;
                    }

                    var objCons = JsonConvert.DeserializeObject<DueDadosCompletos>(response);

                    objCons.Sucesso = true;

                    return objCons;
                }
            }

            return null;
        }

        public static async Task<HttpResponseMessage> CriarRequest(string url, IDictionary<string, string> headers, string xml, string due, string cpfCertificado)
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
                        if (!string.IsNullOrEmpty(due))
                        {
                            return await client.PutAsync(new Uri(string.Concat(UrlSiscomex, url)), stringContent);
                        }
                        else
                        {
                            return await client.PostAsync(new Uri(string.Concat(UrlSiscomex, url)), stringContent);
                        }
                    }
                }
            }
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