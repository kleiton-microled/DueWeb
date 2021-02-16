using Dapper;
using Sistema.DUE.Web.Config;
using System.Data;
using System.Data.SqlClient;

namespace Sistema.DUE.Web.Services
{
    public class LogsService
    {
        public static void Logar(string tela, string mensagem)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Tela", value: tela, direction: ParameterDirection.Input);
                parametros.Add(name: "Mensagem", value: mensagem, direction: ParameterDirection.Input);                

                con.Execute(@"INSERT INTO [dbo].[Tb_Logs] (Tela, Mensagem) VALUES (@Tela, @Mensagem)", parametros);
            }
        }

        public static void LogarEnvioDUE(string tela, string mensagem, string due, string xmlRetorno)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "DUE", value: due, direction: ParameterDirection.Input);
                parametros.Add(name: "XML_RETORNO", value: xmlRetorno, direction: ParameterDirection.Input);

                con.Execute(@"INSERT INTO [dbo].[Tb_Logs] (Tela, Mensagem, DUE, XML_RETORNO) VALUES (@Tela, @Mensagem, @DUE, @XML_RETORNO)", parametros);
            }
        }
    }
}