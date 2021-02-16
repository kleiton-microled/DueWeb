using System;
using System.Configuration;

namespace Sistema.DUE.Web
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            this.lblRazaoEmpresa.Text = Properties.Settings.Default.RazaoEmpresa;
            this.lblTitulo.InnerText = Properties.Settings.Default.RazaoEmpresa;
        }
    }
}