using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ConsultarNFsDUE : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var id = Request.QueryString["id"].ToInt();

                if (id > 0)
                {
                    var dueBusca = _documentoUnicoExportacaoDAO.ObterDUEPorId(id);

                    if (dueBusca.CriadoPorNF > 0)
                    {
                        this.gvNotasFiscais.DataSource = _documentoUnicoExportacaoDAO.ObterNotasPorDueId(id);
                        this.gvNotasFiscais.DataBind();

                        foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
                        {
                            if (linhaGrid.Cells[0].Text != "EXP")
                            {                               
                                var chaveNf = linhaGrid.Cells[1].Text;
                                var quantidade = linhaGrid.Cells[4].Text.ToDecimal();

                                var saldoDue = _documentoUnicoExportacaoDAO.ObterQuantidadeDUEPorNF(chaveNf);

                                var saldoNota = await ServicoSiscomex2.ConsultarDadosNotaPreACD(chaveNf, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                                foreach (var linha in saldoNota)
                                {
                                    if (linha != null)
                                    {
                                        if (linha.Sucesso == false)
                                        {
                                            linhaGrid.BackColor = System.Drawing.Color.MistyRose;
                                        }
                                        else
                                        {
                                            quantidade = quantidade - saldoDue;

                                            if (!((quantidade) <= (decimal)linha.Saldo))
                                            {
                                                linhaGrid.BackColor = System.Drawing.Color.LightYellow;
                                            }
                                            else
                                            {
                                                linhaGrid.BackColor = System.Drawing.Color.LightGreen;
                                            }

                                            linhaGrid.Cells[5].Text = linha.Saldo.ToString();
                                            linhaGrid.Cells[6].Text = saldoDue.ToString();
                                            linhaGrid.Cells[11].Text = linha.Item.ToString();
                                            linhaGrid.Cells[12].Text = linha.Recinto;
                                        }
                                    }
                                }                                
                            }
                        }
                    }
                    else
                    {
                        List<NotaFiscal> notasExportacao = _documentoUnicoExportacaoDAO.ObterNotasExportacaoPorDueId(id).ToList();

                        if (notasExportacao.Count() == 0)
                        {
                            var itensDUE = _documentoUnicoExportacaoDAO.ObterItensDUE(id);

                            foreach (var itemDue in itensDUE)
                            {
                                var notaExportacao = _notaFiscalDAO.ObterNotasExportacaoPorChave(itemDue.NF);

                                if (notaExportacao != null)
                                    notasExportacao.Add(notaExportacao);
                            }
                        }

                        var notasGrid = new List<NotaFiscal>();

                        foreach (var notaExportacao in notasExportacao)
                        {
                            notasGrid.Add(notaExportacao);

                            var notasRemessa = _documentoUnicoExportacaoDAO.ObterNotasRemessaPorDueId(notaExportacao.ChaveNF).ToList();

                            foreach (var notaRemessa in notasRemessa)
                            {
                                notasGrid.Add(notaRemessa);
                            }
                        }

                        this.gvNotasFiscais.DataSource = notasGrid.ToList();
                        this.gvNotasFiscais.DataBind();

                        foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
                        {
                            if (linhaGrid.Cells[0].Text != "EXP")
                            {                               
                                var chaveNf = linhaGrid.Cells[1].Text;
                                var quantidade = linhaGrid.Cells[4].Text.ToDecimal();

                                var saldoDue = _documentoUnicoExportacaoDAO.ObterQuantidadeDUEPorNF(chaveNf);

                                var saldoNota = await ServicoSiscomex2.ConsultarDadosNotaPreACD(chaveNf, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                                foreach (var linha in saldoNota)
                                {
                                    if (linha != null)
                                    {
                                        if (linha.Sucesso == false)
                                        {
                                            linhaGrid.BackColor = System.Drawing.Color.MistyRose;
                                        }
                                        else
                                        {
                                            quantidade = quantidade - saldoDue;

                                            if (!((quantidade) <= (decimal)linha.Saldo))
                                            {
                                                linhaGrid.BackColor = System.Drawing.Color.LightYellow;
                                            }
                                            else
                                            {
                                                linhaGrid.BackColor = System.Drawing.Color.LightGreen;
                                            }

                                            linhaGrid.Cells[5].Text = linha.Saldo.ToString();
                                            linhaGrid.Cells[6].Text = saldoDue.ToString();
                                            linhaGrid.Cells[11].Text = linha.Item.ToString();
                                            linhaGrid.Cells[12].Text = linha.Recinto;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("Default.aspx"));
        }

        protected async void btnGerarExcel_Click(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"].ToInt();

            var notas = new List<NotaFiscalConsultaCCT>();

            foreach (GridViewRow linha in this.gvNotasFiscais.Rows)
            {
                string status = string.Empty;

                if (linha.Cells[0].Text != "EXP")
                {
                    var item = linha.Cells[11].Text.ToInt();

                    if (item == 0)
                        item = 1;

                    var saldoDue = _documentoUnicoExportacaoDAO.ObterQuantidadeDUEPorNF(linha.Cells[1].Text);

                    var saldoNota = await ServicoSiscomex2.ConsultarDadosNotaPreACD(linha.Cells[1].Text, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                    foreach (var linhaSaldo in saldoNota)
                    {
                        if (linhaSaldo != null)
                        {
                            if (linhaSaldo.Sucesso == false)
                            {
                                status = "Nota não encontrada no CCT";
                            }
                            else
                            {
                                var quantidade = linha.Cells[4].Text.ToDecimal() - saldoDue;

                                if (!((quantidade) <= (decimal)linhaSaldo.Saldo))
                                {
                                    status = "Nota com divergência no saldo";
                                }
                                else
                                {
                                    status = "Nota sem divergência";
                                }
                            }
                        }
                    }
                }

                notas.Add(new NotaFiscalConsultaCCT
                {
                    TipoNF = linha.Cells[0].Text,
                    ChaveNF = linha.Cells[1].Text,
                    NumeroNF = linha.Cells[2].Text,
                    CnpjNF = linha.Cells[3].Text,
                    QuantidadeNF = linha.Cells[4].Text.ToDecimal(),
                    SaldoCCT = linha.Cells[5].Text,
                    SaldoOutrasDUES = linha.Cells[6].Text.ToDecimal(),
                    UnidadeNF = linha.Cells[7].Text,
                    NCM = linha.Cells[8].Text,
                    ChaveNFReferencia = linha.Cells[9].Text,
                    Arquivo = linha.Cells[10].Text,
                    Status = status,
                    Item = linha.Cells[11].Text.ToInt(),
                    Recinto = linha.Cells[12].Text
                });

                status = string.Empty;
            }

            string attachment = "attachment; filename=MyExcelPage.xlsx";

            ExcelPackage epackage = new ExcelPackage();
            ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("ExcelTabName");

            excel.Cells["A1"].LoadFromCollection(notas.Select(c => new
            {
                c.TipoNF,
                c.ChaveNF,
                c.NumeroNF,
                c.CnpjNF,
                c.QuantidadeNF,
                c.SaldoCCT,
                c.SaldoOutrasDUES,
                c.UnidadeNF,
                c.NCM,
                ChaveNFReferencia = c.ChaveNFReferencia.Replace("&nbsp;",""),
                c.Arquivo,
                c.Status,
                c.Item,
                c.Recinto
            }), true);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            HttpContext.Current.Response.BinaryWrite(epackage.GetAsByteArray());

            HttpContext.Current.Response.End();
            epackage.Dispose();
        }
    }
}