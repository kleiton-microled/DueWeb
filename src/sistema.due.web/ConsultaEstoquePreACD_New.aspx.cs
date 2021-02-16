using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ConsultaEstoquePreACD_New : System.Web.UI.Page
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

                    arquivos = arquivos.Where(a => a.LastWriteTime < DateTime.Now.AddMinutes(-5)).ToList();

                    if (arquivos.Count() == 0)
                        return;

                    foreach (var arquivo in arquivos)
                        File.Delete(arquivo.FullName);
                }
                catch(Exception ex)
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

        protected void btnGerarCsv_Click(object sender, EventArgs e)
        {
            LogsService.Logar("ConsultaEstoquePreACD_NEW.aspx", $"Iniciou consulta no Banco as {DateTime.Now.ToString()}");
            var dataTable = _estoqueDAO.GerarCsvEstoque(GerarFiltro());
            LogsService.Logar("ConsultaEstoquePreACD_NEW.aspx", $"Finalizou consulta no Banco as {DateTime.Now.ToString()}");
            var agora = DateTime.Now;
            var nomeArquivo = Path.Combine(Server.MapPath("."), "Uploads", $"LM_{agora.ToString("ddMMyyyy")}_{agora.ToString("HHmmss")}.csv");

            LogsService.Logar("ConsultaEstoquePreACD_NEW.aspx", $"Começou montar o arquivo as {DateTime.Now.ToString()}");
            var sw = new StreamWriter(nomeArquivo, false);
                string Head = "";
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (Head.Trim() != "")
                    {
                        Head += ";" + dataTable.Columns[j].ColumnName + "";
                    }
                    else
                    {
                        Head += "" + dataTable.Columns[j].ColumnName + "";
                    }
                }
                sw.WriteLine(Head);                
                for (int j = 0; j < dataTable.Rows.Count; j++)
                {
                    string[] dataArr = new String[dataTable.Rows[j].ItemArray.Length];
                    for (int i = 0; i < dataTable.Rows[j].ItemArray.Length; i++)
                    {
                        object o = dataTable.Rows[j].ItemArray[i].ToString();
                        dataArr[i] = "" + o.ToString() + "";
                    }
                    sw.WriteLine(string.Join(";", dataArr));
                }
                sw.Close();
            LogsService.Logar("ConsultaEstoquePreACD_NEW.aspx", $"Finalizou a montagem do arquivo as {DateTime.Now.ToString()}");


            Response.Clear();
                Response.Buffer = true;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ClearContent();
                Response.ClearHeaders();


                Response.AppendHeader(@"Content-Disposition", "attachment; filename=" + Path.GetFileName(nomeArquivo));
                Response.ContentType = "application/csv";
                Response.WriteFile(nomeArquivo);
                Response.End();
            //}
            //catch (Exception ex)
            //{
            //    Response.Write(ex.Message);
            //}
            //finally
            //{
            //    System.GC.Collect();
            //    System.GC.WaitForPendingFinalizers();

            //    if ((System.IO.File.Exists(nomeArquivo)))
            //        System.IO.File.Delete(nomeArquivo);
            //}
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
    }
}