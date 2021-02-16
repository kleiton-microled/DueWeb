using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace Sistema.DUE.Web.DAO
{
    public class SituacaoDUEDAO
    {
        public IEnumerable<SituacaoDUE> ObterSituacoesDUE()
        {
            MemoryCache cache = MemoryCache.Default;

            var situacoes = cache["SituacaoDUE.ObterSituacoesDUE"] as IEnumerable<SituacaoDUE>;

            if (situacoes == null)
            {
                using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
                {
                    situacoes = con.Query<SituacaoDUE>(@"SELECT [Id], [Descricao] FROM [dbo].[Tb_Situacao_DUE]");
                }

                cache["SituacaoDUE.ObterSituacoesDUE"] = situacoes;
            }

            return situacoes;
        }
    }
}