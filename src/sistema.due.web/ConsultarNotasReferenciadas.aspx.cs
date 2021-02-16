using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using System;
using System.Linq;
using System.Web.UI;

namespace Sistema.DUE.Web
{
    public partial class ConsultarNotasReferenciadas : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO notaFiscalDAO = new NotaFiscalDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListarNotas();
            }
        }

        protected void ListarNotas()
        {
            if (Request.QueryString["chaveNF"] != null)
            {
                var notasRemessa = notaFiscalDAO.ObterNotasFiscaisRemessa(Request.QueryString["chaveNF"].ToString());

                this.gvNotasFiscaisReferenciadas.DataSource = notasRemessa;
                this.gvNotasFiscaisReferenciadas.DataBind();

                this.lblTotalQuantiade.Text = notasRemessa
                    .Sum(c => c.QuantidadeNF)
                    .ToString("n3");
            }
        }

        protected void gvNotasFiscaisReferenciadas_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EXCLUIR_NF")
            {
                var index = Convert.ToInt32(e.CommandArgument);

                var id = this.gvNotasFiscaisReferenciadas.DataKeys[index]["Id"].ToString().ToInt();                

                if (id > 0)
                {
                    notaFiscalDAO.ExcluirNotaFiscal(id);
                    ListarNotas();
                }
            }
        }
    }
}