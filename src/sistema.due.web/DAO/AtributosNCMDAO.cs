using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;

namespace Sistema.DUE.Web.DAO
{
    public class AtributosNCMDAO
    {
        public IEnumerable<AtributoNCM> ObterAtributos(string campo, string ncm)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Campo", value: campo, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);

                return con.Query<AtributoNCM>(@"SELECT Atributo, Codigo, Descricao FROM Tb_Atributos WHERE Campo = @Campo AND NCM = @NCM", parametros);
            }
        }

        public AtributoNCM ObterAtributo(string campo, string ncm, string codigo)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Campo", value: campo, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);
                parametros.Add(name: "Codigo", value: codigo, direction: ParameterDirection.Input);

                return con.Query<AtributoNCM>(@"SELECT TOP 1 Atributo, Descricao, Codigo FROM Tb_Atributos WHERE Campo = @Campo AND NCM = @NCM AND Codigo = @Codigo", parametros).FirstOrDefault();
            }
        }

        public AtributoNCM ObterAtributo(string campo, string ncm)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Campo", value: campo, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);

                return con.Query<AtributoNCM>(@"SELECT TOP 1 Atributo, Descricao FROM Tb_Atributos WHERE Campo = @Campo AND NCM = @NCM", parametros).FirstOrDefault();
            }
        }

        public string ObterDescricaoAtributo(string campo, string ncm, string atributo)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Campo", value: campo, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: ncm, direction: ParameterDirection.Input);
                parametros.Add(name: "Atributo", value: atributo, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT Descricao FROM Tb_Atributos WHERE Campo = @Campo AND NCM = @NCM AND Atributo = @Atributo", parametros).FirstOrDefault();
            }
        }
    }
}