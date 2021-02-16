using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using System.Linq;
using Sistema.DUE.Web.Helpers;
using System;
using Sistema.DUE.Web.DTO;

namespace Sistema.DUE.Web.DAO
{
    public class EstoqueDAO
    {
        private readonly string _tabela;

        public EstoqueDAO(bool baseTeste)
        {
            _tabela = baseTeste
                ? "TbConsultaEstoquePreACD_TESTES"
                : "TbConsultaEstoquePreACD";
        }

        public IEnumerable<Estoque> ObterRegistrosEstoque(string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var limite = string.Empty;

                if (string.IsNullOrEmpty(filtro))
                    limite = " TOP 500 ";

                return con.Query<Estoque>($@"
                    SELECT 
                        {limite}
	                    A.[Id]
                        ,A.[NumeroDocumento]
                        ,A.[Tipo]
                        ,CONVERT(VARCHAR, DataHoraEntradaEstoque, 103) + ' ' + CONVERT(VARCHAR, DataHoraEntradaEstoque, 108) As DataHoraEntradaEstoque
                        ,A.[NumeroNF]
                        ,A.[NumeroDUE]                        
                        ,A.[NumeroItem]
                        ,A.[CodigoNCM]
                        ,A.[CodigoURF]
                        ,B.[Descricao] As DescricaoURF
                        ,A.[CodigoRA]
                        ,C.[Descricao] As DescricaoRA
                        ,A.[Latitude]
                        ,A.[Longitude]
                        ,A.[IdResponsavel]
                        ,A.[NomeResponsavel]
                        ,A.[CodigoPaisDestinatario]
                        ,A.[NomePaisDestinatario]
                        ,A.[AnoDeposito]
                        ,A.[SequenciaDeposito]
                        ,A.[ExisteConteiner]
                        ,A.[DataConsulta]
                        ,A.[CnpjDestinatario]
                        ,A.[Item]
                      FROM 
	                     [dbo].[{_tabela}] A
                      LEFT JOIN
                         [dbo].[Tb_Unidades_RFB] B ON A.CodigoURF = B.Codigo
                      LEFT JOIN
                         [dbo].[Tb_Recintos] C ON A.CodigoRA = C.Id 
                     WHERE
                        A.Id > 0 {filtro} ORDER BY A.Id DESC");
            }
        }

        public IEnumerable<EstoqueDetalhes> ObterDetalhesEstoque(int id)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "Id", value: id, direction: ParameterDirection.Input);

