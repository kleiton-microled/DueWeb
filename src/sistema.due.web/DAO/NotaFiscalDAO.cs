using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.DTO;
using System;

namespace Sistema.DUE.Web.DAO
{
    public class NotaFiscalDAO
    {
        public int Cadastrar(NotaFiscal notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "TipoNF", value: notaFiscal.TipoNF, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: notaFiscal.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroNF", value: notaFiscal.NumeroNF, direction: ParameterDirection.Input);
                parametros.Add(name: "CNPJNF", value: notaFiscal.CnpjNF, direction: ParameterDirection.Input);
                parametros.Add(name: "QuantidadeNF", value: notaFiscal.QuantidadeNF, direction: ParameterDirection.Input);
                parametros.Add(name: "UnidadeNF", value: notaFiscal.UnidadeNF, direction: ParameterDirection.Input);
                parametros.Add(name: "NCM", value: notaFiscal.NCM, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNFReferencia", value: notaFiscal.ChaveNFReferencia, direction: ParameterDirection.Input);
                parametros.Add(name: "Arquivo", value: notaFiscal.Arquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "Empresa", value: notaFiscal.Empresa, direction: ParameterDirection.Input);
                parametros.Add(name: "Filial", value: notaFiscal.Filial, direction: ParameterDirection.Input);
                parametros.Add(name: "Memorando", value: notaFiscal.Memorando, direction: ParameterDirection.Input);
                parametros.Add(name: "DataEmissao", value: notaFiscal.DataNF, direction: ParameterDirection.Input);
                parametros.Add(name: "OBS", value: notaFiscal.OBS, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: notaFiscal.DueId, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: notaFiscal.Usuario, direction: ParameterDirection.Input);
                parametros.Add(name: "VMLE", value: notaFiscal.VMLE, direction: ParameterDirection.Input);
                parametros.Add(name: "VMCV", value: notaFiscal.VMCV, direction: ParameterDirection.Input);
                parametros.Add(name: "Enquadramento", value: notaFiscal.Enquadramento, direction: ParameterDirection.Input);
                parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                return con.Query<int>(@"INSERT INTO [dbo].[TB_DUE_NF] ([TipoNF],[ChaveNF],[Item],[NumeroNF],[CNPJNF],[QuantidadeNF],[UnidadeNF],[NCM],[ChaveNFReferencia],[Arquivo],[Empresa],[Filial],[Memorando],[DataEmissao],[OBS],[DueId],[Usuario],[VMLE],[VMCV],[Enquadramento]) VALUES (@TipoNF, @ChaveNF, @Item, @NumeroNF, @CNPJNF, @QuantidadeNF, @UnidadeNF, @NCM, @ChaveNFReferencia, @Arquivo, @Empresa, @Filial, @Memorando, @DataEmissao, @OBS, @DueId, @Usuario, @VMLE, @VMCV, @Enquadramento); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
            }
        }

        public void AtualizarIdDUE(int id, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                con.Execute(@"UPDATE [dbo].[TB_DUE_NF] SET [DueId] = @DueId WHERE Id = @Id", parametros);
            }
        }

