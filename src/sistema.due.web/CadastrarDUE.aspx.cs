using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class CadastrarDUE : System.Web.UI.Page
    {
        private readonly UnidadesReceitaDAO unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly EnquadramentosDAO enquadramentosDAO = new EnquadramentosDAO();
        private readonly PaisesDAO paisesDAO = new PaisesDAO();
        private readonly RecintosDAO recintosDAO = new RecintosDAO();
        private readonly MoedaDAO moedaDAO = new MoedaDAO();
        private readonly DocumentoUnicoExportacaoDAO documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly AtributosNCMDAO atributosDAO = new AtributosNCMDAO();
        private readonly ParametrosDAO _parametrosDAO = new ParametrosDAO();
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly CertificadoDAO _certificadoDAO = new CertificadoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListarLocaisDespacho();
                ListarLocaisEmbarque();
                ListarMoedas();
                ListarEnquadramentos();
                ListarPaises();

                this.txtDueCompleta.Value = Request.QueryString["completa"] ?? "0";

                if (Request.QueryString["id"] != null)
                {
                    this.txtDueID.Value = Request.QueryString["id"].ToString();

                    try
                    {
                        var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                        if (due != null)
                        {
                            this.txtDocumentoDeclarante.Text = due.DocumentoDeclarante.RemoverCaracteresEspeciais();
                            this.txtRUC.Text = due.RUC;
                            this.cbMoedaNegociacao.SelectedValue = due.MoedaNegociacao;
                            this.cbSituacaoEspecial.SelectedValue = due.SituacaoEspecial.ToString();
                            this.cbFormaExportacao.SelectedValue = due.FormaExportacao.ToString();
                            this.cbViaEspecialTransporte.SelectedValue = due.ViaEspecialTransporte.ToString();
                            this.txtInformacoesComplementares.Text = due.InformacoesComplementares;
                            this.txtValorUnitVMLE_Default.Text = due.ValorUnitVMLE_Default.ToString();
                            this.txtValorUnitVMCV_Default.Text = due.ValorUnitVMCV_Default.ToString();
                            this.txtLPCO_Default.Text = due.LPCO_Default;
                            this.cbPrioridadeCarga_Default.SelectedValue = due.Prioridade_Default.ToString();
                            this.txtDescrComplementar_Default.Text = due.DescricaoComplementar_Default;
                            this.txtComissaoAgenteDefault.Text = due.ComissaoAgente_Default.ToString();
                            this.txtNumeroAC_Default.Text = due.AC_Numero_Default;
                            this.txtCNPJBeneficiarioAC_Default.Text = due.AC_CNPJ_Default;
                            this.txtNumeroItemAC_Default.Text = due.AC_Item_Default;
                            this.txtNCMItemAC_Default.Text = due.AC_NCM_Default;
                            this.txtQuantidadeUtilizadaAC_Default.Text = due.AC_Qtde_Default.ToString();
                            this.txtVMLEComCoberturaCambialAC_Default.Text = due.AC_VMLE_Com_Cob_Default.ToString();
                            this.txtVMLESemCoberturaCambialAC_Default.Text = due.AC_VMLE_Sem_Cob_Default.ToString();

                            this.txtNCM_Default.Text = due.Ncm_Default.ToString();

                            var parametros = _parametrosDAO.ObterParametros();

                            if (!string.IsNullOrEmpty(this.txtNCM_Default.Text))
                            {
                                if (parametros.ValidarAtributosCafe > 0)
                                {
                                    var ncm = this.txtNCM_Default.Text.Replace(".", "");

                                    CarregarAtributos(ncm);
                                    ValidarCamposNCM(ncm);

                                    if (!string.IsNullOrEmpty(due.Attr_Padrao_Qualidade_Default.ToString()))
                                        this.cbAttrPadraoQualidade.SelectedValue = due.Attr_Padrao_Qualidade_Default.ToString();

                                    if (!string.IsNullOrEmpty(due.Attr_Embarque_Em_Default.ToString()))
                                        this.cbAttrEmbarcadoEm.SelectedValue = due.Attr_Embarque_Em_Default.ToString();

                                    if (!string.IsNullOrEmpty(due.Attr_Tipo_Default.ToString()))
                                        this.cbAttrTipo.SelectedValue = due.Attr_Tipo_Default.ToString();

                                    if (!string.IsNullOrEmpty(due.Attr_Metodo_Processamento_Default.ToString()))
                                        this.cbAttrMetodoProcessamento.SelectedValue = due.Attr_Metodo_Processamento_Default.ToString();

                                    if (!string.IsNullOrEmpty(due.Attr_Caracteristica_Especial_Default.ToString()))
                                        this.cbAttrCaracteristicaEspecial.SelectedValue = due.Attr_Caracteristica_Especial_Default.ToString();

                                    if (!string.IsNullOrEmpty(due.Attr_Outra_Caracteristica_Especial_Default.ToString()))
                                        this.txtAttrOutraCaracteristicaEspecial.Text = due.Attr_Outra_Caracteristica_Especial_Default.ToString();

                                    this.chkAttrEmbalagemFinal.Checked = Convert.ToBoolean(due.Attr_Embalagem_Final_Default.ToString().ToInt());
                                }
                            }

                            if (!string.IsNullOrEmpty(due.CondicaoVenda_Default))
                            {
                                this.cbCondicaoVenda_Default.SelectedValue = due.CondicaoVenda_Default;
                            }

                            if (!string.IsNullOrEmpty(due.PaisDestino_Default))
                            {
                                this.cbPaisDestino_Default.SelectedValue = due.PaisDestino_Default;
                            }

                            if (due.Enquadramento1_Default > 0)
                            {
                                this.cbEnquadramento1_Default.SelectedValue = due.Enquadramento1_Default.ToString();
                            }

                            if (due.Enquadramento2_Default > 0)
                            {
                                this.cbEnquadramento2_Default.SelectedValue = due.Enquadramento2_Default.ToString();
                            }

                            if (due.Enquadramento3_Default > 0)
                            {
                                this.cbEnquadramento3_Default.SelectedValue = due.Enquadramento3_Default.ToString();
                            }

                            if (due.Enquadramento4_Default > 0)
                            {
                                this.cbEnquadramento4_Default.SelectedValue = due.Enquadramento4_Default.ToString();
                            }

                            if (due.UnidadeDespacho != null)
                            {
                                this.cbUnidadeDespachoRFB.SelectedValue = due.UnidadeDespacho.Id.ToInt().ToString();
                                this.txtDocumentoResponsavelLocalDespacho.Text = due.UnidadeDespacho.DocumentoResponsavel;
                                this.txtEnderecoLocalDespacho.Text = due.UnidadeDespacho.Endereco;
                                this.txtLatitudeLocalDespacho.Text = due.UnidadeDespacho.Latitude;
                                this.txtLongitudeLocalDespacho.Text = due.UnidadeDespacho.Longitude;
                                this.cbTipoRecintoAduaneiroDespacho.SelectedValue = due.UnidadeDespacho.TipoRecinto.ToString();

                                if (due.UnidadeDespacho.TipoRecinto.ToString() != "19" && due.UnidadeDespacho.TipoRecinto.ToString() != "22")
                                {
                                    if (this.cbUnidadeDespachoRFB.SelectedValue != null)
                                    {
                                        this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeDespachoRFB.SelectedValue.ToInt());
                                        this.cbRecintoAduaneiroDespacho.DataBind();

                                        this.cbRecintoAduaneiroDespacho.SelectedValue = due.UnidadeDespacho.RecintoAduaneiroId.ToString();
                                    }
                                }
                                else
                                {
                                    this.cbRecintoAduaneiroDespacho.SelectedIndex = -1;
                                    this.cbRecintoAduaneiroDespacho.ClearSelection();
                                }

                                this.pnlNaoRecintoAduaneiroDespacho.Visible = this.cbTipoRecintoAduaneiroDespacho.SelectedValue.ToInt() != 281;
                            }

                            if (due.UnidadeEmbarque != null)
                            {
                                this.cbUnidadeEmbarqueRFB.SelectedValue = due.UnidadeEmbarque.Id.ToInt().ToString();
                                this.txtReferenciaEndereco.Text = due.UnidadeEmbarque.EnderecoReferencia;
                                this.cbTipoRecintoAduaneiroEmbarque.SelectedValue = due.UnidadeEmbarque.TipoRecinto.ToString();

                                if (this.cbUnidadeEmbarqueRFB.SelectedValue != null)
                                {
                                    if (string.IsNullOrEmpty(due.UnidadeEmbarque.RecintoAduaneiroId.ToString()))
                                    {
                                        this.cbRecintoAduaneiroEmbarque.SelectedIndex = -1;
                                        this.cbRecintoAduaneiroEmbarque.ClearSelection();
                                    }
                                    else
                                    {
                                        this.cbRecintoAduaneiroEmbarque.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeEmbarqueRFB.SelectedValue.ToInt());
                                        this.cbRecintoAduaneiroEmbarque.DataBind();

                                        this.cbRecintoAduaneiroEmbarque.SelectedValue = due.UnidadeEmbarque.RecintoAduaneiroId.ToString();
                                    }
                                }

                                //this.pnlNaoRecintoAduaneiroEmbarque.Visible = this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt() != 281;

                                if (this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt() != 281)
                                {
                                    this.pnlNaoRecintoAduaneiroEmbarque.Visible = true;

                                    this.cbRecintoAduaneiroEmbarque.SelectedIndex = -1;
                                    this.cbRecintoAduaneiroEmbarque.ClearSelection();
                                    this.cbRecintoAduaneiroEmbarque.Items.Clear();

                                    this.spnRecintoAduaneiroEmbObrigatorio.Visible = false;
                                }
                                else
                                {
                                    this.pnlNaoRecintoAduaneiroEmbarque.Visible = false;
                                    this.spnRecintoAduaneiroEmbObrigatorio.Visible = true;
                                }

                            }

                            this.btnCadastrarDUE.Text = "Atualizar DUE e Continuar";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogsService.Logar("CadastrarDUE.aspx", ex.ToString());
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
            }
        }

        protected void ListarEnquadramentos()
        {
            var enquadramentos = enquadramentosDAO.ObterEnquadramentos();

            foreach (var enquadramento in enquadramentos)
            {
                this.cbEnquadramento1_Default.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento2_Default.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento3_Default.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento4_Default.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
            }

            this.cbEnquadramento1_Default.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento2_Default.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento3_Default.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento4_Default.Items.Insert(0, new ListItem("-- Selecione --", "0"));
        }

        protected void ListarPaises()
        {
            var paises = paisesDAO.ObterPaises();

            this.cbPaisDestino_Default.Items.Add(new ListItem("", ""));

            foreach (var pais in paises)
            {
                this.cbPaisDestino_Default.Items.Add(new ListItem(pais.Descricao, pais.Sigla));
            }

            this.cbPaisDestino_Default.SelectedIndex = 0;
        }

        protected void ListarLocaisDespacho()
        {
            this.cbUnidadeDespachoRFB.DataSource = unidadesReceitaDAO.ObterUnidadesRFB();
            this.cbUnidadeDespachoRFB.DataBind();

            this.cbUnidadeDespachoRFB.Items.Insert(0, new ListItem("-- Selecione --", "0"));
        }

        protected void ListarLocaisEmbarque()
        {
            this.cbUnidadeEmbarqueRFB.DataSource = unidadesReceitaDAO.ObterUnidadesRFB();
            this.cbUnidadeEmbarqueRFB.DataBind();

            this.cbUnidadeEmbarqueRFB.Items.Insert(0, new ListItem("-- Selecione --", "0"));
        }

        protected void ListarMoedas()
        {
            this.cbMoedaNegociacao.DataSource = moedaDAO.ObterMoedas();
            this.cbMoedaNegociacao.DataBind();

            this.cbMoedaNegociacao.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbMoedaNegociacao.SelectedValue = "USD";
        }

        protected void btnImportarSiscomex_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtDUEImportacao.Text))
            {
                ModelState.AddModelError(string.Empty, "Informe o Número da DUE");
                return;
            }

            var dueDbBusca = documentoUnicoExportacaoDAO
                .ObterDUEPorNumero(this.txtDUEImportacao.Text.RemoverCaracteresEspeciaisDUE());

            if (dueDbBusca != null)
            {
                ModelState.AddModelError(string.Empty, $"A DUE {this.txtDUEImportacao.Text} já existe no Portal Microled");
                return;
            }

            //var dadosDue = SisComexService.ObterDUECompleta(this.txtDUEImportacao.Text.RemoverCaracteresEspeciais().Trim(), ConfigurationManager.AppSettings["CpfCertificado"].ToString());

            var cpfsCertificado = _certificadoDAO.ObterCpfs();
            var dadosDue = new Responses.DueDadosCompletos();
            foreach (var cpf in cpfsCertificado)
            {
                dadosDue = SisComexService.ObterDUECompleta(this.txtDUEImportacao.Text.RemoverCaracteresEspeciais().Trim(), cpf);

                if (dadosDue != null)
                {
                    if (dadosDue.Sucesso)
                    {
                        break;
                    }
                }

            }

            if (dadosDue != null)
            {
                if (dadosDue.Sucesso == false)
                {
                    ModelState.AddModelError(string.Empty, "Siscomex: " + dadosDue.Mensagem);
                    return;
                }

                var moedaBusca = moedaDAO.ObterMoedas()
                    .Where(c => c.Id == dadosDue.moeda.codigo)
                    .Select(c => c.CodigoISO)
                    .FirstOrDefault();

                var situacaoEspecialDue = dadosDue.situacaoEspecial ?? string.Empty;
                var situacaoEspecialCod = situacaoEspecialDue.ToUpper().Contains("EMBARQUE_ANTECIPADO") ? 2002 : 0;

                var formaExportacaoDue = dadosDue.formaDeExportacao ?? string.Empty;
                var formaExportacaoCod = formaExportacaoDue.ToUpper().Contains("POR_CONTA_PROPRIA") ? 1001 : 0;

                var due = new DUEMaster(
                    dadosDue.declarante?.numeroDoDocumento.RemoverCaracteresEspeciais(),
                    moedaBusca,
                    dadosDue.ruc,
                    situacaoEspecialCod,
                    formaExportacaoCod,
                    0,
                    dadosDue.informacoesComplementares,
                    Convert.ToInt32(Session["UsuarioId"].ToString()));

                due.DUE = this.txtDUEImportacao.Text.Replace("-", "");
                due.CriadoPorNF = 1;
                due.Completa = 1;
                due.EnviadoSiscomex = 0;
                due.ChaveAcesso = dadosDue.chaveDeAcesso;
                due.ImportadoSiscomex = 1;

                var unidadeDespacho = new UnidadeDespacho
                {
                    Id = dadosDue.unidadeLocalDeDespacho.codigo,
                    DocumentoResponsavel = dadosDue.declarante?.numeroDoDocumento.RemoverCaracteresEspeciais(),
                    Endereco = dadosDue.enderecoDoEstabelecimentoDoLocalDeDespacho,
                    Latitude = dadosDue.latitudeDoLocalDeDespacho,
                    Longitude = dadosDue.longitudeDoLocalDeDespacho,
                    RecintoAduaneiroId = dadosDue.recintoAduaneiroDeDespacho.codigo
                };

                if (dadosDue.despachoEmRecintoDomiciliar)
                    unidadeDespacho.TipoRecinto = 19;
                else
                    unidadeDespacho.TipoRecinto = 281;

                var unidadeEmbarque = new UnidadeEmbarque
                {
                    Id = dadosDue.unidadeLocalDeEmbarque.codigo,
                    EnderecoReferencia = string.Empty,
                    RecintoAduaneiroId = dadosDue.recintoAduaneiroDeEmbarque.codigo,
                };

                if (dadosDue.despachoEmRecintoDomiciliar)
                    unidadeEmbarque.TipoRecinto = 19;
                else
                    unidadeEmbarque.TipoRecinto = 281;

                due.AdicionarUnidadeDespacho(unidadeDespacho);
                due.AdicionarUnidadeEmbarque(unidadeEmbarque);

                DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

                var id = dueDAO.RegistrarDUE(due);

                dueDAO.MarcarComoAutomatica(id);

                foreach (var item in dadosDue.itens)
                {
                    var notaExportacao = item.itemDaNotaFiscalDeExportacao?.notaFiscal;

                    if (notaExportacao != null)
                    {
                        var nfExportacao = new NotaFiscal
                        {
                            DueId = id,
                            Arquivo = "SISCOMEX",
                            Usuario = Convert.ToInt32(Session["UsuarioId"].ToString()),
                            TipoNF = "EXP",
                            ChaveNF = notaExportacao?.chaveDeAcesso ?? string.Empty,
                            NumeroNF = notaExportacao?.numeroDoDocumento.ToString() ?? string.Empty,
                            CnpjNF = notaExportacao?.identificacaoDoEmitente.numero.ToString() ?? string.Empty,
                            QuantidadeNF = item.quantidadeNaUnidadeEstatistica,
                            UnidadeNF = item.unidadeComercializada,
                            NCM = item.ncm.codigo,
                            Item = 1
                        };

                        _notaFiscalDAO.Cadastrar(nfExportacao);

                        foreach (var nfRem in item.itensDaNotaDeRemessa)
                        {
                            var tipoNf = string.Empty;
                            var cfop = nfRem.cfop.ToString();

                            if (cfop.EndsWith("04") || cfop.EndsWith("05"))
                            {
                                tipoNf = "FDL";
                            }

                            if (cfop.EndsWith("01") || cfop.EndsWith("02"))
                            {
                                tipoNf = "REM";
                            }

                            var nfRemessa = new NotaFiscal
                            {
                                DueId = id,
                                Arquivo = "SISCOMEX",
                                Usuario = Convert.ToInt32(Session["UsuarioId"].ToString()),
                                TipoNF = tipoNf,
                                ChaveNF = nfRem.notaFiscal.chaveDeAcesso,
                                NumeroNF = nfRem.notaFiscal.numeroDoDocumento.ToString(),
                                CnpjNF = nfRem.notaFiscal.identificacaoDoEmitente.cnpj.ToString(),
                                QuantidadeNF = (decimal)nfRem.quantidadeEstatistica,
                                UnidadeNF = nfRem.unidadeComercial,
                                NCM = nfRem.ncm.codigo,
                                ChaveNFReferencia = nfExportacao.ChaveNF,
                                Item = nfRem.numeroDoItem
                            };

                            _notaFiscalDAO.Cadastrar(nfRemessa);
                        }
                    }
                }

                foreach (var item in dadosDue.itens)
                {
                    var notaFiscal = item.itemDaNotaFiscalDeExportacao;

                    if (item != null)
                    {
                        var paises = PaisesDAO.ObterPaisesSiscomex();

                        string importador = item?.nomeImportador ?? string.Empty;
                        string enderecoImportador = item?.enderecoImportador ?? string.Empty;
                        string pesoLiquidoTotal = item?.pesoLiquidoTotal.ToString() ?? "0";
                        string incoterm = item?.codigoCondicaoVenda.codigo ?? string.Empty;

                        string unidadeEstatistica = item?.unidadeComercializada ?? string.Empty;
                        string informacoesComplementares = dadosDue?.informacoesComplementares ?? string.Empty;

                        int motivoDispensa = dadosDue.motivoDeDispensaDaNotaFiscal?.codigo ?? 0;

                        var itemDue = new DUEItem(
                            id,
                            motivoDispensa,
                            item.codigoCondicaoVenda.codigo);

                        var notaExportacao = item.itemDaNotaFiscalDeExportacao?.notaFiscal;

                        if (notaExportacao != null)
                        {
                            itemDue.NF = notaExportacao.chaveDeAcesso;
                        }

                        var paisExportador = paises.Where(c => c.Id == item.exportador.nacionalidade.codigo)
                            .Select(c => c.Sigla).FirstOrDefault() ?? string.Empty;

                        var exportadorDue = new Exportador(
                            "",
                            item.exportador.numeroDoDocumento.RemoverCaracteresEspeciais(),
                            "",
                            "",
                            paisExportador);

                        var paisImportador = "";

                        paisImportador = paises.Where(c => c.Id == dadosDue.paisImportador.codigo)
                            .Select(c => c.Sigla).FirstOrDefault() ?? string.Empty;

                        var importadorDue = new Importador(
                            item.nomeImportador,
                            item.enderecoImportador,
                            paisImportador);

                        itemDue.AdicionarExportador(exportadorDue);
                        itemDue.AdicionarImportador(importadorDue);

                        var dueItemId = documentoUnicoExportacaoDAO.RegistrarItemDUE(itemDue);

                        var enquadramentos = item.listaDeEnquadramentos;

                        var enquadramento1 = 0;
                        var enquadramento2 = 0;
                        var enquadramento3 = 0;
                        var enquadramento4 = 0;

                        if (enquadramentos.Count == 1)
                        {
                            enquadramento1 = enquadramentos[0].codigo;
                        }

                        if (enquadramentos.Count == 2)
                        {
                            enquadramento1 = enquadramentos[0].codigo;
                            enquadramento2 = enquadramentos[1].codigo;
                        }

                        if (enquadramentos.Count == 3)
                        {
                            enquadramento1 = enquadramentos[0].codigo;
                            enquadramento2 = enquadramentos[1].codigo;
                            enquadramento3 = enquadramentos[3].codigo;
                        }

                        if (enquadramentos.Count == 4)
                        {
                            enquadramento1 = enquadramentos[0].codigo;
                            enquadramento2 = enquadramentos[1].codigo;
                            enquadramento3 = enquadramentos[3].codigo;
                            enquadramento4 = enquadramentos[4].codigo;
                        }

                        var descricaoUnidade = "";

                        if (item.ncm != null)
                        {
                            if (item.ncm?.unidadeMedidaEstatistica == "TON" ||
                                item.ncm?.unidadeMedidaEstatistica == "TONEL.METR.LIQ.")
                            {
                                descricaoUnidade = "TON";
                            }
                        }

                        var dueDetalhe = new DUEItemDetalhes(
                            dueItemId,
                            item.numero,
                            item.valorDaMercadoriaNoLocalDeEmbarque,
                            paisImportador,
                            item.quantidadeNaUnidadeEstatistica,
                            0,
                            0,
                            string.Empty,
                            item.ncm.codigo,
                            item.valorDaMercadoriaNaCondicaoDeVenda,
                            item.descricaoDaMercadoria,
                            item.quantidadeNaUnidadeEstatistica,
                            descricaoUnidade,
                            0,
                            item.pesoLiquidoTotal,
                            enquadramento1,
                            enquadramento2,
                            enquadramento3,
                            enquadramento4);

                        dueDetalhe.Id = documentoUnicoExportacaoDAO.RegistrarDUEItemDetalhe(dueDetalhe);
                    }
                }

                Response.Redirect(string.Format("CadastrarDUE.aspx?id={0}", id), false);
            }

        }

        protected void btnImportarBancoDeDados_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtDUEImportacao.Text))
            {
                ModelState.AddModelError(string.Empty, "Informe o Número da DUE");
                return;
            }

            try
            {
                var due = documentoUnicoExportacaoDAO.ObterDUEPorNumero(this.txtDUEImportacao.Text);

                if (due == null)
                {
                    ModelState.AddModelError(string.Empty, string.Format("DUE {0} não encontrada", this.txtDUEImportacao.Text));
                    return;
                }

                if (due != null)
                {
                    this.txtDocumentoDeclarante.Text = due.DocumentoDeclarante.RemoverCaracteresEspeciais();
                    this.txtRUC.Text = due.RUC;
                    this.cbMoedaNegociacao.SelectedValue = due.MoedaNegociacao;
                    this.cbSituacaoEspecial.SelectedValue = due.SituacaoEspecial.ToString();
                    this.cbFormaExportacao.SelectedValue = due.FormaExportacao.ToString();
                    this.cbViaEspecialTransporte.SelectedValue = due.ViaEspecialTransporte.ToString();
                    this.txtInformacoesComplementares.Text = due.InformacoesComplementares;

                    if (due.UnidadeDespacho != null)
                    {
                        this.cbUnidadeDespachoRFB.SelectedValue = due.UnidadeDespacho.Id.ToString();
                        this.txtDocumentoResponsavelLocalDespacho.Text = due.UnidadeDespacho.DocumentoResponsavel;
                        this.txtEnderecoLocalDespacho.Text = due.UnidadeDespacho.Endereco;
                        this.txtLatitudeLocalDespacho.Text = due.UnidadeDespacho.Latitude.ToString();
                        this.txtLongitudeLocalDespacho.Text = due.UnidadeDespacho.Longitude.ToString();
                        this.cbTipoRecintoAduaneiroDespacho.SelectedValue = due.UnidadeDespacho.TipoRecinto.ToString();

                        if (this.cbUnidadeDespachoRFB.SelectedValue != null)
                        {
                            try
                            {
                                this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeDespachoRFB.SelectedValue.ToInt());
                                this.cbRecintoAduaneiroDespacho.DataBind();

                                this.cbRecintoAduaneiroDespacho.SelectedValue = due.UnidadeDespacho.RecintoAduaneiroId.ToString();
                            }
                            catch (Exception)
                            {
                            }
                        }

                        this.pnlNaoRecintoAduaneiroDespacho.Visible = this.cbTipoRecintoAduaneiroDespacho.SelectedValue.ToInt() != 281;
                    }

                    if (due.UnidadeEmbarque != null)
                    {
                        this.cbUnidadeEmbarqueRFB.SelectedValue = due.UnidadeEmbarque.Id.ToString();
                        this.txtReferenciaEndereco.Text = due.UnidadeEmbarque.EnderecoReferencia;
                        this.cbTipoRecintoAduaneiroEmbarque.SelectedValue = due.UnidadeEmbarque.TipoRecinto.ToString();

                        if (this.cbUnidadeEmbarqueRFB.SelectedValue != null)
                        {
                            try
                            {
                                this.cbRecintoAduaneiroEmbarque.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeEmbarqueRFB.SelectedValue.ToInt());
                                this.cbRecintoAduaneiroEmbarque.DataBind();

                                this.cbRecintoAduaneiroEmbarque.SelectedValue = due.UnidadeEmbarque.RecintoAduaneiroId.ToString();
                            }
                            catch (Exception)
                            {
                            }
                        }

                        //this.pnlNaoRecintoAduaneiroEmbarque.Visible = this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt() != 281;
                        if (this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt() != 281)
                        {
                            this.pnlNaoRecintoAduaneiroEmbarque.Visible = true;

                            this.cbRecintoAduaneiroEmbarque.SelectedIndex = -1;
                            this.cbRecintoAduaneiroEmbarque.ClearSelection();
                            this.cbRecintoAduaneiroEmbarque.Items.Clear();

                            this.spnRecintoAduaneiroEmbObrigatorio.Visible = false;
                        }
                        else
                        {
                            this.pnlNaoRecintoAduaneiroEmbarque.Visible = false;
                            this.spnRecintoAduaneiroEmbObrigatorio.Visible = true;
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "script", "javascript:irParaEtapaInicial()", true);
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastrarDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }

        }

        protected void btnCadastrarDUE_Click(object sender, EventArgs e)
        {
            try
            {
                //this.txtDocumentoDeclarante.Text.RemoverCaracteresEspeciais(),

                var due = new DUEMaster(
                    this.txtDocumentoDeclarante.Text.RemoverCaracteresEspeciaisCNPJ(),
                    this.cbMoedaNegociacao.SelectedValue,
                    this.txtRUC.Text,
                    this.cbSituacaoEspecial.SelectedValue.ToInt(),
                    this.cbFormaExportacao.SelectedValue.ToInt(),
                    this.cbViaEspecialTransporte.SelectedValue.ToInt(),
                    this.txtInformacoesComplementares.Text,
                    Convert.ToInt32(Session["UsuarioId"].ToString()),
                    this.txtValorUnitVMLE_Default.Text.ToDecimal(),
                    this.txtValorUnitVMCV_Default.Text.ToDecimal(),
                    this.cbPaisDestino_Default.SelectedValue,
                    this.cbEnquadramento1_Default.SelectedValue.ToInt(),
                    this.cbEnquadramento2_Default.SelectedValue.ToInt(),
                    this.cbEnquadramento3_Default.SelectedValue.ToInt(),
                    this.cbEnquadramento4_Default.SelectedValue.ToInt(),
                    this.txtLPCO_Default.Text,
                    this.cbCondicaoVenda_Default.SelectedValue,
                    this.cbPrioridadeCarga_Default.Text.ToInt(),
                    this.txtDescrComplementar_Default.Text,
                    this.txtComissaoAgenteDefault.Text.ToDecimal(),
                    this.cbTipoAC_Default.SelectedValue,
                    this.txtNumeroAC_Default.Text,
                    this.txtCNPJBeneficiarioAC_Default.Text,
                    this.txtNumeroItemAC_Default.Text,
                    this.txtNCMItemAC_Default.Text,
                    this.txtQuantidadeUtilizadaAC_Default.Text.ToDecimal(),
                    this.txtVMLESemCoberturaCambialAC_Default.Text.ToDecimal(),
                    this.txtVMLEComCoberturaCambialAC_Default.Text.ToDecimal(),
                    this.cbAttrPadraoQualidade.SelectedValue,
                    this.cbAttrEmbarcadoEm.SelectedValue,
                    this.cbAttrTipo.SelectedValue,
                    this.cbAttrMetodoProcessamento.SelectedValue,
                    this.cbAttrCaracteristicaEspecial.SelectedValue,
                    this.txtAttrOutraCaracteristicaEspecial.Text,
                    Convert.ToInt32(this.chkAttrEmbalagemFinal.Checked),
                    this.txtNCM_Default.Text);

                var unidadeDespacho = new UnidadeDespacho
                {
                    Id = this.cbUnidadeDespachoRFB.SelectedValue,
                    DocumentoResponsavel = this.txtDocumentoResponsavelLocalDespacho.Text,
                    Endereco = this.txtEnderecoLocalDespacho.Text,
                    Latitude = this.txtLatitudeLocalDespacho.Text,
                    Longitude = this.txtLongitudeLocalDespacho.Text,
                    RecintoAduaneiroId = this.cbRecintoAduaneiroDespacho.SelectedValue,
                    TipoRecinto = this.cbTipoRecintoAduaneiroDespacho.SelectedValue.ToInt()
                };

                var unidadeEmbarque = new UnidadeEmbarque
                {
                    Id = this.cbUnidadeEmbarqueRFB.SelectedValue,
                    EnderecoReferencia = this.txtReferenciaEndereco.Text,
                    RecintoAduaneiroId = this.cbRecintoAduaneiroEmbarque.SelectedValue,
                    TipoRecinto = this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt()
                };

                due.AdicionarUnidadeDespacho(unidadeDespacho);
                due.AdicionarUnidadeEmbarque(unidadeEmbarque);

                if (this.txtDueSiscomex.Value != string.Empty)
                {
                    due.DUE = this.txtDueSiscomex.Value;
                    due.EnviadoSiscomex = this.txtDueEnviadoSiscomex.Value.ToInt();
                }

                DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

                if (string.IsNullOrEmpty(this.txtDueID.Value))
                {
                    due.DUE = string.Empty;
                    due.EnviadoSiscomex = 0;
                    due.Completa = this.txtDueCompleta.Value.ToInt();

                    var id = dueDAO.RegistrarDUE(due);

                    if (!string.IsNullOrEmpty(this.txtDueCompleta.Value))
                    {
                        if (this.txtDueCompleta.Value.ToInt() == 1)
                        {
                            Response.Redirect(string.Format("CadastrarNotas.aspx?id={0}", id), false);
                            return;
                        }
                    }

                    Response.Redirect(string.Format("CadastrarItensDUE.aspx?id={0}", id), false);
                }
                else
                {
                    due.Id = this.txtDueID.Value.ToInt();
                    dueDAO.AtualizarDUE(due);

                    var itensDue = documentoUnicoExportacaoDAO.ObterItensDUE(due.Id);

                    foreach (var item in itensDue)
                    {
                        if (item.CondicaoVenda != due.CondicaoVenda_Default)
                            item.CondicaoVenda = due.CondicaoVenda_Default;

                        if (item.ValorUnitVMLE != due.ValorUnitVMLE_Default)
                            item.ValorUnitVMLE = due.ValorUnitVMLE_Default;

                        if (item.ValorUnitVMCV != due.ValorUnitVMCV_Default)
                            item.ValorUnitVMCV = due.ValorUnitVMCV_Default;

                        documentoUnicoExportacaoDAO.AtualizarItemDUE(item);

                        var detalhes = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                        foreach (var detalhe in detalhes)
                        {
                            var atosC = documentoUnicoExportacaoDAO.ObterAtosConcessorios(detalhe.Id);

                            if (detalhe.Enquadramento1Id != 81101 && detalhe.Enquadramento2Id != 81101 && detalhe.Enquadramento3Id != 81101 && detalhe.Enquadramento4Id != 81101)
                            {
                                if (due.Enquadramento1_Default != 81101 && due.Enquadramento2_Default != 81101 && due.Enquadramento3_Default != 81101 && due.Enquadramento4_Default != 81101)
                                {
                                    if (atosC.Any())
                                    {
                                        foreach (var ato in atosC)
                                        {
                                            documentoUnicoExportacaoDAO.ExcluirAtoConcessorio(ato.Id);
                                        }
                                    }
                                }
                            }

                            if (detalhe.Enquadramento1Id == 81101 || detalhe.Enquadramento2Id == 81101 || detalhe.Enquadramento3Id == 81101 || detalhe.Enquadramento4Id != 81101)
                            {
                                if (!string.IsNullOrEmpty(this.txtNumeroAC_Default.Text))
                                {
                                    documentoUnicoExportacaoDAO.ExcluirAtoConcessorioDoItem(detalhe.Id);

                                    var exportadorBeneficiarioAC = 0;

                                    if (this.rbAcBeneficiarioSim.Checked)
                                    {
                                        exportadorBeneficiarioAC = 1;
                                    }

                                    if (this.rbAcBeneficiarioNao.Checked)
                                    {
                                        exportadorBeneficiarioAC = 0;
                                    }

                                    documentoUnicoExportacaoDAO.RegistrarAtoConcessorio(
                                        new DUEItemDetalhesAC(
                                            detalhe.Id,
                                            this.txtNumeroAC_Default.Text,
                                            this.txtCNPJBeneficiarioAC_Default.Text,
                                            this.txtNumeroItemAC_Default.Text,
                                            this.txtNCMItemAC_Default.Text,
                                            this.txtQuantidadeUtilizadaAC_Default.Text.ToDecimal(),
                                            this.txtVMLEComCoberturaCambialAC_Default.Text.ToDecimal(),
                                            this.txtVMLESemCoberturaCambialAC_Default.Text.ToDecimal(),
                                            this.cbTipoAC_Default.SelectedValue,
                                            exportadorBeneficiarioAC));
                                }
                            }

                            if (!string.IsNullOrEmpty(due.DescricaoComplementar_Default))
                            {
                                detalhe.DescricaoComplementar = due.DescricaoComplementar_Default;
                            }

                            if (due.Enquadramento1_Default > 0)
                            {
                                if (detalhe.Enquadramento1Id != due.Enquadramento1_Default)
                                    detalhe.Enquadramento1Id = due.Enquadramento1_Default;
                            }

                            if (due.Enquadramento2_Default > 0)
                            {
                                if (detalhe.Enquadramento2Id != due.Enquadramento2_Default)
                                    detalhe.Enquadramento2Id = due.Enquadramento2_Default;
                            }

                            if (due.Enquadramento3_Default > 0)
                            {
                                if (detalhe.Enquadramento3Id != due.Enquadramento3_Default)
                                    detalhe.Enquadramento3Id = due.Enquadramento3_Default;
                            }

                            if (due.Enquadramento4_Default > 0)
                            {
                                if (detalhe.Enquadramento4Id != due.Enquadramento4_Default)
                                    detalhe.Enquadramento4Id = due.Enquadramento4_Default;
                            }

                            if (due.PaisDestino_Default != string.Empty)
                            {
                                if (detalhe.PaisDestino != due.PaisDestino_Default)
                                    detalhe.PaisDestino = due.PaisDestino_Default;
                            }

                            if (due.Prioridade_Default != 0)
                            {
                                if (detalhe.PrioridadeCarga != due.Prioridade_Default)
                                    detalhe.PrioridadeCarga = due.Prioridade_Default;
                            }

                            if (due.ComissaoAgente_Default > 0)
                            {
                                if (detalhe.ComissaoAgente != due.ComissaoAgente_Default)
                                    detalhe.ComissaoAgente = due.ComissaoAgente_Default;
                            }

                            if (due.DescricaoComplementar_Default != string.Empty)
                            {
                                if (detalhe.DescricaoComplementar != due.DescricaoComplementar_Default)
                                    detalhe.DescricaoComplementar = due.DescricaoComplementar_Default;
                            }

                            if (!string.IsNullOrEmpty(due.Attr_Padrao_Qualidade_Default))
                            {
                                if (detalhe.Attr_Padrao_Qualidade != due.Attr_Padrao_Qualidade_Default)
                                    detalhe.Attr_Padrao_Qualidade = due.Attr_Padrao_Qualidade_Default;
                            }

                            if (!string.IsNullOrEmpty(due.Attr_Tipo_Default))
                            {
                                if (detalhe.Attr_Tipo != due.Attr_Tipo_Default)
                                    detalhe.Attr_Tipo = due.Attr_Tipo_Default;
                            }

                            if (!string.IsNullOrEmpty(due.Attr_Metodo_Processamento_Default))
                            {
                                if (detalhe.Attr_Metodo_Processamento != due.Attr_Metodo_Processamento_Default)
                                    detalhe.Attr_Metodo_Processamento = due.Attr_Metodo_Processamento_Default;
                            }

                            if (!string.IsNullOrEmpty(due.Attr_Caracteristica_Especial_Default))
                            {
                                if (detalhe.Attr_Caracteristica_Especial != due.Attr_Caracteristica_Especial_Default)
                                    detalhe.Attr_Caracteristica_Especial = due.Attr_Caracteristica_Especial_Default;
                            }

                            if (!string.IsNullOrEmpty(due.Attr_Outra_Caracteristica_Especial_Default))
                            {
                                if (detalhe.Attr_Outra_Caracteristica_Especial != due.Attr_Outra_Caracteristica_Especial_Default)
                                    detalhe.Attr_Outra_Caracteristica_Especial = due.Attr_Outra_Caracteristica_Especial_Default;
                            }

                            if (due.Attr_Embalagem_Final_Default > 0)
                            {
                                if (detalhe.Attr_Embalagem_Final != due.Attr_Embalagem_Final_Default)
                                    detalhe.Attr_Embalagem_Final = due.Attr_Embalagem_Final_Default;
                            }

                            var resultadoVMLE = ((detalhe.PesoLiquidoTotal * (item.ValorUnitVMLE / 1000)));
                            var resultadoVMCV = ((detalhe.PesoLiquidoTotal * (item.ValorUnitVMCV / 1000)));

                            if (resultadoVMLE != detalhe.ValorMercadoriaLocalEmbarque)
                                detalhe.ValorMercadoriaLocalEmbarque = resultadoVMLE;

                            if (resultadoVMCV != detalhe.ValorMercadoriaCondicaoVenda)
                                detalhe.ValorMercadoriaCondicaoVenda = resultadoVMCV;

                            documentoUnicoExportacaoDAO.AtualizarDUEItemDetalhe(detalhe);

                            if (detalhe.Enquadramento1Id == 80380 || detalhe.Enquadramento2Id == 80380 || detalhe.Enquadramento3Id == 80380 || detalhe.Enquadramento4Id == 80380)
                            {
                                //Alteração 08/01/2020 - A exclusão deve ocorrer em qualquer caso, mesmo se o campo txtLPCO_Default estiver nulo
                                documentoUnicoExportacaoDAO.ExcluirTodosLPCODoItem(detalhe.Id);
                                if (!string.IsNullOrEmpty(this.txtLPCO_Default.Text))
                                {
                                    //documentoUnicoExportacaoDAO.ExcluirTodosLPCODoItem(detalhe.Id);

                                    foreach (var lpco in this.txtLPCO_Default.Text.Split(','))
                                    {
                                        documentoUnicoExportacaoDAO.RegistrarLPCO(new DUEItemDetalhesLPCO
                                        {
                                            IdDetalheItem = detalhe.Id,
                                            Numero = lpco
                                        });
                                    }
                                }
                            }
                            else
                            {
                                documentoUnicoExportacaoDAO.ExcluirTodosLPCODoItem(detalhe.Id);
                            }
                        }
                    }

                    var dueBusca = documentoUnicoExportacaoDAO.ObterDUEPorId(due.Id);

                    if (dueBusca != null)
                    {
                        if (dueBusca.Completa > 0)
                        {
                            if (this.txtDueCompleta.Value.ToInt() == 1)
                            {
                                Response.Redirect(string.Format("CadastrarNotas.aspx?id={0}", dueBusca.Id), false);
                                return;
                            }
                        }
                    }

                    Response.Redirect(string.Format("CadastrarItensDUE.aspx?id={0}", this.txtDueID.Value), false);
                }
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastrarDUE.aspx", ex.ToString());
                if (ex is SqlException)
                {
                    ModelState.AddModelError(string.Empty, $"Ocorreu um problema ao cadastrar DUE [Código erro:{(ex as SqlException).Number}]");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Ocorreu um problema ao cadastrar DUE");
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

        }

        protected void cbUnidadeDespachoRFB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbUnidadeDespachoRFB.SelectedValue != null)
            {
                var codigoRFB = this.cbUnidadeDespachoRFB.SelectedValue.ToInt();

                this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(codigoRFB);
                this.cbRecintoAduaneiroDespacho.DataBind();
            }
        }

        protected void cbUnidadeEmbarqueRFB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbUnidadeEmbarqueRFB.SelectedValue != null)
            {
                var codigoRFB = this.cbUnidadeEmbarqueRFB.SelectedValue.ToInt();

                this.cbRecintoAduaneiroEmbarque.DataSource = recintosDAO.ObterRecintos(codigoRFB);
                this.cbRecintoAduaneiroEmbarque.DataBind();
            }
        }

        protected void cbTipoRecintoAduaneiroDespacho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTipoRecintoAduaneiroDespacho.SelectedValue.ToInt() != 281)
            {
                this.pnlNaoRecintoAduaneiroDespacho.Visible = true;
                this.cbRecintoAduaneiroDespacho.SelectedIndex = -1;
                this.cbRecintoAduaneiroDespacho.ClearSelection();
                this.cbRecintoAduaneiroDespacho.Items.Clear();

                this.spnRecintoAduaneiroDespObrigatorio.Visible = false;
            }
            else
            {
                this.pnlNaoRecintoAduaneiroDespacho.Visible = false;

                this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeDespachoRFB.SelectedValue.ToInt());
                this.cbRecintoAduaneiroDespacho.DataBind();

                this.txtDocumentoResponsavelLocalDespacho.Text = string.Empty;
                this.txtLatitudeLocalDespacho.Text = string.Empty;
                this.txtLongitudeLocalDespacho.Text = string.Empty;
                this.txtEnderecoLocalDespacho.Text = string.Empty;

                this.spnRecintoAduaneiroDespObrigatorio.Visible = true;
            }

        }

        protected void cbTipoRecintoAduaneiroEmbarque_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbTipoRecintoAduaneiroEmbarque.SelectedValue.ToInt() != 281)
            {
                this.pnlNaoRecintoAduaneiroEmbarque.Visible = true;

                this.cbRecintoAduaneiroEmbarque.SelectedIndex = -1;
                this.cbRecintoAduaneiroEmbarque.ClearSelection();
                this.cbRecintoAduaneiroEmbarque.Items.Clear();

                this.spnRecintoAduaneiroEmbObrigatorio.Visible = false;
            }
            else
            {
                this.pnlNaoRecintoAduaneiroEmbarque.Visible = false;

                this.cbRecintoAduaneiroEmbarque.DataSource = recintosDAO.ObterRecintos(this.cbUnidadeEmbarqueRFB.SelectedValue.ToInt());
                this.cbRecintoAduaneiroEmbarque.DataBind();

                this.spnRecintoAduaneiroEmbObrigatorio.Visible = true;
            }
        }

        public void CarregarAtributos(string ncm)
        {
            var attrPadraoQualidade = atributosDAO.ObterAtributos("PadraoQualidade", ncm).ToList();

            this.cbAttrPadraoQualidade.DataSource = atributosDAO.ObterAtributos("PadraoQualidade", ncm);
            this.cbAttrPadraoQualidade.DataBind();

            this.cbAttrEmbarcadoEm.DataSource = atributosDAO.ObterAtributos("EmbarcadoEm", ncm);
            this.cbAttrEmbarcadoEm.DataBind();

            this.cbAttrTipo.DataSource = atributosDAO.ObterAtributos("Tipo", ncm);
            this.cbAttrTipo.DataBind();

            this.cbAttrMetodoProcessamento.DataSource = atributosDAO.ObterAtributos("MetodoProcessamento", ncm);
            this.cbAttrMetodoProcessamento.DataBind();

            this.cbAttrCaracteristicaEspecial.DataSource = atributosDAO.ObterAtributos("CaracteristicaEspecial", ncm);
            this.cbAttrCaracteristicaEspecial.DataBind();

            this.cbAttrPadraoQualidade.Items.Insert(0, new ListItem("-- Selecione --", ""));
            this.cbAttrEmbarcadoEm.Items.Insert(0, new ListItem("-- Selecione --", ""));
            this.cbAttrTipo.Items.Insert(0, new ListItem("-- Selecione --", ""));
            this.cbAttrMetodoProcessamento.Items.Insert(0, new ListItem("-- Selecione --", ""));
            this.cbAttrCaracteristicaEspecial.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void txtNCM_Default_TextChanged(object sender, EventArgs e)
        {
            var parametros = _parametrosDAO.ObterParametros();

            if (!string.IsNullOrEmpty(this.txtNCM_Default.Text))
            {
                if (parametros.ValidarAtributosCafe > 0)
                {
                    var ncm = this.txtNCM_Default.Text.Replace(".", "");

                    CarregarAtributos(ncm);
                    ValidarCamposNCM(ncm);
                }
            }
        }

        private void ValidarCamposNCM(string ncm)
        {
            this.cbAttrPadraoQualidade.SelectedIndex = -1;
            this.cbAttrEmbarcadoEm.SelectedIndex = -1;
            this.cbAttrTipo.SelectedIndex = -1;
            this.cbAttrMetodoProcessamento.SelectedIndex = -1;
            this.cbAttrCaracteristicaEspecial.SelectedIndex = -1;
            this.txtAttrOutraCaracteristicaEspecial.Text = string.Empty;
            this.cbAttrPadraoQualidade.Enabled = false;
            this.cbAttrEmbarcadoEm.Enabled = false;
            this.cbAttrTipo.Enabled = false;
            this.cbAttrMetodoProcessamento.Enabled = false;
            this.cbAttrCaracteristicaEspecial.Enabled = false;
            this.txtAttrOutraCaracteristicaEspecial.Enabled = false;
            this.chkAttrEmbalagemFinal.Checked = false;
            this.chkAttrEmbalagemFinal.Enabled = false;

            if (ncm == "09011110" || ncm == "09011190" || ncm == "09011200" || ncm == "09012100" || ncm == "09012200" || ncm == "21011110" || ncm == "21011190" || ncm == "21011200")
            {
                if (ncm == "09011110" || ncm == "09011190" || ncm == "09011200")
                {
                    this.cbAttrPadraoQualidade.Enabled = true;
                    this.cbAttrEmbarcadoEm.Enabled = true;
                    this.cbAttrTipo.Enabled = true;
                    this.cbAttrMetodoProcessamento.Enabled = true;
                    this.cbAttrCaracteristicaEspecial.Enabled = true;
                    this.txtAttrOutraCaracteristicaEspecial.Enabled = true;
                }

                if (ncm == "09012100" || ncm == "09012200" || ncm == "21011200" || ncm == "21011190")
                {
                    this.cbAttrEmbarcadoEm.Enabled = true;
                    this.cbAttrCaracteristicaEspecial.Enabled = true;
                    this.txtAttrOutraCaracteristicaEspecial.Enabled = true;
                }

                if (ncm == "21011110")
                {
                    this.cbAttrEmbarcadoEm.Enabled = true;
                    this.cbAttrMetodoProcessamento.Enabled = true;
                    this.cbAttrCaracteristicaEspecial.Enabled = true;
                    this.txtAttrOutraCaracteristicaEspecial.Enabled = true;
                    this.chkAttrEmbalagemFinal.Enabled = true;
                }

                this.pnlAtributosNCM.Visible = true;
            }
            else
            {
                this.pnlAtributosNCM.Visible = false;

                this.cbAttrPadraoQualidade.SelectedIndex = -1;
                this.cbAttrEmbarcadoEm.SelectedIndex = -1;
                this.cbAttrTipo.SelectedIndex = -1;
                this.cbAttrMetodoProcessamento.SelectedIndex = -1;
                this.cbAttrCaracteristicaEspecial.SelectedIndex = -1;
                this.txtAttrOutraCaracteristicaEspecial.Text = string.Empty;
            }
        }
    }
}