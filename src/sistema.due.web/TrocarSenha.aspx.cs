using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Helpers;
using System;

namespace Sistema.DUE.Web
{
    public partial class TrocarSenha : System.Web.UI.Page
    {
        private readonly UsuarioDAO _usuarioDAO = new UsuarioDAO();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTrocarSenha_Click(object sender, EventArgs e)
        {
            _usuarioDAO.TrocarSenha(Criptografia.EcriptarSenha(this.txtNovaSenha.Text), Convert.ToInt32(Session["UsuarioId"].ToString()));

            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}