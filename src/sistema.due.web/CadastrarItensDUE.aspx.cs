using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.RetificacaoSemServico;
using Sistema.DUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Xml;

namespace Sistema.DUE.Web
{
    public partial class CadastrarItensDUE : System.Web.UI.Page
    {
        private readonly PaisesDAO paisesDAO = new PaisesDAO();
        private readonly EnquadramentosDAO enquadramentosDAO = new EnquadramentosDAO();
        private readonly DocumentoUnicoExportacaoDAO documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly NotaFiscalDAO notaFiscalDAO = new NotaFiscalDAO();
        private readonly CertificadoDAO _certificadoDAO = new CertificadoDAO();
        private readonly AtributosNCMDAO atributosDAO = new AtributosNCMDAO();
        private readonly ParametrosDAO _parametrosDAO = new ParametrosDAO();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    this.txtDueID.Value = Request.QueryString["id"].ToString();

                    var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                    if (due == null)
                    {
                        ModelState.AddModelError(string.Empty, "Portal Microled: DUE não encontrada");
                        return;
                    }

                    BtnSalvarXMLVisible(due.Id);

                    ListarEnquadramentos();
                    ListarItensDUE();
                    ListarPaises();
                    CarregarInformacoesDefault(this.txtDueID.Value.ToInt());

                    var dadosDue = ServicoSiscomex2.ObterDetalhesDUE(due.DUE, ConfigurationManager.AppSettings["CpfCertificado"].ToString()).Result;

