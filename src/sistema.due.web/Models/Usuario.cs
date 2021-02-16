using Sistema.DUE.Web.Helpers;
using System;

namespace Sistema.DUE.Web.Models
{
    public class Usuario
    {
        public Usuario() { }

        public Usuario(string nome, string cpf, string login, string email, string senha)
        {
            Nome = nome;
            CPF = cpf;
            Login = login;
            Email = email;
            Senha = Criptografia.EcriptarSenha(senha);
        }

        public int Id { get; set; }

        public string Nome { get; set; }

        public string CPF { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string SenhaTemporaria { get; set; }

        public string SenhaConfirmada { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool FlagAdmin { get; set; }

        public bool FlagAtivo { get; set; }

        public bool Autenticar(string login, string senha)
        {
            return this.Login == login && (this.Senha == senha || this.SenhaTemporaria == senha);
        }
    }
}