using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Sistema.DUE.Web
{
    public partial class RelatorioDUES : System.Web.UI.Page
    {
        private readonly RelatorioDUEsDAO _relatorioDUEsDAO = new RelatorioDUEsDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsPostBack)
            {
                Consultar();

                try
                {
                    var diretorioUploads = Path.Combine(Server.MapPath("."), "Uploads");

                    var arquivos = new DirectoryInfo(diretorioUploads).GetFiles()
                        .Where(a => a.Name.StartsWith("REL_DUES_"));

                    arquivos = arquivos.Where(a => a.LastWriteTime < DateTime.Now.AddMinutes(-25)).ToList();

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
      
        private void Consultar()
        {
            this.gvEstoque.DataSource = _relatorioDUEsDAO.ObterRegistrosEstoque(GerarFiltro());
            this.gvEstoque.DataBind();
        }

        private string GerarFiltro()
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtDUE.Text))
            {
                filtro.Append(" AND A.NUMERO_DUE = '" + this.txtDUE.Text.Replace("-", "") + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoDUEDe.Text))
            {
                if (StringHelpers.IsDate(this.txtEmissaoDUEDe.Text))
                {
                    filtro.Append(" AND A.DATA_DUE >= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEmissaoDUEDe.Text).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoDueAte.Text))
            {
                if (StringHelpers.IsDate(this.txtEmissaoDueAte.Text))
                {
                    filtro.Append(" AND A.DATA_DUE <= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtEmissaoDueAte.Text).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtCnpjExportador.Text))
            {
                if (StringHelpers.IsDate(this.txtCnpjExportador.Text))
                {
                    filtro.Append(" AND C.CNPJ_EXPORTADOR = '" + this.txtCnpjExportador.Text.Replace(".","").Replace("/","").Replace("-","") + "' ");
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
       

        protected void btnGerarCsv_Click(object sender, EventArgs e)
        {
            var dataTable = _relatorioDUEsDAO.GerarCsv(GerarFiltro());        

            var agora = DateTime.Now;
            var nomeArquivo = Path.Combine(Server.MapPath("."), "Uploads", $"REL_DUES_{agora.ToString("ddMMyyyy")}_{agora.ToString("HHmmss")}.csv");

            var sw = new StreamWriter(nomeArquivo, false, Encoding.UTF8);
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

        protected void btnLimparFiltro_Click(object sender, EventArgs e)
        {
            this.txtEmissaoDUEDe.Text = string.Empty;
            this.txtEmissaoDueAte.Text = string.Empty;
            this.txtCnpjExportador.Text = string.Empty;
        }
    }
}