using Dapper;
using Sistema.DUE.Web.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sistema.DUE.Web.DAO
{
    public class EstoquePosACD
    {
        public int Id { get; set; }
        public DateTime DataDUE { get; set; }
        public string DUE { get; set; }
        public string ChaveDUE { get; set; }
        public string DeclaranteCnpj { get; set; }
        public string DeclaranteNome { get; set; }
        public string UltimoEvento { get; set; }
        public string StatusDUE { get; set; }
        public DateTime DataUltimoEvento { get; set; }
        public DateTime? DataAverbacao { get; set; }
    }
   
    public class EstoquePosACDDetalhes
    {
        public int Id { get; set; }
        public int ConsultaId { get; set; }
        public string NCM { get; set; }
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string ChaveNF { get; set; }
        public string Numero { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string UF { get; set; }
        public string CnpjEmitente { get; set; }
        public int Item { get; set; }
        public string CFOP { get; set; }
        public string TipoNF { get; set; }
    }

    public class EstoquePosACDDAO
    {
        public IEnumerable<EstoquePosACD> ObterRegistrosEstoque(string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var limite = string.Empty;

                if (string.IsNullOrEmpty(filtro))
                    limite = " TOP 200 ";

                return con.Query<EstoquePosACD>($@"
                    SELECT 
                        DISTINCT
                        {limite}
	                       A.[AUTONUM] As Id
                          ,A.[DATA_DUE] As DataDUE
                          ,A.[CHAVE_DUE] As ChaveDUE
                          ,A.[NUMERO_DUE] As DUE
                          ,A.[CNPJ_DECLARANTE] As DeclaranteCnpj
                          ,A.[NOME_DECLARANTE] As DeclaranteNome
                          ,A.[ULTIMO_EVENTO] As UltimoEvento
                          ,A.[DATA_ULTIMO_EVENTO] As DataUltimoEvento
                          ,A.[DATA_AVERBACAO] As DataAverbacao
                          ,A.[STATUS_DUE] As StatusDUE
                      FROM 
	                     [dbo].[TB_CONSULTA_DUE_POS_ACD] A
                      LEFT JOIN
                         [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS] B ON A.AUTONUM = B.AUTONUM_CONSULTA
                      LEFT JOIN
                         [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] C ON B.AUTONUM = C.AUTONUM_POS_ACD_ITEM
                     WHERE
                        A.AUTONUM > 0 {filtro}");
            }
        }

        public IEnumerable<EstoquePosACDDetalhes> ObterDetalhesEstoque(int id, string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "ConsultaId", value: id, direction: ParameterDirection.Input);

                return con.Query<EstoquePosACDDetalhes>($@"
                    SELECT 
                        DISTINCT
	                     A.[AUTONUM] As Id
                        ,A.[AUTONUM_CONSULTA] As ConsultaId
                        ,A.[NCM]
                        ,A.[DESCRICAO]
                        ,A.[QTDE_ESTATISTICA] As Quantidade
                        ,A.[CHAVE_NF_EXPORTACAO] As ChaveNF
                        ,A.[NUMERO]
                        ,A.[MODELO]
                        ,A.[SERIE]
                        ,A.[UF]
                        ,A.[CNPJ_EMITENTE] As CnpjEmitente
                        ,A.[ITEM]
                        ,A.[CFOP]
                    FROM 
	                    [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS] A
                    LEFT JOIN
                        [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] B ON B.AUTONUM_POS_ACD_ITEM = A.AUTONUM
                    WHERE
	                    A.[AUTONUM_CONSULTA] = @ConsultaId {filtro} ORDER BY A.[ITEM]", parametros);
            }
        }

        public IEnumerable<EstoquePosACDDetalhes> ObterNotasRemessaPosACD(int detalheId, string filtro)
        {
            using (SqlConnection con = new SqlConnection(Banco.StringConexao()))
            {
                var parametros = new DynamicParameters();
                parametros.Add(name: "DetalheId", value: detalheId, direction: ParameterDirection.Input);

                return con.Query<EstoquePosACDDetalhes>($@"
                    SELECT 
                        DISTINCT
                           [CHAVE_ACESSO] As ChaveNF
                          ,[TIPO_NF] As TipoNF
                  FROM 
                      [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] 
                  WHERE 
                    [AUTONUM_POS_ACD_ITEM] = @DetalheId {filtro}", parametros);
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
                                   A.AUTONUM     
                                  ,B.[DATA_DUE] As DataDUE
                                  ,B.[NUMERO_DUE] As DUE
                                  ,B.[CHAVE_DUE] As ChaveDUE
                                  ,B.[CNPJ_DECLARANTE] As DeclaranteCnpj
                                  ,B.[NOME_DECLARANTE] As DeclaranteNome
                                  ,B.[ULTIMO_EVENTO] As UltimoEvento
                                  ,B.[DATA_ULTIMO_EVENTO] As DataUltimoEvento
                                  ,B.[DATA_AVERBACAO] As DataAverbacao                              
                                  ,A.[NCM]
                                  ,A.[ITEM]
                                  ,A.[CFOP]
                                  ,A.[DESCRICAO]                                  
                                  ,'EXP' As TipoNF
                                  ,A.[CHAVE_NF_EXPORTACAO] As CHAVE_NF
                                  ,A.[NUMERO]
                                  ,A.[MODELO]
                                  ,A.[SERIE]
                                  ,A.[UF]
                                  ,A.[CNPJ_EMITENTE]
                                  ,B.[STATUS_DUE] As EventoAverbacao
                                  ,A.[VMLE] As VMLE
                                  ,A.[VMLE_REAIS] As ValorEmReais
                                  ,A.[VMCV]
                                  ,A.[IMPORTADOR]
                                  ,A.[ENDERECO_IMPORTADOR] As ImportadorEndereco
                                  ,A.[PAIS_IMPORTADOR] As ImportadorPais
                                  ,A.[PAIS_DESTINO] As PaisDestino
                                  ,A.[UNIDADE_ESTATISTICA] As Unidade
                                  ,A.[PESO_LIQUIDO_TOTAL] As PesoLiquidoTotal
                                  ,A.[MOEDA_NEGOCIACAO] As Moeda
                                  ,A.[INCOTERM]
                                  ,REPLACE(A.[INFORMACOES_COMPLEMENTARES],';','') As InformacoesComplementares
                                  ,B.[PORTO] As UnidadeRFB
                                  ,C.[DESCRICAO] As DescricaoUnidadeRFB
                                  ,B.[RECINTO] As RecintoDespacho
                                  ,D.[DESCRICAO] As DescricaoRecintoDespacho
                                  ,A.[QTDE_ESTATISTICA] As QuantidadeNF
                                  ,0 As QuantidadeAverbada
                              FROM 
	                                [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS] A
                              INNER JOIN 
	                                [dbo].[TB_CONSULTA_DUE_POS_ACD] B ON A.[AUTONUM_CONSULTA] = B.[AUTONUM]
                              LEFT JOIN 
	                                [dbo].[TB_UNIDADES_RFB] C ON CAST(B.[PORTO] AS INT) = CAST(C.[CODIGO] AS INT)
                              LEFT JOIN 
	                                [dbo].[TB_RECINTOS] D ON B.[RECINTO] = D.[ID]
                              LEFT JOIN 
	                                [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] E ON E.[AUTONUM_POS_ACD_ITEM] = A.[AUTONUM]
                              WHERE
                                    A.AUTONUM > 0 {filtro}
                              ORDER BY
	                                B.[NUMERO_DUE]";

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);
                        return Ds.Tables[0];
                    }
                }
            }
        }

        public DataTable GerarCsvEstoqueRemessa(int itemId, string filtro)
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
                                 A.AUTONUM 
                                ,C.[DATA_DUE] As DataDUE
                                ,C.[NUMERO_DUE] As DUE
                                ,C.[CHAVE_DUE] As ChaveDUE
                                ,C.[CNPJ_DECLARANTE] As DeclaranteCnpj
                                ,C.[NOME_DECLARANTE] As DeclaranteNome
                                ,C.[ULTIMO_EVENTO] As UltimoEvento
                                ,C.[DATA_ULTIMO_EVENTO] As DataUltimoEvento
                                ,C.[DATA_AVERBACAO] As DataAverbacao                              
                                ,A.[NCM]
                                ,B.[NUMERO_ITEM] As Item
                                ,B.[CFOP]
                                ,A.[DESCRICAO]                                
                                ,B.TIPO_NF As TipoNF
                                ,B.[CHAVE_ACESSO] As CHAVE_NF
                                ,B.[NUMERO_DOCUMENTO]
                                ,B.[MODELO]
                                ,SUBSTRING(B.CHAVE_ACESSO, 23,3) [SERIE]
                                ,B.[UF]
                                ,B.[EMISSOR_DOCUMENTO]
                                ,C.[STATUS_DUE] As EventoAverbacao
                                ,A.[VMLE] As VMLE
                                ,A.[VMLE_REAIS] As ValorEmReais
                                ,A.[VMCV]
                                ,A.[IMPORTADOR]
                                ,A.[ENDERECO_IMPORTADOR] As ImportadorEndereco
                                ,A.[PAIS_IMPORTADOR] As ImportadorPais
                                ,A.[PAIS_DESTINO] As PaisDestino
                                ,A.[UNIDADE_ESTATISTICA] As Unidade
                                ,A.[PESO_LIQUIDO_TOTAL] As PesoLiquidoTotal
                                ,A.[MOEDA_NEGOCIACAO] As Moeda
                                ,A.[INCOTERM]
                                ,REPLACE(A.[INFORMACOES_COMPLEMENTARES],';','') As InformacoesComplementares
                                ,C.[PORTO] As UnidadeRFB
                                ,D.[DESCRICAO] As DescricaoUnidadeRFB
                                ,C.[RECINTO] As RecintoDespacho
                                ,E.[DESCRICAO] As DescricaoRecintoDespacho                                
                                ,B.[QTDE_ESTATISTICA] As QuantidadeNF
                                ,B.[QUANTIDADE_CONSUMIDA] As QuantidadeAverbada                            
                            FROM 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS] A
                            INNER JOIN 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] B ON A.[AUTONUM] = B.[AUTONUM_POS_ACD_ITEM]
                            INNER JOIN 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD] C ON A.[AUTONUM_CONSULTA] = C.[AUTONUM]
                            LEFT JOIN 
	                            [dbo].[TB_UNIDADES_RFB] D ON CAST(C.[PORTO] AS INT) = CAST(D.[CODIGO] AS INT)
                            LEFT JOIN 
	                            [dbo].[TB_RECINTOS] E ON C.[RECINTO] = E.[ID]
                            WHERE
                                B.[AUTONUM_POS_ACD_ITEM] = {itemId} {filtro}
                            ORDER BY
	                            B.[NUMERO_ITEM]";

                    using (SqlDataAdapter Adp = new SqlDataAdapter(Cmd))
                    {
                        Adp.Fill(Ds);
                        return Ds.Tables[0];
                    }
                }
            }
        }

        public DataTable ObterNotasRemessa(string filtro)
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
                                 A.AUTONUM 
                                ,C.[DATA_DUE] As DataDUE
                                ,C.[NUMERO_DUE] As DUE
                                ,C.[CHAVE_DUE] As ChaveDUE
                                ,C.[CNPJ_DECLARANTE] As DeclaranteCnpj
                                ,C.[NOME_DECLARANTE] As DeclaranteNome
                                ,C.[ULTIMO_EVENTO] As UltimoEvento
                                ,C.[DATA_ULTIMO_EVENTO] As DataUltimoEvento
                                ,C.[DATA_AVERBACAO] As DataAverbacao
                                ,A.[ITEM] As ItemDUE
                                ,A.[NCM]                                
                                ,B.[NUMERO_ITEM] As ItemNF
                                ,B.[CFOP]
                                ,A.[DESCRICAO]                                
                                ,B.TIPO_NF As TipoNF
                                ,B.[CHAVE_ACESSO] As CHAVE_NF
                                ,A.[NUMERO]
                                ,A.[MODELO]
                                ,A.[SERIE]
                                ,A.[UF]
                                ,A.[CNPJ_EMITENTE]
                                ,C.[STATUS_DUE] As EventoAverbacao
                                ,A.[VMLE] As VMLE
                                ,A.[VMLE_REAIS] As ValorEmReais
                                ,A.[VMCV]
                                ,A.[IMPORTADOR]
                                ,A.[ENDERECO_IMPORTADOR] As ImportadorEndereco
                                ,A.[PAIS_IMPORTADOR] As ImportadorPais
                                ,A.[PAIS_DESTINO] As PaisDestino
                                ,A.[UNIDADE_ESTATISTICA] As Unidade
                                ,A.[PESO_LIQUIDO_TOTAL] As PesoLiquidoTotal
                                ,A.[MOEDA_NEGOCIACAO] As Moeda
                                ,A.[INCOTERM]
                                ,REPLACE(A.[INFORMACOES_COMPLEMENTARES],';','') As InformacoesComplementares
                                ,C.[PORTO] As UnidadeRFB
                                ,D.[DESCRICAO] As DescricaoUnidadeRFB
                                ,C.[RECINTO] As RecintoDespacho
                                ,E.[DESCRICAO] As DescricaoRecintoDespacho                                
                                ,B.[QTDE_ESTATISTICA] As QuantidadeNF
                                ,B.[QUANTIDADE_CONSUMIDA] As QuantidadeAverbada                            
                            FROM 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS] A
                            INNER JOIN 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD_ITENS_REMESSA] B ON A.[AUTONUM] = B.[AUTONUM_POS_ACD_ITEM]
                            INNER JOIN 
	                            [dbo].[TB_CONSULTA_DUE_POS_ACD] C ON A.[AUTONUM_CONSULTA] = C.[AUTONUM]
                            LEFT JOIN 
	                            [dbo].[TB_UNIDADES_RFB] D ON CAST(C.[PORTO] AS INT) = CAST(D.[CODIGO] AS INT)
                            LEFT JOIN 
	                            [dbo].[TB_RECINTOS] E ON C.[RECINTO] = E.[ID]
                            WHERE
                                1=1 {filtro}
                            ORDER BY
	                            C.[NUMERO_DUE], A.[ITEM]";

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