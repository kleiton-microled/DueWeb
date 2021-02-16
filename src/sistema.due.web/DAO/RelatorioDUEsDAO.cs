using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sistema.DUE.Web.DAO
{
    public class RelatorioDUEsDAO
    {
        public IEnumerable<DadosRelatorioDUE> ObterRegistrosEstoque(string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {                
                return con.Query<DadosRelatorioDUE>($@"
                    SELECT
						A.NUMERO_DUE As Due,
						C.CNPJ_EXPORTADOR As ExportadorCnpj,
						A.ULTIMO_EVENTO As UltimoEvento,
						A.DATA_DUE As DataDue,
						A.Data_Embarque As DataEmbarque,
						A.DATA_AVERBACAO As DataAverbacao,
						A.Canal,
						C.ITEM,
						C.VMLE,
						A.CPF_REGISTRO_DUE As CpfDue,	
						C.NCM,
						SUM(C.PESO_LIQUIDO_TOTAL) As PesoLiquidoTotal
					FROM
						[TB_CONSULTA_DUE_POS_ACD] A
					INNER JOIN
						[TB_CONS_DUE_PORTAL_SCRAP] B ON A.NUMERO_DUE = B.DUE  COLLATE Latin1_General_CI_AS
					LEFT JOIN
						[TB_CONSULTA_DUE_POS_ACD_ITENS] C ON A.AUTONUM = C.AUTONUM_CONSULTA
					WHERE
						A.AUTONUM > 0 {filtro}
					GROUP BY
						A.NUMERO_DUE,
						C.CNPJ_EXPORTADOR,
						A.ULTIMO_EVENTO,
						A.DATA_DUE,
						A.Data_Embarque,
						A.DATA_AVERBACAO,
						A.Canal,
						C.ITEM,
						C.VMLE,
						A.CPF_REGISTRO_DUE,
						C.NCM
					ORDER BY
						A.DATA_AVERBACAO desc,
						A.NUMERO_DUE,
						C.ITEM");
            }
        }

        public DataTable GerarCsv(string filtro)
        {
            DataSet Ds = new DataSet();

            using (SqlConnection Con = new SqlConnection(Banco.StringConexao()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.CommandTimeout = 4800;

                    Cmd.Connection = Con;
                    Cmd.CommandType = CommandType.Text;

                    Cmd.CommandText = $@"
						SELECT
							A.NUMERO_DUE As Due,
							C.CNPJ_EXPORTADOR As ExportadorCnpj,
							A.ULTIMO_EVENTO As UltimoEvento,
							A.DATA_DUE As DataDue,
							A.Data_Embarque As DataEmbarque,
							A.DATA_AVERBACAO As DataAverbacao,
							A.Canal,
							C.ITEM,
							C.VMLE,
							A.CPF_REGISTRO_DUE As CpfDue,	
							C.NCM,
							SUM(C.PESO_LIQUIDO_TOTAL) As PesoLiquidoTotal
						FROM
							[TB_CONSULTA_DUE_POS_ACD] A
						INNER JOIN
							[TB_CONS_DUE_PORTAL_SCRAP] B ON A.NUMERO_DUE = B.DUE  COLLATE Latin1_General_CI_AS
						LEFT JOIN
							[TB_CONSULTA_DUE_POS_ACD_ITENS] C ON A.AUTONUM = C.AUTONUM_CONSULTA
						WHERE
							A.AUTONUM > 0 {filtro}
						GROUP BY
							A.NUMERO_DUE,
							C.CNPJ_EXPORTADOR,
							A.ULTIMO_EVENTO,
							A.DATA_DUE,
							A.Data_Embarque,
							A.DATA_AVERBACAO,
							A.Canal,
							C.ITEM,
							C.VMLE,
							A.CPF_REGISTRO_DUE,
							C.NCM
						ORDER BY
							A.DATA_AVERBACAO desc,
							A.NUMERO_DUE,
							C.ITEM";

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);
                        return Ds.Tables[0];
                    }
                }
            }
        }
    }
}