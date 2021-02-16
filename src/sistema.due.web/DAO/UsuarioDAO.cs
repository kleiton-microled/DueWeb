using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class UsuarioDAO
    {
        public int Registrar(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<int>(@"INSERT INTO [dbo].[TB_DUE_USUARIOS] ([Login],[Senha],[Nome],[Cpf],[EmpresaId],[Email],[FlagAdmin],[DataCadastro]) VALUES (@Login, @Senha, @Nome, @Cpf, 1, @Email, 1, GETDATE());SELECT CAST(SCOPE_IDENTITY() AS INT)", usuario).Single();
            }
        }

        public int AtualizarUsuario(Usuario usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "FlagAtivo", value: usuario.FlagAtivo, direction: ParameterDirection.Input);
                parametros.Add(name: "FlagAdmin", value: usuario.FlagAdmin, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: usuario.Id, direction: ParameterDirection.Input);

                return con.Execute(@"UPDATE [dbo].[TB_DUE_USUARIOS] SET [FlagAdmin] = @FlagAdmin, [FlagAtivo] = @FlagAtivo WHERE [Id] = @Id", parametros);
            }
        }

        public int AtualizarSenhaTemporaria(string senha, int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "SenhaTemporaria", value: senha, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Execute(@"UPDATE [dbo].[TB_DUE_USUARIOS] SET [SenhaTemporaria] = @SenhaTemporaria WHERE [Id] = @Id", parametros);
            }
        }

        public int TrocarSenha(string novaSenha, int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "NovaSenha", value: novaSenha, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Execute(@"UPDATE [dbo].[TB_DUE_USUARIOS] SET [Senha] = @NovaSenha WHERE [Id] = @Id", parametros);
            }
        }

        public Usuario ObterUsuarioPorCpfOuLogin(string cpf, string login)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Usuario>(@"SELECT * FROM [dbo].[TB_DUE_USUARIOS] WHERE [Cpf] = @uCpf OR [Login] = @uLogin", new { uCpf = cpf, uLogin = login }).FirstOrDefault();
            }
        }

        public Usuario ObterUsuarioPorLogin(string login)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Usuario>(@"SELECT * FROM [dbo].[TB_DUE_USUARIOS] WHERE [Login] = @uLogin", new { uLogin = login }).FirstOrDefault();
            }
        }

        public Usuario ObterUsuarioPorLoginOuEmail(string termo)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Termo", value: termo, direction: ParameterDirection.Input);

                return con.Query<Usuario>(@"SELECT * FROM [dbo].[TB_DUE_USUARIOS] WHERE [Login] = @Termo OR [Email] =  @Termo", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<Usuario> ObterUsuarios()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Usuario>(@"SELECT * FROM [dbo].[TB_DUE_USUARIOS] ORDER BY Id DESC");
            }
        }

        public IEnumerable<Usuario> ObterUsuariosAtivos()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Usuario>(@"SELECT * FROM [dbo].[TB_DUE_USUARIOS] WHERE FlagAtivo = 1 ORDER BY Nome");
            }
        }

        public void CompartilharUsuario(int usuarioId, int[] usuariosVinculados)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                var parametros = new DynamicParameters();

                parametros.Add(name: "IdUsuario", value: usuarioId, direction: ParameterDirection.Input);

                using (var transaction = con.BeginTransaction())
                {
                    con.Execute(@"DELETE FROM [dbo].[TB_USUARIO_VINCULADO] WHERE IdUsuario = @IdUsuario", parametros, transaction);

                    for (int i = 0; i < usuariosVinculados.Length; i++)
                    {
                        parametros.Add(name: "IdUsuarioVinculado", value: usuariosVinculados[i], direction: ParameterDirection.Input);
                        con.Execute(@"INSERT INTO [dbo].[TB_USUARIO_VINCULADO] (IdUsuario, IdUsuarioVinculado) VALUES (@IdUsuario, @IdUsuarioVinculado)", parametros, transaction);
                    }

                    transaction.Commit();
                }
            }
        }

        public List<int> ObterUsuariosVinculados(int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);

                return con.Query<int>(@"select IDusuarioVinculado from Tb_usuario_vinculado WHERE IdUsuario = @UsuarioId ORDER BY IDusuarioVinculado", parametros).ToList();
            }
        }
    }
}