                    if (dadosDue != null)
                    {
                        if (dadosDue.situacaoDUE == 70) //  Averbada
                        {
                            //this.btnFinalizarDUE.Enabled = false;
                            //this.btnCadastrarMaster.Enabled = false;
                            //this.btnLimparItem.Enabled = false;
                            //this.btnAdicionarItemDetalhe.Enabled = false;

                            //this.gvItensDUE.Columns[6].Visible = false;
                            //this.gvItensDUE.Columns[7].Visible = false;

                            //this.gvDetalhesItem.Columns[7].Visible = false;

                            //ModelState.AddModelError(string.Empty, $"A DUE {due.DUE} já foi Averbada. Não é possível retifica-la.");
                            //return;

                            ViewState["DueAverbada"] = true;
                            ViewState["DueAverbadaNumero"] = due.DUE;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }

        private void BtnSalvarXMLVisible(int id)
        {
            var xmlEnviado = documentoUnicoExportacaoDAO.ObterUltimoXMLGerado(id);
            var xmlRetorno = documentoUnicoExportacaoDAO.ObterUltimoXMLRetorno(id);
            btnSalvarXML.Visible = !string.IsNullOrEmpty(xmlEnviado)||!string.IsNullOrEmpty(xmlRetorno);
            btnXMLRetorno.Visible = !string.IsNullOrEmpty(xmlRetorno);
            btnXMLEnviado.Visible = !string.IsNullOrEmpty(xmlEnviado);
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

        private void CarregarInformacoesDefault(int dueId)
        {
            var due = documentoUnicoExportacaoDAO.ObterDUEPorId(dueId);

            this.txtSituacaoEspecial.Value = due.SituacaoEspecial.ToString();

            if (due.CriadoPorNF > 0)
            {
                //this.gvItensDUE.Columns[6].Visible = false;
                this.gvItensDUE.Columns[7].Visible = false;
            }

            if (due.ValorUnitVMCV_Default > 0)
            {
                this.txtValorUnitVMCV.Text = due.ValorUnitVMCV_Default.ToString();
            }

            if (due.ValorUnitVMLE_Default > 0)
            {
                this.txtValorUnitVMLE.Text = due.ValorUnitVMLE_Default.ToString();
            }

            if (due.CondicaoVenda_Default != string.Empty)
            {
                this.cbCondicaoVenda.SelectedValue = due.CondicaoVenda_Default;
            }

            if (due.PaisDestino_Default != string.Empty)
            {
                this.cbPaisDestino.SelectedValue = due.PaisDestino_Default;
            }

            if (due.Enquadramento1_Default > 0)
            {
                this.cbEnquadramento1.SelectedValue = due.Enquadramento1_Default.ToString();
            }

            if (due.Enquadramento2_Default > 0)
            {
                this.cbEnquadramento2.SelectedValue = due.Enquadramento2_Default.ToString();
            }

            if (due.Enquadramento3_Default > 0)
            {
                this.cbEnquadramento3.SelectedValue = due.Enquadramento3_Default.ToString();
            }

            if (due.Enquadramento4_Default > 0)
            {
                this.cbEnquadramento4.SelectedValue = due.Enquadramento4_Default.ToString();
            }

            if (due.Prioridade_Default != 0)
            {
                this.cbPrioridadeCarga.SelectedValue = due.Prioridade_Default.ToString();
            }

            if (due.DescricaoComplementar_Default != string.Empty)
            {
                this.txtDescricaoComplementarItem.Text = due.DescricaoComplementar_Default;
            }



            if (due.EnviadoSiscomex == 0)
            {
                this.btnFinalizarDUE.Text = "Finalizar e Enviar ao Siscomex";

                if (due.Completa == 0)
                {
                    if (due.SituacaoEspecial == 0)
                    {
                        this.btnFinalizarDUE.Text = "Finalizar e Enviar ao Siscomex";
                    }
                }
            }
            else
            {
                this.btnFinalizarDUE.Text = "Finalizar e Enviar ao Siscomex";
            }
        }

        protected void ListarEnquadramentos()
        {
            var enquadramentos = enquadramentosDAO.ObterEnquadramentos();

            foreach (var enquadramento in enquadramentos)
            {
                this.cbEnquadramento1.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento2.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento3.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
                this.cbEnquadramento4.Items.Add(new ListItem(enquadramento.Descricao, enquadramento.Codigo.ToString()));
            }

            this.cbEnquadramento1.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento2.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento3.Items.Insert(0, new ListItem("-- Selecione --", "0"));
            this.cbEnquadramento4.Items.Insert(0, new ListItem("-- Selecione --", "0"));

            this.cbEnquadramento1.SelectedValue = "80000";
        }

        protected void ListarItensDUE()
        {
            this.gvItensDUE.DataSource = documentoUnicoExportacaoDAO.ObterResumoItensDUE(this.txtDueID.Value.ToInt());
            this.gvItensDUE.DataBind();
        }

        protected void ListarPaises()
        {
            var paises = paisesDAO.ObterPaises();

            this.cbExportadorPais.Items.Add(new ListItem("", ""));
            this.cbImportadorPais.Items.Add(new ListItem("", ""));
            this.cbPaisDestino.Items.Add(new ListItem("", ""));

            foreach (var pais in paises)
            {
                this.cbExportadorPais.Items.Add(new ListItem(pais.Descricao, pais.Sigla));
                this.cbImportadorPais.Items.Add(new ListItem(pais.Descricao, pais.Sigla));
                this.cbPaisDestino.Items.Add(new ListItem(pais.Descricao, pais.Sigla));
            }

            this.cbExportadorPais.SelectedIndex = 0;
            this.cbImportadorPais.SelectedIndex = 0;
            this.cbPaisDestino.SelectedIndex = 0;
        }

        protected void ListarDetalhesItem(int dueItemId)
        {
            this.gvDetalhesItem.DataSource = documentoUnicoExportacaoDAO.ObterDetalhesItem(dueItemId);
            this.gvDetalhesItem.DataBind();
        }

        protected void btnCadastrarMaster_Click(object sender, EventArgs e)
        {
            try
            {
                var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                string condicaoVenda = due.CondicaoVenda_Default;

                if (this.cbCondicaoVenda.SelectedValue != string.Empty)
                {
                    if (this.cbCondicaoVenda.SelectedValue != due.CondicaoVenda_Default)
                    {
                        condicaoVenda = this.cbCondicaoVenda.SelectedValue;
                    }
                }

                var item = new DUEItem(
                    this.txtDueID.Value.ToInt(),
                    this.cbMotivoDispensaNotaFiscal.SelectedValue.ToInt(),
                    condicaoVenda);

                var exportador = new Exportador(
                    this.txtItemExportador.Text,
                    this.txtItemDocumentoExportador.Text.RemoverCaracteresEspeciaisCNPJ(),
                    this.txtItemExportadorEndereco.Text,
                    this.txtItemExportadorUF.Text,
                    this.cbExportadorPais.Text);

                var importador = new Importador(this.txtImportadorNome.Text, this.txtImportadorEndereco.Text, this.cbImportadorPais.Text);

                item.AdicionarExportador(exportador);
                item.AdicionarImportador(importador);

                if (due.ValorUnitVMLE_Default > 0)
                {
                    item.ValorUnitVMLE = due.ValorUnitVMLE_Default;
                }
                else
                {
                    item.ValorUnitVMLE = this.txtValorUnitVMLE.Text.ToDecimal();
                }

                if (due.ValorUnitVMCV_Default > 0)
                {
                    item.ValorUnitVMCV = due.ValorUnitVMCV_Default;
                }
                else
                {
                    item.ValorUnitVMCV = this.txtValorUnitVMCV.Text.ToDecimal();
                }

                if (string.IsNullOrEmpty(this.txtDUEItemID.Value))
                {
                    if (Request.QueryString["id"] != null)
                    {
                        if (due.CriadoPorNF > 0)
                        {
                            ModelState.AddModelError(string.Empty, "Não é permitido incluir novos itens de uma DUE criada a partir de um arquivo de Notas Fiscais.");
                            return;
                        }
                    }

                    documentoUnicoExportacaoDAO.RegistrarItemDUE(item);
                }
                else
                {
                    item.Id = this.txtDUEItemID.Value.ToInt();
                    documentoUnicoExportacaoDAO.AtualizarItemDUE(item);
                }

                ListarItensDUE();
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastraritensDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        protected void gvItensDUE_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (this.gvItensDUE.Rows.Count == 0)
                    return;

                var index = Convert.ToInt32(e.CommandArgument);

                var dueItemId = this.gvItensDUE.DataKeys[index]["Id"].ToString().ToInt();

                if (e.CommandName == "LANCAR_ITENS")
                {
                    LimparDetalhesItem();

                    var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                    if (dueItemId > 0)
                    {
                        this.txtDUEItemID.Value = dueItemId.ToString();

                        var itemDue = documentoUnicoExportacaoDAO.ObterItemDUEPorID(dueItemId);

                        if (itemDue != null)
                        {
                            this.txtItemExportador.Text = itemDue.Exportador;
                            this.txtItemDocumentoExportador.Text = itemDue.ExportadorDocumento;
                            this.txtItemExportadorEndereco.Text = itemDue.ExportadorEndereco;
                            this.txtItemExportadorUF.Text = itemDue.ExportadorUF;

                            if (!string.IsNullOrEmpty(itemDue.ExportadorPais))
                                this.cbExportadorPais.SelectedValue = itemDue.ExportadorPais;

                            this.txtImportadorNome.Text = itemDue.Importador;
                            this.txtImportadorEndereco.Text = itemDue.ImportadorEndereco;

                            if (!string.IsNullOrEmpty(itemDue.ImportadorPais))
                                this.cbImportadorPais.SelectedValue = itemDue.ImportadorPais;

                            this.cbMotivoDispensaNotaFiscal.SelectedValue = itemDue.MotivoDispensaNF;

                            CarregarInformacoesDefault(this.txtDueID.Value.ToInt());

                            if (!string.IsNullOrEmpty(itemDue.CondicaoVenda))
                                this.cbCondicaoVenda.SelectedValue = itemDue.CondicaoVenda;

                            this.txtValorUnitVMLE.Text = itemDue.ValorUnitVMLE;
                            this.txtValorUnitVMCV.Text = itemDue.ValorUnitVMCV;

                            this.txtChaveNFExp.Text = !string.IsNullOrEmpty(itemDue.NF) ? itemDue.NF : string.Empty;

                            ListarDetalhesItem(dueItemId);

                            this.pnlDetalhesItens.Visible = true;
                            this.lblItemSelecionado.Text = (index + 1).ToString();

                            CalcularValoresMercadoria();
                            this.txtItem.Text = documentoUnicoExportacaoDAO.ObterSequenciaItem(itemDue.Id);
                        }
                    }

                    foreach (GridViewRow linha in this.gvItensDUE.Rows)
                    {
                        linha.BackColor = System.Drawing.Color.White;
                    }

                    this.gvItensDUE.Rows[index].BackColor = System.Drawing.Color.LightGreen;
                }

                if (e.CommandName == "REPLICAR_ITEM")
                {
                    if (dueItemId > 0)
                    {
                        var itemBusca = documentoUnicoExportacaoDAO.ObterItemDUEPorID(dueItemId);

                        if (itemBusca != null)
                        {
                            if (itemBusca.DueId.ToInt() > 0)
                            {
                                if (!string.IsNullOrEmpty(itemBusca.NF))
                                {
                                    ModelState.AddModelError(string.Empty, "Opção não permitida para itens criados a partir de um arquivo e que já possui uma Nota de Exportação vinculada.");
                                    return;
                                }
                            }

                            documentoUnicoExportacaoDAO.ReplicarItemDUE(dueItemId);
                            ListarItensDUE();
                        }
                    }
                }

                if (e.CommandName == "EXCLUIR_ITEM")
                {
                    var itemDue = documentoUnicoExportacaoDAO.ObterItemDUEPorID(dueItemId);

                    if (itemDue != null)
                    {
                        documentoUnicoExportacaoDAO.ExcluirItemDUE(dueItemId);
                        notaFiscalDAO.ExcluirNotaFiscalPorChaveEDUE(this.txtDueID.Value.ToInt(), itemDue.NF);
                        ListarItensDUE();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastraritensDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        protected void btnAdicionarItemDetalhe_Click(object sender, EventArgs e)
        {
            try
            {
                var parametros = _parametrosDAO.ObterParametros();

                if (parametros.ValidarAtributosCafe > 0)
                {
                    var ncm = this.txtNCM.Text;

                    if (ncm == "09011110" || ncm == "09011190" || ncm == "09011200")
                    {
                        if (this.cbAttrPadraoQualidade.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Padrão de Qualidade");
                            return;
                        }

                        if (this.cbAttrEmbarcadoEm.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Modo de Embarque");
                            return;
                        }

                        if (this.cbAttrTipo.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Tipo do Produto");
                            return;
                        }

                        if (this.cbAttrMetodoProcessamento.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Modo de Processamento");
                            return;
                        }

                        //if (this.cbAttrCaracteristicaEspecial.SelectedValue == string.Empty)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Selecione a Característica Especial");
                        //    return;
                        //}
                    }

                    if (ncm == "09012100" || ncm == "09012200" || ncm == "21011200" || ncm == "21011190")
                    {
                        if (this.cbAttrEmbarcadoEm.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Modo de Embarque");
                            return;
                        }

                        //if (this.cbAttrCaracteristicaEspecial.SelectedValue == string.Empty)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Selecione a Característica Especial");
                        //    return;
                        //}
                    }

                    if (ncm == "21011110")
                    {
                        if (this.cbAttrEmbarcadoEm.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Modo de Embarque");
                            return;
                        }

                        if (this.cbAttrMetodoProcessamento.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Selecione o Modo de Processamento");
                            return;
                        }

                        //if (this.cbAttrCaracteristicaEspecial.SelectedValue == string.Empty)
                        //{
                        //    ModelState.AddModelError(string.Empty, "Selecione a Característica Especial");
                        //    return;
                        //}
                    }
                }

                if (string.IsNullOrEmpty(this.txtDUEDetalheItemID.Value))
                {
                    var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                    var paisDestino = string.Empty;
                    var enquadramento1 = 0;
                    var enquadramento2 = 0;
                    var enquadramento3 = 0;
                    var enquadramento4 = 0;
                    var prioridadeCarga = 0;
                    var descricaoComplementar = string.Empty;

                    if (!string.IsNullOrEmpty(due.PaisDestino_Default))
                    {
                        paisDestino = due.PaisDestino_Default;
                    }
                    else
                    {
                        if (this.cbPaisDestino.SelectedValue == string.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Informe o País de destino");
                            return;
                        }

                        paisDestino = this.cbPaisDestino.SelectedValue;
                    }

                    if (this.txtDescricaoUnidade.SelectedValue == string.Empty)
                    {
                        ModelState.AddModelError(string.Empty, "Selecione a Unidade (TON, KG ou UN)");
                        return;
                    }

                    prioridadeCarga = due.Prioridade_Default != 0 ? due.Prioridade_Default : this.cbPrioridadeCarga.SelectedValue.ToInt();

                    descricaoComplementar = due.DescricaoComplementar_Default != string.Empty ? due.DescricaoComplementar_Default : this.txtDescricaoComplementarItem.Text;

                    enquadramento1 = due.Enquadramento1_Default > 0 ? due.Enquadramento1_Default : this.cbEnquadramento1.SelectedValue.ToInt();

                    enquadramento2 = due.Enquadramento2_Default > 0 ? due.Enquadramento2_Default : this.cbEnquadramento2.SelectedValue.ToInt();

                    enquadramento3 = due.Enquadramento3_Default > 0 ? due.Enquadramento3_Default : this.cbEnquadramento3.SelectedValue.ToInt();

                    enquadramento4 = due.Enquadramento4_Default > 0 ? due.Enquadramento4_Default : this.cbEnquadramento4.SelectedValue.ToInt();

                    var dueDetalhe = new DUEItemDetalhes(
                        this.txtDUEItemID.Value.ToInt(),
                        this.txtItem.Text.ToInt(),
                        this.txtValorMercadoriaLocalEmbarque.Text.ToDecimal(),
                        paisDestino,
                        this.txtQtdeItem.Text.ToDecimal(),
                        prioridadeCarga,
                        this.txtDataHoraLimite.Text.ToInt(),
                        descricaoComplementar,
                        this.txtNCM.Text,
                        this.txtValorMercadoriaCondicaoVenda.Text.ToDecimal(),
                        this.txtDescricaoMercadoria.Text,
                        this.txtQtdUnidades.Text.ToDecimal(),
                        this.txtDescricaoUnidade.Text,
                        this.txtCodProduto.Text.ToInt(),
                        this.txtPesoLiquidoTotal.Text.ToDecimal(),
                        enquadramento1,
                        enquadramento2,
                        enquadramento3,
                        enquadramento4,
                        this.txtComissaoAgente.Text.ToDecimal(),
                        this.cbAttrPadraoQualidade.SelectedValue,
                        this.cbAttrEmbarcadoEm.SelectedValue,
                        this.cbAttrTipo.SelectedValue,
                        this.cbAttrMetodoProcessamento.SelectedValue,
                        this.cbAttrCaracteristicaEspecial.SelectedValue,
                        this.txtAttrOutraCaracteristicaEspecial.Text,
                        Convert.ToInt32(this.chkAttrEmbalagemFinal.Checked));

                    dueDetalhe.Id = documentoUnicoExportacaoDAO.RegistrarDUEItemDetalhe(dueDetalhe);

                    this.txtDUEDetalheItemID.Value = dueDetalhe.Id.ToString();

                    if (!string.IsNullOrEmpty(due.LPCO_Default))
                    {
                        var lpcos = due.LPCO_Default.Split(',');

                        foreach (var lpco in lpcos)
                        {
                            documentoUnicoExportacaoDAO.RegistrarLPCO(new DUEItemDetalhesLPCO(dueDetalhe.Id, lpco));
                        }
                    }
                }
                else
                {
                    if (this.cbPaisDestino.SelectedValue == string.Empty)
                    {
                        ModelState.AddModelError(string.Empty, "Informe o País de destino");
                        return;
                    }

                    if (this.txtDescricaoUnidade.SelectedValue == string.Empty)
                    {
                        ModelState.AddModelError(string.Empty, "Selecione a Unidade (TON, KG ou UN)");
                        return;
                    }

                    var dueDetalheUpdate = new DUEItemDetalhes(
                        this.txtDUEItemID.Value.ToInt(),
                        this.txtItem.Text.ToInt(),
                        this.txtValorMercadoriaLocalEmbarque.Text.ToDecimal(),
                        this.cbPaisDestino.SelectedValue,
                        this.txtQtdeItem.Text.ToDecimal(),
                        this.cbPrioridadeCarga.SelectedValue.ToInt(),
                        this.txtDataHoraLimite.Text.ToInt(),
                        this.txtDescricaoComplementarItem.Text,
                        this.txtNCM.Text,
                        this.txtValorMercadoriaCondicaoVenda.Text.ToDecimal(),
                        this.txtDescricaoMercadoria.Text,
                        this.txtQtdUnidades.Text.ToDecimal(),
                        this.txtDescricaoUnidade.Text,
                        this.txtCodProduto.Text.ToInt(),
                        this.txtPesoLiquidoTotal.Text.ToDecimal(),
                        this.cbEnquadramento1.SelectedValue.ToInt(),
                        this.cbEnquadramento2.SelectedValue.ToInt(),
                        this.cbEnquadramento3.SelectedValue.ToInt(),
                        this.cbEnquadramento4.SelectedValue.ToInt(),
                        this.txtComissaoAgente.Text.ToDecimal(),
                        this.cbAttrPadraoQualidade.SelectedValue,
                        this.cbAttrEmbarcadoEm.SelectedValue,
                        this.cbAttrTipo.SelectedValue,
                        this.cbAttrMetodoProcessamento.SelectedValue,
                        this.cbAttrCaracteristicaEspecial.SelectedValue,
                        this.txtAttrOutraCaracteristicaEspecial.Text,
                        Convert.ToInt32(this.chkAttrEmbalagemFinal.Checked));

                    dueDetalheUpdate.Id = this.txtDUEDetalheItemID.Value.ToInt();
                    documentoUnicoExportacaoDAO.AtualizarDUEItemDetalhe(dueDetalheUpdate);

                    var atosC = documentoUnicoExportacaoDAO.ObterAtosConcessorios(dueDetalheUpdate.Id);
                    var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(dueDetalheUpdate.Id);

                    if (this.cbEnquadramento1.SelectedValue.ToInt() != 81101
                        && this.cbEnquadramento2.SelectedValue.ToInt() != 81101
                        && this.cbEnquadramento3.SelectedValue.ToInt() != 81101
                        && this.cbEnquadramento4.SelectedValue.ToInt() != 81101)
                    {
                        if (atosC.Any())
                        {
                            foreach (var ato in atosC)
                            {
                                documentoUnicoExportacaoDAO.ExcluirAtoConcessorio(ato.Id);
                            }
                        }
                    }

                    if (this.cbEnquadramento1.SelectedValue.ToInt() != 80380
                        && this.cbEnquadramento2.SelectedValue.ToInt() != 80380
                        && this.cbEnquadramento3.SelectedValue.ToInt() != 80380
                        && this.cbEnquadramento4.SelectedValue.ToInt() != 80380)
                    {
                        if (lpcos.Any())
                        {
                            foreach (var lpco in lpcos)
                            {
                                documentoUnicoExportacaoDAO.ExcluirLPCO(lpco.Id);
                            }
                        }
                    }
                }

                ListarDetalhesItem(this.txtDUEItemID.Value.ToInt());

                if (this.cbEnquadramento1.SelectedValue.ToInt() != 81101 && this.cbEnquadramento2.SelectedValue.ToInt() != 81101 && this.cbEnquadramento3.SelectedValue.ToInt() != 81101 && this.cbEnquadramento4.SelectedValue.ToInt() != 81101)
                {
                    if (this.cbEnquadramento1.SelectedValue.ToInt() != 80380 && this.cbEnquadramento2.SelectedValue.ToInt() != 80380 && this.cbEnquadramento3.SelectedValue.ToInt() != 80380 && this.cbEnquadramento4.SelectedValue.ToInt() != 80380)
                    {
                        LimparDetalhesItem();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastraritensDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        protected void btnLimparItem_Click(object sender, EventArgs e)
        {
            this.txtDUEItemID.Value = string.Empty;
            this.txtItemExportador.Text = string.Empty;
            this.txtItemDocumentoExportador.Text = string.Empty;
            this.txtItemExportadorEndereco.Text = string.Empty;
            this.cbExportadorPais.SelectedIndex = -1;
            this.txtItemExportadorUF.Text = string.Empty;

            this.txtImportadorNome.Text = string.Empty;
            this.txtImportadorEndereco.Text = string.Empty;
            this.cbImportadorPais.SelectedIndex = -1;

            this.cbMotivoDispensaNotaFiscal.SelectedIndex = -1;
            this.cbCondicaoVenda.SelectedIndex = -1;

            this.cbExportadorPais.SelectedValue = "BR";

            this.txtChaveNFExp.Text = string.Empty;

            CarregarInformacoesDefault(this.txtDueID.Value.ToInt());
        }

        protected void btnVoltarDetalhesItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("CadastrarItensDUE.aspx?id={0}", this.txtDueID.Value), false);
        }

        protected void gvDetalhesItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (this.gvDetalhesItem.Rows.Count == 0)
                return;

            try
            {
                var index = Convert.ToInt32(e.CommandArgument);

                var detalheItemId = this.gvDetalhesItem.DataKeys[index]["Id"].ToString().ToInt();

                if (e.CommandName == "EXCLUIR_DETALHE_ITEM")
                {
                    if (detalheItemId > 0)
                    {
                        var detalheItem = documentoUnicoExportacaoDAO.ObterDetalhesItemPorId(detalheItemId);

                        if (detalheItem != null)
                        {
                            documentoUnicoExportacaoDAO.ExcluirDetalheItem(detalheItemId);
                            ListarDetalhesItem(this.txtDUEItemID.Value.ToInt());
                            LimparDetalhesItem();
                        }
                    }
                }

                if (e.CommandName == "EDITAR_DETALHE_ITEM")
                {
                    var detalheItem = documentoUnicoExportacaoDAO.ObterDetalhesItemPorId(detalheItemId);

                    if (detalheItem != null)
                    {
                        this.txtDUEDetalheItemID.Value = detalheItem.Id.ToString();
                        this.txtDUEItemID.Value = detalheItem.DUEItemId.ToString();
                        this.txtItem.Text = detalheItem.Item.ToString();
                        this.txtValorMercadoriaLocalEmbarque.Text = detalheItem.ValorMercadoriaLocalEmbarque.ToString();
                        this.txtQtdeItem.Text = detalheItem.QuantidadeEstatistica.ToString();
                        this.cbPrioridadeCarga.SelectedValue = detalheItem.PrioridadeCarga.ToString();
                        this.txtDataHoraLimite.Text = detalheItem.Limite.ToString();
                        this.txtDescricaoComplementarItem.Text = detalheItem.DescricaoComplementar;
                        this.txtNCM.Text = detalheItem.NCM;
                        this.txtValorMercadoriaCondicaoVenda.Text = detalheItem.ValorMercadoriaCondicaoVenda.ToString();
                        this.txtDescricaoMercadoria.Text = detalheItem.DescricaoMercadoria;
                        this.txtQtdUnidades.Text = detalheItem.QuantidadeUnidades.ToString();
                        this.txtDescricaoUnidade.Text = detalheItem.DescricaoUnidade;
                        this.txtCodProduto.Text = detalheItem.CodigoProduto.ToString();
                        this.txtPesoLiquidoTotal.Text = detalheItem.PesoLiquidoTotal.ToString();
                        this.cbEnquadramento1.SelectedValue = detalheItem.Enquadramento1Id.ToString();
                        this.cbEnquadramento2.SelectedValue = detalheItem.Enquadramento2Id.ToString();
                        this.cbEnquadramento3.SelectedValue = detalheItem.Enquadramento3Id.ToString();
                        this.cbEnquadramento4.SelectedValue = detalheItem.Enquadramento4Id.ToString();
                        this.txtComissaoAgente.Text = detalheItem.ComissaoAgente.ToString();
                        this.chkAttrEmbalagemFinal.Checked = Convert.ToBoolean(detalheItem.Attr_Embalagem_Final);
                        

                        var parametros = _parametrosDAO.ObterParametros();

                        if (parametros.ValidarAtributosCafe > 0)
                        {
                            CarregarAtributos(detalheItem.NCM);

                            ValidarCamposNCM(detalheItem.NCM);

                            if (!string.IsNullOrEmpty(detalheItem.Attr_Padrao_Qualidade))
                            {
                                if (detalheItem.Attr_Padrao_Qualidade.ToInt() > 0)
                                {
                                    this.cbAttrPadraoQualidade.SelectedValue = detalheItem.Attr_Padrao_Qualidade.ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(detalheItem.Attr_Embarque_Em))
                            {
                                if (detalheItem.Attr_Embarque_Em.ToInt() > 0)
                                {
                                    this.cbAttrEmbarcadoEm.SelectedValue = detalheItem.Attr_Embarque_Em.ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(detalheItem.Attr_Tipo))
                            {
                                if (detalheItem.Attr_Tipo.ToInt() > 0)
                                {
                                    this.cbAttrTipo.SelectedValue = detalheItem.Attr_Tipo.ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(detalheItem.Attr_Metodo_Processamento))
                            {
                                if (detalheItem.Attr_Metodo_Processamento.ToInt() > 0)
                                {
                                    this.cbAttrMetodoProcessamento.SelectedValue = detalheItem.Attr_Metodo_Processamento.ToString();
                                }
                            }

                            if (!string.IsNullOrEmpty(detalheItem.Attr_Caracteristica_Especial))
                                this.cbAttrCaracteristicaEspecial.SelectedValue = detalheItem.Attr_Caracteristica_Especial;

                            this.txtAttrOutraCaracteristicaEspecial.Text = detalheItem.Attr_Outra_Caracteristica_Especial;
                        }

                        if (!string.IsNullOrEmpty(detalheItem.PaisDestino))
                            this.cbPaisDestino.SelectedValue = detalheItem.PaisDestino;

                        this.btnAdicionarItemDetalhe.Text = "Atualizar";

                        var ncm = this.txtNCM.Text.Replace(".", "");

                        CalcularValoresMercadoria();
                    }
                }
            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastraritensDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        protected void btnFinalizarDUE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtDueID.Value))
            {
                ModelState.AddModelError(string.Empty, "Portal Microled: DUE não informada");
                return;
            }

            try
            {
                var due = documentoUnicoExportacaoDAO.ObterDUEPorId(this.txtDueID.Value.ToInt());

                if (due == null)
                {
                    ModelState.AddModelError(string.Empty, "Portal Microled: DUE não encontrada");
                    return;
                }

                if (due.UnidadeDespacho == null)
                    ModelState.AddModelError(string.Empty, "Portal Microled: Unidade de Despacho não informada");

                if (due.UnidadeEmbarque == null)
                    ModelState.AddModelError(string.Empty, "Portal Microled: Unidade de Embarque não informada");

                var itens = documentoUnicoExportacaoDAO.ObterItensDUE(due.Id);

                if (itens == null || itens.Count() == 0)
                    ModelState.AddModelError(string.Empty, "Portal Microled: A DUE não possui nenhum item vinculado");

                due.AdicionarItens(itens);

                if (due.Itens.Where(c => c.CondicaoVenda == string.Empty).Any())
                    ModelState.AddModelError(string.Empty, "Portal Microled: Existem itens sem Condição de Venda");

                foreach (var item in due.Itens)
                {
                    var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                    if (subItens.Where(c => c.PaisDestino == string.Empty).Any())
                    {
                        ModelState.AddModelError(string.Empty, "Portal Microled: Existem itens sem País de Destino");
                        break;
                    }
                }

                if (!ModelState.IsValid)
                    return;

                var xml = string.Empty;

                if (due.Completa > 0 || due.CriadoPorNF > 0 || itens.Any(c => !string.IsNullOrEmpty(c.NF)))
                {
                    xml = GerarXMLDUESiscomexComNotaFiscal(due.Id);
                }
                else
                {                   
                    xml = GerarXMLDUESiscomex(due);
                }

                //Gravar XML
                XmlDocument documentoXML = new XmlDocument();
                documentoXML.LoadXml(xml);
                documentoUnicoExportacaoDAO.AtualizarInformacoesEnvioUltimoXMLGerado(due.Id, xml);

                BtnSalvarXMLVisible(due.Id);

                using (var ws = new ServicoSiscomex.WsDUE())
                {
                    ws.Timeout = 900000;

                    var retorno = new ServicoSiscomex.RetornoSiscomex();

                    var cpfCertificado = _certificadoDAO.ObterCpfCertificado(due.DocumentoDeclarante);
                    var documentoFormatado = string.Empty;

                    if (string.IsNullOrEmpty(cpfCertificado))
                    {
                        if (!string.IsNullOrEmpty(due.DocumentoDeclarante))
                        {
                            if (due.DocumentoDeclarante.Length == 14)
                            {
                                documentoFormatado = Convert.ToUInt64(due.DocumentoDeclarante).ToString(@"00\.000\.000\/0000-00");
                            }
                            else
                            {
                                documentoFormatado = Convert.ToUInt64(due.DocumentoDeclarante).ToString(@"000\.000\.000\-00");
                            }
                        }

                        var cnpjExportador = due.Itens
                            .Select(c => c.Exportador?.Documento)
                            .Distinct()
                            .FirstOrDefault();

                        cpfCertificado = _certificadoDAO.ObterCpfCertificado(cnpjExportador);

                        if (string.IsNullOrEmpty(cpfCertificado))
                        {
                            if (!string.IsNullOrEmpty(cnpjExportador))
                            {
                                documentoFormatado = Convert.ToUInt64(cnpjExportador).ToString(@"00\.000\.000\/0000-00");
                            }

                            ModelState.AddModelError(string.Empty, $"Certificado Digital para o CNPJ {documentoFormatado} não encontrado");
                            return;
                        }
                    }

                    if (due.EnviadoSiscomex == 0)
                    {
                        retorno = ws.EnviarDUESemNF(xml, cpfCertificado);
                    }
                    else
                    {
                        if (due.Completa > 0 || due.CriadoPorNF > 0 || itens.Any(c => !string.IsNullOrEmpty(c.NF)))
                        {
                            var parametros = _parametrosDAO.ObterParametros();

                            if (parametros.EnvioRetificacaoSemServico > 0)
                            {
                                retorno = EnvioRetificacao.EnviarSemServico(documentoXML.InnerXml, due.DUE.Replace("-", ""), cpfCertificado).GetAwaiter().GetResult();
                            }
                            else
                            {
                                retorno = ws.EnviarRetificacao(xml, due.DUE.Replace("-", ""), cpfCertificado);
                            }
                        }
                        else
                        {
                            Response.Redirect("ConsultarDUE.aspx");
                        }
                    }

                    var xmlEnviado = xml;

                    xmlEnviado = xmlEnviado.Replace("\r\n", string.Empty);
                    xmlEnviado = System.Text.RegularExpressions.Regex.Replace(xmlEnviado, @"\t|\n|\r", "");
                    xmlEnviado = xmlEnviado.Replace("      ", " ");
                    xmlEnviado = xmlEnviado.Replace("    ", " ");
                    xmlEnviado = xmlEnviado.Replace("   ", " ");
                    xmlEnviado = xmlEnviado.Replace("  ", " ");
                    xmlEnviado = xmlEnviado.Replace(Environment.NewLine, "");

                    if (retorno.Sucesso)
                    {
                        ViewState["RetornoSucesso"] = true;
                        ViewState["RetornoWarnings"] = retorno.Warnings;
                        ViewState["RetornoMessage"] = retorno.Message;
                        ViewState["RetornoDUE"] = retorno?.DUE ?? due.DUE;
                        ViewState["RetornoRUC"] = retorno?.RUC ?? due.RUC;

                        if (due.EnviadoSiscomex == 0)
                        {
                            ViewState["ChaveDeAcesso"] = retorno?.ChaveDeAcesso ?? string.Empty;
                            documentoUnicoExportacaoDAO.AtualizarInformacoesEnvio(due.Id, retorno.DUE, retorno.RUC, retorno.ChaveDeAcesso, xmlEnviado, retorno.XmlRetorno, 1);
                        }
                        else
                        {
                            ViewState["ChaveDeAcesso"] = due.ChaveAcesso ?? string.Empty;
                            documentoUnicoExportacaoDAO.MarcarComoRetificado(due.Id);
                        }
                    }
                    else
                    {
                        if (retorno.Criticas != null)
                        {
                            foreach (var erro in retorno.Criticas)
                                ModelState.AddModelError(string.Empty, "Siscomex: " + erro.message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Siscomex: " + retorno.Message);
                        }

                        documentoUnicoExportacaoDAO.AtualizarInformacoesErro(due.Id, xmlEnviado, retorno.XmlRetorno);
                    }
                }
                BtnSalvarXMLVisible(due.Id);

            }
            catch (Exception ex)
            {
                LogsService.Logar("CadastraritensDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        private string GerarXMLDUESiscomexComNotaFiscal(int dueId)
        {
            var parametros = _parametrosDAO.ObterParametros();

            var due = documentoUnicoExportacaoDAO.ObterDUEPorId(dueId);

            var itens = documentoUnicoExportacaoDAO.ObterItensDUE(dueId);

            due.AdicionarItens(itens);

            var recintoDespachoId = string.IsNullOrEmpty(due.UnidadeDespacho.RecintoAduaneiroId)
                ? due.UnidadeDespacho.DocumentoResponsavel
                : due.UnidadeDespacho.RecintoAduaneiroId;

            var xml = $@"
                    <Declaration 
                        xsi:schemaLocation='urn:wco:datamodel:WCO:GoodsDeclaration:1 GoodsDeclaration_1p0_DUE.xsd' 
                        xmlns:ds='urn:wco:datamodel:WCO:GoodsDeclaration_DS:1' 
                        xmlns='urn:wco:datamodel:WCO:GoodsDeclaration:1' 
                        xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                            <DeclarationNFe>                            
                            <DeclarationOffice>
                                <ID listID='token'>{due.UnidadeDespacho.Id.ToString()}</ID>
                                <Warehouse>
                                    <ID>{recintoDespachoId}</ID>
                                    <TypeCode>{due.UnidadeDespacho.TipoRecinto}</TypeCode>";

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Latitude))
            {
                xml = xml + $@"<LatitudeMeasure>{due.UnidadeDespacho.Latitude}</LatitudeMeasure>";
            }

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Longitude))
            {
                xml = xml + $@"<LongitudeMeasure>{due.UnidadeDespacho.Longitude}</LongitudeMeasure>";
            }

            if (!string.IsNullOrEmpty(due.UnidadeDespacho.Endereco))
            {
                xml = xml + $@"<Address><Line>{due.UnidadeDespacho.Endereco}</Line></Address>";
            }

            xml = xml + $@"
                                </Warehouse>
                            </DeclarationOffice>
                            <AdditionalInformation>
                                <StatementCode>{due.FormaExportacao.ToString()}</StatementCode>
                                <StatementTypeCode>CUS</StatementTypeCode>
                            </AdditionalInformation>";

            if (!string.IsNullOrEmpty(due.DUE))
            {
                xml = xml + $@"<AdditionalInformation>
                                <StatementTypeCode>DEF</StatementTypeCode>
                                <StatementDescription>Vinculo de notas fiscais</StatementDescription>
                            </AdditionalInformation>";
            }

            if (due.SituacaoEspecial != 0)
            {
                xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{due.SituacaoEspecial.ToString()}</StatementCode>
                                <StatementTypeCode>AHZ</StatementTypeCode>
                            </AdditionalInformation>";
            }

            if (due.ViaEspecialTransporte != 0)
            {
                xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{due.ViaEspecialTransporte.ToString()}</StatementCode>
                                <StatementTypeCode>TRA</StatementTypeCode>
                            </AdditionalInformation>";
            }

            xml = xml + $@"<AdditionalInformation>
                                <StatementDescription languageID=''>{due.InformacoesComplementares.ToString().RemoverCaracteresEspeciais().QuebraDeLinhaXML()}</StatementDescription>
                                <LimitDateTime>10</LimitDateTime>
                                <StatementTypeCode>AAI</StatementTypeCode>
                            </AdditionalInformation>
                            <CurrencyExchange>
                                <CurrencyTypeCode>{due.MoedaNegociacao}</CurrencyTypeCode>
                            </CurrencyExchange>
                            <Declarant>
                                <ID schemeID='token'>{due.DocumentoDeclarante}</ID>
                            </Declarant>
                            <ExitOffice>
                                <ID>{due.UnidadeEmbarque.Id}</ID>
                                    <Warehouse>
                                        <ID>{due.UnidadeEmbarque.RecintoAduaneiroId}</ID>
                                        <TypeCode>{due.UnidadeEmbarque.TipoRecinto}</TypeCode>
                                    </Warehouse>
                            </ExitOffice>";

            var sequenciaItem = 0;
            var ultimaChave = "";

            foreach (var item in due.Itens)
            {
                sequenciaItem++;

                ultimaChave = item.NF;

                xml = xml + $@"<GoodsShipment>";

                if (!string.IsNullOrEmpty(item.Exportador?.Descricao))
                {
                    xml = xml + $@"<Exporter>
                            <Name languageID=''>{item.Exportador?.Descricao}</Name>
                            <ID schemeID='token'>{item.Exportador?.Documento}</ID>
                            <Address>
                                <CountryCode>{item.Exportador?.Pais}</CountryCode>
                                <CountrySubDivisionCode>{string.Concat(item.Exportador?.Pais, "-", item.Exportador?.UF.ToUpper())}</CountrySubDivisionCode>
                                <Line languageID=''>{item.Exportador?.Endereco}</Line>
                            </Address>
                        </Exporter>";
                }

                var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                foreach (var subItem in subItens)
                {
                    xml = xml + $@"
	                            <GovernmentAgencyGoodsItem>
	                                <CustomsValueAmount languageID=''>{subItem.ValorMercadoriaLocalEmbarque.ToString().PPonto()}</CustomsValueAmount>
	                                <SequenceNumeric>{sequenciaItem}</SequenceNumeric>
	                                    <Destination>
		                                    <CountryCode>{subItem.PaisDestino}</CountryCode>
		                                    <GoodsMeasure>
		                                    <TariffQuantity>{subItem.QuantidadeEstatistica.ToString().PPonto()}</TariffQuantity>
		                                    </GoodsMeasure>
	                                    </Destination>";

                    if (subItem.PrioridadeCarga != 0)
                    {
                        xml = xml + $@"
                            <AdditionalInformation>
                                <StatementCode>{subItem.PrioridadeCarga.ToString()}</StatementCode>
                                <StatementDescription languageID=''></StatementDescription> 
                                <LimitDateTime>{subItem.Limite.ToString()}</LimitDateTime>
                                <StatementTypeCode>PRI</StatementTypeCode>
                            </AdditionalInformation>";
                    }

                    xml = xml + $@"
	                            <Commodity>
		                            <Description>{subItem.DescricaoComplementar}</Description>
		                            <ValueAmount schemeID='token'>{subItem.ValorMercadoriaCondicaoVenda.ToString().PPonto()}</ValueAmount>
                                    <CommercialDescription languageID=''>{subItem.DescricaoMercadoria}</CommercialDescription>   
                                    <Classification>
                                        <ID schemeID='token'>{subItem.NCM.ToString()}</ID>
                                        <IdentificationTypeCode>HS</IdentificationTypeCode>
                                    </Classification>";

                    if (subItem.NCM.ToString() == "35051000")
                    {
                        xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>ATT_725</TypeCode>
                                        <Description>99</Description>
                                    </ProductCharacteristics>";

                       
                    }

                    if (parametros.ValidarAtributosCafe > 0)
                    {
                        if (!string.IsNullOrEmpty(subItem.Attr_Padrao_Qualidade))
                        {
                            if (subItem.Attr_Padrao_Qualidade.ToInt() > 0)
                            {
                                var atributoPadraoQualidade = atributosDAO.ObterAtributo(
                                    "PadraoQualidade",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Padrao_Qualidade.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoPadraoQualidade?.Atributo}</TypeCode>
                                        <Description>{atributoPadraoQualidade?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Embarque_Em))
                        {
                            if (subItem.Attr_Embarque_Em.ToInt() > 0)
                            {
                                var atributoEmbarcadoEm = atributosDAO.ObterAtributo(
                                    "EmbarcadoEm",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Embarque_Em.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoEmbarcadoEm?.Atributo}</TypeCode>
                                        <Description>{atributoEmbarcadoEm?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Tipo))
                        {
                            if (subItem.Attr_Tipo.ToInt() > 0)
                            {
                                var atributoTipo = atributosDAO.ObterAtributo(
                                    "Tipo",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Tipo.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoTipo?.Atributo}</TypeCode>
                                        <Description>{atributoTipo?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Metodo_Processamento))
                        {
                            if (subItem.Attr_Metodo_Processamento.ToInt() > 0)
                            {
                                var atributoMetodoProcessamento = atributosDAO.ObterAtributo(
                                    "MetodoProcessamento",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Metodo_Processamento.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoMetodoProcessamento?.Atributo}</TypeCode>
                                        <Description>{atributoMetodoProcessamento?.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Caracteristica_Especial))
                        {
                            var atributoCaracteristicaEspecial = atributosDAO.ObterAtributo(
                                "CaracteristicaEspecial",
                                subItem.NCM.ToString(),
                                subItem.Attr_Caracteristica_Especial);

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoCaracteristicaEspecial?.Atributo}</TypeCode>
                                    <Description>{atributoCaracteristicaEspecial?.Codigo}</Description>
                                </ProductCharacteristics>";
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Outra_Caracteristica_Especial))
                        {
                            var atributoOutraCaracteristicaEspecial = atributosDAO.ObterAtributo("OutraCaracteristicaEspecial", subItem.NCM.ToString());

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoOutraCaracteristicaEspecial?.Atributo}</TypeCode>
                                    <Description>{subItem?.Attr_Outra_Caracteristica_Especial}</Description>
                                </ProductCharacteristics>";
                        }
                    }

                    xml = xml + $@"<Product>
                                        <ID schemeID='token'>{subItem.CodigoProduto.ToString()}</ID>
                                        <IdentifierTypeCode>VN</IdentifierTypeCode>
                                    </Product>";

                    xml = xml + $@"<InvoiceLine>
		                                <SequenceNumeric>{subItem.Item}</SequenceNumeric>";

                    IEnumerable<NotaFiscal> notasReferenciadas = new List<NotaFiscal>();

                    if (due.Completa > 0 || due.CriadoPorNF > 0)
                    {
                        // Pega somente as notas que foram cadastradas NA DUE
                        notasReferenciadas = notaFiscalDAO.ObterNotasFiscaisRemessaDUE(item.NF, item.DueId);
                    }
                    else
                    {
                        // Pega somente as notas que foram importadas PELO ARQUIVO

                        notasReferenciadas = notaFiscalDAO.ObterNotasFiscaisRemessa(item.NF);
                    }

                    foreach (var nfRef in notasReferenciadas)
                    {
                        // <SequenceNumeric>1</SequenceNumeric>
                        // Alterado em 09/04 para
                        // <SequenceNumeric>{nfRef.Item}</SequenceNumeric>

                        var nfRefItem = nfRef.Item == 0 ? 1 : nfRef.Item;

                        xml = xml + $@"                            
		                                <ReferencedInvoiceLine>
			                                <SequenceNumeric>{nfRefItem}</SequenceNumeric>
			                                <InvoiceIdentificationID schemeID='token'>{nfRef.ChaveNF}</InvoiceIdentificationID>
			                                <GoodsMeasure>
			                                    <TariffQuantity unitCode=''>{nfRef.QuantidadeNF.ToString().PPonto()}</TariffQuantity>
			                                </GoodsMeasure>
		                                </ReferencedInvoiceLine>";
                    }

                    xml = xml + $@"</InvoiceLine>";

                    xml = xml + $@"</Commodity>
	                            <GoodsMeasure>
		                            <NetNetWeightMeasure>{subItem.PesoLiquidoTotal.ToString().PPonto()}</NetNetWeightMeasure>
	                            </GoodsMeasure>
	                            <GovernmentProcedure>
		                            <CurrentCode>{subItem.Enquadramento1Id.ToString()}</CurrentCode>
	                            </GovernmentProcedure>";

                    if (subItem.Enquadramento2Id > 0)
                    {
                        xml = xml + $@"<GovernmentProcedure>
		                                 <CurrentCode>{subItem.Enquadramento2Id.ToString()}</CurrentCode>
	                                   </GovernmentProcedure>";
                    }

                    if (subItem.ComissaoAgente > 0)
                    {
                        xml = xml + $@"
                                    <ValuationAdjustment>
		                                <AdditionCode>149</AdditionCode>
		                                <PercentageNumeric>{subItem.ComissaoAgente.ToString().PPonto()}</PercentageNumeric>
	                                </ValuationAdjustment>";
                    }

                    var dadosAtosConcessorios = documentoUnicoExportacaoDAO.ObterAtosConcessorios(subItem.Id);

                    if (dadosAtosConcessorios != null)
                    {
                        foreach (var atoConcessorio in dadosAtosConcessorios)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>AC</CategoryCode>
                                      <ID>{atoConcessorio.Numero}</ID>
                                      <ItemID>{atoConcessorio.NumeroItem}</ItemID>
                                      <QuantityQuantity>{atoConcessorio.QuantidadeUtilizada.ToString().PPonto()}</QuantityQuantity>
                                      <ValueWithExchangeCoverAmount>{atoConcessorio.VMLEComCoberturaCambial.ToString().PPonto()}</ValueWithExchangeCoverAmount>
                                      <ValueWithoutExchangeCoverAmount>{atoConcessorio.VMLESemCoberturaCambial.ToString().PPonto()}</ValueWithoutExchangeCoverAmount>
                                      <DrawbackHsClassification>{atoConcessorio.NCMItem}</DrawbackHsClassification>
                                      <DrawbackRecipientId>{atoConcessorio.CNPJBeneficiario}</DrawbackRecipientId>
                                   </AdditionalDocument>";
                        }
                    }

                    var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(subItem.Id);

                    if (lpcos != null)
                    {
                        foreach (var lpco in lpcos)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>LPCO</CategoryCode>
                                      <ID>{lpco.Numero}</ID>                                     
                                   </AdditionalDocument>";
                        }
                    }

                    xml = xml + "</GovernmentAgencyGoodsItem>";
                }


                if (!string.IsNullOrEmpty(item.Importador?.Descricao))
                {
                    xml = xml + $@"
                            <Importer>
                                <Name languageID=''>{item.Importador?.Descricao}</Name>
                                <Address>
                                    <CountryCode>{item.Importador?.Pais}</CountryCode>
                                    <Line languageID=''>{item.Importador?.Endereco}</Line>
                                </Address>
                            </Importer>";
                }

                IEnumerable<NotaFiscal> notasRef = new List<NotaFiscal>();

                if (due.Completa > 0 || due.CriadoPorNF > 0)
                {
                    // Pega somente as notas que foram cadastradas NA DUE
                    notasRef = notaFiscalDAO.ObterNotasFiscaisRemessaDUE(item.NF, item.DueId);
                }
                else
                {
                    // Pega somente as notas que foram importadas PELO ARQUIVO
                    notasRef = notaFiscalDAO.ObterNotasFiscaisRemessa(item.NF);
                }

                xml = xml + $@"<Invoice>
	                            <ID schemeID='token'>{item.NF}</ID>
	                            <TypeCode>388</TypeCode>";

                var notasRefSemRepeticao = notasRef
                        .Select(c => new
                        {
                            c.ChaveNF,
                            c.CnpjNF
                        }).ToList()
                        .Distinct().ToList();

                foreach (var nfRef in notasRefSemRepeticao)
                {
                    var cpfCnpj = string.Empty;
                    var cpfFormatado = string.Empty;

                    if (nfRef.CnpjNF.Length >= 11)
                    {
                        cpfFormatado = nfRef.CnpjNF.Substring(nfRef.CnpjNF.Length - 11);
                    }

                    if (ValidacaoCPF.Validar(cpfFormatado))
                        cpfCnpj = cpfFormatado;
                    else
                        cpfCnpj = nfRef.CnpjNF;

                    xml = xml + $@"                            
                                <ReferencedInvoice>
		                            <ID schemeID='token'>{nfRef.ChaveNF}</ID>
		                            <TypeCode>REM</TypeCode>
		                            <Submitter>
		                                <ID schemeID='token'>{cpfCnpj}</ID>
		                            </Submitter>
	                            </ReferencedInvoice>";
                }

                xml = xml + $@"</Invoice>";


                xml = xml + $@"<TradeTerms>
	                                <ConditionCode>{item.CondicaoVenda}</ConditionCode>
	                           </TradeTerms>
                        </GoodsShipment>";                
            }

            xml = xml + $@"                        
                            <UCR>
	                            <TraderAssignedReferenceID>{due.RUC}</TraderAssignedReferenceID>
                            </UCR>
                        </DeclarationNFe>
                    </Declaration>";

            return xml;
        }        

        private string GerarXMLDUESiscomex(DUEMaster due)
        {
            var parametros = _parametrosDAO.ObterParametros();

            var recintoDespachoId = string.IsNullOrEmpty(due.UnidadeDespacho.RecintoAduaneiroId)
                ? due.UnidadeDespacho.DocumentoResponsavel
                : due.UnidadeDespacho.RecintoAduaneiroId;

            var xml = $@"<Declaration 
                        xsi:schemaLocation='urn:wco:datamodel:WCO:GoodsDeclaration:1 GoodsDeclaration_1p0_DUE.xsd' 
                        xmlns:ds='urn:wco:datamodel:WCO:GoodsDeclaration_DS:1' 
                        xmlns='urn:wco:datamodel:WCO:GoodsDeclaration:1' 
                        xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                          <DeclarationNoNF>
                            <ID schemeID='token'/>
                            <DeclarationOffice>
                              <ID listID='token'>{due.UnidadeDespacho.Id}</ID>
                              <Warehouse>
                                <ID schemeID='token'>{recintoDespachoId}</ID>
                                <TypeCode>{due.UnidadeDespacho.TipoRecinto.ToString()}</TypeCode>";

            if (due.UnidadeDespacho.TipoRecinto != 281)
            {
                xml = xml + $@"
                    <LatitudeMeasure unitCode=''>{due.UnidadeDespacho.Latitude}</LatitudeMeasure>
                    <LongitudeMeasure unitCode=''>{due.UnidadeDespacho.Longitude}</LongitudeMeasure>
                    <Address>
                        <Line languageID=''>{due.UnidadeDespacho.Endereco}</Line>
                    </Address>";
            }

            xml = xml + $@" 
                             </Warehouse>
                    </DeclarationOffice>
                    <AdditionalInformation>
                        <StatementCode>{due.FormaExportacao.ToString()}</StatementCode>
                        <StatementTypeCode>CUS</StatementTypeCode>
                    </AdditionalInformation>";

            if (due.ViaEspecialTransporte != 0)
            {
                xml = xml + $@"
                    <AdditionalInformation>
                        <StatementCode>{due.ViaEspecialTransporte.ToString()}</StatementCode>
                        <StatementTypeCode>TRA</StatementTypeCode>
                    </AdditionalInformation>";
            }

            if (due.SituacaoEspecial != 0)
            {
                xml = xml + $@"
                    <AdditionalInformation>
                        <StatementCode>{due.SituacaoEspecial.ToString()}</StatementCode>
                        <StatementTypeCode>AHZ</StatementTypeCode>
                    </AdditionalInformation>";
            }

            xml = xml + $@"<AdditionalInformation>
                        <StatementDescription languageID=''>{due.InformacoesComplementares}</StatementDescription>
                        <StatementTypeCode>AAI</StatementTypeCode>
                    </AdditionalInformation>
                    <CurrencyExchange>
                        <CurrencyTypeCode>{due.MoedaNegociacao}</CurrencyTypeCode>
                    </CurrencyExchange>
                    <Declarant>
                        <ID schemeID='token'>{due.DocumentoDeclarante}</ID>
                    </Declarant>
                    <ExitOffice>
                        <ID schemeID='token'>{due.UnidadeEmbarque.Id.ToString()}</ID>
                        <Warehouse>
                        <ID schemeID='token'>{due.UnidadeEmbarque.RecintoAduaneiroId.ToString()}</ID>
                        <TypeCode>{due.UnidadeEmbarque.TipoRecinto.ToString()}</TypeCode>
                        </Warehouse>
                    </ExitOffice>";

            var sequenciaItem = 1;

            foreach (var item in due.Itens)
            {
                xml = xml + $@"<GoodsShipment>";

                if (!string.IsNullOrEmpty(item.Exportador.Descricao))
                {
                    xml = xml + $@"
                        <Exporter>
                            <Name languageID=''>{item.Exportador.Descricao}</Name>
                            <ID schemeID='token'>{item.Exportador.Documento}</ID>
                            <Address>
                                <CountryCode>{item.Exportador.Pais}</CountryCode>
                                <CountrySubDivisionCode>{string.Concat(item.Exportador.Pais, "-", item.Exportador.UF.ToUpper())}</CountrySubDivisionCode>
                                <Line languageID=''>{item.Exportador.Endereco}</Line>
                            </Address>
                        </Exporter>";
                }

                var subItens = documentoUnicoExportacaoDAO.ObterDetalhesItem(item.Id);

                foreach (var subItem in subItens)
                {
                    xml = xml + $@"
                        <GovernmentAgencyGoodsItem>
                            <CustomsValueAmount languageID=''>{subItem.ValorMercadoriaLocalEmbarque.ToString().PPonto()}</CustomsValueAmount>
                            <SequenceNumeric>{sequenciaItem}</SequenceNumeric>
                            <Destination>
                                <CountryCode>{subItem.PaisDestino}</CountryCode>
                                <GoodsMeasure>
                                <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>
                            </Destination>";

                    if (subItem.PrioridadeCarga != 0)
                    {
                        xml = xml + $@"<AdditionalInformation>
                                            <StatementCode>{subItem.PrioridadeCarga.ToString()}</StatementCode>
                                            <StatementDescription languageID=''></StatementDescription>
                                            <LimitDateTime>{subItem.Limite.ToString()}</LimitDateTime>
                                            <StatementTypeCode>PRI</StatementTypeCode>
                                       </AdditionalInformation>";
                    }

                    xml = xml + $@"                                                                
                            <Commodity>
                                <Description languageID=''>{subItem.DescricaoComplementar}</Description>
                                <ValueAmount schemeID='token'>{subItem.ValorMercadoriaCondicaoVenda.ToString().PPonto()}</ValueAmount>
                                <CommercialDescription languageID=''>{subItem.DescricaoMercadoria}</CommercialDescription>
                                <Classification>
                                    <ID schemeID='token'>{subItem.NCM.ToString()}</ID>
                                    <IdentificationTypeCode>HS</IdentificationTypeCode>
                                </Classification>
                                <GoodsMeasure>
                                    <TypeCode>AAF</TypeCode>
                                    <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>
                                <GoodsMeasure>
                                    <UnitDescription languageID=''>{subItem.DescricaoUnidade}</UnitDescription>
                                    <TypeCode>ABW</TypeCode>
                                    <TariffQuantity unitCode=''>{subItem.QuantidadeUnidades.ToString().PPonto()}</TariffQuantity>
                                </GoodsMeasure>";

                    if (parametros.ValidarAtributosCafe > 0)
                    {
                        if (!string.IsNullOrEmpty(subItem.Attr_Padrao_Qualidade))
                        {
                            if (subItem.Attr_Padrao_Qualidade.ToInt() > 0)
                            {
                                var atributoPadraoQualidade = atributosDAO.ObterAtributo(
                                    "PadraoQualidade",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Padrao_Qualidade.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoPadraoQualidade.Atributo}</TypeCode>
                                        <Description>{atributoPadraoQualidade.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Embarque_Em))
                        {
                            if (subItem.Attr_Embarque_Em.ToInt() > 0)
                            {
                                var atributoEmbarcadoEm = atributosDAO.ObterAtributo(
                                    "EmbarcadoEm",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Embarque_Em.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoEmbarcadoEm.Atributo}</TypeCode>
                                        <Description>{atributoEmbarcadoEm.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Tipo))
                        {
                            if (subItem.Attr_Tipo.ToInt() > 0)
                            {
                                var atributoTipo = atributosDAO.ObterAtributo(
                                    "Tipo",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Tipo.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoTipo.Atributo}</TypeCode>
                                        <Description>{atributoTipo.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Metodo_Processamento))
                        {
                            if (subItem.Attr_Metodo_Processamento.ToInt() > 0)
                            {
                                var atributoMetodoProcessamento = atributosDAO.ObterAtributo(
                                    "MetodoProcessamento",
                                    subItem.NCM.ToString(),
                                    subItem.Attr_Metodo_Processamento.ToString());

                                xml = xml + $@"
                                    <ProductCharacteristics>
                                        <TypeCode>{atributoMetodoProcessamento.Atributo}</TypeCode>
                                        <Description>{atributoMetodoProcessamento.Codigo}</Description>
                                    </ProductCharacteristics>";
                            }
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Caracteristica_Especial))
                        {
                            var atributoCaracteristicaEspecial = atributosDAO.ObterAtributo(
                                "CaracteristicaEspecial",
                                subItem.NCM.ToString(),
                                subItem.Attr_Caracteristica_Especial);

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoCaracteristicaEspecial.Atributo}</TypeCode>
                                    <Description>{atributoCaracteristicaEspecial.Codigo}</Description>
                                </ProductCharacteristics>";
                        }

                        if (!string.IsNullOrEmpty(subItem.Attr_Outra_Caracteristica_Especial))
                        {
                            var atributoOutraCaracteristicaEspecial = atributosDAO.ObterAtributo("OutraCaracteristicaEspecial", subItem.NCM.ToString());

                            xml = xml + $@"
                                <ProductCharacteristics>
                                    <TypeCode>{atributoOutraCaracteristicaEspecial.Atributo}</TypeCode>
                                    <Description>{subItem.Attr_Outra_Caracteristica_Especial}</Description>
                                </ProductCharacteristics>";
                        }
                    }

                    xml = xml + $@" <Product>
                                    <ID schemeID='token'>{subItem.CodigoProduto.ToString()}</ID>
                                    <IdentifierTypeCode>VN</IdentifierTypeCode>
                                </Product>
                            </Commodity>
                            <GoodsMeasure>
                                <NetNetWeightMeasure unitCode=''>{subItem.PesoLiquidoTotal.ToString().PPonto()}</NetNetWeightMeasure>
                            </GoodsMeasure>";

                    if (subItem.Enquadramento1Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento1Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento2Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento2Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento3Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento3Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (subItem.Enquadramento4Id > 0)
                    {
                        xml = xml + $@"
                            <GovernmentProcedure>
                                <CurrentCode schemeID='token'>{subItem.Enquadramento4Id.ToString()}</CurrentCode>
                            </GovernmentProcedure>";
                    }

                    if (!string.IsNullOrEmpty(due.DUE))
                    {
                        xml = xml + $@"
                            <PreviousDocument>
                                <ID schemeID='token'>ID</ID>
                                <TypeCode>830</TypeCode>
                            </PreviousDocument>";
                    }

                    if (subItem.ComissaoAgente != 0)
                    {
                        xml = xml + $@"
                            <ValuationAdjustment>
                                <AdditionCode>149</AdditionCode>
                                <PercentageNumeric>{subItem.ComissaoAgente.ToString().PPonto()}</PercentageNumeric>
                            </ValuationAdjustment>";
                    }

                    var dadosAtosConcessorios = documentoUnicoExportacaoDAO.ObterAtosConcessorios(subItem.Id);

                    if (dadosAtosConcessorios != null)
                    {
                        foreach (var atoConcessorio in dadosAtosConcessorios)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>AC</CategoryCode>
                                      <ID>{atoConcessorio.Numero}</ID>
                                      <ItemID>{atoConcessorio.NumeroItem}</ItemID>
                                      <QuantityQuantity>{atoConcessorio.QuantidadeUtilizada.ToString().PPonto()}</QuantityQuantity>
                                      <ValueWithExchangeCoverAmount>{atoConcessorio.VMLEComCoberturaCambial.ToString().PPonto()}</ValueWithExchangeCoverAmount>
                                      <ValueWithoutExchangeCoverAmount>{atoConcessorio.VMLESemCoberturaCambial.ToString().PPonto()}</ValueWithoutExchangeCoverAmount>
                                      <DrawbackHsClassification>{atoConcessorio.NCMItem}</DrawbackHsClassification>
                                      <DrawbackRecipientId>{atoConcessorio.CNPJBeneficiario}</DrawbackRecipientId>
                                   </AdditionalDocument>";
                        }
                    }

                    var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(subItem.Id);

                    if (lpcos != null)
                    {
                        foreach (var lpco in lpcos)
                        {
                            xml = xml + $@"
                                    <AdditionalDocument>
                                      <CategoryCode>LPCO</CategoryCode>
                                      <ID>{lpco.Numero}</ID>                                     
                                   </AdditionalDocument>";
                        }
                    }

                    xml = xml + "</GovernmentAgencyGoodsItem>";
                }

                if (!string.IsNullOrEmpty(item.Importador.Descricao))
                {
                    xml = xml + $@"
                        <Importer>
                            <Name languageID=''>{item.Importador.Descricao}</Name>
                            <Address>
                                <CountryCode>{item.Importador.Pais}</CountryCode>
                                <Line languageID=''>{item.Importador.Endereco.Replace("'", " ")}</Line>
                            </Address>
                        </Importer>
                        <Invoice>
                            <TypeCode>388</TypeCode>";
                }


                if (item.MotivoDispensaNF != 0)
                {
                    xml = xml + $@"
                        <AdditionalInformation>
                            <StatementCode>{item.MotivoDispensaNF.ToString()}</StatementCode>
                            <StatementTypeCode>ACG</StatementTypeCode>
                        </AdditionalInformation>";
                }

                xml = xml + $@"
                    </Invoice>
                        <TradeTerms>
                            <ConditionCode>{item.CondicaoVenda}</ConditionCode>
                        </TradeTerms>
                </GoodsShipment>";

                sequenciaItem++;
            }

            xml = xml + $@"<UCR>                            
                            <TraderAssignedReferenceID schemeID='token'>{due.RUC}</TraderAssignedReferenceID>
                           </UCR>
                        </DeclarationNoNF>
                    </Declaration>";

            return xml;
        }

        protected void btnLimparItemDetalhe_Click(object sender, EventArgs e)
        {
            LimparDetalhesItem();
        }

        private void LimparDetalhesItem()
        {
            this.txtDUEDetalheItemID.Value = string.Empty;
            this.txtItem.Text = string.Empty;
            this.txtValorMercadoriaLocalEmbarque.Text = string.Empty;
            this.cbPaisDestino.SelectedIndex = 0;
            this.cbImportadorPais.SelectedIndex = 0;
            this.txtQtdeItem.Text = string.Empty;
            this.cbPrioridadeCarga.SelectedIndex = -1;
            this.txtDataHoraLimite.Text = string.Empty;
            this.txtDescricaoComplementarItem.Text = string.Empty;
            this.txtNCM.Text = string.Empty;
            this.txtValorMercadoriaCondicaoVenda.Text = string.Empty;
            this.txtDescricaoMercadoria.Text = string.Empty;
            this.txtQtdUnidades.Text = string.Empty;
            this.txtDescricaoUnidade.SelectedIndex = 0;
            this.txtCodProduto.Text = string.Empty;
            this.txtPesoLiquidoTotal.Text = string.Empty;
            this.cbEnquadramento1.SelectedIndex = -1;
            this.cbEnquadramento2.SelectedIndex = -1;
            this.cbEnquadramento3.SelectedIndex = -1;
            this.cbEnquadramento4.SelectedIndex = -1;
            this.txtComissaoAgente.Text = string.Empty;

            this.txtItem.Text = documentoUnicoExportacaoDAO.ObterSequenciaItem(this.txtDUEItemID.Value.ToInt());
        }

        protected void btnRetornar_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("CadastrarDUE.aspx?id={0}#step-2", this.txtDueID.Value));
        }

        [WebMethod]
        public static string AdicionarAtoConcessorio(
            string detalheItemId,
            string numeroAc,
            string cnpjAc,
            string numeroItemAc,
            string ncmAc,
            string qtdeUtilizadaAc,
            string vmleComCobertura,
            string vmleSemCobertura,
            string tipoAc,
            string expBeneficiario,
            string id)
        {
            if (detalheItemId.ToInt() == 0)
                return string.Empty;

            var atoConcessorio = new DUEItemDetalhesAC(
                detalheItemId.ToInt(),
                numeroAc,
                cnpjAc,
                numeroItemAc,
                ncmAc,
                qtdeUtilizadaAc.ToDecimal(),
                vmleComCobertura.ToDecimal(),
                vmleSemCobertura.ToDecimal(),
                tipoAc,
                expBeneficiario.ToInt());

            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

            if (string.IsNullOrEmpty(id))
                documentoUnicoExportacaoDAO.RegistrarAtoConcessorio(atoConcessorio);
            else
            {
                atoConcessorio.Id = Convert.ToInt32(id);
                documentoUnicoExportacaoDAO.AtualizarAtoConcessorio(atoConcessorio);
            }

            return ObterAtosConcessoriosJson(detalheItemId.ToInt());
        }

        [WebMethod]
        public static string RemoverAtoConcessorio(string id, string detalheItemId)
        {
            if (id.ToInt() == 0)
                return string.Empty;

            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            documentoUnicoExportacaoDAO.ExcluirAtoConcessorio(id.ToInt());

            return ObterAtosConcessoriosJson(detalheItemId.ToInt());
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterAtosConcessoriosJson(int detalheItemId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var dadosAtosConcessorios = documentoUnicoExportacaoDAO.ObterAtosConcessorios(detalheItemId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dadosAtosConcessorios);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterAtosConcessoriosPorIdJson(int id)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var dadosAtosConcessorio = documentoUnicoExportacaoDAO.ObterAtosConcessorioPorId(id);

            return Newtonsoft.Json.JsonConvert.SerializeObject(dadosAtosConcessorio);
        }

        [WebMethod]
        public static string AdicionarLPCO(
            string detalheItemId,
            string numeroLPCO,
            string id)
        {
            if (detalheItemId.ToInt() == 0)
                return string.Empty;

            var lpco = new DUEItemDetalhesLPCO(
                detalheItemId.ToInt(),
                numeroLPCO);

            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

            if (string.IsNullOrEmpty(id))
                documentoUnicoExportacaoDAO.RegistrarLPCO(lpco);
            else
            {
                lpco.Id = Convert.ToInt32(id);
                documentoUnicoExportacaoDAO.AtualizarLPCO(lpco);
            }

            return ObterLPCOJson(detalheItemId.ToInt());
        }

        [WebMethod]
        public static string RemoverLPCO(string id, string detalheItemId)
        {
            if (id.ToInt() == 0)
                return string.Empty;

            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            documentoUnicoExportacaoDAO.ExcluirLPCO(id.ToInt());

            return ObterLPCOJson(detalheItemId.ToInt());
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterLPCOJson(int detalheItemId)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var lpcos = documentoUnicoExportacaoDAO.ObterLPCO(detalheItemId);

            return Newtonsoft.Json.JsonConvert.SerializeObject(lpcos);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterLPCOPorIdJson(int id)
        {
            var documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
            var lpco = documentoUnicoExportacaoDAO.ObterLPCOPorId(id);

            return Newtonsoft.Json.JsonConvert.SerializeObject(lpco);
        }

        protected void txtPesoLiquidoTotal_TextChanged(object sender, EventArgs e)
        {
            CalcularValoresMercadoria();
        }

        private void CalcularValoresMercadoria()
        {
            if (this.txtValorUnitVMLE.Text.ToDecimal() > 0)
            {
                var resultadoVMLE = ((this.txtPesoLiquidoTotal.Text.ToDecimal() * (this.txtValorUnitVMLE.Text.ToDecimal() / 1000)));
                this.txtValorMercadoriaLocalEmbarque.Text = resultadoVMLE.ToString("n2").Replace(".", "");
            }

            if (this.txtValorUnitVMCV.Text.ToDecimal() > 0)
            {
                var resultadoVMCV = ((this.txtPesoLiquidoTotal.Text.ToDecimal() * (this.txtValorUnitVMCV.Text.ToDecimal() / 1000)));
                this.txtValorMercadoriaCondicaoVenda.Text = resultadoVMCV.ToString("n2").Replace(".", "");
            }
        }

        protected void txtValorUnitVMLE_TextChanged(object sender, EventArgs e)
        {
            CalcularValoresMercadoria();
        }

        protected void txtValorUnitVMCV_TextChanged(object sender, EventArgs e)
        {
            CalcularValoresMercadoria();
        }

        protected void btnIrParaNotas_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastrarNotas.aspx?id=" + this.txtDueID.Value);
        }

        protected void txtNCM_TextChanged(object sender, EventArgs e)
        {
            var parametros = _parametrosDAO.ObterParametros();

            if (!string.IsNullOrEmpty(this.txtNCM.Text))
            {
                if (parametros.ValidarAtributosCafe > 0)
                {
                    var ncm = this.txtNCM.Text.Replace(".", "");

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
