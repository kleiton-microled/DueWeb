using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ImportarNFs : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly PaisesDAO paisesDAO = new PaisesDAO();
        private readonly EnquadramentosDAO enquadramentosDAO = new EnquadramentosDAO();
        private readonly AtributosNCMDAO atributosDAO = new AtributosNCMDAO();
        private readonly ParametrosDAO _parametrosDAO = new ParametrosDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListarEnquadramentos();
                ListarPaises();
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

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("Default.aspx"));
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtUpload.PostedFile == null)
                    ModelState.AddModelError(string.Empty, "Nenhum arquivo informado");

                if (this.txtUpload.PostedFile.ContentLength == 0)
                    ModelState.AddModelError(string.Empty, "Arquivo inválido");

                if (this.txtNCM_Default.Text != string.Empty)
                {
                    if (this.txtNCM_Default.Text.Replace(".", "").Length != 8)
                        ModelState.AddModelError(string.Empty, "NCM inválido. Informe um NCM de 8 dígitos");
                }


                //if (!this.txtUpload.PostedFile.FileName.ToUpper().EndsWith("TXT") && !this.txtUpload.PostedFile.FileName.ToUpper().EndsWith("CSV"))
                //ModelState.AddModelError(string.Empty, "É permitido apenas importação de arquivos .txt");

                if (_notaFiscalDAO.ExisteNotaFiscalPorNomeArquivo(this.txtUpload.PostedFile.FileName))
                    ModelState.AddModelError(string.Empty, string.Format("O arquivo {0} já foi importado.", this.txtUpload.PostedFile.FileName));

                if (this.chkCriarItensDUE.Checked)
                {
                    if (this.cbCondicaoVenda_Default.SelectedValue == string.Empty)
                    {
                        ModelState.AddModelError(string.Empty, "Selecione a Condição de Venda");
                        this.chkCriarItensDUE.Checked = false;
                    }
                }

                if (!ModelState.IsValid)
                    return;

                if (!UploadArquivo(this.txtUpload))
                {
                    throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
                }

                int quantidadeImportada = 0;

                var notasFiscais = ProcessarArquivo(this.txtUpload.PostedFile.InputStream, ";");

                ExcluirNotasReferenciadas(notasFiscais);

                foreach (var nf in notasFiscais)
                {
                    nf.Arquivo = this.txtUpload.FileName;
                    nf.Usuario = Convert.ToInt32(Session["UsuarioId"].ToString());
                    nf.VMLE = this.txtValorUnitVMLE_Default.Text.ToDecimal();
                    nf.VMCV = this.txtValorUnitVMCV_Default.Text.ToDecimal();

                    var existeNf = _notaFiscalDAO.ExisteNotaFiscal(nf);

                    if (existeNf > 0)
                    {
                        _notaFiscalDAO.ExcluirNotaFiscal(existeNf);
                    }

                    nf.Id = _notaFiscalDAO.Cadastrar(nf);

                    quantidadeImportada++;
                }

                int dueId = 0;

                if (this.chkCriarItensDUE.Checked)
                {
                    dueId = CriarItensDUEAutomaticamente(notasFiscais);
                    _documentoUnicoExportacaoDAO.MarcarComoAutomatica(dueId);
                    ViewState["DueId"] = dueId;
                    this.pnlDueCriada.Visible = true;
                }

                ValidarNotasSiscomex(quantidadeImportada, notasFiscais);

                ViewState["Sucesso"] = true;
                ViewState["TotalNotasFiscais"] = notasFiscais.Count;
                ViewState["QuantidadeImportada"] = quantidadeImportada;

            }
            catch (Exception ex)
            {
                DeletarArquivo(this.txtUpload);
                LogsService.Logar("CadastrarDUE.aspx", ex.ToString());
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        private int CriarItensDUEAutomaticamente(List<NotaFiscal> notasFiscais)
        {
            var exportadorBeneficiarioAc = 0;

            if (this.rbAcBeneficiarioSim.Checked)
            {
                exportadorBeneficiarioAc = 1;
            }

            if (this.rbAcBeneficiarioNao.Checked)
            {
                exportadorBeneficiarioAc = 0;
            }

            var mensagemPadrao = "";

            var dueId = _documentoUnicoExportacaoDAO.CriarItensDUE(
                notasFiscais.Where(c => c.TipoNF == "EXP").ToList(),
                Convert.ToInt32(Session["UsuarioId"].ToString()),
                this.txtValorUnitVMLE_Default.Text,
                this.txtValorUnitVMCV_Default.Text,
                this.cbPaisDestino_Default.SelectedValue,
                this.cbEnquadramento1_Default.SelectedValue,
                this.cbEnquadramento2_Default.SelectedValue,
                this.cbEnquadramento3_Default.SelectedValue,
                this.cbEnquadramento4_Default.SelectedValue,
                this.cbCondicaoVenda_Default.SelectedValue,
                this.txtLPCO_Default.Text,
                this.cbPrioridadeCarga_Default.SelectedValue,
                this.txtDescrComplementar_Default.Text,
                this.txtComissaoAgenteDefault.Text,
                this.cbTipoAC_Default.SelectedValue,
                exportadorBeneficiarioAc.ToString(),
                this.txtNumeroAC_Default.Text,
                this.txtCNPJBeneficiarioAC_Default.Text,
                this.txtNumeroItemAC_Default.Text,
                this.txtNCMItemAC_Default.Text,
                this.txtQuantidadeUtilizadaAC_Default.Text,
                this.txtVMLESemCoberturaCambialAC_Default.Text,
                this.txtVMLEComCoberturaCambialAC_Default.Text,
                this.cbAttrPadraoQualidade.SelectedValue,
                this.cbAttrEmbarcadoEm.SelectedValue,
                this.cbAttrTipo.SelectedValue,
                this.cbAttrMetodoProcessamento.SelectedValue,
                this.cbAttrCaracteristicaEspecial.SelectedValue,
                this.txtAttrOutraCaracteristicaEspecial.Text,
                Convert.ToInt32(this.chkAttrEmbalagemFinal.Checked),
                this.txtNCM_Default.Text.Replace(".", ""));

            foreach (var nf in notasFiscais)
            {
                _notaFiscalDAO.AtualizarIdDUE(nf.Id, dueId);
            }

            return dueId;
        }

        private void ValidarNotasSiscomex(int quantidadeImportada, List<NotaFiscal> notasFiscais)
        {
            if (quantidadeImportada > 0)
            {
                this.gvNotasFiscais.DataSource = notasFiscais.ToList();
                this.gvNotasFiscais.DataBind();

                foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
                {
                    if (linhaGrid.Cells[0].Text != "EXP")
                    {
                        var item = linhaGrid.Cells[12].Text.ToInt();

                        if (item == 0)
                            item = 1;

                        var chaveNf = linhaGrid.Cells[1].Text;
                        var quantidade = linhaGrid.Cells[4].Text.ToDecimal();

                        var saldoDue = _documentoUnicoExportacaoDAO.ObterQuantidadeDUEPorNF(chaveNf);

                        var saldoNota = SisComexService.ConsultarDadosNotaPreACD(chaveNf, item, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                        if (saldoNota != null)
                        {
                            if (saldoNota.Sucesso == false)
                            {
                                linhaGrid.BackColor = System.Drawing.Color.MistyRose;
                                linhaGrid.Cells[11].Text = "1";
                            }
                            else
                            {
                                quantidade = quantidade - saldoDue;

                                if (quantidade <= (decimal)saldoNota.Saldo)
                                {
                                    linhaGrid.BackColor = System.Drawing.Color.LightGreen;
                                    linhaGrid.Cells[11].Text = "3";
                                }
                                else
                                {
                                    linhaGrid.BackColor = System.Drawing.Color.LightYellow;
                                    linhaGrid.Cells[11].Text = "2";
                                }

                                linhaGrid.Cells[5].Text = saldoNota.Saldo.ToString();
                                linhaGrid.Cells[6].Text = saldoDue.ToString();
                                linhaGrid.Cells[13].Text = saldoNota.Recinto;
                            }
                        }
                    }
                }

                if (this.gvNotasFiscais.Rows.Count > 0)
                {
                    this.pnlLegenda.Visible = true;
                }
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
                                    if (!string.IsNullOrEmpty(registro[9]))
                                    {
                                        string empresa = registro[9].Substring(0, 4);
                                        string filial = registro[9].Substring(4, 4);
                                        string memorando = registro[9].Substring(8, 12);

                                        notaFiscal.Empresa = empresa;
                                        notaFiscal.Filial = filial;
                                        notaFiscal.Memorando = memorando;
                                    }
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
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError(string.Empty, ex.Message);
                //}

                return notasFiscais;
            }
        }

        private bool UploadArquivo(FileUpload arquivo)
        {
            string nomeArquivo = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

            try
            {
                arquivo.SaveAs(nomeArquivo);
                return true;
            }
            catch
            {
                return false;
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

        protected void btnGerarExcel_Click(object sender, EventArgs e)
        {
            var notas = new List<NotaFiscal>();

            foreach (GridViewRow linha in this.gvNotasFiscais.Rows)
            {
                string status = string.Empty;
                string codSituacao = string.Empty;

                if (linha.Cells[0].Text != "EXP")
                {
                    var item = linha.Cells[12].Text.ToInt();

                    if (item == 0)
                        item = 1;

                    var saldoDue = _documentoUnicoExportacaoDAO.ObterQuantidadeDUEPorNF(linha.Cells[1].Text);

                    var saldoNota = SisComexService.ConsultarDadosNotaPreACD(linha.Cells[1].Text, item, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                    if (saldoNota != null)
                    {
                        if (saldoNota.Sucesso == false)
                        {
                            status = "Nota não encontrada no CCT";
                            codSituacao = "1";
                        }
                        else
                        {
                            var quantidade = linha.Cells[4].Text.ToDecimal() - saldoDue;

                            if (!((quantidade) <= (decimal)saldoNota.Saldo))
                            {
                                status = "Nota com divergência no saldo";
                                codSituacao = "2";
                            }
                            else
                            {
                                status = "Nota sem divergência";
                                codSituacao = "3";
                            }
                        }
                    }
                }

                notas.Add(new NotaFiscal
                {
                    TipoNF = linha.Cells[0].Text,
                    ChaveNF = linha.Cells[1].Text,
                    NumeroNF = linha.Cells[2].Text,
                    CnpjNF = linha.Cells[3].Text,
                    QuantidadeNF = linha.Cells[4].Text.ToDecimal(),
                    SaldoCCT = linha.Cells[5].Text.ToDecimal(),
                    SaldoOutrasDUES = linha.Cells[6].Text.ToDecimal(),
                    UnidadeNF = linha.Cells[7].Text,
                    NCM = linha.Cells[8].Text,
                    ChaveNFReferencia = linha.Cells[9].Text,
                    Arquivo = linha.Cells[10].Text,
                    Status = status,
                    CodSituacao = codSituacao,
                    Item = linha.Cells[12].Text.ToInt(),
                    Recinto = linha.Cells[13].Text
                });

                status = string.Empty;
            }

            ExcelPackage epackage = new ExcelPackage();
            ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("ExcelTabName");

            excel.Cells["A1"].LoadFromCollection(notas.Select(c => new
            {
                c.TipoNF,
                c.ChaveNF,
                c.NumeroNF,
                c.CnpjNF,
                c.QuantidadeNF,
                c.SaldoCCT,
                c.SaldoOutrasDUES,
                c.UnidadeNF,
                c.NCM,
                ChaveNFReferencia = c.ChaveNFReferencia.Replace("&nbsp;", ""),
                c.Arquivo,
                c.Status,
                c.CodSituacao,
                c.Item,
                c.Recinto
            }), true);

            var nomeArquivo = notas.Select(c => c.Arquivo).FirstOrDefault();

            if (string.IsNullOrEmpty(nomeArquivo))
                nomeArquivo = "ArquivoNotas";

            string attachment = $"attachment; filename={nomeArquivo}.xlsx";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            HttpContext.Current.Response.BinaryWrite(epackage.GetAsByteArray());

            HttpContext.Current.Response.End();
            epackage.Dispose();
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

            if (parametros.ValidarAtributosCafe > 0)
            {
                var ncm = this.txtNCM_Default.Text.Replace(".", "");

                CarregarAtributos(ncm);
                ValidarCamposNCM(ncm);
            }

            if (this.chkCriarItensDUE.Checked)
            {
                this.pnlInformacoesaDefault.Visible = true;
                //this.pnlInformacoesaDefault.CssClass.Replace("invisivel", "display: block;");

                this.pnlInformacoesaDefault.CssClass = this.pnlInformacoesaDefault.CssClass.Replace("invisivel", "");
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