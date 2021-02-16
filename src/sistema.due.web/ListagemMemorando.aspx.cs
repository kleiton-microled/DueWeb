using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ListagemMemorando : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly EmpresaDAO _empresaDAO = new EmpresaDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ConsultarEmpresas();
            }
        }

        private void ConsultarEmpresas()
        {
            this.cbEmpresa.DataSource = _empresaDAO.ObterEmpresasMemorando();
            this.cbEmpresa.DataBind();

            this.cbEmpresa.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        private DataTable Consultar()
        {
            var filtro = new StringBuilder();

            if (StringHelpers.IsDate(this.txtDe.Text))
            {
                filtro.Append(" AND A.DataEmissao >= CONVERT(DATETIME, '" + this.txtDe.Text + "', 103) ");
            }

            if (StringHelpers.IsDate(this.txtAte.Text))
            {
                filtro.Append(" AND A.DataEmissao <= CONVERT(DATETIME, '" + this.txtAte.Text + "', 103) ");
            }

            if (this.cbEmpresa.SelectedValue.ToInt() > 0)
            {
                filtro.Append(" AND A.Empresa = '" + this.cbEmpresa.SelectedValue + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtFilial.Text))
            {
                filtro.Append(" AND A.Filial = '" + this.txtFilial.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtMemorando.Text))
            {
                filtro.Append(" AND A.Memorando = '" + this.txtMemorando.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtDUE.Text))
            {
                filtro.Append(" AND C.DUE = '" + this.txtDUE.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtChaveDUE.Text))
            {
                filtro.Append(" AND C.ChaveAcesso = '" + this.txtChaveDUE.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtChaveNF.Text))
            {
                filtro.Append(" AND A.ChaveNF LIKE '%" + this.txtChaveNF.Text + "%' ");
            }

            if (!string.IsNullOrEmpty(this.txtNumeroNF.Text))
            {
                filtro.Append(" AND A.NumeroNF = '" + this.txtNumeroNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtCNPJ.Text))
            {
                filtro.Append(" AND A.CNPJNF = '" + this.txtCNPJ.Text.Replace(".","").Replace("/","").Replace("-","") + "' ");
            }

            return _notaFiscalDAO.ListagemMemorando(filtro.ToString());
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            this.gvNotasFiscais.DataSource = Consultar();
            this.gvNotasFiscais.DataBind();

            this.btnGerarCsv.Visible = this.gvNotasFiscais.Rows.Count > 0;
        }

        protected void gvNotasFiscais_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            this.gvNotasFiscais.PageIndex = e.NewPageIndex;
            this.gvNotasFiscais.DataSource = Consultar();
            this.gvNotasFiscais.DataBind(); 
        }

        protected void btnGerarCsv_Click(object sender, EventArgs e)
        {
            var dataTable = Consultar();

            StringBuilder builder = new StringBuilder();
            List<string> rows = new List<string>();

            foreach (DataRow row in dataTable.Rows)
            {
                List<string> currentRow = new List<string>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName.ToUpper() != "CNPJNF")
                    {
                        object item = row[column];

                        currentRow.Add(item.ToString());
                    }                    
                }

                rows.Add(string.Join(";", currentRow.ToArray()));
            }

            builder.Append(string.Join("\n", rows.ToArray()));

            var agora = DateTime.Now;
            var nomeArquivo = $"LM_{agora.ToString("ddMMyyyy")}_{agora.ToString("HHmmss")}";

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename={nomeArquivo}.csv");
            Response.Write(builder.ToString());
            Response.End();
        }
    }
}