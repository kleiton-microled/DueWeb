using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Sistema.DUE.Web.Config;
using Sistema.DUE.Web.Models;
using System.Linq;
using Sistema.DUE.Web.Helpers;
using System;

namespace Sistema.DUE.Web.DAO
{
    public class HistoricoAverbadosDAO
    {
        private readonly string _tabela;
        
        public IEnumerable<Estoque> ObterRegistros(string filtro)
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
                        ,A.[Data_Averbacao] As DataAverbacao
                        ,CASE 
	                        WHEN A.UnidadeEstatistica LIKE '%QUILO%' THEN CAST(A.Saldo as numeric(18,3))
	                        WHEN (A.UnidadeEstatistica LIKE '%TONEL%' OR A.UnidadeEstatistica IS NULL) THEN CAST(A.Saldo as numeric(18,3)) * 1000 
                        END 
                         -CAST(PesoAferido as numeric) as Sobra
                      FROM 
	                     [dbo].[TbConsultaEstoquePreACD_Historico] A
                      LEFT JOIN
                         [dbo].[Tb_Unidades_RFB] B ON A.CodigoURF = B.Codigo
                      LEFT JOIN
                         [dbo].[Tb_Recintos] C ON A.CodigoRA = C.Id 
                     WHERE
                        A.Id > 0 {filtro} ORDER BY A.Id DESC");
            }
        }

        public IEnumerable<EstoqueDetalhes> ObterDetalhes(int id)
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
                        ,[Data_Averbacao] As DataAverbacao
                        ,CASE 
	                        WHEN UnidadeEstatistica LIKE '%QUILO%' THEN CAST(Saldo as numeric(18,3))
	                        WHEN (UnidadeEstatistica LIKE '%TONEL%' OR UnidadeEstatistica IS NULL) THEN CAST(Saldo as numeric(18,3)) * 1000 
                        END 
                         -CAST(PesoAferido as numeric) as Sobra
                    FROM 
	                    [dbo].[TbConsultaEstoquePreACD_Historico]
                    WHERE
	                    [Id] = @Id", parametros);
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
	                            DISTINCT
                                   A.[DataEmissaoNF]
                                  ,A.[NumeroNF]		                          
                                  ,A.[NotaFiscalModelo]
                                  ,A.[NotaFiscalSerie]
                                  ,A.[QuantidadeExportada] As QuantidadeAverbada
                                  ,A.[Saldo] As PesoEntrada
                                  ,A.[PesoAferido]          
                                  ,CASE 
	                                  WHEN A.UnidadeEstatistica LIKE '%QUILO%' THEN CAST(A.Saldo as numeric(18,3))
	                                  WHEN (A.UnidadeEstatistica LIKE '%TONEL%' OR A.UnidadeEstatistica IS NULL) THEN CAST(A.Saldo as numeric(18,3)) * 1000 
                                  END 
                                      - CAST(PesoAferido as numeric) as Sobras
                                  ,A.[Data_Averbacao] As DataAverbacao
                                  ,A.NumeroDUE As DUE
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
	                            [dbo].[TbConsultaEstoquePreACD_Historico] A
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
    }
}