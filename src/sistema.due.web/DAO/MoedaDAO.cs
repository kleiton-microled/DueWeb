using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class MoedaDAO
    {      
        public IEnumerable<Moeda> ObterMoedas()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Moeda>(@"SELECT Id, CodigoISO, Descricao FROM [dbo].[Tb_Moedas] ORDER BY Descricao");
            }
        }
    }
}