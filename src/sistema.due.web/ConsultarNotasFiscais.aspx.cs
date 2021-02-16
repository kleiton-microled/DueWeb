using Sistema.DUE.Web.DAO;
using System;
using System.Text;
using System.Web.UI;

namespace Sistema.DUE.Web
{
    public partial class ConsultarNotasFiscais : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
     
        }

        protected void ListarNotasFiscaisExportacao(string filtro)
        {
            this.gvNotasFiscais.DataSource = _notaFiscalDAO.ObterNotasFiscaisExportacaoComDUE(filtro, Convert.ToInt32(Session["UsuarioId"].ToString()));
            this.gvNotasFiscais.DataBind();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtData.Text))
            {
                filtro.Append(" AND A.DataCadastro = CONVERT(DATETIME, '" + this.txtData.Text + "', 103)");
            }

            if (!string.IsNullOrEmpty(this.txtNumeroNF.Text))
            {
                filtro.Append(" AND A.NumeroNF = '" + this.txtNumeroNF.Text + "'");
            }

            if (!string.IsNullOrEmpty(this.txtChave.Text))
            {
                filtro.Append(" AND A.ChaveNF = '" + this.txtChave.Text + "'");
            }

            ListarNotasFiscaisExportacao(filtro.ToString());
        }
    }
}