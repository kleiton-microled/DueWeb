using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Services;
using System;

namespace Sistema.DUE.Web
{
    public partial class RecuperarSenha : System.Web.UI.Page
    {
        private UsuarioDAO _usuarioDAO = new UsuarioDAO();
        private ParametrosDAO _parametrosDAO = new ParametrosDAO();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRecuperarSenha_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtUsuario.Text))
                ModelState.AddModelError(string.Empty, "Informe o nome de usuário / email");

            if (!ModelState.IsValid)
                return;

            var usuario = _usuarioDAO.ObterUsuarioPorLoginOuEmail(this.txtUsuario.Text);

            if (usuario != null)
            {               
                try
                {

                    var novaSenha = Guid.NewGuid().ToString().ToLower().Substring(0, 8);

                    var novaSenhaEncriptada = Criptografia.EcriptarSenha(novaSenha);

                    var parametros = _parametrosDAO.ObterParametros();

                    string mensagem = string.Format(parametros.MensagemEmailRedefinicaoSenha, usuario.Nome, novaSenha);

                    _usuarioDAO.AtualizarSenhaTemporaria(novaSenhaEncriptada, usuario.Id);

                    EmailService.EnviarEmail(usuario.Email, usuario.Nome, "DUE / Redefinição de Senha", mensagem);

                    this.lblInfo.Visible = false;
                    this.lblSucessoInfo.Visible = true;
                    this.lblSucessoMsg.Text = $"Um email foi enviado para <strong>{usuario.Email}</strong> com as instruções de redefinição de senha";
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Falha ao enviar o Email");
                }                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Usuário não registrado");
            }
        }
    }
}