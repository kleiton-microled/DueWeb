using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Helpers;
using System;

namespace Sistema.DUE.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private UsuarioDAO _usuarioDAO = new UsuarioDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] != null)
                Response.Redirect("Default.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUsuario.Text))
                ModelState.AddModelError(string.Empty, "O campo Usuário é obrigatório");

            if (string.IsNullOrEmpty(this.txtSenha.Text))
                ModelState.AddModelError(string.Empty, "O campo Senha é obrigatório");

            if (!ModelState.IsValid)
                return;

            var usuario = _usuarioDAO.ObterUsuarioPorLogin(this.txtUsuario.Text);

            if (usuario != null)
            {
                if (usuario.FlagAtivo == false)
                {
                    ModelState.AddModelError(string.Empty, "Usuário inativo (pendente liberação)");
                    return;
                }

                if (usuario.Autenticar(this.txtUsuario.Text.Trim(), Criptografia.EcriptarSenha(this.txtSenha.Text.Trim())))
                {
                    Session["Logado"] = true;
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioLogin"] = usuario.Login;
                    Session["FlagAdmin"] = usuario.FlagAdmin;

                    _usuarioDAO.AtualizarSenhaTemporaria(string.Empty, usuario.Id);

                    if (!string.IsNullOrEmpty(usuario.SenhaTemporaria))
                    {
                        Session["Redefiniu"] = true;
                        Response.Redirect("TrocarSenha.aspx");
                    }

                    Response.Redirect("Default.aspx");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário / Senha inválidos");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário não registrado");
            }
        }
    }
}