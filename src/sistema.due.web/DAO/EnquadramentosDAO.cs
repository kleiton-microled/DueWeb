using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class EnquadramentosDAO
    {      
        public IEnumerable<Enquadramento> ObterEnquadramentos()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Enquadramento>(@"SELECT Codigo, CONVERT(VARCHAR, Codigo) + ' - ' + Descricao As Descricao FROM [dbo].[Tb_Enquadramentos] ORDER BY Descricao");
            }
        }
    }
}