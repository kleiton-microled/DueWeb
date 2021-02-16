using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Responses;
using Sistema.DUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Xml;
using System.Xml.Serialization;

namespace Sistema.DUE.Web.XML
{
    public class ValidarXML
    {
        private readonly DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

        public async Task<RetornoXMLValidacao> Validar(XmlDocument document)
        {
            XmlNodeList retificacao = document.GetElementsByTagName("DeclarationNFe");
            var retornoValidacao = new RetornoXMLValidacao();
            if (retificacao.Count > 0)
            {
                retornoValidacao = await ValidaRetificacao(retificacao, document );
         

            }

            XmlNodeList embarqueAntecipado = document.GetElementsByTagName("DeclarationNoNF");

            if (embarqueAntecipado.Count > 0)
            {
                retornoValidacao  = await ValidaEmbarqueAntecipado(embarqueAntecipado, document);
            }

            
            // document.Save(path);
            return retornoValidacao;
        }

        private async Task<RetornoXMLValidacao> ValidaEmbarqueAntecipado(XmlNodeList embarqueAntecipado, XmlDocument document)
        {
            string NumeroDUE = string.Empty;
            var retornoValidacao = new RetornoXMLValidacao();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("bk", "urn:wco:datamodel:WCO:GoodsDeclaration:1");

            foreach (XmlNode xmlEmbarqueAntecipado in embarqueAntecipado)
            {
                var GoodsShipment = document.SelectNodes("//bk:GoodsShipment", nsmgr);

                var ultimoGoodsShipment = GoodsShipment[GoodsShipment.Count - 1];

                var elementosUltimoGoodsShipment = ultimoGoodsShipment.ChildNodes;

                foreach (XmlNode elementoGoodsShipment in elementosUltimoGoodsShipment)
                {
                    if (elementoGoodsShipment.Name == "GovernmentAgencyGoodsItem")
                    {
                        XmlDocumentFragment xfrag = document.CreateDocumentFragment();

                        xfrag.InnerXml = @"
                            <AdditionalDocument xmlns=""" + xmlEmbarqueAntecipado.ParentNode.NamespaceURI + @""">
		                      <CategoryCode>LPCO</CategoryCode>
		                      <ID>MICROLED</ID>                                     
	                        </AdditionalDocument>";

                        elementoGoodsShipment.AppendChild(xfrag);

                        var retorno = await EnviarXMLDUE("/due/api/ext/due/", document.InnerXml, string.Empty, ConfigurationManager.AppSettings["CpfCertificado"].ToString());
                        retornoValidacao.XmlRetorno = retorno.XmlRetorno;
                        retornoValidacao.Debug = retorno.Debug;
                        if (retorno.Sucesso == false)
                        {
                            if (retorno.Criticas != null)
                            {
                                var erros = retorno.Criticas.Where(c => c.Code != "DUEX-ER0644" && c.Code != "DUEX-AL0018").ToList();

                                if (erros.Any())
                                {
                                    foreach (var erro in erros)
                                    {
                                        retornoValidacao.AdicionarMensagem("Siscomex: " + erro.Message);
                                    }

                                    retornoValidacao.Sucesso = false;
                                }
                                else
                                {
                                    retornoValidacao.Sucesso = true;
                                }
                            }
                            else
                            {
                                if (retorno.Code != "DUEX-ER0644" && retorno.Code != "DUEX-AL0018")
                                {
                                    retornoValidacao.AdicionarMensagem("Siscomex: " + retorno.Message);
                                    retornoValidacao.Sucesso = false;
                                }
                                else
                                {
                                    retornoValidacao.Sucesso = true;
                                }
                            }
                        }
                    }
                }
            }

            return retornoValidacao;
        }

