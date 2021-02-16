using Sistema.DUE.Web.DAO;
using System;
using System.Web.UI;

namespace Sistema.DUE.Web
{
    public partial class ConsultarDUESemVinculoNF : System.Web.UI.Page
    {
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacao = new DocumentoUnicoExportacaoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.gvDUE.DataSource = _documentoUnicoExportacao.ObterDUESSemVinculoNotaFiscal(Convert.ToInt32(Session["UsuarioId"].ToString()));
                this.gvDUE.DataBind();
            }
        }      
    }
}