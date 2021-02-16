using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class ParametrosDAO
    {      
        public Parametros ObterParametros()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Parametros>(@"SELECT MensagemEmailRedefinicaoSenha, ValidarAtributosCafe, EnvioRetificacaoSemServico FROM [dbo].[TB_Parametros]").FirstOrDefault();
            }
        }
    }
}