        private async Task<RetornoXMLValidacao> ValidaRetificacao(XmlNodeList retificacao, XmlDocument document)
        {
            string NumeroDUE = string.Empty;
            bool temNotasRemessa = false;
            var retornoValidacao = new RetornoXMLValidacao();

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("bk", "urn:wco:datamodel:WCO:GoodsDeclaration:1");

            foreach (XmlNode xmlRetificacao in retificacao)
            {
                foreach (XmlNode dadosDUE in xmlRetificacao.FirstChild)
                {
                    if (dadosDUE.ParentNode.Name.Equals("ID"))
                    {
                        if (dadosDUE.Value.Contains("BR"))
                        {
                            NumeroDUE = dadosDUE.Value;
                        }
                    }
                }

                var GoodsShipment = document.SelectNodes("//bk:GoodsShipment", nsmgr);

                var ultimoGoodsShipment = GoodsShipment[GoodsShipment.Count - 1];

                var elementosUltimoGoodsShipment = ultimoGoodsShipment.ChildNodes;

                foreach (XmlNode elementoGoodsShipment in elementosUltimoGoodsShipment)
                {
                    if (elementoGoodsShipment.Name == "GovernmentAgencyGoodsItem")
                    {
                        var elementosGovernmentAgencyGoodsItem = elementoGoodsShipment.ChildNodes;

                        foreach (XmlNode elementoGovernmentAgencyGoodsItem in elementosGovernmentAgencyGoodsItem)
                        {
                            if (elementoGovernmentAgencyGoodsItem.Name == "Commodity")
                            {
                                var elementosCommodity = elementoGovernmentAgencyGoodsItem.ChildNodes;

                                foreach (XmlNode elementoCommodity in elementosCommodity)
                                {
                                    if (elementoCommodity.Name == "InvoiceLine")
                                    {
                                        var notasDeRemessa = elementoCommodity.ChildNodes;

                                        var ultimaNota = notasDeRemessa[notasDeRemessa.Count - 1];

                                        if (ultimaNota != null)
                                        {
                                            if (ultimaNota.Name == "ReferencedInvoiceLine")
                                            {
                                                temNotasRemessa = true;

                                                var goodsMeasure = ultimaNota.SelectSingleNode("bk:GoodsMeasure", nsmgr);
                                                var quantidadeXml = goodsMeasure.SelectSingleNode("bk:TariffQuantity", nsmgr);

                                                var quantidade = Convert.ToDouble(quantidadeXml.InnerText, new System.Globalization.CultureInfo("en-US"));

                                                quantidade = quantidade - 1;

                                                quantidadeXml.InnerText = quantidade.ToString(new System.Globalization.CultureInfo("en-US"));

                                                var ultimaNotaClonada = ultimaNota.Clone();

                                                if (ultimaNotaClonada != null)
                                                {
                                                    goodsMeasure = ultimaNotaClonada.SelectSingleNode("bk:GoodsMeasure", nsmgr);
                                                    quantidadeXml = goodsMeasure.SelectSingleNode("bk:TariffQuantity", nsmgr);

                                                    quantidadeXml.InnerText = "1.000";

                                                    XmlDocumentFragment xfrag = document.CreateDocumentFragment();

                                                    xfrag.InnerXml = ultimaNotaClonada.OuterXml;

                                                    elementoCommodity.AppendChild(xfrag);

                                                    var retorno = await EnviarXMLDUE(string.Format("/due/api/ext/due/{0}", NumeroDUE), document.InnerXml, NumeroDUE, ConfigurationManager.AppSettings["CpfCertificado"].ToString());
                                                    retornoValidacao.XmlRetorno = retorno.XmlRetorno;
                                                    retornoValidacao.NumeroDUE = NumeroDUE;
                                                    if (retorno.Sucesso == false)
                                                    {
                                                        if (retorno.Criticas != null)
                                                        {
                                                            var erros = retorno.Criticas.Where(c => c.Code != "DUEX-ER0175").ToList();

                                                            if (erros.Any())
                                                            {
                                                                foreach (var erro in erros)
                                                                {
                                                                    retornoValidacao.AdicionarMensagem("Siscomex: " + erro.Message);
                                                                    retornoValidacao.Sucesso = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                retornoValidacao.Sucesso = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (retorno.Code != "DUEX-ER0175")
                                                            {
                                                                retornoValidacao.AdicionarMensagem("Siscomex: " + retorno.Message);
                                                                retornoValidacao.Sucesso = false;
                                                            }
                                                            else
                                                            {
                                                                retornoValidacao.Sucesso = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            if (temNotasRemessa == false)
            {
                retornoValidacao.AdicionarMensagem("O XML não contém notas de remessas");
                retornoValidacao.Sucesso = false;
            }

            return retornoValidacao;
        }

        public async Task<ValidarXmlRetornoSiscomex> EnviarXMLDUE(string url, string xml, string due, string cpfCertificado)
        {
            try
            {
                var retornoSiscomex = new ValidarXmlRetornoSiscomex();

                var token = SisComexService.ObterToken(cpfCertificado);

                if (token == null)
                    throw new Exception("Token não recebido");

                var headers = SisComexService.ObterHeaders(token);

                var response = await SisComexService.CriarRequest(url, headers, xml, due, cpfCertificado);

                var retornoResponse = await response.Content.ReadAsStringAsync();

                retornoSiscomex.Debug = HttpUtility.HtmlEncode(retornoResponse);
                retornoSiscomex.XmlRetorno = retornoResponse;
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Unescape(retornoResponse))))
                {
                    var reader = new StreamReader(memoryStream, Encoding.UTF8);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        try
                        {
                            var xmlSerializer = new XmlSerializer(typeof(ValidarXmlRetornoCriticas));
                            var criticas = (ValidarXmlRetornoCriticas)xmlSerializer.Deserialize(reader);

                            retornoSiscomex.Sucesso = false;
                            retornoSiscomex.Message = criticas.Message;
                            retornoSiscomex.Code = criticas.Code;
                            retornoSiscomex.Criticas = criticas.Detail?.Errors;
                           
                        }
                        catch (Exception ex)
                        {
                            retornoSiscomex.Sucesso = false;
                            retornoSiscomex.Message = "Erro 500. XML recusado pelo Serpro";
                        }
                    }
                    else
                    {
                        var xmlSerializer = new XmlSerializer(typeof(ValidarXmlRetornoSucesso));
                        var sucesso = (ValidarXmlRetornoSucesso)xmlSerializer.Deserialize(reader);

                        retornoSiscomex.Sucesso = true;
                        retornoSiscomex.Message = sucesso.Message;
                        retornoSiscomex.DUE = sucesso.Due;
                        retornoSiscomex.RUC = sucesso.Ruc;
                    }
                }

                return retornoSiscomex;
            }
            catch (Exception ex)
            {
                return new ValidarXmlRetornoSiscomex
                {
                    Sucesso = false,
                    Message = ex.Message
                };
            }
        }
    }
}
