using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using System.Collections.Generic;

namespace Sistema.DUE.Web.DAO
{
    public class CertificadoDAO
    {
        public string ObterCpfCertificado(string cnpjExportador)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "CnpjExportador", value: cnpjExportador, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT CPF FROM [dbo].[TB_CNPJ_CPF] Where CNPJ = @CnpjExportador", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<string> ObterCpfs()
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {

                return con.Query<string>(@"SELECT DISTINCT CPF FROM [dbo].[TB_CNPJ_CPF]");
            }
        }
    }
}