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
    public static class ServicoSiscomex2
    {
        private static string PerfilSiscomex = ConfigurationManager.AppSettings["PerfilSiscomex"].ToString();
        private static string UrlSiscomex = ConfigurationManager.AppSettings["UrlSiscomexConsultas"].ToString();

        private static HttpClient _httpClient;
        private static WebRequestHandler _handler;

        static ServicoSiscomex2()
        {
            if (_httpClient == null)
            {
                _handler = new WebRequestHandler();
                _httpClient = new HttpClient(_handler);
            }
        }

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

            token = Autenticar(cpfCertificado);

            return token;
        }

        public static async Task<HttpResponseMessage> CriarRequestGet(string url, IDictionary<string, string> headers, string certificado)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            _handler.ClientCertificates.Clear();

            _handler.ClientCertificates.Add(ObterCertificado(certificado));
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;

            _httpClient.DefaultRequestHeaders.Clear();

            foreach (var header in headers)
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

            return await _httpClient.GetAsync(new Uri(UrlSiscomex + url)).ConfigureAwait(false);
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

        public static async Task<List<DadosNotaPreACD>> ConsultarDadosNotaPreACD(string chave, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = await CriarRequestGet(string.Format("/cct/api/ext/deposito-carga/estoque-nota-fiscal/{0}", chave), headers, cpfCertificado);

                if (response.IsSuccessStatusCode)
                {
                    var retorno = await response.Content.ReadAsStringAsync();

                    var dadosNota = JsonConvert.DeserializeObject<DadosNotaPreACD>(retorno);

                    if (dadosNota != null)
                    {
                        if (dadosNota.estoqueNotasFiscais.Count == 0)
                        {
                            return new List<DadosNotaPreACD>
                            {
                                new DadosNotaPreACD{
                                    Sucesso = false,
                                    Mensagem = dadosNota.ObterMensagem()
                                }
                            };
                        }

                        var lista = new List<DadosNotaPreACD>();

                        foreach (var nf in dadosNota.estoqueNotasFiscais)
                        {
                            foreach (var itemNf in nf.itens)
                            {
                                //DateTime? dtRegistro = null;
                                //if (nf.registro.HasValue)
                                //{
                                //    dtRegistro = DateTimeHelpers.ConvertFromUnixTimestamp(Convert.ToInt64(nf.registro));
                                //}

                                lista.Add(new DadosNotaPreACD
                                {
                                    Sucesso = true,
                                    Saldo = itemNf.saldo,
                                    Recinto = nf.recinto,
                                    ResponsavelIdentificacao = nf.responsavel,
                                    Registro = nf.registro?.ToString("dd/MM/yyyy HH:mm") ?? "",
                                    UnidadeReceita = nf.urf,
                                    Item = itemNf.item,
                                    PesoAferido = nf.pesoAferido,
                                    MotivoNaoPesagem = nf.motivoNaoPesagem
                                });
                            }
                        }

                        return lista;
                    }
                }
            }

            return null;
        }

        public static async Task<ConsultaDueDadosResumidos> ObterDetalhesDUE(string due, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = await CriarRequestGet(string.Format("/due/api/ext/due/consultarDadosResumidosDUE?numero={0}", due), headers, cpfCertificado)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var retorno = await response.Content.ReadAsStringAsync();

                    var dadosDUE = JsonConvert.DeserializeObject<ConsultaDueDadosResumidos>(retorno);

                    if (dadosDUE != null)
                    {
                        var obj = new ConsultaDueDadosResumidos
                        {
                            Sucesso = true,
                            numeroDUE = due,
                            situacaoDUE = dadosDUE.situacaoDUE,
                            codigoRecintoAduaneiroDespacho = dadosDUE.codigoRecintoAduaneiroDespacho,
                            uaDespacho = dadosDUE.uaDespacho,
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
            }

            return new ConsultaDueDadosResumidos
            {
                Sucesso = false,
                numeroDUE = due
            };
        }

        public static async Task<DadosDUE> ConsultarDUEPorDanfe(string danfe, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = await CriarRequestGet(string.Format("/due/api/ext/due?nota-fiscal={0}", danfe), headers, cpfCertificado)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return new DadosDUE
                        {
                            Sucesso = false,
                            Mensagem = $"Nenhuma DUE não encontrada para a chave {danfe}"
                        };
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new DadosDUE
                        {
                            Sucesso = false,
                            Mensagem = "Sem permissão para consultar a DUE"
                        };
                    }

                    if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        return new DadosDUE
                        {
                            Sucesso = false,
                            Mensagem = "Serviço Siscomex indisponível no momento. Tente novamente após alguns minutos."
                        };
                    }

                    var retorno = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        var dadosResumidos = JsonConvert.DeserializeObject<List<ListaDadosDUE>>(retorno);

                        if (dadosResumidos != null)
                        {
                            return new DadosDUE
                            {
                                Sucesso = true,
                                ListaDadosDUE = dadosResumidos
                            };
                        }
                    }
                }
                else
                {
                    var retorno = await response.Content.ReadAsStringAsync();

                    return new DadosDUE
                    {
                        Sucesso = false,
                        Mensagem = $"Não foi possível consultar a DUE. Detalhes: " + retorno
                    };
                };
            }

            return null;
        }

        public static async Task<DueDadosCompletos> ObterDUECompleta(string due, string cpfCertificado)
        {
            var token = ObterToken(cpfCertificado);

            if (token != null)
            {
                var headers = ObterHeaders(token);

                var response = await CriarRequestGet(string.Format("/due/api/ext/due/numero-da-due/{0}", due), headers, cpfCertificado)
                    .ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var retorno = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        var objCons = JsonConvert.DeserializeObject<DueDadosCompletos>(retorno);

                        objCons.Sucesso = true;

                        return objCons;
                    }
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new DueDadosCompletos
                        {
                            Sucesso = false,
                            Mensagem = "Siscomex: CPF logado não representa o CNPJ do declarante e/ou o CNPJ do exportador informado - para o perfil selecionado."
                        };
                    }
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
}