                return con.Query<EstoqueDetalhes>($@"
                    SELECT 
	                     [Id]
                        ,[IdentificacaoCondutor]
                        ,[NomeCondutor]
                        ,CONVERT(VARCHAR, DataHoraEntradaEstoque, 103) + ' ' + CONVERT(VARCHAR, DataHoraEntradaEstoque, 108) As DataHoraEntradaEstoque
                        ,[DescricaoAvarias]
                        ,[LocalRaCodigo]
                        ,[LocalRaDescricao]
                        ,[LocalUrfCodigo]
                        ,[LocalUrfDescricao]
                        ,[NcmCodigo]
                        ,[NcmDescricao]
                        ,[NotaFiscalDestinatarioNome]
                        ,[NotaFiscalDestinatarioPais]
                        ,[NotaFiscalEmissao]
                        ,[NotaFiscalEmitenteIdentificacao]
                        ,[NotaFiscalEmitenteNome]
                        ,[NotaFiscalEmitentePais]
                        ,[NotaFiscalModelo]
                        ,[NotaFiscalNumero]
                        ,[NotaFiscalSerie]
                        ,[NotaFiscalUf]
                        ,[NumeroDue]
                        ,[NumeroItem]
                        ,[PaisDestino]
                        ,[PesoAferido]
                        ,[QuantidadeExportada]
                        ,[ResponsavelIdentificacao]
                        ,[ResponsavelNome]
                        ,[ResponsavelPais]
                        ,[Saldo]
                        ,[TransportadorIdentificacao]
                        ,[TransportadorNome]
                        ,[TransportadorPais]
                        ,[UnidadeEstatistica]
                        ,[Valor]
                        ,[CnpjDestinatario]
                        ,[Item]
                    FROM 
	                    [dbo].[{_tabela}]
                    WHERE
	                    [Id] = @Id", parametros);
            }
        }

        public DataTable GerarCsvEstoque(string filtro)
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
	                            DISTINCT
                                   A.[DataEmissaoNF]
                                  ,A.[NumeroNF]		                          
                                  ,A.[NotaFiscalModelo]
                                  ,A.[NotaFiscalSerie]
                                  ,A.[PesoAferido]
                                  ,A.[Saldo]
                                  ,A.[UnidadeEstatistica]
                                  ,A.[DataHoraEntradaEstoque]
                                  ,A.[ResponsavelNome]
                                  ,A.[ResponsavelIdentificacao]
                                  ,A.[Latitude]
                                  ,A.[Longitude]
                                  ,A.[LocalRaCodigo]                                  
                                  ,A.[LocalRaDescricao]
                                  ,A.[NotaFiscalEmitenteIdentificacao]
                                  ,A.[NotaFiscalEmitenteNome]
                                  ,A.[NotaFiscalUf]
                                  ,A.[TransportadorIdentificacao]
                                  ,A.[TransportadorNome]
                                  ,A.[CodigoRA]
                                  ,A.[NumeroDocumento]
                                  ,A.[Tipo]                                  
                                  ,A.[CodigoNCM]
                                  ,A.[CodigoURF]                                                                   
                                  ,A.[IdResponsavel]
                                  ,A.[NomeResponsavel]
                                  ,A.[CodigoPaisDestinatario]
                                  ,A.[NomePaisDestinatario]
                                  ,A.[AnoDeposito]
                                  ,A.[SequenciaDeposito]
                                  ,A.[ExisteConteiner]
                                  ,A.[DataConsulta]
                                  ,A.[IdentificacaoCondutor]
                                  ,A.[NomeCondutor]                                  
                                  ,A.[DescricaoAvarias]
                                  ,A.[LocalUrfCodigo]
                                  ,A.[LocalUrfDescricao]
                                  ,A.[NcmCodigo]
                                  ,A.[NcmDescricao]
                                  ,A.[NotaFiscalDestinatarioNome]
                                  ,A.[NotaFiscalDestinatarioPais]
                                  ,A.[NotaFiscalEmissao]                                                                   
                                  ,A.[NotaFiscalEmitentePais]                                  
                                  ,A.[NotaFiscalNumero]
                                  ,A.[NumeroDue]
                                  ,A.[NumeroItem]
                                  ,A.[PaisDestino]                                  
                                  ,A.[QuantidadeExportada]                                                                   
                                  ,A.[ResponsavelPais]                                                                   
                                  ,A.[TransportadorPais]                                  
                                  ,A.[Valor]
                                  ,A.[CnpjDestinatario]
                                  ,A.[Item]
                                  ,A.[SaldoAtual]
                                  ,A.[Cancelado]
                            FROM 
	                            [dbo].[{_tabela}] A
                            WHERE
                                A.Id > 0 {filtro}";

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);
                        return Ds.Tables[0];
                    }
                }
            }
        }

        public DataTable GerarCsvEstoque2(int pagina, int registrosPorPagina, string filtro, out int totalFiltro)
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
                            WITH Resultado AS
                            ( 
	                            SELECT 
		                             [Id]
		                            ,[NumeroDocumento]
		                            ,[Tipo]
		                            ,[NumeroNF]
		                            ,[DataEmissaoNF]
		                            ,[CodigoNCM]
		                            ,[CodigoURF]
		                            ,[CodigoRA]
		                            ,[Latitude]
		                            ,[Longitude]
		                            ,[IdResponsavel]
		                            ,[NomeResponsavel]
		                            ,[CodigoPaisDestinatario]
		                            ,[NomePaisDestinatario]
		                            ,[AnoDeposito]
		                            ,[SequenciaDeposito]
		                            ,[ExisteConteiner]
		                            ,[DataConsulta]
		                            ,[IdentificacaoCondutor]
		                            ,[NomeCondutor]
		                            ,[DataHoraEntradaEstoque]
		                            ,[DescricaoAvarias]
		                            ,[LocalRaCodigo]
		                            ,[LocalRaDescricao]
		                            ,[LocalUrfCodigo]
		                            ,[LocalUrfDescricao]
		                            ,[NcmCodigo]
		                            ,[NcmDescricao]
		                            ,[NotaFiscalDestinatarioNome]
		                            ,[NotaFiscalDestinatarioPais]
		                            ,[NotaFiscalEmissao]
		                            ,[NotaFiscalEmitenteIdentificacao]
		                            ,[NotaFiscalEmitenteNome]
		                            ,[NotaFiscalEmitentePais]
		                            ,[NotaFiscalModelo]
		                            ,[NotaFiscalNumero]
		                            ,[NotaFiscalSerie]
		                            ,[NotaFiscalUf]
		                            ,[NumeroDue]
		                            ,[NumeroItem]
		                            ,[PaisDestino]
		                            ,[PesoAferido]
		                            ,[QuantidadeExportada]
		                            ,[ResponsavelIdentificacao]
		                            ,[ResponsavelNome]
		                            ,[ResponsavelPais]
		                            ,[Saldo]
		                            ,[TransportadorIdentificacao]
		                            ,[TransportadorNome]
		                            ,[TransportadorPais]
		                            ,[UnidadeEstatistica]
		                            ,[Valor]
		                            ,[CnpjDestinatario]
		                            ,[Item]
		                            ,[Arquivo]
		                            ,[SaldoAtual]
		                            ,[Cancelado]
		                            ,[DataCadastro]
		                            ,[Critica]
		                            ,[MotivoNaoPesagem]
		                            ,[Data_averbacao]
		                            ,[Chave_NF]
		                            ,[OBS]
		                            ,[Flag_Duplicata]
		                            ,ROW_NUMBER() OVER (ORDER BY Id) AS Linha
		                            ,COUNT(*)
                             OVER() TotalLinhas
	                            FROM
		                            [dbo].[TbConsultaEstoquePreACD]    
                            )
	                        
                                SELECT 
                                    *
	                            FROM 
		                            Resultado
	                            WHERE 
	                                Linha < (({pagina} * {registrosPorPagina}) + 1)
	                            AND
	                                Linha >= ((({pagina} - 1) * {registrosPorPagina}) + 1) ";

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);

                        if (Ds.Tables[0].Rows.Count > 0)
                        {
                            totalFiltro = Convert.ToInt32(Ds.Tables[0].Rows[0]["TotalLinhas"].ToString());
                        }
                        else
                        {
                            totalFiltro = 0;
                        }

                        return Ds.Tables[0];
                    }
                }
            }
        }

        public SqlDataReader GerarCsvEstoque3(int pagina, int registrosPorPagina, string filtro)
        {
            using (SqlConnection Con = new SqlConnection(Banco.StringConexao()))
            {
                using (SqlCommand Cmd = new SqlCommand())
                {
                    Cmd.CommandTimeout = 4800;

                    Cmd.Connection = Con;
                    Cmd.CommandType = CommandType.Text;

                    Cmd.CommandText = $@"
                            WITH Resultado AS
                            ( 
	                            SELECT 
		                             [Id]
		                            ,[NumeroDocumento]
		                            ,[Tipo]
		                            ,[NumeroNF]
		                            ,[DataEmissaoNF]
		                            ,[CodigoNCM]
		                            ,[CodigoURF]
		                            ,[CodigoRA]
		                            ,[Latitude]
		                            ,[Longitude]
		                            ,[IdResponsavel]
		                            ,[NomeResponsavel]
		                            ,[CodigoPaisDestinatario]
		                            ,[NomePaisDestinatario]
		                            ,[AnoDeposito]
		                            ,[SequenciaDeposito]
		                            ,[ExisteConteiner]
		                            ,[DataConsulta]
		                            ,[IdentificacaoCondutor]
		                            ,[NomeCondutor]
		                            ,[DataHoraEntradaEstoque]
		                            ,[DescricaoAvarias]
		                            ,[LocalRaCodigo]
		                            ,[LocalRaDescricao]
		                            ,[LocalUrfCodigo]
		                            ,[LocalUrfDescricao]
		                            ,[NcmCodigo]
		                            ,[NcmDescricao]
		                            ,[NotaFiscalDestinatarioNome]
		                            ,[NotaFiscalDestinatarioPais]
		                            ,[NotaFiscalEmissao]
		                            ,[NotaFiscalEmitenteIdentificacao]
		                            ,[NotaFiscalEmitenteNome]
		                            ,[NotaFiscalEmitentePais]
		                            ,[NotaFiscalModelo]
		                            ,[NotaFiscalNumero]
		                            ,[NotaFiscalSerie]
		                            ,[NotaFiscalUf]
		                            ,[NumeroDue]
		                            ,[NumeroItem]
		                            ,[PaisDestino]
		                            ,[PesoAferido]
		                            ,[QuantidadeExportada]
		                            ,[ResponsavelIdentificacao]
		                            ,[ResponsavelNome]
		                            ,[ResponsavelPais]
		                            ,[Saldo]
		                            ,[TransportadorIdentificacao]
		                            ,[TransportadorNome]
		                            ,[TransportadorPais]
		                            ,[UnidadeEstatistica]
		                            ,[Valor]
		                            ,[CnpjDestinatario]
		                            ,[Item]
		                            ,[Arquivo]
		                            ,[SaldoAtual]
		                            ,[Cancelado]
		                            ,[DataCadastro]
		                            ,[Critica]
		                            ,[MotivoNaoPesagem]
		                            ,[Data_averbacao]
		                            ,[Chave_NF]
		                            ,[OBS]
		                            ,[Flag_Duplicata]
		                            ,ROW_NUMBER() OVER (ORDER BY Id) AS Linha
		                            ,COUNT(*) OVER() TotalLinhas
	                            FROM
		                            [dbo].[TbConsultaEstoquePreACD]    
                            )
	                        
                                SELECT 
                                    *
	                            FROM 
		                            Resultado
	                            WHERE 
	                                Linha < (({pagina} * {registrosPorPagina}) + 1)
	                            AND
	                                Linha >= ((({pagina} - 1) * {registrosPorPagina}) + 1) ";

                    Con.Open();
                    SqlDataReader dr = Cmd.ExecuteReader();
                   
                    return dr;
                }
            }
        }

        public EstoquePreACDPesos ObterPesoCCT(string recinto, string cnpjNf, string numeroNf, string dataEntradaEstoque, string emissaoMesAno)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Recinto", value: recinto, direction: ParameterDirection.Input);
                parametros.Add(name: "CnpjNf", value: cnpjNf, direction: ParameterDirection.Input);
                parametros.Add(name: "NumeroNf", value: numeroNf, direction: ParameterDirection.Input);
                parametros.Add(name: "DataEntradaEstoque", value: dataEntradaEstoque, direction: ParameterDirection.Input);
                parametros.Add(name: "EmissaoMesAno", value: emissaoMesAno, direction: ParameterDirection.Input);

                return con.Query<EstoquePreACDPesos>($@"
                    SELECT 
                        Saldo As PesoEntradaCCT, PesoAferido, MotivoNaoPesagem FROM [dbo].[TbConsultaEstoquePreACD] 
                    WHERE 
                        NumeroNF = @NumeroNf AND 
                            (
                                CAST(CnpjDestinatario As Numeric) = @CnpjNf 
                                    OR CAST(NotaFiscalEmitenteIdentificacao As Numeric) = @CnpjNf 
                                        OR CAST(Responsavelidentificacao As Numeric) = @CnpjNf
                            ) 
                        AND CodigoRA = @Recinto
                        AND NotaFiscalEmissao = @EmissaoMesAno", parametros).FirstOrDefault();
            }
        }

        public void InserirStatusExportacaoCSV(string guid, int usuarioId, int totalRegistros, int totalRegistrosProcessados, string status)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Guid", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);
                parametros.Add(name: "TotalRegistros", value: totalRegistros, direction: ParameterDirection.Input);
                parametros.Add(name: "TotalRegistrosProcessados", value: totalRegistrosProcessados, direction: ParameterDirection.Input);
                parametros.Add(name: "Status", value: status, direction: ParameterDirection.Input);

                con.Execute(@"
                    INSERT INTO 
                        [dbo].[TEMP_CONS_PRE_ACD] 
                        (
                            Guid,
                            UsuarioId,
                            TotalRegistros,
                            TotalRegistrosProcessados,
                            Status
                        ) 
                            VALUES 
                        (
                            @Guid, 
                            @UsuarioId, 
                            @TotalRegistros, 
                            @TotalRegistrosProcessados, 
                            @Status)", parametros);
            }
        }

        public void AtualizarStatusExportacaoCSV(string guid, int usuarioId, int totalRegistrosProcessados, string status)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Guid", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);
                parametros.Add(name: "TotalRegistrosProcessados", value: totalRegistrosProcessados, direction: ParameterDirection.Input);
                parametros.Add(name: "Status", value: status, direction: ParameterDirection.Input);

                con.Execute(@"
                    UPDATE 
                        [dbo].[TEMP_CONS_PRE_ACD] 
                    SET
                        TotalRegistrosProcessados = @TotalRegistrosProcessados,
                        Status = @Status
                    WHERE 
                         Guid = @Guid and UsuarioId = @UsuarioId", parametros);
            }
        }

        public StatusConsultaPreACD ObterStatusExportacaoCSV(string guid, int usuarioId)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();

                parametros.Add(name: "Guid", value: guid, direction: ParameterDirection.Input);
                parametros.Add(name: "UsuarioId", value: usuarioId, direction: ParameterDirection.Input);
                
                return con.Query<StatusConsultaPreACD>(@"
                        SELECT 
                            GUID,
                            UsuarioId,
                            TotalRegistros,
                            TotalRegistrosProcessados,
                            Status,
                            DataAtualizacao
                        FROM 
                            TEMP_CONS_PRE_ACD 
                        WHERE 
                            Guid = @Guid and UsuarioId = @UsuarioId", parametros).FirstOrDefault();
                
            }
        }

        public IEnumerable<ConsCCTDadosDUE> ObterDadosDueConsCCT(string chaveNF)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ChaveNF", value: chaveNF, direction: ParameterDirection.Input);

                return con.Query<ConsCCTDadosDUE>(@"
                        SELECT 
	                        A.QUANTIDADE_CONSUMIDA As Quantidade,
	                        C.NUMERO_DUE As DUE
                        FROM 
	                        TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA A
                        INNER JOIN 
	                        TB_CONSULTA_DUE_POS_ACD_ITENS B ON A.AUTONUM_POS_ACD_ITEM = B.AUTONUM
                        INNER JOIN
	                        TB_CONSULTA_DUE_POS_ACD C ON B.AUTONUM_CONSULTA = C.AUTONUM
                        WHERE 
	                        A.CHAVE_ACESSO = @ChaveNF", parametros);

            }
        }
    }
}