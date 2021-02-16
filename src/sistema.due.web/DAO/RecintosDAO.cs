using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using System.Runtime.Caching;

namespace Sistema.DUE.Web.DAO
{
    public class RecintosDAO
    {
        public IEnumerable<Recinto> ObterRecintos(int unidadeRFB)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                return con.Query<Recinto>(@"SELECT Id, CONVERT(VARCHAR, Id) + ' - ' + Descricao As Descricao FROM [dbo].[Tb_Recintos] Where CodigoUnidadeReceita = @unidadeRFB", new { unidadeRFB });
            }
        }

        public Recinto ObterRecintoPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<Recinto>(@"SELECT Id, CONVERT(VARCHAR, Id) + ' - ' + Descricao As Descricao FROM [dbo].[Tb_Recintos] Where Id = @Id", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<Recinto> ObterRecintos()
        {
            MemoryCache cache = MemoryCache.Default;

            var recintos = cache["Recinto.ObterRecintos"] as IEnumerable<Recinto>;

            if (recintos == null)
            {
                using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
                {
                    recintos = con.Query<Recinto>(@"SELECT Id, CONVERT(VARCHAR, Id) + ' - ' + Descricao As Descricao, CodigoUnidadeReceita FROM [dbo].[Tb_Recintos]");
                }

                cache["Recinto.ObterRecintos"] = recintos;
            }

            return recintos;
        }
    }
}