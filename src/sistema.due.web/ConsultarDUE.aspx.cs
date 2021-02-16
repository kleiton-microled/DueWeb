using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.OperacoesXML;
using Sistema.DUE.Web.XML;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace Sistema.DUE.Web
{
    public partial class ConsultarDUE : System.Web.UI.Page
    {
        private readonly DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ConsultarDUEs();
            }
        }

        protected void ConsultarDUEs()
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtDUE.Text))
            {
                filtro.Append($" AND A.DUE LIKE '%{this.txtDUE.Text.Trim().Replace("-", "")}%' ");
            }

            if (!string.IsNullOrEmpty(this.txtDe.Text))
            {
                filtro.Append($" AND A.DataCadastro >= CONVERT(DATETIME, '{this.txtDe.Text} 00:00:00', 103)");
            }

            if (!string.IsNullOrEmpty(this.txtAte.Text))
            {
                filtro.Append($" AND A.DataCadastro <= CONVERT(DATETIME, '{this.txtAte.Text} 23:59:59', 103)");
            }

            if (!string.IsNullOrEmpty(this.txtRUC.Text))
            {
                filtro.Append($" AND A.RUC LIKE '%{this.txtRUC.Text}%' ");
            }

            if (!string.IsNullOrEmpty(this.txtChaveDUE.Text))
            {
                filtro.Append($" AND A.ChaveAcesso LIKE '%{this.txtChaveDUE.Text}%' ");
            }

            if (this.cbStatus.SelectedValue != null)
            {
                if (this.cbStatus.SelectedValue != string.Empty)
                {
                    filtro.Append($" AND A.EnviadoSiscomex = {this.cbStatus.SelectedValue}");
                }
            }

            this.gvDUE.DataSource = dueDAO.ObterDUEs(filtro.ToString(), Convert.ToInt32(Session["UsuarioId"].ToString()));
            this.gvDUE.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            ConsultarDUEs();
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDUE.Text = string.Empty;
            txtDe.Text = string.Empty;
            txtAte.Text = string.Empty;
            txtRUC.Text = string.Empty;
            cbStatus.SelectedIndex = -1;
            txtChaveDUE.Text = string.Empty;
        }

        protected async void gvDUE_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (this.gvDUE.Rows.Count == 0)
                return;

            if (e.CommandName == "EXCLUIR")
            {
                var index = Convert.ToInt32(e.CommandArgument);

                var dueId = this.gvDUE.DataKeys[index]["Id"].ToString().ToInt();

                var due = dueDAO.ObterDUEPorId(dueId);

                if (due != null)
                {
                    dueDAO.ExcluirDUE(dueId);
                    ConsultarDUEs();
                }
            }

            if (e.CommandName == "GerarXML")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                var arquivo = new GerarXML();
                arquivo.DUEId = this.gvDUE.DataKeys[index]["Id"].ToString().ToInt();
                arquivo.Validar(ModelState);
                if (!ModelState.IsValid)
                    return;
                arquivo.Gerar();
            }

            if (e.CommandName == "ValidarXML")
            {
                var index = Convert.ToInt32(e.CommandArgument);

                await ValidarXMLAsync(this.gvDUE.DataKeys[index]["Id"].ToString().ToInt());
            }

            if (e.CommandName == "EnviarSiscomex")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                var arquivo = new GerarXML();
                arquivo.DUEId = this.gvDUE.DataKeys[index]["Id"].ToString().ToInt();

                arquivo.Retificar(ModelState);
            }
        }

        protected void gvDUE_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            this.gvDUE.PageIndex = e.NewPageIndex;
            ConsultarDUEs();
        }

        private async System.Threading.Tasks.Task ValidarXMLAsync(int id)
        {
            var arquivo = new GerarXML();
            arquivo.DUEId = id;
            arquivo.Validar(ModelState);
            if (!ModelState.IsValid)
                return;
            arquivo.Gerar();
            var validarXML = new ValidarXML();

            var xml = dueDAO.ObterUltimoXMLGerado(arquivo.DUEId);
            XmlDocument documentoXML = new XmlDocument();
            documentoXML.LoadXml(xml);

            var retorno = await validarXML.Validar(documentoXML);

            ViewState["RetornoSucesso"] = retorno.Sucesso;

            dueDAO.AtualizarXMLRetorno(id, retorno.XmlRetorno);

            alertaValidacao.Visible = retorno.Sucesso;

            if (retorno.Mensagens.Count > 0 & retorno.XmlRetorno != null)
            {
                alertaValidacao.Visible = true;
                ViewState["RetornoSucesso"] = null;
            }

            if (!retorno.Sucesso & retorno.XmlRetorno == null)
            {
                foreach (var erro in retorno.Mensagens)
                    ModelState.AddModelError(string.Empty, erro);
            }
            else if (!retorno.Sucesso & retorno.XmlRetorno != null)
            {
                //Response.Redirect("ArquivoXML.aspx?id=" + id + "&dest=SALVAR&tipo=RETORNO", false);
                StringBuilder conteudo = new StringBuilder();
                foreach (var erro in retorno.Mensagens)
                    conteudo.AppendLine(erro);

                SalvarTXT(arquivo.DUE.DUE, conteudo);
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
    }
}