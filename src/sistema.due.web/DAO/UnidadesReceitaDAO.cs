using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class UnidadesReceitaDAO
    {      
        public IEnumerable<UnidadeReceita> ObterUnidadesRFB()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<UnidadeReceita>(@"SELECT Codigo, Codigo + ' - ' + Descricao As Descricao FROM [dbo].[Tb_Unidades_RFB] ORDER BY Id");
            }
        }

        public UnidadeReceita ObterUnidadesRFBPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<UnidadeReceita>(@"SELECT Codigo, Codigo + ' - ' + Descricao As Descricao FROM [dbo].[Tb_Unidades_RFB] Where Codigo = @Id", parametros).FirstOrDefault();
            }
        }
    }
}