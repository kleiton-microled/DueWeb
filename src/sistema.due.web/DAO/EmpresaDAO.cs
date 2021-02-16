using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using Sistema.DUE.Web.Config;

namespace Sistema.DUE.Web.DAO
{
    public class EmpresaDAO
    {      
        public IEnumerable<string> ObterEmpresasMemorando()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<string>(@"SELECT DISTINCT EMPRESA FROM TB_DUE_NF WHERE EMPRESA IS NOT NULL");
            }
        }
    }
}