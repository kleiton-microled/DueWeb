using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Models;
using System;

namespace Sistema.DUE.Web
{
    public partial class Registrar : System.Web.UI.Page
    {
        private UsuarioDAO _usuarioDAO = new UsuarioDAO();

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNome.Text))
                ModelState.AddModelError(string.Empty, "O campo Nome é obrigatório");

            if (string.IsNullOrEmpty(this.txtEmail.Text))
                ModelState.AddModelError(string.Empty, "O campo Email é obrigatório");

            if (string.IsNullOrEmpty(this.txtCPF.Text))
                ModelState.AddModelError(string.Empty, "O campo CPF é obrigatório");

            if (string.IsNullOrEmpty(this.txtLogin.Text))
                ModelState.AddModelError(string.Empty, "O campo Login é obrigatório");

            if (string.IsNullOrEmpty(this.txtSenha.Text))
                ModelState.AddModelError(string.Empty, "O campo Senha é obrigatório");

            if (this.txtSenha.Text != this.txtSenhaConfirmada.Text)
                ModelState.AddModelError(string.Empty, "As senhas não conferem.");

            if (!ValidacaoCPF.Validar(this.txtCPF.Text))
                ModelState.AddModelError(string.Empty, "O CPF informado é inválido.");

            if (!ValidacaoEmail.Validar(this.txtEmail.Text))
                ModelState.AddModelError(string.Empty, "O Email informado é inválido.");

            var usuarioJaRegistrado = _usuarioDAO.ObterUsuarioPorCpfOuLogin(this.txtCPF.Text, this.txtLogin.Text);

            if (usuarioJaRegistrado != null)
                ModelState.AddModelError(string.Empty, "Já existe um usuário com o mesmo CPF/Login");

            if (!ModelState.IsValid)
                return;

            var usuario = new Usuario(
                this.txtNome.Text, 
                this.txtCPF.Text, 
                this.txtLogin.Text, 
                this.txtEmail.Text, 
                this.txtSenha.Text);

            _usuarioDAO.Registrar(usuario);

            Response.Redirect("Login.aspx");

        }
    }
}