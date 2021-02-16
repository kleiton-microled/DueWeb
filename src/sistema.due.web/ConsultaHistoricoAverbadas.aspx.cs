using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ConsultaHistoricoAverbadas : System.Web.UI.Page
    {
        private readonly HistoricoAverbadosDAO _estoqueDAO = new HistoricoAverbadosDAO();
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
            this.gvEstoque.DataSource = _estoqueDAO.ObterRegistros(GerarFiltro());
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
                        gvEstoqueDetalhes.DataSource = _estoqueDAO.ObterDetalhes(id);
                        gvEstoqueDetalhes.DataBind();
                    }
                }
            }
        }

        protected void btnGerarCsv_Click(object sender, EventArgs e)
        {
            var dataTable = _estoqueDAO.GerarCsv(GerarFiltro());

            StringBuilder builder = new StringBuilder();
            List<string> rows = new List<string>();

            List<string> colunas = new List<string>();

            foreach (DataColumn column in dataTable.Columns)
            {
                colunas.Add(column.ColumnName);
            }

            foreach (DataRow row in dataTable.Rows)
            {
                List<string> campos = new List<string>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    object item = row[column];
                    campos.Add(item.ToString());
                }

                rows.Add(string.Join(";", campos.ToArray()));
            }

            builder.Append(string.Join(";", colunas.ToArray()));
            builder.Append("\n");
            builder.Append(string.Join("\n", rows.ToArray()));

            var agora = DateTime.Now;
            var nomeArquivo = $"LM_{agora.ToString("ddMMyyyy")}_{agora.ToString("HHmmss")}";

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename={nomeArquivo}.csv");
            Response.Write(builder.ToString());
            Response.End();
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