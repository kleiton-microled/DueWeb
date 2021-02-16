using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml;

namespace Sistema.DUE.Web
{
    public partial class CadastrarNotas : System.Web.UI.Page
    {
        private readonly CertificadoDAO _certificadoDAO = new CertificadoDAO();
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly EnquadramentosDAO enquadramentosDAO = new EnquadramentosDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    this.txtDueID.Value = Request.QueryString["id"].ToString();
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }

                var enquadramentos = enquadramentosDAO.ObterEnquadramentos();

                foreach (var enquadramento in enquadramentos)
                    this.cbEnquadramento.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));

                this.cbEnquadramento.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            }
        }

        [WebMethod]
        public static string AdicionarNotaFiscal(
            string chaveNF,
            string numero,
            string cnpj,
            string quantidade,
            string unidade,
            string ncm,
            string dueId,
            string vmcv,
            string vmle)
        {
            if (!string.IsNullOrEmpty(chaveNF))
            {
                chaveNF = chaveNF.Replace(" ", "");
            }

            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var notaFiscalDAO = new NotaFiscalDAO();

            var dueBusca = documentoUnicoExportacaoDAO.ObterDUEPorId(dueId.ToInt());

            var atributosCafe = dueBusca.Attr_Padrao_Qualidade_Default + dueBusca.Attr_Tipo_Default + dueBusca.Attr_Embarque_Em_Default +
                dueBusca.Attr_Caracteristica_Especial_Default + dueBusca.Attr_Outra_Caracteristica_Especial_Default + dueBusca.Attr_Metodo_Processamento_Default;

            if (ncm != "09011110" && ncm != "09011190" && ncm != "09011200" && ncm != "09012100" && ncm != "09012200" && ncm != "21011110" && ncm != "21011190" && ncm != "21011200")
            {
                if (!string.IsNullOrEmpty(atributosCafe))
                {
                    return @"<strong>Alerta:</strong> Você informou atributos de café nas informações default, mas o NCM da nota que está sendo cadastrada é diferente de NCMs para exportação de café.";
                }
            }

            if (dueBusca != null)
            {
                var itensDue = documentoUnicoExportacaoDAO
                    .ObterItensDUE(dueId.ToInt())
                    .Where(c => !string.IsNullOrEmpty(c.NF))
                    .ToList();

                if (itensDue.Any() && (dueBusca.Completa == 0 || dueBusca.CriadoPorNF == 0))
                {
                    return @"<strong>Alerta: Não é possível cadastrar Notas de forma manual para uma DUE que já contém itens ou que não foi criada de forma automática.</strong> <br /><br/>
                            Opção 1: Caso necessite vincular uma Nota Fiscal para um item já existente, utilize a opção do menu 'Vincular DUE com NF' ou importe o arquivo de notas.<br /><br/>
                            Opção 2: Caso necessite cadastrar as Notas Fiscais manualmente, exclua os itens já existentes nesta DUE (o sistema irá recria-los de forma automática).";
                }

                if (!documentoUnicoExportacaoDAO.JaExisteNotaCadastrada(chaveNF, dueId.ToInt()))
                {
                    notaFiscalDAO.Cadastrar(new NotaFiscal
                    {
                        TipoNF = "EXP",
                        Item = 1,
                        ChaveNF = chaveNF,
                        NumeroNF = numero,
                        CnpjNF = cnpj,
                        QuantidadeNF = quantidade.ToDecimal(),
                        UnidadeNF = unidade,
                        NCM = ncm,
                        Usuario = Convert.ToInt32(HttpContext.Current.Session["UsuarioId"].ToString()),
                        OBS = "DUE MANUAL",
                        DueId = dueId.ToInt()
                    });
                }

                var id = documentoUnicoExportacaoDAO.RegistrarItemDUE(new DUEItem
                {
                    DueId = dueId.ToInt(),
                    NF = chaveNF,
                    ValorUnitVMLE = vmle.ToDecimal(),
                    ValorUnitVMCV = vmcv.ToDecimal(),
                    CondicaoVenda = dueBusca.CondicaoVenda_Default
                });

                if (id > 0)
                {
                    documentoUnicoExportacaoDAO.MarcarComoAutomatica(dueBusca.Id);

                    decimal resultadoVMLE = 0;
                    decimal resultadoVMCV = 0;
                    decimal quantidadeTotal = 0;

                    if (unidade == "TON")
                    {
                        resultadoVMLE = (((quantidade.ToDecimal() * 1000) * (dueBusca.ValorUnitVMLE_Default / 1000)));
                        resultadoVMCV = (((quantidade.ToDecimal() * 1000) * (dueBusca.ValorUnitVMCV_Default / 1000)));
                        quantidadeTotal = (quantidade.ToDecimal() * 1000);
                    }
                    else
                    {
                        resultadoVMLE = (((quantidade.ToDecimal()) * (dueBusca.ValorUnitVMLE_Default / 1000)));
                        resultadoVMCV = (((quantidade.ToDecimal()) * (dueBusca.ValorUnitVMCV_Default / 1000)));
                        quantidadeTotal = quantidade.ToDecimal();
                    }

                    var dueDetalhe = new DUEItemDetalhes(
                        id,
                        1,
                        resultadoVMLE,
                        dueBusca.PaisDestino_Default,
                        quantidade.ToDecimal(),
                        ncm,
                        resultadoVMCV,
                        quantidade.ToDecimal(),
                        quantidadeTotal,
                        dueBusca.Enquadramento1_Default,
                        dueBusca.Enquadramento2_Default,
                        dueBusca.Enquadramento3_Default,
                        dueBusca.Enquadramento4_Default,
                        dueBusca.DescricaoComplementar_Default,
                        dueBusca.Attr_Padrao_Qualidade_Default,
                        dueBusca.Attr_Embarque_Em_Default,
                        dueBusca.Attr_Tipo_Default,
                        dueBusca.Attr_Metodo_Processamento_Default,
                        dueBusca.Attr_Caracteristica_Especial_Default,
                        dueBusca.Attr_Outra_Caracteristica_Especial_Default,
                        dueBusca.Attr_Embalagem_Final_Default);

                    var itemDetalheId = documentoUnicoExportacaoDAO.RegistrarDUEItemDetalhe(dueDetalhe);

                    if (itemDetalheId > 0)
                    {
                        if (!string.IsNullOrEmpty(dueBusca.LPCO_Default))
                        {
                            if (dueBusca.Enquadramento1_Default == 80380 || dueBusca.Enquadramento2_Default == 80380 || dueBusca.Enquadramento3_Default == 80380 || dueBusca.Enquadramento4_Default == 80380)
                            {
                                var lpcos = dueBusca.LPCO_Default.Split(',');

                                foreach (var lpco in lpcos)
                                {
                                    documentoUnicoExportacaoDAO.RegistrarLPCO(new DUEItemDetalhesLPCO(itemDetalheId, lpco));
                                }
                            }
                        }
                    }
                }
            }

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisDUE(dueId.ToInt());

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);
        }

        [WebMethod]
        public static string RemoverNotaFiscal(int id)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var notaFiscalDAO = new NotaFiscalDAO();

            var nf = notaFiscalDAO.ObterNotaFiscalPorId(id);

            if (nf == null)
                return "Erro: Nota Fiscal não encontrada";

            var item = documentoUnicoExportacaoDAO.ObterItemPorNota(nf.ChaveNF, nf.DueId);

            if (item != null)
            {
                documentoUnicoExportacaoDAO.ExcluirItemDUE(item.Id);
            }
            
            documentoUnicoExportacaoDAO.ExcluirNotaFiscalManual(id);

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisDUE(nf.DueId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterNotas(string dueId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisDUE(dueId.ToInt());

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string VisualizarNF(int id)
        {
            var notaFiscalDAO = new NotaFiscalDAO();

            var nota = notaFiscalDAO.ObterNotaFiscalPorId(id);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                nota.ChaveNF,
                nota.NumeroNF,
                nota.CnpjNF,
                nota.QuantidadeNF,
                nota.UnidadeNF,
                nota.NCM
            });
        }

        [WebMethod]
        public static string AdicionarNotaFiscalRemessa(
           string tipo,
           string item,
           string chaveNF,
           string chaveReferenciada,
           string numero,
           string cnpj,
           string quantidade,
           string unidade,
           string ncm,
           string dueId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var notaFiscalDAO = new NotaFiscalDAO();

            if (!documentoUnicoExportacaoDAO.JaExisteNotaRemessaCadastrada(chaveNF, item.ToInt(), dueId.ToInt(), chaveReferenciada))
            {
                notaFiscalDAO.Cadastrar(new NotaFiscal
                {
                    TipoNF = tipo,
                    Item = item.ToInt() == 0 ? 1 : item.ToInt(),
                    ChaveNF = chaveNF,
                    ChaveNFReferencia = chaveReferenciada,
                    NumeroNF = numero,
                    CnpjNF = cnpj,
                    QuantidadeNF = quantidade.ToDecimal(),
                    UnidadeNF = unidade,
                    NCM = ncm,
                    Usuario = Convert.ToInt32(HttpContext.Current.Session["UsuarioId"].ToString()),
                    OBS = "DUE MANUAL",
                    DueId = dueId.ToInt()
                });
            }

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisRemessaDUE(chaveReferenciada, dueId.ToInt());

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);
        }

        [WebMethod]
        public static string RemoverNotaFiscalRemessa(int id)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var notaFiscalDAO = new NotaFiscalDAO();

            var nota = notaFiscalDAO.ObterNotaFiscalPorId(id);

            if (nota == null)
                return "Nota Fiscal não encontrada";

            notaFiscalDAO.ExcluirNotaFiscal(id);

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisRemessaDUE(nota.ChaveNFReferencia, nota.DueId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterNotasRemessa(string chaveReferenciada, string dueId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

            var notas = documentoUnicoExportacaoDAO.ObterNotasFiscaisRemessaDUE(chaveReferenciada, dueId.ToInt());

            return Newtonsoft.Json.JsonConvert.SerializeObject(notas);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string VisualizarNotaFiscalRemessa(int id)
        {
            var notaFiscalDAO = new NotaFiscalDAO();

            var nota = notaFiscalDAO.ObterNotaFiscalPorId(id);

            if (nota == null)
            {
                return "Nota Fiscal não encontrada";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                nota.ChaveNF,
                nota.TipoNF,
                nota.Item,
                nota.NumeroNF,
                nota.CnpjNF,
                nota.QuantidadeNF,
                nota.UnidadeNF,
                nota.NCM
            });
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static bool DueManual(string dueId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

            var dueBusca = documentoUnicoExportacaoDAO.ObterDUEPorId(dueId.ToInt());

            if (dueBusca != null)
            {
                var itensDue = documentoUnicoExportacaoDAO
                    .ObterItensDUE(dueId.ToInt())
                    .Where(c => !string.IsNullOrEmpty(c.NF))
                    .ToList();

                if (itensDue.Any() && (dueBusca.Completa == 0 || dueBusca.CriadoPorNF == 0))
                {
                    return true;
                }
            }

            return false;
        }

        protected void txtChaveNF_TextChanged(object sender, EventArgs e)
        {
            var chaveNF = string.Empty;

            if (this.txtChaveNF.Text != string.Empty)
            {
                chaveNF = this.txtChaveNF.Text.Replace(" ", "");

                if (chaveNF.Length == 44)
                {
                    this.txtNumero.Enabled = false;
                    this.txtCNPJ.Enabled = false;

                    var nfe = chaveNF.Trim();

                    this.txtNumero.Text = nfe.Substring(25, 9).ToInt().ToString();
                    this.txtCNPJ.Text = nfe.Substring(6, 14);

                }
                else
                {
                    this.txtNumero.Enabled = true;
                    this.txtCNPJ.Enabled = true;
                }
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtUpload.PostedFile == null)
                    ModelState.AddModelError(string.Empty, "Nenhum arquivo informado");

                if (this.txtUpload.PostedFile.ContentLength == 0)
                    ModelState.AddModelError(string.Empty, "Arquivo inválido");

                var extensao = new FileInfo(this.txtUpload.PostedFile.FileName)?.Extension?.ToUpper() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(extensao))
                    ModelState.AddModelError(string.Empty, "Arquivo inválido");

                if (!extensao.Equals(".XML") && !extensao.Equals(".TXT") && !extensao.Equals(".CSV"))
                    ModelState.AddModelError(string.Empty, "É permitido apenas importação de arquivos .txt, .csv ou .xml");

                if (!ModelState.IsValid)
                    return;

                if (!UploadArquivo(this.txtUpload))
                {
                    throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
                }

                if (extensao == ".XML")
                {
                    try
                    {
                        var dueBusca = _documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                        if (dueBusca == null)
                            throw new Exception("DUE não encontrada");

                        if (this.txtUpload.HasFiles)
                        {
                            int xmlsImportados = 1;                            

                            foreach (HttpPostedFile uploadedFile in this.txtUpload.PostedFiles)
                            {                              
                                string nomeArquivo = Path.Combine(Server.MapPath("Uploads"), uploadedFile.FileName);

                                XmlDocument doc = new XmlDocument();
                                doc.Load(nomeArquivo);

                                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                                ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");

                                var cnpjEmitente = "";
                                var razaoSocialEmitente = "";
                                var enderecoEmitente = "";
                                var estadoEmitente = "";

                                var informacoesEmitente = doc.SelectNodes("//nfe:emit", ns);

                                foreach (XmlNode dadoEmit in informacoesEmitente)
                                {
                                    foreach (XmlNode childNode in dadoEmit.ChildNodes)
                                    {
                                        if (childNode.Name == "CNPJ")
                                        {
                                            cnpjEmitente = childNode.InnerText;
                                        }

                                        if (childNode.Name == "xNome")
                                        {
                                            razaoSocialEmitente = childNode.InnerText;
                                        }

                                        if (childNode.Name == "enderEmit")
                                        {
                                            foreach (XmlNode emitEndereco in childNode.ChildNodes)
                                            {
                                                if (emitEndereco.Name == "xLgr")
                                                {
                                                    enderecoEmitente = emitEndereco.InnerText;
                                                }

                                                if (emitEndereco.Name == "nro")
                                                {
                                                    enderecoEmitente += ", " + emitEndereco.InnerText;
                                                }

                                                if (emitEndereco.Name == "UF")
                                                {
                                                    estadoEmitente = emitEndereco.InnerText;
                                                }
                                            }                                                
                                        }
                                    }
                                }

                                var dadosNfe = doc.SelectNodes("//nfe:ide", ns);

                                var numeroNf = "";

                                foreach (XmlNode dadoNfe in dadosNfe)
                                {
                                    foreach (XmlNode childNode in dadoNfe.ChildNodes)
                                    {
                                        if (childNode.Name == "nNF")
                                        {
                                            numeroNf = childNode.InnerText;
                                        }
                                    }
                                }

                                var paisesDanfe = PaisesDAO.ObterPaisesDanfe();

                                var nomeDestinatario = string.Empty;
                                var paisDestinatario = string.Empty;
                                var logradouroDestinatario = string.Empty;
                                var logradouroNumeroDestinatario = string.Empty;

                                var dadosDestinatario = doc.SelectNodes("//nfe:dest", ns);

                                foreach (XmlNode dadoDest in dadosDestinatario)
                                {
                                    foreach (XmlNode childNode in dadoDest.ChildNodes)
                                    {
                                        if (childNode.Name == "xNome")
                                        {
                                            nomeDestinatario = childNode.InnerText;
                                        }

                                        if (childNode.Name == "enderDest")
                                        {
                                            foreach (XmlNode childNodeDest in childNode.ChildNodes)
                                            {
                                                if (childNodeDest.Name == "xLgr")
                                                {
                                                    logradouroDestinatario = childNodeDest.InnerText;
                                                }

                                                if (childNodeDest.Name == "nro")
                                                {
                                                    logradouroNumeroDestinatario = childNodeDest.InnerText;
                                                }

                                                if (childNodeDest.Name == "cPais")
                                                {
                                                    var paisSped = paisesDanfe
                                                        .Where(c => c.CodigoSPED == childNodeDest.InnerText)
                                                        .FirstOrDefault();

                                                    if (paisSped != null)
                                                    {
                                                        if (!string.IsNullOrEmpty(paisSped.Sigla1))
                                                        {
                                                            paisDestinatario = paisSped.Sigla1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                var enderecoDestinatario = $"{logradouroDestinatario}, {logradouroNumeroDestinatario}";

                                var informacoesChaveNfe = doc.SelectSingleNode("//nfe:infProt", ns);
                                var chaveNfe = informacoesChaveNfe.ChildNodes[2].InnerText;

                                var item = doc.SelectSingleNode("//nfe:det", ns)?.FirstChild;

                                var listaProdutos = new List<ProdutoNfe>();

                                int contaItem = 1;
                                if (item != null)
                                {
                                    var produtos = doc.SelectNodes("//nfe:prod", ns);

                                    foreach (XmlNode produto in produtos)
                                    {
                                        var descricaoProduto = "";
                                        var unidadeComercializada = "";
                                        var quantidadeEstatistica = "";
                                        var quantidadeComercializada = "";
                                        var valorProduto = "";
                                        var ncm = "";

                                        foreach (XmlNode childNode in produto.ChildNodes)
                                        {
                                            if (childNode.Name == "xProd")
                                            {
                                                descricaoProduto = childNode.InnerText;
                                            }

                                            if (childNode.Name == "uTrib")
                                            {
                                                unidadeComercializada = childNode.InnerText;
                                            }

                                            if (childNode.Name == "qCom")
                                            {
                                                quantidadeComercializada = childNode.InnerText.Replace(".", ",");
                                            }

                                            if (childNode.Name == "qTrib")
                                            {
                                                quantidadeEstatistica = childNode.InnerText.Replace(".", ",");
                                            }

                                            if (childNode.Name == "vProd")
                                            {
                                                valorProduto = childNode.InnerText.Replace(".", ",");
                                            }

                                            if (childNode.Name == "NCM")
                                            {
                                                ncm = childNode.InnerText;
                                            }
                                        }

                                        var produtoNfe = new ProdutoNfe(
                                           contaItem,
                                           descricaoProduto,
                                           unidadeComercializada,
                                           quantidadeComercializada.ToDecimal(),
                                           quantidadeEstatistica.ToDecimal(),
                                           valorProduto.ToDecimal(),
                                           chaveNfe,
                                           ncm,
                                           cnpjEmitente,
                                           razaoSocialEmitente,
                                           numeroNf.ToInt());

                                        listaProdutos.Add(produtoNfe);
                                        contaItem++;
                                    }
                                    
                                    contaItem = 1;

                                    ExcluirItensSemVinculoDeNotaFiscal(dueBusca.Id);

                                    if (listaProdutos.Any())
                                    {
                                        foreach (var produtoNFE in listaProdutos)
                                        {
                                            var notaFiscal = new NotaFiscal();

                                            notaFiscal.TipoNF = "EXP";
                                            notaFiscal.Item = produtoNFE.Item;
                                            notaFiscal.ChaveNF = chaveNfe;
                                            notaFiscal.NumeroNF = numeroNf;
                                            notaFiscal.CnpjNF = cnpjEmitente;
                                            notaFiscal.QuantidadeNF = produtoNFE.QuantidadeComercializada;
                                            notaFiscal.UnidadeNF = produtoNFE.UnidadeComercializada;
                                            notaFiscal.NCM = produtoNFE.NCM;
                                            notaFiscal.Arquivo = this.txtUpload.FileName;
                                            notaFiscal.Usuario = Convert.ToInt32(Session["UsuarioId"].ToString());
                                            notaFiscal.DueId = this.txtDueID.Value.ToInt();                                            
                                            notaFiscal.VMLE = this.txtVMLE.Text.ToDecimal();
                                            notaFiscal.VMCV = this.txtVMCV.Text.ToDecimal();
                                            notaFiscal.Enquadramento = this.cbEnquadramento.SelectedValue.ToInt();

                                            var existeNf = _notaFiscalDAO.ExisteNotaFiscalNaDUEFromXML(notaFiscal);

                                            if (existeNf > 0)
                                            {
                                                _notaFiscalDAO.ExcluirNotaFiscaiDuePorId(existeNf);
                                            }

                                            _notaFiscalDAO.Cadastrar(notaFiscal);

                                            string enquadramento = string.Empty;
                                            decimal vmle = notaFiscal.VMLE;
                                            decimal vmcv = notaFiscal.VMCV;

                                            if (this.cbEnquadramento.SelectedValue.ToInt() > 0)
                                                enquadramento = this.cbEnquadramento.SelectedValue;
                                            else
                                                enquadramento = dueBusca.Enquadramento1_Default.ToString();

                                            //Para cada item da nota fiscal, criar um item na DUE com a mesma chave

                                            var dueId = _documentoUnicoExportacaoDAO.CriarItensDUEFromXML(
                                                dueBusca.Id,
                                                Convert.ToInt32(Session["UsuarioId"].ToString()),
                                                cnpjEmitente,
                                                razaoSocialEmitente,
                                                enderecoEmitente,
                                                estadoEmitente,
                                                nomeDestinatario,
                                                enderecoDestinatario,
                                                paisDestinatario,
                                                produtoNFE.ChaveNFE,
                                                produtoNFE.Item,
                                                produtoNFE.NCM,
                                                produtoNFE.DescricaoProduto,
                                                produtoNFE.UnidadeComercializada,
                                                produtoNFE.QuantidadeEstatistica,
                                                produtoNFE.QuantidadeComercializada,
                                                vmle,
                                                vmcv,
                                                enquadramento,
                                                dueBusca);

                                            _documentoUnicoExportacaoDAO.MarcarComoAutomatica(this.txtDueID.Value.ToInt());
                                        }
                                    }
                                }

                                xmlsImportados++;
                            }

                            ViewState["Sucesso"] = true;
                            ViewState["TotalNotasFiscais"] = this.txtUpload.PostedFiles.Count;
                            ViewState["QuantidadeImportada"] = xmlsImportados;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Ocorreu um problema ao processar o XML - Detalhes:" + ex.ToString());
                    }
                }
                else
                {
                    int quantidadeImportada = 0;

                    var notasFiscais = ProcessarArquivo(this.txtUpload.PostedFile.InputStream, ";");

                    var dueBusca = _documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                    if (dueBusca != null)
                    {
                        ExcluirItensSemVinculoDeNotaFiscal(dueBusca.Id);

                        var notasValidas = new List<NotaFiscal>();

                        foreach (var nf in notasFiscais)
                        {
                            nf.Arquivo = this.txtUpload.FileName;
                            nf.Usuario = Convert.ToInt32(Session["UsuarioId"].ToString());
                            nf.DueId = this.txtDueID.Value.ToInt();
                            nf.VMCV = this.txtVMCV.Text.ToDecimal();
                            nf.VMLE = this.txtVMLE.Text.ToDecimal();
                            nf.Enquadramento = this.cbEnquadramento.SelectedValue.ToInt();

                            if (nf.TipoNF == "EXP")
                            {

                            }
                            var existeNf = _notaFiscalDAO.ExisteNotaFiscalNaDUE(nf);

                            if (existeNf > 0)
                            {
                                _notaFiscalDAO.ExcluirNotaFiscaiDuePorId(existeNf);
                            }

                            _notaFiscalDAO.Cadastrar(nf);
                            notasValidas.Add(nf);
                            quantidadeImportada++;
                        }

                        var atributosCafe = dueBusca.Attr_Padrao_Qualidade_Default + dueBusca.Attr_Tipo_Default + dueBusca.Attr_Embarque_Em_Default +
                            dueBusca.Attr_Caracteristica_Especial_Default + dueBusca.Attr_Outra_Caracteristica_Especial_Default + dueBusca.Attr_Metodo_Processamento_Default;

                        foreach (var notaNcm in notasFiscais)
                        {
                            if (notaNcm.NCM != "09011110" && notaNcm.NCM != "09011190" && notaNcm.NCM != "09011200" && notaNcm.NCM != "09012100" && notaNcm.NCM != "09012200" && notaNcm.NCM != "21011110" && notaNcm.NCM != "21011190" && notaNcm.NCM != "21011200")
                            {
                                if (!string.IsNullOrEmpty(atributosCafe))
                                {
                                    ModelState.AddModelError(string.Empty, "Você informou atributos de café nas informações default, mas o NCM da nota que está sendo cadastrada é diferente de NCMs para exportação de café.");
                                    return;
                                }
                            }
                        }


                        CriarItensDUEAutomaticamente(notasValidas);

                        _documentoUnicoExportacaoDAO.MarcarComoAutomatica(this.txtDueID.Value.ToInt());

                        ViewState["Sucesso"] = true;
                        ViewState["TotalNotasFiscais"] = notasFiscais.Count;
                        ViewState["QuantidadeImportada"] = quantidadeImportada;
                    }
                }
            }
            catch (Exception ex)
            {
                DeletarArquivo(this.txtUpload);
                LogsService.Logar("CadastrarDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        private void ExcluirItensSemVinculoDeNotaFiscal(int dueId)
        {
            var itensDueSemVinculoNF = _documentoUnicoExportacaoDAO
                                   .ObterItensDUE(dueId)
                                   .Where(c => string.IsNullOrEmpty(c.NF))
                                   .ToList();

            foreach (var item in itensDueSemVinculoNF)
            {
                _documentoUnicoExportacaoDAO.ExcluirItemDUE(item.Id);
            }
        }

        private void CriarItensDUEAutomaticamente(List<NotaFiscal> notasFiscais)
        {
            var dueBusca = _documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

            if (dueBusca != null)
            {
                var notasExportacao = notasFiscais.Where(c => c.TipoNF == "EXP").ToList();

                string vmcv = string.Empty;
                string vmle = string.Empty;
                string enquadramento = string.Empty;

                if (!string.IsNullOrEmpty(this.txtVMCV.Text))
                    vmcv = this.txtVMCV.Text;
                else
                    vmcv = dueBusca.ValorUnitVMCV_Default.ToString();

                if (!string.IsNullOrEmpty(this.txtVMLE.Text))
                    vmle = this.txtVMLE.Text;
                else
                    vmle = dueBusca.ValorUnitVMLE_Default.ToString();

                if (this.cbEnquadramento.SelectedValue.ToInt() > 0)
                    enquadramento = this.cbEnquadramento.SelectedValue;
                else
                    enquadramento = dueBusca.Enquadramento1_Default.ToString();

                var dueId = _documentoUnicoExportacaoDAO.CriarItensDeDUEJaExistente(
                     notasExportacao,
                     dueBusca.Id,
                     Convert.ToInt32(Session["UsuarioId"].ToString()),
                     vmle,
                     vmcv,
                     enquadramento,
                     dueBusca.PaisDestino_Default,
                     dueBusca.Enquadramento1_Default.ToString(),
                     dueBusca.Enquadramento2_Default.ToString(),
                     dueBusca.Enquadramento3_Default.ToString(),
                     dueBusca.Enquadramento4_Default.ToString(),
                     dueBusca.CondicaoVenda_Default,
                     dueBusca.LPCO_Default,
                     dueBusca.Prioridade_Default.ToString(),
                     dueBusca.DescricaoComplementar_Default,
                     dueBusca.ComissaoAgente_Default.ToString(),
                     dueBusca.AC_Tipo_Default,
                     dueBusca.AC_Exp_Benefic_Default,
                     dueBusca.AC_Numero_Default,
                     dueBusca.AC_CNPJ_Default,
                     dueBusca.AC_Item_Default,
                     dueBusca.AC_NCM_Default,
                     dueBusca.AC_Qtde_Default.ToString(),
                     dueBusca.AC_VMLE_Sem_Cob_Default.ToString(),
                     dueBusca.AC_VMLE_Com_Cob_Default.ToString(),
                     dueBusca.Attr_Padrao_Qualidade_Default,
                     dueBusca.Attr_Embarque_Em_Default,
                     dueBusca.Attr_Tipo_Default,
                     dueBusca.Attr_Metodo_Processamento_Default,
                     dueBusca.Attr_Caracteristica_Especial_Default,
                     dueBusca.Attr_Outra_Caracteristica_Especial_Default,
                     dueBusca.Attr_Embalagem_Final_Default,
                     dueBusca.Ncm_Default);


                foreach (var nf in notasFiscais)
                {
                    _notaFiscalDAO.AtualizarIdDUE(nf.Id, dueId);
                }
            }
        }

        private bool UploadArquivo(FileUpload arquivo)
        {
            if (Request.Files.Count == 1)
            {
                string nomeArquivo = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

                try
                {
                    arquivo.SaveAs(nomeArquivo);
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Falha ao gravar o arquivo {nomeArquivo} no diretório Uploads. " + ex.ToString());
                }
            }
            else
            {
                var nomeArquivoXml = string.Empty;

                try
                {
                    if (arquivo.HasFiles)
                    {
                        foreach (HttpPostedFile uploadedFile in arquivo.PostedFiles)
                        {
                            nomeArquivoXml = Path.Combine(Server.MapPath("Uploads"), uploadedFile.FileName);

                            uploadedFile.SaveAs(nomeArquivoXml);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Falha ao gravar o arquivo {nomeArquivoXml} no diretório Uploads. " + ex.ToString());
                }
            }
        }

        private bool DeletarArquivo(FileUpload arquivo)
        {
            try
            {
                string caminho = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

                if (System.IO.File.Exists(caminho))
                    System.IO.File.Delete(caminho);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void ExcluirNotasReferenciadas(List<NotaFiscal> notasFiscais)
        {
            foreach (var nf in notasFiscais)
            {
                nf.Usuario = Convert.ToInt32(Session["UsuarioId"].ToString());

                if (nf.TipoNF == "EXP")
                {
                    _notaFiscalDAO.ExcluirNotasReferenciadas(nf);
                }
            }
        }

        private List<NotaFiscal> ProcessarArquivo(Stream arquivo, string delimitador)
        {
            List<NotaFiscal> notasFiscais = new List<NotaFiscal>();

            using (StreamReader reader = new StreamReader(arquivo))
            {
                string linha = string.Empty;
                int numeroLinha = 1;
                string[] registro;

                try
                {
                    while ((linha = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(linha))
                        {
                            registro = linha.Split(delimitador.ToCharArray());

                            if (registro.Length >= 7)
                            {
                                var notaFiscal = new NotaFiscal
                                {
                                    TipoNF = registro[0].ToString(),
                                    ChaveNF = registro[1].ToString(),
                                    NumeroNF = registro[2].ToString(),
                                    CnpjNF = registro[3].ToString(),
                                    QuantidadeNF = registro[4].ToString().ToDecimal(),
                                    UnidadeNF = registro[5].ToString(),
                                    NCM = registro[6].ToString()
                                };

                                if (notaFiscal.TipoNF != "EXP")
                                {
                                    if (registro.Length > 7)
                                    {
                                        notaFiscal.ChaveNFReferencia = registro[7].ToString();

                                        if (notaFiscal.ChaveNFReferencia.Length > 44)
                                            notaFiscal.ChaveNFReferencia = notaFiscal.ChaveNFReferencia.Substring(0, 44);
                                    }

                                    if (registro.Length > 8)
                                    {
                                        notaFiscal.Item = registro[8].ToInt();
                                    }

                                    notaFiscal.Item = notaFiscal.Item == 0
                                        ? 1
                                        : notaFiscal.Item;
                                }
                                else
                                {
                                    if (registro.Length > 8)
                                    {
                                        if (registro[8].ToString().Length == 8)
                                        {
                                            string data = registro[8].ToString();

                                            int ano, mes, dia = 0;

                                            ano = data.Substring(0, 4).ToInt();
                                            mes = data.Substring(4, 2).ToInt();
                                            dia = data.Substring(6, 2).ToInt();

                                            notaFiscal.DataNF = new DateTime(ano, mes, dia);
                                        }
                                    }

                                    if (registro.Length > 9)
                                    {
                                        string empresa = registro[9].Substring(0, 4);
                                        string filial = registro[9].Substring(4, 4);
                                        string memorando = registro[9].Substring(8, 12);

                                        notaFiscal.Empresa = empresa;
                                        notaFiscal.Filial = filial;
                                        notaFiscal.Memorando = memorando;
                                    }

                                    notaFiscal.Item = 1;
                                }

                                var validacoes = notaFiscal.Validar();

                                if (validacoes.IsValid)
                                {
                                    notasFiscais.Add(notaFiscal);
                                }
                                else
                                {
                                    foreach (var erro in validacoes.Errors)
                                        ModelState.AddModelError(string.Empty, string.Format("Linha {0}: {1} - Mensagem: {2}", numeroLinha, erro.PropertyName, erro.ErrorMessage));
                                }

                                numeroLinha += 1;
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return notasFiscais;
            }
        }
    }
}
