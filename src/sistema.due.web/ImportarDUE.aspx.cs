using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI.WebControls;

namespace Sistema.DUE.Web
{
    public partial class ImportarDUE : System.Web.UI.Page
    {
        private readonly NotaFiscalDAO _notaFiscalDAO = new NotaFiscalDAO();
        private readonly DocumentoUnicoExportacaoDAO _documentoUnicoExportacaoDAO = new DocumentoUnicoExportacaoDAO();
        private readonly RecintosDAO _recintosDAO = new RecintosDAO();
        private readonly UnidadesReceitaDAO _unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly SituacaoDUEDAO _situacaoDUEDAO = new SituacaoDUEDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(ConfigurationManager.AppSettings["CpfCertificado"].ToString());

            var cert = ObterCertificado(ConfigurationManager.AppSettings["CpfCertificado"].ToString());

            Response.Write("Certificado: " + cert.Subject);
        }

        public static X509Certificate2 ObterCertificado(string cpf)
        {
            var certificado = ListarCertificadosInstalados(StoreLocation.LocalMachine)
                .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));

            if (certificado == null)
            {
                certificado = ListarCertificadosInstalados(StoreLocation.CurrentUser)
                    .FirstOrDefault(a => a.SubjectName.Name.Contains(cpf));
            }

            if (certificado == null)
                throw new Exception($"Certificado Digital de CPF {cpf} não encontrado");

            return certificado;
        }

        public static IEnumerable<X509Certificate2> ListarCertificadosInstalados(StoreLocation storeLocation)
        {
            var stores = new X509Store(StoreName.My, storeLocation);

            stores.Open(OpenFlags.ReadOnly);

            var certificadosInstalados = stores.Certificates;

            certificadosInstalados.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            certificadosInstalados.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

            stores.Close();

            var certificados = new List<X509Certificate2>();

            foreach (X509Certificate2 certificado in certificadosInstalados)
                yield return certificado;
        }

        protected void btnSair_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("Default.aspx"));
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            if (this.txtUpload.PostedFile == null)
                ModelState.AddModelError(string.Empty, "Nenhum arquivo informado");

            if (this.txtUpload.PostedFile.ContentLength == 0)
                ModelState.AddModelError(string.Empty, "Arquivo inválido");

            if (!this.txtUpload.PostedFile.FileName.ToUpper().EndsWith("TXT") && !this.txtUpload.PostedFile.FileName.ToUpper().EndsWith("CSV"))
                ModelState.AddModelError(string.Empty, "É permitido apenas importação de arquivos .txt");

            if (!ModelState.IsValid)
                return;

            if (!UploadArquivo(this.txtUpload))
            {
                throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
            }

            var dues = ProcessarArquivo(this.txtUpload.PostedFile.InputStream, ";");

            ValidarDUESSiscomex(dues);

            this.btnGerarExcel.Visible = dues.Count > 0;

            if (HttpContext.Current.Session["TOKEN"] != null)
            {
                HttpContext.Current.Session["TOKEN"] = null;
            }
        }

        private void ValidarDUESSiscomex(List<DUEMaster> dues)
        {
            this.gvNotasFiscais.DataSource = dues.ToList();
            this.gvNotasFiscais.DataBind();

            foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
            {
                var due = linhaGrid.Cells[0].Text.Replace(";", "");

                var dadosDue = ServicoSiscomex2.ObterDetalhesDUE(due, ConfigurationManager.AppSettings["CpfCertificado"].ToString()).Result;

                if (dadosDue.Sucesso == false)
                {
                    linhaGrid.Cells[1].Text = string.Empty;
                    linhaGrid.Cells[2].Text = string.Empty;
                    linhaGrid.Cells[3].Text = dadosDue.Mensagem;

                    linhaGrid.BackColor = System.Drawing.Color.MistyRose;
                }
                else
                {
                    var situacao = _situacaoDUEDAO.ObterSituacoesDUE()
                        .Where(c => c.Id == dadosDue.situacaoDUE).FirstOrDefault();

                    if (situacao != null)
                    {
                        linhaGrid.Cells[2].Text = situacao.Descricao;
                    }
                    else
                    {
                        linhaGrid.Cells[2].Text = string.Empty;
                    }

                    linhaGrid.Cells[1].Text = dadosDue.dataSituacaoDUE;
                }
            }
        }

        private List<DUEMaster> ProcessarArquivo(Stream arquivo, string delimitador)
        {
            List<DUEMaster> dues = new List<DUEMaster>();

            using (StreamReader reader = new StreamReader(arquivo))
            {
                string linha = string.Empty;

                try
                {
                    while ((linha = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(linha))
                        {
                            dues.Add(new DUEMaster
                            {
                                DUE = linha
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return dues;
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

        private bool DeletarArquivo(FileUpload arquivo)
        {
            try
            {
                string caminho = Path.Combine(Server.MapPath("Uploads"), this.txtUpload.PostedFile.FileName);

                if (System.IO.File.Exists(caminho))
                    System.IO.File.Delete(caminho);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void btnGerarExcel_Click(object sender, EventArgs e)
        {
            var dues = new List<DUEMaster>();

            foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
            {
                var due = linhaGrid.Cells[0].Text.Replace(";", "");

                var dadosDue = ServicoSiscomex2.ObterDetalhesDUE(due, ConfigurationManager.AppSettings["CpfCertificado"].ToString()).Result;

                if (dadosDue.Sucesso == false)
                {
                    linhaGrid.Cells[1].Text = string.Empty;
                    linhaGrid.Cells[2].Text = string.Empty;
                    linhaGrid.Cells[3].Text = dadosDue.Mensagem;

                    linhaGrid.BackColor = System.Drawing.Color.MistyRose;

                    dues.Add(new DUEMaster
                    {
                        DUE = due,
                        DescricaoSituacao = string.Empty,
                        DataSituacaoDUE = string.Empty,
                        StatusSiscomex = dadosDue.Mensagem
                    });
                }
                else
                {
                    var situacao = _situacaoDUEDAO.ObterSituacoesDUE()
                        .Where(c => c.Id == dadosDue.situacaoDUE).FirstOrDefault();

                    if (situacao != null)
                    {
                        linhaGrid.Cells[2].Text = situacao.Descricao;
                    }
                    else
                    {
                        linhaGrid.Cells[2].Text = string.Empty;
                    }

                    linhaGrid.Cells[1].Text = dadosDue.dataSituacaoDUE;

                    dues.Add(new DUEMaster
                    {
                        DUE = due,
                        DescricaoSituacao = linhaGrid.Cells[2].Text,
                        DataSituacaoDUE = linhaGrid.Cells[1].Text,
                        StatusSiscomex = string.Empty
                    });
                }
            }

            ExcelPackage epackage = new ExcelPackage();
            ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("CCT");

            excel.Cells["A1"].LoadFromCollection(dues.Select(c => new
            {
                c.DUE,
                c.DescricaoSituacao,
                c.DataSituacaoDUE,
                c.StatusSiscomex
            }), true);

            string attachment = "attachment; filename=ListaDUEs.xlsx";

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