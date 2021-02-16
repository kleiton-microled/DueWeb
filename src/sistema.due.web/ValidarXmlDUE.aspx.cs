using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Responses;
using Sistema.DUE.Web.Services;
using Sistema.DUE.Web.XML;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Sistema.DUE.Web
{
    public partial class ValidarXmlDUE : System.Web.UI.Page
    {
        private readonly DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnValidarDUE_Click(object sender, EventArgs e)
        {
            if (this.txtUpload.PostedFile != null)
            {
                if (this.txtUpload.PostedFile.ContentLength > 0)
                {
                    using (StreamReader reader = new StreamReader(this.txtUpload.PostedFile.InputStream))
                    {
                        StringBuilder conteudo = new StringBuilder();
                        XmlDocument document = new XmlDocument();
                        document.Load(reader);

                        var validarXML = new ValidarXML();
                       var retorno = await validarXML.Validar(document);
                        foreach (var erro in retorno.Mensagens)
                        {
                            ModelState.AddModelError(string.Empty, erro);
                        }

                        ViewState["RetornoSucesso"] = retorno.Sucesso;
                        ViewState["Debug"] = retorno.Debug;

                        document.Save(Path.Combine(Server.MapPath("Uploads"), "NOVO_" + this.txtUpload.PostedFile.FileName + "SS"));

                        var due = dueDAO.ObterDUEPorNumero(retorno.NumeroDUE);
                        if(due!= null)
                        {
                            dueDAO.AtualizarXMLRetorno(due.Id, retorno.XmlRetorno);
                            SalvarTXT(due.DUE, conteudo);
                        }

                    }
                }
            }
        }

        public void SalvarTXT(string due, StringBuilder conteudo)
        {
            string fileName, contentType;
            fileName = $"{due}_XML_CRITICAS.txt";
            contentType = "text/plain";
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            //Response.BinaryWrite(bytes);
            Response.Write(conteudo);
            Response.Flush();
            Response.End();
        }
        /*
    public async Task Validar(XmlDocument document)
    {
        XmlNodeList retificacao = document.GetElementsByTagName("DeclarationNFe");

        if (retificacao.Count > 0)
        {
            await ValidaRetificacao(retificacao, document);
        }

        XmlNodeList embarqueAntecipado = document.GetElementsByTagName("DeclarationNoNF");

        if (embarqueAntecipado.Count > 0)
        {
            await ValidaEmbarqueAntecipado(embarqueAntecipado, document);
        }

        document.Save(Path.Combine(Server.MapPath("Uploads"), "NOVO_" + this.txtUpload.PostedFile.FileName + "SS"));
    }


    private async Task ValidaEmbarqueAntecipado(XmlNodeList embarqueAntecipado, XmlDocument document)
    {
        string NumeroDUE = string.Empty;

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

                    if (retorno.Sucesso == false)
                    {
                        if (retorno.Criticas != null)
                        {
                            var erros = retorno.Criticas.Where(c => c.Code != "DUEX-ER0644" && c.Code != "DUEX-AL0018").ToList();

                            if (erros.Any())
                            {
                                foreach (var erro in erros)
                                {
                                    ModelState.AddModelError(string.Empty, "Siscomex: " + erro.Message);                                        
                                }

                                ViewState["RetornoSucesso"] = false;
                            }
                            else
                            {
                                ModelState.Clear();
                                ViewState["RetornoSucesso"] = true;
                            }
                        }
                        else
                        {
                            if (retorno.Code != "DUEX-ER0644" && retorno.Code != "DUEX-AL0018")
                            {
                                ModelState.AddModelError(string.Empty, "Siscomex: " + retorno.Message);
                                ViewState["RetornoSucesso"] = false;
                            }
                            else
                            {
                                ModelState.Clear();
                                ViewState["RetornoSucesso"] = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private async Task ValidaRetificacao(XmlNodeList retificacao, XmlDocument document)
    {
        string NumeroDUE = string.Empty;
        bool temNotasRemessa = false;

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

                                                if (retorno.Sucesso == false)
                                                {
                                                    if (retorno.Criticas != null)
                                                    {
                                                        var erros = retorno.Criticas.Where(c => c.Code != "DUEX-ER0175").ToList();

                                                        if (erros.Any())
                                                        {
                                                            foreach (var erro in erros)
                                                            {
                                                                ModelState.AddModelError(string.Empty, "Siscomex: " + erro.Message);
                                                                ViewState["RetornoSucesso"] = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ModelState.Clear();
                                                            ViewState["RetornoSucesso"] = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (retorno.Code != "DUEX-ER0175")
                                                        {
                                                            ModelState.AddModelError(string.Empty, "Siscomex: " + retorno.Message);
                                                            ViewState["RetornoSucesso"] = false;
                                                        }
                                                        else
                                                        {
                                                            ModelState.Clear();
                                                            ViewState["RetornoSucesso"] = true;
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
            ModelState.AddModelError(string.Empty, "O XML não contém notas de remessas");
            ViewState["RetornoSucesso"] = false;
        }

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

            ViewState["Debug"] = HttpUtility.HtmlEncode(retornoResponse);

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
    */
    }
}