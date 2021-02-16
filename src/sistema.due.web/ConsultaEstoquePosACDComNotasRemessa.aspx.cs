using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ConsultaEstoquePosACDComNotasRemessa : System.Web.UI.Page
    {
        private readonly EstoquePosACDDAO _estoqueDAO = new EstoquePosACDDAO();
        private readonly UnidadesReceitaDAO unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly RecintosDAO recintosDAO = new RecintosDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsPostBack)
            {
                ListarUnidadesRFB();
                //Consultar();
            }
        }

        protected void ListarUnidadesRFB()
        {
            this.cbUnidadeRFB.DataSource = unidadesReceitaDAO.ObterUnidadesRFB();
            this.cbUnidadeRFB.DataBind();

            this.cbUnidadeRFB.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        private void Consultar()
        {
            this.gvEstoque.DataSource = _estoqueDAO.ObterRegistrosEstoque(GerarFiltro());
            this.gvEstoque.DataBind();
        }

        private string GerarFiltro()
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtDataDUEDe.Text))
            {
                if (StringHelpers.IsDate(this.txtDataDUEDe.Text))
                {
                    filtro.Append(" AND A.DATA_DUE >= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtDataDUEDe.Text).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtDataDUEAte.Text))
            {
                if (StringHelpers.IsDate(this.txtDataDUEAte.Text))
                {
                    filtro.Append(" AND A.DATA_DUE <= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtDataDUEAte.Text).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtNCM.Text))
            {
                filtro.Append(" AND B.NCM = '" + this.txtNCM.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtCNPJEmitente.Text))
            {
                filtro.Append(" AND REPLACE(REPLACE(REPLACE(B.CNPJ_EMITENTE, '.', ''), '/', ''), '-', '')  = '" + this.txtCNPJEmitente.Text.RemoverCaracteresEspeciais() + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtNumeroNF.Text))
            {
                filtro.Append(" AND B.NUMERO = '" + this.txtNumeroNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtModeloNF.Text))
            {
                filtro.Append(" AND B.MODELO = '" + this.txtModeloNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.cbUnidadeRFB.Text))
            {
                filtro.Append(" AND A.PORTO = '" + this.cbUnidadeRFB.SelectedValue + "' ");
            }

            if (!string.IsNullOrEmpty(this.cbRecintoAduaneiroDespacho.Text))
            {
                filtro.Append(" AND A.RECINTO = '" + this.cbRecintoAduaneiroDespacho.SelectedValue + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtSerieNF.Text))
            {
                filtro.Append(" AND B.SERIE = '" + this.txtSerieNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtDUE.Text))
            {
                filtro.Append(" AND A.NUMERO_DUE = '" + this.txtDUE.Text.Replace("-", "") + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtChavesNF.Text))
            {
                var chaves = this.txtChavesNF.Text
                   .Split(',')
                   .Select(c => $"'{c.RemoverCaracteresEspeciais()}'");

                filtro.Append($" AND B.CHAVE_NF_EXPORTACAO IN ({string.Join(",", chaves)}) ");
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpDe.Text))
            {
                filtro.Append(" AND SUBSTRING(B.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(B.CHAVE_NF_EXPORTACAO, 5, 2) >= " + this.txtEmissaoNotaExpDe.Text.Replace("/", "") + " ");
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpAte.Text))
            {
                filtro.Append(" AND SUBSTRING(B.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(B.CHAVE_NF_EXPORTACAO, 5, 2) <= " + this.txtEmissaoNotaExpAte.Text.Replace("/", "") + " ");
            }

            var filtroNfs = string.Empty;

            if (this.txtUpload.PostedFile != null)
            {
                if (this.txtUpload.PostedFile.ContentLength > 0)
                {
                    if (!UploadArquivo(this.txtUpload))
                    {
                        throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
                    }

                    var notasFiscais = ProcessarArquivo(this.txtUpload.PostedFile.InputStream);

                    var notasFiscaisFiltro = new List<string>();

                    foreach (var nf in notasFiscais)
                    {
                        notasFiscaisFiltro.Add($"'{nf}'");
                    }

                    filtroNfs = string.Join(",", notasFiscaisFiltro);

                    if (!string.IsNullOrEmpty(filtroNfs))
                    {
                        Session["FILTRO_CHAVES_REMESSA_UPLOAD"] = filtroNfs;
                        filtro.Append($" AND C.CHAVE_ACESSO IN ({filtroNfs}) ");
                    }

                    try
                    {
                        File.Delete(Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName));
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                    {
                        filtro.Append($" AND C.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()}) ");
                    }
                }
            }

            return filtro.ToString();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
            {
                Session["FILTRO_CHAVES_REMESSA_UPLOAD"] = null;
            }

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
                        var filtro = new StringBuilder();

                        if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpDe.Text))
                        {
                            filtro.Append(" AND SUBSTRING(A.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(A.CHAVE_NF_EXPORTACAO, 5, 2) >= " + this.txtEmissaoNotaExpDe.Text.Replace("/", "") + " ");
                        }

                        if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpAte.Text))
                        {
                            filtro.Append(" AND SUBSTRING(A.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(A.CHAVE_NF_EXPORTACAO, 5, 2) <= " + this.txtEmissaoNotaExpAte.Text.Replace("/", "") + " ");
                        }

                        //if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                        //{
                        //    filtro.Append($" AND B.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()})");
                        //}

                        gvEstoqueDetalhes.DataSource = _estoqueDAO.ObterDetalhesEstoque(id, filtro.ToString());
                        gvEstoqueDetalhes.DataBind();
                    }
                }
            }
        }

        protected void gvEstoqueDetalhes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                GridView gvEstoqueDetalhes = (GridView)sender;

                if (gvEstoqueDetalhes != null)
                {
                    var id = gvEstoqueDetalhes.DataKeys[e.Row.RowIndex].Value.ToString().ToInt();

                    if (id > 0)
                    {
                        GridView gvNotasRemessa = (GridView)e.Row.FindControl("gvNotasRemessa");

                        if (gvNotasRemessa != null)
                        {
                            var filtro = new StringBuilder();

                            if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                            {
                                filtro.Append($" AND CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()})");
                            }

                            gvNotasRemessa.DataSource = _estoqueDAO.ObterNotasRemessaPosACD(id, filtro.ToString());
                            gvNotasRemessa.DataBind();
                        }
                    }
                }
            }
        }

        protected void btnGerarCsv_Click(object sender, EventArgs e)
        {
            var filtro = new StringBuilder();

            if (!string.IsNullOrEmpty(this.txtDataDUEDe.Text))
            {
                if (StringHelpers.IsDate(this.txtDataDUEDe.Text))
                {
                    filtro.Append(" AND B.DATA_DUE >= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtDataDUEDe.Text).ToString("dd/MM/yyyy") + " 00:00:00', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtDataDUEAte.Text))
            {
                if (StringHelpers.IsDate(this.txtDataDUEAte.Text))
                {
                    filtro.Append(" AND B.DATA_DUE <= CONVERT(DATETIME,'" + Convert.ToDateTime(this.txtDataDUEAte.Text).ToString("dd/MM/yyyy") + " 23:59:59', 103) ");
                }
            }

            if (!string.IsNullOrEmpty(this.txtNCM.Text))
            {
                filtro.Append(" AND A.NCM = '" + this.txtNCM.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtCNPJEmitente.Text))
            {
                filtro.Append(" AND REPLACE(REPLACE(REPLACE(A.CNPJ_EMITENTE, '.', ''), '/', ''), '-', '')  = '" + this.txtCNPJEmitente.Text.RemoverCaracteresEspeciais() + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtNumeroNF.Text))
            {
                filtro.Append(" AND A.NUMERO = '" + this.txtNumeroNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtModeloNF.Text))
            {
                filtro.Append(" AND A.MODELO = '" + this.txtModeloNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtSerieNF.Text))
            {
                filtro.Append(" AND A.SERIE = '" + this.txtSerieNF.Text + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtDUE.Text))
            {
                filtro.Append(" AND B.NUMERO_DUE = '" + this.txtDUE.Text.Replace("-", "") + "' ");
            }

            if (!string.IsNullOrEmpty(this.cbUnidadeRFB.Text))
            {
                filtro.Append(" AND B.PORTO = '" + this.cbUnidadeRFB.SelectedValue + "' ");
            }

            if (!string.IsNullOrEmpty(this.cbRecintoAduaneiroDespacho.Text))
            {
                filtro.Append(" AND B.RECINTO = '" + this.cbRecintoAduaneiroDespacho.SelectedValue + "' ");
            }

            if (!string.IsNullOrEmpty(this.txtChavesNF.Text))
            {
                var chaves = this.txtChavesNF.Text
                   .Split(',')
                   .Select(c => $"'{c.RemoverCaracteresEspeciais()}'");

                filtro.Append($" AND A.CHAVE_NF_EXPORTACAO IN ({string.Join(",", chaves)}) ");
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpDe.Text))
            {
                filtro.Append(" AND SUBSTRING(A.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(A.CHAVE_NF_EXPORTACAO, 5, 2) >= " + this.txtEmissaoNotaExpDe.Text.Replace("/", "") + " ");
            }

            if (!string.IsNullOrEmpty(this.txtEmissaoNotaExpAte.Text))
            {
                filtro.Append(" AND SUBSTRING(A.CHAVE_NF_EXPORTACAO, 3, 2) + SUBSTRING(A.CHAVE_NF_EXPORTACAO, 5, 2) <= " + this.txtEmissaoNotaExpAte.Text.Replace("/", "") + " ");
            }

            if (this.txtUpload.PostedFile != null)
            {
                if (this.txtUpload.PostedFile.ContentLength > 0)
                {
                    if (!UploadArquivo(this.txtUpload))
                    {
                        throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
                    }

                    var notasFiscais = ProcessarArquivo(this.txtUpload.PostedFile.InputStream);

                    var notasFiscaisFiltro = new List<string>();

                    foreach (var nf in notasFiscais)
                    {
                        notasFiscaisFiltro.Add($"'{nf}'");
                    }

                    var filtroNfs = string.Join(",", notasFiscaisFiltro);

                    if (!string.IsNullOrEmpty(filtroNfs))
                    {
                        Session["FILTRO_CHAVES_REMESSA_UPLOAD"] = filtroNfs;
                        filtro.Append($" AND E.CHAVE_ACESSO IN ({filtroNfs}) ");
                    }

                    try
                    {
                        File.Delete(Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName));
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                    {
                        filtro.Append($" AND E.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()}) ");
                    }
                }
            }

            var dataTable = _estoqueDAO.GerarCsvEstoque(filtro.ToString());

            StringBuilder builder = new StringBuilder();
            List<string> rows = new List<string>();

            List<string> colunas = new List<string>();

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.ColumnName != "AUTONUM")
                {
                    colunas.Add(column.ColumnName);
                }                
            }

            foreach (DataRow row in dataTable.Rows)
            {
                List<string> campos = new List<string>();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.ColumnName != "AUTONUM")
                    {
                        object item = row[column];
                        var texto = Regex.Replace(item.ToString(), @"\t|\n|\r", "");
                        campos.Add(texto);
                    }
                }

                rows.Add(string.Join(";", campos.ToArray()));

                if (this.chkRelComNotasRemessa.Checked)
                {
                    var filtroRem = new StringBuilder();

                    if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                    {
                        filtroRem.Append($" AND B.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()}) ");
                    }

                    var dataTableRemessa = _estoqueDAO.GerarCsvEstoqueRemessa(row.ItemArray[0].ToString().ToInt(), filtroRem.ToString());

                    foreach (DataRow rowRemessa in dataTableRemessa.Rows)
                    {
                        List<string> camposRemessa = new List<string>();

                        foreach (DataColumn column in dataTableRemessa.Columns)
                        {
                            if (column.ColumnName != "AUTONUM")
                            {
                                object item = rowRemessa[column];
                                var texto = Regex.Replace(item.ToString(), @"\t|\n|\r", "");
                                camposRemessa.Add(texto);
                            }
                        }

                        rows.Add(string.Join(";", camposRemessa.ToArray()));
                    }
                }
            }

            builder.Append(string.Join(";", colunas.ToArray()));
            builder.Append("\n");
            builder.Append(string.Join("\n", rows.ToArray()));

            var agora = DateTime.Now;
            var nomeArquivo = $"LM_{agora.ToString("ddMMyyyy")}_{agora.ToString("HHmmss")}";

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", $"attachment;filename={nomeArquivo}.csv");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
            Response.Write(builder.ToString());
            Response.End();
        }

        protected void btnLimparFiltro_Click(object sender, EventArgs e)
        {
            this.txtCNPJEmitente.Text = string.Empty;
            this.txtDataDUEDe.Text = string.Empty;
            this.txtDataDUEAte.Text = string.Empty;
            this.txtModeloNF.Text = string.Empty;
            this.txtNCM.Text = string.Empty;
            this.txtNumeroNF.Text = string.Empty;
            this.txtSerieNF.Text = string.Empty;
            this.txtChavesNF.Text = string.Empty;
            this.cbUnidadeRFB.SelectedIndex = -1;
            this.cbRecintoAduaneiroDespacho.SelectedIndex = -1;
        }

        protected void ListarRecintos(int unidadeRfb)
        {
            this.cbRecintoAduaneiroDespacho.DataSource = recintosDAO.ObterRecintos(unidadeRfb);
            this.cbRecintoAduaneiroDespacho.DataBind();

            this.cbRecintoAduaneiroDespacho.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void cbUnidadeRFB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbUnidadeRFB.SelectedValue != null)
            {
                ListarRecintos(this.cbUnidadeRFB.SelectedValue.ToInt());
            }
        }

        private bool UploadArquivo(FileUpload arquivo)
        {
            string nomeArquivo = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

            try
            {
                arquivo.SaveAs(nomeArquivo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private List<string> ProcessarArquivo(Stream arquivo)
        {
            List<string> notasFiscais = new List<string>();

            using (StreamReader reader = new StreamReader(arquivo))
            {
                string linha = string.Empty;

                while ((linha = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(linha))
                    {
                        notasFiscais.Add(linha);
                    }

                }
            }

            return notasFiscais;
        }
    }
}