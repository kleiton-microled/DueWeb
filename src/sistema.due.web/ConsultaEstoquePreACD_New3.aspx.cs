using Cargill.DUE.Web.Config;
using Cargill.DUE.Web.DAO;
using Cargill.DUE.Web.Extensions;
using Cargill.DUE.Web.Helpers;
using Cargill.DUE.Web.Models;
using Cargill.DUE.Web.Services;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace Cargill.DUE.Web
{
    public partial class ConsultaEstoquePreACD_New3 : System.Web.UI.Page
    {
        private readonly EstoqueDAO _estoqueDAO = new EstoqueDAO(false);
        private readonly UnidadesReceitaDAO unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly RecintosDAO recintosDAO = new RecintosDAO();
        private readonly PaisesDAO paisesDAO = new PaisesDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsPostBack)
            {
                Consultar();
                ListarUnidadesRFB();
                ListarPaises();

                try
                {
                    var diretorioUploads = Path.Combine(Server.MapPath("."), "Uploads");

                    var arquivos = new DirectoryInfo(diretorioUploads).GetFiles()
                        .Where(a => a.Name.StartsWith("LM_"));

                    arquivos = arquivos.Where(a => a.LastWriteTime < DateTime.Now.AddHours(1)).ToList();

                    if (arquivos.Count() == 0)
                        return;

                    foreach (var arquivo in arquivos)
                        File.Delete(arquivo.FullName);
                }
                catch (Exception ex)
                {

                }
            }
        }

        protected void ListarUnidadesRFB()
        {
            this.cbUnidadeRFB.DataSource = unidadesReceitaDAO.ObterUnidadesRFB();
            this.cbUnidadeRFB.DataBind();

            this.cbUnidadeRFB.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void ListarRecintos(int unidadeRfb)
        {
            this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(unidadeRfb);
            this.cbRecintoAduaneiroDespacho.DataBind();

            this.cbRecintoAduaneiroDespacho.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void ListarPaises()
        {
            this.cbPaisDestinatario.DataSource = paisesDAO.ObterPaises();
            this.cbPaisDestinatario.DataBind();

            this.cbPaisDestinatario.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void cbUnidadeRFB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbUnidadeRFB.SelectedValue != null)
            {
                ListarRecintos(this.cbUnidadeRFB.SelectedValue.ToInt());
            }
        }

        private void Consultar()
        {
            this.gvEstoque.DataSource = _estoqueDAO.ObterRegistrosEstoque(GerarFiltro());
            this.gvEstoque.DataBind();
        }

        private string GerarFiltro()
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtEntradaDe.Text))
            {
                if (StringHelpers.IsDate(this.txtEntradaDe.Text))
                {
                    filtro.Append(" AND A.DataHoraEntradaEstoque >= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEntradaDe.Text).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtEntradaAte.Text))
            {
                if (StringHelpers.IsDate(this.txtEntradaAte.Text))
                {
                    filtro.Append(" AND A.DataHoraEntradaEstoque <= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEntradaAte.Text).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                }
            }

            if (this.cbUnidadeRFB.SelectedValue != null)
            {
                if (this.cbUnidadeRFB.SelectedValue.ToInt() > 0)
                {
                    filtro.Append(" AND A.CodigoURF = '" + this.cbUnidadeRFB.SelectedValue + "' ");
                }
            }

            if (this.cbRecintoAduaneiroDespacho.SelectedValue != null)
            {
                if (this.cbRecintoAduaneiroDespacho.SelectedValue.ToInt() > 0)
                {
                    filtro.Append(" AND A.CodigoRA = '" + this.cbRecintoAduaneiroDespacho.SelectedValue + "' ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtCNPJResponsavel.Text))
            {
                filtro.Append(" AND REPLACE(REPLACE(REPLACE(A.IdResponsavel, '.', ''), '/', ''), '-', '') = '" + this.txtCNPJResponsavel.Text.RemoverCaracteresEspeciais() + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtNCM.Text))
            {
                filtro.Append(" AND A.NcmCodigo = '" + this.txtNCM.Text + "' ");
            }

            if (this.cbPaisDestinatario.SelectedValue != null)
            {
                if (this.cbPaisDestinatario.SelectedValue.ToInt() > 0)
                {
                    filtro.Append(" AND A.CodigoPaisDestinatario = '" + this.cbPaisDestinatario.SelectedValue + "' ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNFDe.Text))
            {
                if (StringHelpers.IsDate(this.txtEmissaoNFDe.Text))
                {
                    filtro.Append(" AND A.DataEmissaoNF >= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEmissaoNFDe.Text).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNFAte.Text))
            {
                if (StringHelpers.IsDate(this.txtEmissaoNFAte.Text))
                {
                    filtro.Append(" AND A.DataEmissaoNF <= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEmissaoNFAte.Text).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtCNPJEmitente.Text))
            {
                filtro.Append(" AND REPLACE(REPLACE(REPLACE(A.NotaFiscalEmitenteIdentificacao, '.', ''), '/', ''), '-', '')  = '" + this.txtCNPJEmitente.Text.RemoverCaracteresEspeciais() + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtCNPJDestinatario.Text))
            {
                var cnpjs = this.txtCNPJDestinatario.Text
                    .Split(',')
                    .Select(c => $"'{c.RemoverCaracteresEspeciais()}'");

                filtro.Append($" AND REPLACE(REPLACE(REPLACE(A.CnpjDestinatario, '.', ''), '/', ''), '-', '') IN ({string.Join(",", cnpjs)}) ");
            }

            if (!string.IsNullOrEmpty(this.txtNumeroNF.Text))
            {
                filtro.Append(" AND A.NumeroNF = '" + this.txtNumeroNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtModeloNF.Text))
            {
                filtro.Append(" AND A.NotaFiscalModelo = '" + this.txtModeloNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtSerieNF.Text))
            {
                filtro.Append(" AND A.NotaFiscalSerie = '" + this.txtSerieNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtItem.Text))
            {
                if (this.txtItem.Text.ToInt() > 0)
                {
                    filtro.Append(" AND A.Item = " + this.txtItem.Text + " ");
                }
            }

            return filtro.ToString();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        protected void gvEstoque_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            this.gvEstoque.PageIndex = e.NewPageIndex;
            Consultar();
        }

        protected void gvEstoque_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var id = gvEstoque.DataKeys[e.Row.RowIndex].Value.ToString().ToInt();

                if (id > 0)
                {
                    GridView gvEstoqueDetalhes = (GridView)e.Row.FindControl("gvEstoqueDetalhes");

                    if (gvEstoqueDetalhes != null)
                    {
                        gvEstoqueDetalhes.DataSource = _estoqueDAO.ObterDetalhesEstoque(id);
                        gvEstoqueDetalhes.DataBind();
                    }
                }
            }
        }

        [WebMethod]
        public static string ConsultarEstoquePreACD(string guid, int pagina, int registrosPorPagina, string filtro)
        {
            try
            {
                var filtroSQL = new StringBuilder();

                var nomeArquivo = Path.Combine(HttpContext.Current.Server.MapPath("."), "Uploads", $"PACD_{guid}.csv");

                int totalFiltro = 0;

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    FiltroConsultaPreACD  filtroConsultaPreACD = JsonConvert.DeserializeObject<FiltroConsultaPreACD>(filtro);

                    if (filtroConsultaPreACD != null)
                    {
                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.EntradaDe))
                        {
                            if (StringHelpers.IsDate(filtroConsultaPreACD.EntradaDe))
                            {
                                filtroSQL.Append(" AND DataHoraEntradaEstoque >= CONVERT(DATETIME,'" + Convert.ToDateTime(filtroConsultaPreACD.EntradaDe).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.EntradaAte))
                        {
                            if (StringHelpers.IsDate(filtroConsultaPreACD.EntradaAte))
                            {
                                filtroSQL.Append(" AND DataHoraEntradaEstoque <= CONVERT(DATETIME,'" + Convert.ToDateTime(filtroConsultaPreACD.EntradaAte).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.UnidadeRFB))
                        {
                            if (filtroConsultaPreACD.UnidadeRFB.ToInt() > 0)
                            {
                                filtroSQL.Append(" AND CodigoURF = '" + filtroConsultaPreACD.UnidadeRFB + "' ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.RecintoDespacho))
                        {
                            if (filtroConsultaPreACD.RecintoDespacho.ToInt() > 0)
                            {
                                filtroSQL.Append(" AND CodigoRA = '" + filtroConsultaPreACD.RecintoDespacho + "' ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.CnpjResponsavel))
                        {
                            filtroSQL.Append(" AND REPLACE(REPLACE(REPLACE(IdResponsavel, '.', ''), '/', ''), '-', '') = '" + filtroConsultaPreACD.CnpjResponsavel.RemoverCaracteresEspeciais() + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.NCM))
                        {
                            filtroSQL.Append(" AND NcmCodigo = '" + filtroConsultaPreACD.NCM + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.PaisDestinatario))
                        {
                            if (filtroConsultaPreACD.PaisDestinatario.ToInt() > 0)
                            {
                                filtroSQL.Append(" AND CodigoPaisDestinatario = '" + filtroConsultaPreACD.PaisDestinatario + "' ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.EmissaoNFDe))
                        {
                            if (StringHelpers.IsDate(filtroConsultaPreACD.EmissaoNFDe))
                            {
                                filtroSQL.Append(" AND DataEmissaoNF >= CONVERT(DATETIME,'" + Convert.ToDateTime(filtroConsultaPreACD.EmissaoNFDe).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.EmissaoNFAte))
                        {
                            if (StringHelpers.IsDate(filtroConsultaPreACD.EmissaoNFAte))
                            {
                                filtroSQL.Append(" AND DataEmissaoNF <= CONVERT(DATETIME,'" + Convert.ToDateTime(filtroConsultaPreACD.EmissaoNFAte).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                            }
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.CnpjEmitente))
                        {
                            filtroSQL.Append(" AND REPLACE(REPLACE(REPLACE(NotaFiscalEmitenteIdentificacao, '.', ''), '/', ''), '-', '')  = '" + filtroConsultaPreACD.CnpjEmitente.RemoverCaracteresEspeciais() + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.CnpjDestinatario))
                        {
                            var cnpjs = filtroConsultaPreACD.CnpjDestinatario
                                .Split(',')
                                .Select(c => $"'{c.RemoverCaracteresEspeciais()}'");

                            filtroSQL.Append($" AND REPLACE(REPLACE(REPLACE(CnpjDestinatario, '.', ''), '/', ''), '-', '') IN ({string.Join(",", cnpjs)}) ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.NumeroNF))
                        {
                            filtroSQL.Append(" AND NumeroNF = '" + filtroConsultaPreACD.NumeroNF + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.ModeloNF))
                        {
                            filtroSQL.Append(" AND NotaFiscalModelo = '" + filtroConsultaPreACD.ModeloNF + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.SerieNF))
                        {
                            filtroSQL.Append(" AND NotaFiscalSerie = '" + filtroConsultaPreACD.SerieNF + "' ");
                        }

                        if (!string.IsNullOrEmpty(filtroConsultaPreACD.Item))
                        {
                            if (filtroConsultaPreACD.Item.ToInt() > 0)
                            {
                                filtroSQL.Append(" AND Item = " + filtroConsultaPreACD.Item + " ");
                            }
                        }
                    }
                }

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
                                WHERE
                                    Id > 0 {filtroSQL.ToString()}
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
                        SqlDataReader dataReader = Cmd.ExecuteReader();

                        dataReader.Read();

                        if (dataReader.HasRows)
                        {
                            totalFiltro = Convert.ToInt32(dataReader.GetValue(dataReader.GetOrdinal("TotalLinhas")));
                        }

                        StreamWriter sw;

                        if (!File.Exists(nomeArquivo))
                        {
                            sw = new StreamWriter(new FileStream(nomeArquivo, FileMode.Create), Encoding.UTF8, 65536);

                            StringBuilder sb = new StringBuilder();

                            for (int index = 0; index < dataReader.FieldCount; index++)
                            {
                                if (dataReader.GetName(index) != null)
                                    sb.Append(dataReader.GetName(index));

                                if (index < dataReader.FieldCount - 1)
                                    sb.Append(";");
                            }

                            sw.WriteLine(sb.ToString());

                            sw.Close();
                        }

                        sw = new StreamWriter(new FileStream(nomeArquivo, FileMode.Append), Encoding.UTF8, 65536);

                        while (dataReader.Read())
                        {
                            var sb = new StringBuilder();
                            for (int index = 0; index < dataReader.FieldCount - 1; index++)
                            {
                                if (!dataReader.IsDBNull(index))
                                {
                                    string value = dataReader.GetValue(index).ToString();
                                    if (dataReader.GetFieldType(index) == typeof(String))
                                    {
                                        if (value.IndexOf("\"") >= 0)
                                            value = value.Replace("\"", "\"\"");

                                        if (value.IndexOf(";") >= 0)
                                            value = "\"" + value + "\"";
                                    }
                                    sb.Append(value);
                                }

                                if (index < dataReader.FieldCount - 1)
                                    sb.Append(";");
                            }

                            if (!dataReader.IsDBNull(dataReader.FieldCount - 1))
                                sb.Append(dataReader.GetValue(dataReader.FieldCount - 1).ToString().Replace(";", " "));

                           sw.WriteLine(string.Join(";", sb.ToString().QuebraDeLinhaXML()));
                        }

                        dataReader.Close();
                        sw.Close();

                        return "";

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnLimparFiltro_Click(object sender, EventArgs e)
        {
            this.txtCNPJDestinatario.Text = string.Empty;
            this.txtCNPJEmitente.Text = string.Empty;
            this.txtCNPJResponsavel.Text = string.Empty;
            this.txtEmissaoNFAte.Text = string.Empty;
            this.txtEmissaoNFDe.Text = string.Empty;
            this.txtEntradaAte.Text = string.Empty;
            this.txtEntradaDe.Text = string.Empty;
            this.txtModeloNF.Text = string.Empty;
            this.txtNCM.Text = string.Empty;
            this.txtNumeroNF.Text = string.Empty;
            this.txtSerieNF.Text = string.Empty;
            this.cbPaisDestinatario.SelectedIndex = -1;
            this.cbRecintoAduaneiroDespacho.SelectedIndex = -1;
            this.cbUnidadeRFB.SelectedIndex = -1;
        }

        protected void btnGerarCSVParcial_Click(object sender, EventArgs e)
        {
            var guid = hddnGUID.Value;

            if (string.IsNullOrEmpty(guid))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um problema ao exportar arquivo");

                return;
            }

            var nomeArquivo = Path.Combine(HttpContext.Current.Server.MapPath("."), "Uploads", $"PACD_{guid}.csv");

            Response.Clear();
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ClearContent();
            Response.ClearHeaders();

            Response.AppendHeader(@"Content-Disposition", "attachment; filename=" + Path.GetFileName(nomeArquivo));
            Response.ContentType = "application/csv";
            Response.WriteFile(nomeArquivo);
            Response.End();
        }
    }
}