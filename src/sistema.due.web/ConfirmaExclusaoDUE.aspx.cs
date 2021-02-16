using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using System;

namespace Sistema.DUE.Web
{
    public partial class ConfirmaExclusaoDUE : System.Web.UI.Page
    {
        private readonly DocumentoUnicoExportacaoDAO dueDAO = new DocumentoUnicoExportacaoDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnExcluirDUE_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"].ToString().ToInt();

            var due = dueDAO.ObterDUEPorId(id);

            if (due != null)
            {
                dueDAO.ExcluirDUE(id);
                Response.Redirect("ConsultarDUE.aspx");
            }
        }
    }
}