        public bool ExisteNotaFiscalPorNomeArquivo(string nomeArquivo)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Arquivo", value: nomeArquivo.Trim(), direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT ChaveNF FROM [dbo].[TB_DUE_NF] WHERE Arquivo = @Arquivo", parametros).Any();
            }
        }
      
        public void ExcluirNotasReferenciadas(NotaFiscal notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: notaFiscal.Usuario, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE TipoNF <> 'EXP' AND ChaveNFReferencia = @ChaveNF AND ISNULL(DueId, 0) = 0", parametros);
            }
        }

        public int ExisteNotaFiscal(NotaFiscal notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoNF", value: notaFiscal.TipoNF, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroNF", value: notaFiscal.NumeroNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Arquivo", value: notaFiscal.Arquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: notaFiscal.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNFReferencia", value: notaFiscal.ChaveNFReferencia, direction: ParameterDirection.Input);

                if (!string.IsNullOrWhiteSpace(notaFiscal.ChaveNFReferencia))
                {
                    return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND ChaveNFReferencia = @ChaveNFReferencia AND Item = @Item AND ISNULL(DueId, 0) = 0", parametros).FirstOrDefault();
                }

                return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND ISNULL(DueId, 0) = 0", parametros).FirstOrDefault();
            }
        }

        public int ExisteNotaFiscalNaDUE(NotaFiscal notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoNF", value: notaFiscal.TipoNF, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroNF", value: notaFiscal.NumeroNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Arquivo", value: notaFiscal.Arquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: notaFiscal.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNFReferencia", value: notaFiscal.ChaveNFReferencia, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: notaFiscal.DueId, direction: ParameterDirection.Input);

                if (!string.IsNullOrWhiteSpace(notaFiscal.ChaveNFReferencia))
                {
                    return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND ChaveNFReferencia = @ChaveNFReferencia AND Item = @Item AND DueId = @DueId", parametros).FirstOrDefault();
                }

                return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND DueId = @DueId", parametros).FirstOrDefault();
            }
        }

        public int ExisteNotaFiscalNaDUEFromXML(NotaFiscal notaFiscal)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "TipoNF", value: notaFiscal.TipoNF, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroNF", value: notaFiscal.NumeroNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Arquivo", value: notaFiscal.Arquivo, direction: ParameterDirection.Input);
                parametros.Add(name: "Item", value: notaFiscal.Item, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveNFReferencia", value: notaFiscal.ChaveNFReferencia, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: notaFiscal.DueId, direction: ParameterDirection.Input);

                if (!string.IsNullOrWhiteSpace(notaFiscal.ChaveNFReferencia))
                {
                    return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND ChaveNFReferencia = @ChaveNFReferencia AND Item = @Item AND DueId = @DueId", parametros).FirstOrDefault();
                }

                return con.Query<int>(@"SELECT Id FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = @TipoNF AND NumeroNF = @NumeroNF AND Item = @Item AND DueId = @DueId", parametros).FirstOrDefault();
            }
        }

        public void ExcluirNotaFiscal(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE Id = @Id", parametros);
            }
        }

        public void ExcluirNotaFiscalPorChaveEDUE(int dueId, string chaveExp)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);
                parametros.Add(name: "ChaveExp", value: chaveExp, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE DueId = @DueId AND ChaveNF = @ChaveExp", parametros);
                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE DueId = @DueId AND ChaveNFReferencia = @ChaveExp", parametros);
            }
        }

        public NotaFiscal ObterNotaFiscalPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT Id, TipoNF, Item, ChaveNF, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo, Usuario, DueId FROM [dbo].[TB_DUE_NF] WHERE Id = @Id", parametros).FirstOrDefault();
            }
        }

        public void ExcluirNotaFiscalPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                con.Open();

                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                var nota = ObterNotaFiscalPorId(id);

                using (var transaction = con.BeginTransaction())
                {
                    if (nota != null)
                    {
                        parametros.Add(name: "ChaveNF", value: nota.ChaveNF, direction: ParameterDirection.Input);

                        con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE ChaveNFReferencia = @ChaveNF AND ISNULL(DueId, 0) = 0", parametros, transaction);
                        con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND ISNULL(DueId, 0) = 0", parametros, transaction);

                        transaction.Commit();
                    }
                }
            }
        }

        public void ExcluirNotasFiscaisPorDueId(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE DueId = @DueId", parametros);
            }
        }

        public void ExcluirNotaFiscaiDuePorId(int notaId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "NotaId", value: notaId, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TB_DUE_NF] WHERE Id = @NotaId", parametros);
            }
        }

        public NotaFiscal ObterNotasFiscalPorChave(string chaveNF, int usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT Id, TipoNF, Item, ChaveNF, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND ISNULL(DueId, 0) = 0 AND (Usuario = @Usuario OR Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario))", parametros).FirstOrDefault();
            }
        }

        public NotaFiscal ObterNotasExportacaoPorChave(string chaveNF)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT Id, TipoNF, Item, ChaveNF, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF ", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasFiscaisExportacaoSemVinculo(int usuario, string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);
                parametros.Add(name: "Filtro", value: "%" + filtro + "%", direction: ParameterDirection.Input);

                var filtroSQL = string.Empty;

                if (!string.IsNullOrEmpty(filtro))
                    filtroSQL = " AND A.ChaveNF LIKE @Filtro ";

                return con.Query<NotaFiscal>($@"
                        SELECT 
                            A.Id, 
                            A.TipoNF, 
                            A.Item,
                            A.ChaveNF, 
                            A.NumeroNF, 
                            A.CNPJNF, 
                            A.QuantidadeNF, 
                            A.UnidadeNF, 
                            A.NCM, 
                            A.ChaveNFReferencia, 
                            A.Arquivo,
                            B.Login,
                            A.DataCadastro As DataNF
                        FROM 
                            [dbo].[TB_DUE_NF] A
                        LEFT JOIN
                            [dbo].[TB_DUE_USUARIOS] B ON A.Usuario = B.Id
                        WHERE 
                            A.TipoNF = 'EXP' 
                        AND 
                            A.ChaveNF NOT IN (SELECT ISNULL(NF, ' ') FROM [dbo].[Tb_DUE_Item])                         
                        AND 
                            (A.Usuario = @Usuario OR A.Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario)) {filtroSQL}
                        AND 
                            ISNULL(DueId, 0) = 0
                        ORDER BY 
                            A.NumeroNF", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasFiscaisExportacaoComDUE(string filtro, int usuario)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Usuario", value: usuario, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(string.Format(@"
                    SELECT 
	                    DISTINCT
		                    A.Id, 
		                    A.TipoNF, 
                            A.Item,
		                    A.ChaveNF, 
		                    A.NumeroNF, 
		                    A.CNPJNF, 
		                    A.QuantidadeNF, 
		                    A.UnidadeNF, 
		                    A.NCM, 
		                    A.ChaveNFReferencia, 
		                    A.Arquivo,
                            B.Login,
                            A.DataCadastro AS DataNF,
		                    (SELECT Due FROM TB_DUE_ITEM Item INNER JOIN TB_DUE Due ON Item.DueId = due.Id WHERE A.ChaveNF = Item.NF AND Due.DUE IS NOT NULL AND ISNULL(Item.DueId, 0) = 0) DUE
                    FROM
                        [dbo].[TB_DUE_NF] A
                    LEFT JOIN
                        [dbo].[TB_DUE_Usuarios] B ON A.Usuario = B.Id
                    WHERE 
	                    A.TipoNF = 'EXP' {0}
                    AND
                        (A.Usuario = @Usuario OR A.Usuario in (SELECT IdUsuario FROM [dbo].[Tb_usuario_vinculado] WHERE IDUsuarioVinculado = @Usuario))
                    AND 
                        ISNULL(A.DueId, 0) = 0
                    ORDER BY 
	                    A.NumeroNF", filtro), parametros);
            }
        }

        public DataTable ListagemMemorando(string filtro)
        {
            DataSet Ds = new DataSet();

            using (SqlConnection Con = new SqlConnection(Banco.StringConexao()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.CommandTimeout = 4800;

                    Cmd.Connection = Con;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = string.Format(@"
                            SELECT 
	                            DISTINCT
		                            A.Empresa, 
		                            A.Filial, 
		                            A.Memorando, 
		                            C.DUE, 
		                            C.ChaveAcesso, 
		                            LTRIM(A.NumeroNF) NumeroNF, 
		                            Convert(Varchar, A.DataEmissao, 112) DataEmissao,
		                            A.QuantidadeNF, 
		                            A.ChaveNF,
                                    A.CNPJNF
                            FROM 
	                            [dbo].[TB_DUE_NF] A
                            LEFT JOIN
	                            [dbo].[Tb_DUE_Item] B ON A.ChaveNF = B.NF
                            LEFT JOIN
	                            [dbo].[TB_DUE] C ON B.DueId = C.Id
                            WHERE
                                A.Id > 0 AND A.TipoNF = 'EXP' {0}                           
                            ORDER BY 
	                    NumeroNF", filtro);

                    //AND 
                    //ISNULL(A.DueId, 0) = 0

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);
                        return Ds.Tables[0];
                    }
                }
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasFiscaisRemessa(string chaveNF)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveReferencia", value: chaveNF, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT DISTINCT Id, TipoNF, Item, ChaveNF, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo, Usuario FROM [dbo].[TB_DUE_NF] WHERE ChaveNFReferencia = @ChaveReferencia AND (TipoNF = 'REM' OR TipoNF = 'FDL' OR TipoNF = 'NFF') AND ISNULL(DueId, 0) = 0", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasFiscaisPorDue(int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT DISTINCT Id, TipoNF, Item, ChaveNF, NumeroNF, CNPJNF, QuantidadeNF, UnidadeNF, NCM, ChaveNFReferencia, Arquivo, Usuario FROM [dbo].[TB_DUE_NF] WHERE ISNULL(DueId, 0) = @DueId", parametros);
            }
        }

        public IEnumerable<NotaFiscal> ObterNotasFiscaisRemessaDUE(string chaveNF, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveReferencia", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT DISTINCT TipoNF, ChaveNF, Item, NumeroNF, CNPJNF, QuantidadeNF, ChaveNFReferencia FROM [dbo].[TB_DUE_NF] WHERE ChaveNFReferencia = @ChaveReferencia AND (TipoNF = 'REM' OR TipoNF = 'FDL' OR TipoNF = 'NFF') AND DueId = @DueId", parametros);
            }
        }

        public NotaFiscal ObterNotaExportacaoPorChaveEDue(string chaveNF, int dueId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);
                parametros.Add(name: "DueId", value: dueId, direction: ParameterDirection.Input);

                return con.Query<NotaFiscal>(@"SELECT Id, TipoNF, ChaveNF, Item, NumeroNF, CNPJNF, QuantidadeNF, ChaveNFReferencia FROM [dbo].[TB_DUE_NF] WHERE ChaveNF = @ChaveNF AND TipoNF = 'EXP' AND DueId = @DueId", parametros).FirstOrDefault();
            }
        }

        public int CadastrarNFImportacaoConsulta(NotaFiscalConsultaCCT notaFiscal, string guid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
                {

                    var parametros = new DynamicParameters();
                    DateTime dtx = notaFiscal.DataRegistro.ToDateTime();
                    if (dtx > Convert.ToDateTime("2000-01-01 00:00:00"))
                    {
                        parametros.Add(name: "DataRegistro", value: notaFiscal.DataRegistro.ToDateTime(), direction: ParameterDirection.Input);
                    }
                    else
                    {
                        dtx = System.DateTime.Now;
                        parametros.Add(name: "DataRegistro", value: dtx, direction: ParameterDirection.Input);
                    }
                    parametros.Add(name: "ChaveNF", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                    parametros.Add(name: "SaldoCCT", value: notaFiscal.SaldoCCT, direction: ParameterDirection.Input);
                    parametros.Add(name: "PesoEntradaCCT", value: notaFiscal.PesoEntradaCCT, direction: ParameterDirection.Input);
                    parametros.Add(name: "PesoAferido", value: notaFiscal.PesoAferido, direction: ParameterDirection.Input);
                    parametros.Add(name: "OBS", value: notaFiscal.OBS, direction: ParameterDirection.Input);
                    parametros.Add(name: "Recinto", value: notaFiscal.Recinto, direction: ParameterDirection.Input);
                    parametros.Add(name: "UnidadeNF", value: notaFiscal.UnidadeReceita, direction: ParameterDirection.Input);
                    parametros.Add(name: "Item", value: notaFiscal.Item, direction: ParameterDirection.Input);
                    parametros.Add(name: "Due", value: notaFiscal.DUE, direction: ParameterDirection.Input);
                    parametros.Add(name: "QtdeAverbada", value: notaFiscal.QtdeAverbada, direction: ParameterDirection.Input);
                    parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);
                    parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    return con.Query<int>(@"INSERT INTO [dbo].[TEMP_CONS_CCT] (REGISTRO, CHAVE, SALDO_CCT, PESO_ENTRADA_CCT, PESO_AFERIDO, OBS,RECINTO, UNIDADE, ITEM, DUE, QTDE_AVERBADA, SESSAO_HASH) VALUES (@DataRegistro, @ChaveNF, @SaldoCCT, @PesoEntradaCCT, @PesoAferido, @OBS, @Recinto, @UnidadeNF, @Item, @Due, @QtdeAverbada, @GUID); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
                }
            }
            catch (SqlException ex)
            {

                throw;
            }


        }
        public void ExcluiNFImportacaoConsulta(string guid)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);

                con.Execute(@"DELETE FROM [dbo].[TEMP_CONS_CCT] WHERE SESSAO_HASH = @GUID", parametros);
            }
        }

        public int ObterTotalNFsImportadasPorGUID(string guid)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT count(*) Total FROM [dbo].[TEMP_CONS_CCT] WHERE SESSAO_HASH = @GUID ", parametros).FirstOrDefault();
            }
        }

        public IEnumerable<ConsultaNfs> ObterNFsImportadasPorGUID(string guid)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);

                return con.Query<ConsultaNfs>($@"
                        SELECT 
                            REGISTRO as DataRegistro,
                            CHAVE as ChaveNF,
                            SALDO_CCT as SaldoCCT,
                            PESO_ENTRADA_CCT as PesoEntradaCCT,
                            PESO_AFERIDO as PesoAferido,
                            OBS as Observacoes,
                            RECINTO,
                            UNIDADE UnidadeReceita,
                            CASE WHEN ITEM = 0 THEN NULL ELSE ITEM END ITEM,
                            DUE,
                            QTDE_AVERBADA As QtdeAverbada
                        FROM 
                            TEMP_CONS_CCT 
                        WHERE 
                            SESSAO_HASH = @GUID
                        ORDER BY 
                            CHAVE", parametros);
            }
        }
        public IEnumerable<NFRemessaPosACD> ObterNFsConsultaRemessaPosACDPorGUID(string guid, int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);

                return con.Query<NFRemessaPosACD>(@"SELECT CAMPOS_REMESSA as CamposRemessa FROM [dbo].[TEMP_CONS_NF_REMESSA_POS_ACD] WHERE SESSAO_HASH = @GUID and UsuarioId = @UsuarioId and DadosSISCOMEX = 0 ", parametros);
            }
        }

        public IEnumerable<NFRemessaPosACD> ObterNFsConsultaRemessaPosACDPorGUIDSISCOMEX(string guid, int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);

                return con.Query<NFRemessaPosACD>(@"SELECT CAMPOS_REMESSA as CamposRemessa, DadosSISCOMEX FROM [dbo].[TEMP_CONS_NF_REMESSA_POS_ACD] WHERE SESSAO_HASH = @GUID and UsuarioId = @UsuarioId and DadosSISCOMEX = 1", parametros);
            }
        }
        public int ObterTotalNFsConsultaRemessaPosACDPorGUID(string guid, int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "GUID", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);

                return con.Query<int>(@"SELECT count(*) Total FROM [dbo].[TEMP_CONS_NF_REMESSA_POS_ACD] WHERE SESSAO_HASH = @GUID and UsuarioId = @UsuarioId ", parametros).FirstOrDefault();
            }
        }

        public string ObterCodigoUFNF(string sigla)
        {            
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Sigla", value: sigla, direction: ParameterDirection.Input);

                return con.Query<string>(@"SELECT Id FROM [dbo].[TB_NF_UF] WHERE Sigla = @Sigla", parametros).FirstOrDefault();
            }
        }

        public DateTime? ObterDataEmissaoPreACD(int numero, string modelo, int serie, string notaFiscalUF, string notaEmitenteCnpj)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Numero", value: numero, direction: ParameterDirection.Input);
                parametros.Add(name: "Modelo", value: modelo, direction: ParameterDirection.Input);
                parametros.Add(name: "Serie", value: serie, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaFiscalUF", value: notaFiscalUF, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaEmitenteCnpj", value: notaEmitenteCnpj, direction: ParameterDirection.Input);

                var emissao = con.Query<DateTime>(@"SELECT TOP 1 DataEmissaoNF FROM TbConsultaEstoquePreACD 
                        WHERE NumeroDocumento = @Numero AND NotaFiscalModelo = @Modelo AND NotaFiscalSerie = @Serie 
                            and NotaFiscalUf = @NotaFiscalUF and NotaFiscalEmitenteIdentificacao = @NotaEmitenteCnpj", parametros).FirstOrDefault();

                if (emissao == null)
                {
                    return con.Query<DateTime>(@"SELECT TOP 1 DataEmissaoNF FROM TbConsultaEstoquePreACD_historico 
                        WHERE NumeroDocumento = @Numero AND NotaFiscalModelo = @Modelo AND NotaFiscalSerie = @Serie 
                            and NotaFiscalUf = @NotaFiscalUF and NotaFiscalEmitenteIdentificacao = @NotaEmitenteCnpj", parametros).FirstOrDefault();
                }

                return emissao;
            }
        }

        public string ObterUnidadePreACD(int numero, string modelo, int serie, string notaFiscalUF, string notaEmitenteCnpj)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Numero", value: numero, direction: ParameterDirection.Input);
                parametros.Add(name: "Modelo", value: modelo, direction: ParameterDirection.Input);
                parametros.Add(name: "Serie", value: serie, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaFiscalUF", value: notaFiscalUF, direction: ParameterDirection.Input);
                parametros.Add(name: "NotaEmitenteCnpj", value: notaEmitenteCnpj, direction: ParameterDirection.Input);

                var emissao = con.Query<string>(@"SELECT TOP 1 UnidadeEstatistica FROM TbConsultaEstoquePreACD 
                        WHERE NumeroDocumento = @Numero AND NotaFiscalModelo = @Modelo AND NotaFiscalSerie = @Serie 
                            and NotaFiscalUf = @NotaFiscalUF and NotaFiscalEmitenteIdentificacao = @NotaEmitenteCnpj", parametros).FirstOrDefault();

                if (emissao == null)
                {
                    return con.Query<string>(@"SELECT TOP 1 UnidadeEstatistica FROM TbConsultaEstoquePreACD_historico 
                        WHERE NumeroDocumento = @Numero AND NotaFiscalModelo = @Modelo AND NotaFiscalSerie = @Serie 
                            and NotaFiscalUf = @NotaFiscalUF and NotaFiscalEmitenteIdentificacao = @NotaEmitenteCnpj", parametros).FirstOrDefault();
                }

                return emissao;
            }
        }

        public int CadastrarNFPosADC(NFRemessaPosACD notaFiscal)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
                {

                    var parametros = new DynamicParameters();

                    parametros.Add(name: "GUID", value: notaFiscal.SessaoHash, direction: ParameterDirection.Input);
                    parametros.Add(name: "CamposRemessa", value: notaFiscal.CamposRemessa, direction: ParameterDirection.Input);
                    parametros.Add(name: "UsuarioId", value: notaFiscal.UsuarioId, direction: ParameterDirection.Input);
                    parametros.Add(name: "DadosSISCOMEX", value: notaFiscal.DadosSISCOMEX, direction: ParameterDirection.Input);

                    parametros.Add(name: "DataDUE", value: notaFiscal.DataDUE, direction: ParameterDirection.Input);
                    parametros.Add(name: "DUE", value: notaFiscal.DUE, direction: ParameterDirection.Input);
                    parametros.Add(name: "ChaveDUE", value: notaFiscal.ChaveDUE, direction: ParameterDirection.Input);
                    parametros.Add(name: "DeclaranteCnpj", value: notaFiscal.DeclaranteCnpj, direction: ParameterDirection.Input);
                    parametros.Add(name: "DeclaranteNome", value: notaFiscal.DeclaranteNome, direction: ParameterDirection.Input);
                    parametros.Add(name: "UltimoEvento", value: notaFiscal.UltimoEvento, direction: ParameterDirection.Input);
                    parametros.Add(name: "DataUltimoEvento", value: notaFiscal.DataUltimoEvento, direction: ParameterDirection.Input);
                    parametros.Add(name: "DataAverbacao", value: notaFiscal.DataAverbacao, direction: ParameterDirection.Input);
                    parametros.Add(name: "ItemDUE", value: notaFiscal.ItemDUE, direction: ParameterDirection.Input);
                    parametros.Add(name: "NCM", value: notaFiscal.NCM, direction: ParameterDirection.Input);
                    parametros.Add(name: "ItemNF", value: notaFiscal.ItemNF, direction: ParameterDirection.Input);
                    parametros.Add(name: "CFOP", value: notaFiscal.CFOP, direction: ParameterDirection.Input);
                    parametros.Add(name: "DESCRICAO", value: notaFiscal.DESCRICAO, direction: ParameterDirection.Input);
                    parametros.Add(name: "TipoNF", value: notaFiscal.TipoNF, direction: ParameterDirection.Input);
                    parametros.Add(name: "ChaveNf", value: notaFiscal.ChaveNF, direction: ParameterDirection.Input);
                    parametros.Add(name: "NUMERO", value: notaFiscal.NUMERO, direction: ParameterDirection.Input);
                    parametros.Add(name: "MODELO", value: notaFiscal.MODELO, direction: ParameterDirection.Input);
                    parametros.Add(name: "SERIE", value: notaFiscal.SERIE, direction: ParameterDirection.Input);
                    parametros.Add(name: "UF", value: notaFiscal.UF, direction: ParameterDirection.Input);
                    parametros.Add(name: "CNPJEmitente", value: notaFiscal.CNPJEmitente, direction: ParameterDirection.Input);
                    parametros.Add(name: "EventoAverbacao", value: notaFiscal.EventoAverbacao, direction: ParameterDirection.Input);
                    parametros.Add(name: "VMLE", value: notaFiscal.VMLE, direction: ParameterDirection.Input);
                    parametros.Add(name: "ValorEmReais", value: notaFiscal.ValorEmReais, direction: ParameterDirection.Input);
                    parametros.Add(name: "VMCV", value: notaFiscal.VMCV, direction: ParameterDirection.Input);
                    parametros.Add(name: "IMPORTADOR", value: notaFiscal.IMPORTADOR, direction: ParameterDirection.Input);
                    parametros.Add(name: "ImportadorEndereco", value: notaFiscal.ImportadorEndereco, direction: ParameterDirection.Input);
                    parametros.Add(name: "ImportadorPais", value: notaFiscal.ImportadorPais, direction: ParameterDirection.Input);
                    parametros.Add(name: "PaisDestino", value: notaFiscal.PaisDestino, direction: ParameterDirection.Input);
                    parametros.Add(name: "Unidade", value: notaFiscal.Unidade, direction: ParameterDirection.Input);
                    parametros.Add(name: "PesoLiquidoTotal", value: notaFiscal.PesoLiquidoTotal, direction: ParameterDirection.Input);
                    parametros.Add(name: "Moeda", value: notaFiscal.Moeda, direction: ParameterDirection.Input);
                    parametros.Add(name: "INCOTERM", value: notaFiscal.INCOTERM, direction: ParameterDirection.Input);
                    parametros.Add(name: "InformacoesComplementares", value: notaFiscal.InformacoesComplementares, direction: ParameterDirection.Input);
                    parametros.Add(name: "UnidadeRFB", value: notaFiscal.UnidadeRFB, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoUnidadeRFB", value: notaFiscal.DescricaoUnidadeRFB, direction: ParameterDirection.Input);
                    parametros.Add(name: "RecintoDespacho", value: notaFiscal.RecintoDespacho, direction: ParameterDirection.Input);
                    parametros.Add(name: "DescricaoRecintoDespacho", value: notaFiscal.DescricaoRecintoDespacho, direction: ParameterDirection.Input);
                    parametros.Add(name: "QuantidadeNF", value: notaFiscal.QuantidadeNF, direction: ParameterDirection.Input);
                    parametros.Add(name: "QuantidadeAverbada", value: notaFiscal.QuantidadeAverbada, direction: ParameterDirection.Input);

                    parametros.Add(name: "Id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    return con.Query<int>(@"
                        INSERT INTO 
                            [dbo].[TEMP_CONS_NF_REMESSA_POS_ACD] (
                                SESSAO_HASH, 
                                CAMPOS_REMESSA, 
                                UsuarioId,
                                DadosSISCOMEX,
                                DataDUE,
                                DUE,
                                ChaveDUE,
                                DeclaranteCnpj,
                                DeclaranteNome,
                                UltimoEvento,
                                DataUltimoEvento,
                                DataAverbacao,
                                ItemDUE,
                                NCM,
                                ItemNF,
                                CFOP,
                                DESCRICAO,
                                TipoNF,
                                CHAVE_NF,
                                NUMERO,
                                MODELO,
                                SERIE,
                                UF,
                                CNPJ_EMITENTE,
                                EventoAverbacao,
                                VMLE,
                                ValorEmReais,
                                VMCV,
                                IMPORTADOR,
                                ImportadorEndereco,
                                ImportadorPais,
                                PaisDestino,
                                Unidade,
                                PesoLiquidoTotal,
                                Moeda,
                                INCOTERM,
                                InformacoesComplementares,
                                UnidadeRFB,
                                DescricaoUnidadeRFB,
                                RecintoDespacho,
                                DescricaoRecintoDespacho,
                                QuantidadeNF,
                                QuantidadeAverbada
                        ) VALUES (
                                @GUID, 
                                @CamposRemessa, 
                                @UsuarioId,
                                @DadosSISCOMEX,
                                @DataDUE,
                                @DUE,
                                @ChaveDUE,
                                @DeclaranteCnpj,
                                @DeclaranteNome,
                                @UltimoEvento,
                                @DataUltimoEvento,
                                @DataAverbacao,
                                @ItemDUE,
                                @NCM,
                                @ItemNF,
                                @CFOP,
                                @DESCRICAO,
                                @TipoNF,
                                @ChaveNF,
                                @NUMERO,
                                @MODELO,
                                @SERIE,
                                @UF,
                                @CNPJEmitente,
                                @EventoAverbacao,
                                @VMLE,
                                @ValorEmReais,
                                @VMCV,
                                @IMPORTADOR,
                                @ImportadorEndereco,
                                @ImportadorPais,
                                @PaisDestino,
                                @Unidade,
                                @PesoLiquidoTotal,
                                @Moeda,
                                @INCOTERM,
                                @InformacoesComplementares,
                                @UnidadeRFB,
                                @DescricaoUnidadeRFB,
                                @RecintoDespacho,
                                @DescricaoRecintoDespacho,
                                @QuantidadeNF,
                                @QuantidadeAverbada    
                                ); SELECT CAST(SCOPE_IDENTITY() AS INT)", parametros).Single();
                }
            }
            catch (SqlException ex)
            {

                throw;
            }


        }
    }
}