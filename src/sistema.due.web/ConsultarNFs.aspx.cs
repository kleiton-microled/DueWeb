using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Models;
using Sistema.DUE.Web.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using NLog;
using System.Diagnostics;

namespace Sistema.DUE.Web
{
    public partial class ConsultarNFs : System.Web.UI.Page
    {
        private readonly RecintosDAO _recintosDAO = new RecintosDAO();
        private readonly UnidadesReceitaDAO _unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly EstoqueDAO _estoqueDAO = new EstoqueDAO(false);
        static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["PROGRESSO"] = "0";
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

            HttpContext.Current.Session["PROGRESSO"] = "Fazendo Upload do Arquivo de Notas... Aguarde";

            if (!UploadArquivo(this.txtUpload))
            {
                throw new Exception("O arquivo não pode ser processado. Certifique-se que já não esteja aberto em outro programa");
            }

            var notasFiscais = ProcessarArquivo(this.txtUpload.PostedFile.InputStream, ";");

            ValidarNotasSiscomex(notasFiscais);

            this.btnGerarExcel.Visible = notasFiscais.Count > 0;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string ObterProgresso()
        {
            if (HttpContext.Current.Session["PROGRESSO"] != null)
            {
                return HttpContext.Current.Session["PROGRESSO"].ToString();
            }

            return "N/A";
        }

        private async void ValidarNotasSiscomex(List<NotaFiscalConsultaCCT> notasFiscais)
        {
            var contador = 0;

            var listaGrid = new List<ConsultaNfs>();
            var numeroNf = string.Empty;
            var cnpjNf = string.Empty;

            var totalNotas = notasFiscais.Count;
            var acumulador = 0;

            foreach (var linhaGrid in notasFiscais)
            {
                acumulador++;

                HttpContext.Current.Session["PROGRESSO"] = $"Processando {acumulador} de {totalNotas} Notas Fiscais. Aguarde...";

                var chaveNf = linhaGrid.ChaveNF.Replace(";", "");

                if (chaveNf.Length > 30)
                {
                    cnpjNf = chaveNf.Substring(6, 14);
                    numeroNf = chaveNf.Substring(25, 9);
                }

                var saldoNota = await ServicoSiscomex2.ConsultarDadosNotaPreACD(chaveNf, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                if (saldoNota != null)
                {
                    foreach (var linha in saldoNota)
                    {
                        contador += 1;

                        if (linha.Sucesso == false)
                        {
                            listaGrid.Add(new ConsultaNfs
                            {
                                Id = contador,
                                ChaveNF = chaveNf,
                                Observacoes = linha.Mensagem
                            });
                        }
                        else
                        {
                            string descricaoRecinto = string.Empty;
                            string descricaoUnidade = string.Empty;

                            if (linha.Recinto != null)
                            {
                                var recintoBusca = _recintosDAO.ObterRecintos()
                                    .Where(c => c.Id == linha.Recinto.ToInt()).FirstOrDefault();

                                if (recintoBusca != null)
                                {
                                    descricaoRecinto = recintoBusca.Descricao;
                                }
                            }
                            else
                            {
                                descricaoRecinto = "Indisponível";
                            }

                            if (linha.UnidadeReceita != null)
                            {
                                var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                    .Where(c => c.Codigo.ToInt() == linha.UnidadeReceita.ToInt()).FirstOrDefault();

                                if (unidadeBusca != null)
                                {
                                    descricaoUnidade = unidadeBusca.Descricao;
                                }
                            }
                            else
                            {
                                descricaoUnidade = "Indisponível";
                            }

                            var pesos = _estoqueDAO.ObterPesoCCT(linha.Recinto, linha.ResponsavelIdentificacao, numeroNf, "", "");

                            if (pesos != null)
                            {
                                listaGrid.Add(new ConsultaNfs
                                {
                                    Id = contador,
                                    DataRegistro = linha.Registro,
                                    SaldoCCT = linha.Saldo.ToString(),
                                    ChaveNF = chaveNf,
                                    Recinto = descricaoRecinto,
                                    UnidadeReceita = descricaoUnidade,
                                    Observacoes = linha.MotivoNaoPesagem,
                                    Item = linha.Item.ToString(),
                                    PesoEntradaCCT = pesos.PesoEntradaCCT,
                                    PesoAferido = linha.PesoAferido.ToString()
                                });
                            }
                            else
                            {
                                listaGrid.Add(new ConsultaNfs
                                {
                                    Id = contador,
                                    DataRegistro = linha.Registro,
                                    SaldoCCT = linha.Saldo.ToString(),
                                    ChaveNF = chaveNf,
                                    Recinto = descricaoRecinto,
                                    UnidadeReceita = descricaoUnidade,
                                    Observacoes = linha.MotivoNaoPesagem,
                                    Item = linha.Item.ToString(),
                                    PesoAferido = linha.PesoAferido.ToString()
                                });
                            }
                        }
                    }
                }
                else
                {
                    listaGrid.Add(new ConsultaNfs
                    {
                        Id = contador,
                        ChaveNF = chaveNf,
                        Observacoes = "Indisponibilidade Siscomex. Por favor, tente novamente após alguns minutos."
                    });
                }
            }

            this.gvNotasFiscais.DataSource = listaGrid.OrderBy(c => c.ChaveNF).ThenBy(c => c.Item).ToList();
            this.gvNotasFiscais.DataBind();

            foreach (GridViewRow linha in gvNotasFiscais.Rows)
            {
                if (linha.Cells[0].Text.Replace("&nbsp;", "") == string.Empty && linha.Cells[5].Text.Replace("&nbsp;", "") != string.Empty)
                {
                    linha.BackColor = System.Drawing.Color.MistyRose;
                }
            }
        }

        private List<NotaFiscalConsultaCCT> ProcessarArquivo(Stream arquivo, string delimitador)
        {
            List<NotaFiscalConsultaCCT> notasFiscais = new List<NotaFiscalConsultaCCT>();

            using (StreamReader reader = new StreamReader(arquivo))
            {
                string linha = string.Empty;

                try
                {
                    while ((linha = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(linha))
                        {
                            notasFiscais.Add(new NotaFiscalConsultaCCT
                            {
                                ChaveNF = linha.ToString(),
                                SaldoCCT = "0.0",
                                OBS = string.Empty
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return notasFiscais;
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
            var notas = new List<NotaFiscalConsultaCCT>();

            foreach (GridViewRow linhaGrid in this.gvNotasFiscais.Rows)
            {
                notas.Add(new NotaFiscalConsultaCCT
                {
                    DataRegistro = linhaGrid.Cells[0].Text.Replace("&nbsp;", ""),
                    ChaveNF = linhaGrid.Cells[1].Text,
                    SaldoCCT = linhaGrid.Cells[2].Text,
                    PesoEntradaCCT = linhaGrid.Cells[3].Text,
                    PesoAferido = linhaGrid.Cells[4].Text,
                    OBS = linhaGrid.Cells[5].Text.Replace("&nbsp;", ""),
                    Recinto = linhaGrid.Cells[6].Text.Replace("&nbsp;", ""),
                    UnidadeReceita = linhaGrid.Cells[7].Text.Replace("&nbsp;", ""),
                    Item = linhaGrid.Cells[8].Text.ToInt()
                });


            }

            ExcelPackage epackage = new ExcelPackage();
            ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("CCT");

            excel.Cells["A1"].LoadFromCollection(notas.Select(c => new
            {
                c.DataRegistro,
                c.ChaveNF,
                c.SaldoCCT,
                c.PesoEntradaCCT,
                c.PesoAferido,
                c.OBS,
                c.Recinto,
                c.UnidadeReceita,
                c.Item
            }), true);

            string attachment = $"attachment; filename=ArquivoNotas.xlsx";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            HttpContext.Current.Response.BinaryWrite(epackage.GetAsByteArray());

            HttpContext.Current.Response.End();
            epackage.Dispose();
        }

        [WebMethod]
        public static int ObterTotalNFsImportadas(string guid)
        {
            var _notaFiscalDAO = new NotaFiscalDAO();

            return _notaFiscalDAO.ObterTotalNFsImportadasPorGUID(guid);
        }

        [WebMethod]
        public static string ImportarNFs(List<string> chaves, string guid)
        {
            try
            {
                logger.Info($"{DateTime.Now} - Início consulta NF's: {String.Join(",", chaves)}");
                //ConsultaNfs cnf = new ConsultaNfs();
                var _notaFiscalDAOx = new NotaFiscalDAO();

                foreach (var chave in chaves)
                {
                    // Levi alterou aqui 08/01/2021 - pesquisava qualque coisa do txt. Se fizesse upload de arquivo errado poderia gerar milhares de consultas no siscomex.

                    string chaveaux = chave.Trim();
                    if (chaveaux.Length == 44)
                    {
                        var nf = new NotaFiscalConsultaCCT()
                        {
                            ChaveNF = chaveaux,
                            SaldoCCT = "0.0",
                            OBS = string.Empty
                        };

                        ValidarNotasSiscomexUnitario(nf, guid);
                    }
                    else
                    {
                        chaveaux = chaveaux + "                                                      ";
                        var nf = new NotaFiscalConsultaCCT()
                        {
                            ChaveNF = chaveaux.Substring(0, 44),
                            DataRegistro = System.DateTime.Now.ToString(),
                            SaldoCCT = "",
                            OBS = "Chave inválida, somente chaves de 44 dígitos podem se consutadas. Notas formulários não estão disponíveis no Siscomex."
                        };
                        _notaFiscalDAOx.CadastrarNFImportacaoConsulta(nf, guid);
                    }
                    // até aqui
                }
                logger.Info($"{DateTime.Now} - Término consulta NF's");
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível importar NFs");
            }
        }

        private static void ValidarNotasSiscomexUnitario(NotaFiscalConsultaCCT notaFiscalChave, string guid)
        {
            var _notaFiscalDAO = new NotaFiscalDAO();
            var _recintosDAO = new RecintosDAO();
            var _unidadesReceitaDAO = new UnidadesReceitaDAO();
            var _estoqueDAO = new EstoqueDAO(false);

            var numeroNf = string.Empty;
            var cnpjNf = string.Empty;
            var mesNf = string.Empty;
            var anoNf = string.Empty;

            var chaveNf = notaFiscalChave.ChaveNF.Replace(";", "");

            if (chaveNf.Length > 30)
            {
                cnpjNf = chaveNf.Substring(6, 14);
                numeroNf = chaveNf.Substring(25, 9);
                anoNf = chaveNf.Substring(2, 2);
                mesNf = chaveNf.Substring(4, 2);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var saldoNota = AsyncHelpers.RunSync<List<Responses.DadosNotaPreACD>>(() => ServicoSiscomex2.ConsultarDadosNotaPreACD(chaveNf, ConfigurationManager.AppSettings["CpfCertificado"].ToString()));

            stopWatch.Stop();
            LogsService.Logar("ConsultarNFs.aspx", $"Consulta Siscomex: {stopWatch.Elapsed}");

            if (saldoNota != null)
            {
                foreach (var linha in saldoNota)
                {
                    try
                    {
                        if (linha.Sucesso == false)
                        {
                            var informacoesDue = _estoqueDAO.ObterDadosDueConsCCT(chaveNf);

                            string due = "";
                            string qtdeAverbada = "";

                            if (informacoesDue != null)
                            {
                                if (informacoesDue.Any())
                                {
                                    due = string.Join("|", informacoesDue.Select(c => c.DUE));
                                    qtdeAverbada = string.Join("|", informacoesDue.Select(c => c.Quantidade));
                                }
                            }

                            var notaFiscal = new NotaFiscalConsultaCCT
                            {
                                ChaveNF = chaveNf,
                                OBS = linha.Mensagem,
                                DUE = due,
                                QtdeAverbada = qtdeAverbada
                            };

                            stopWatch.Reset();
                            stopWatch.Start();
                            _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid);
                            stopWatch.Stop();
                            LogsService.Logar("ConsultarNFs.aspx", $"_notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid): {stopWatch.Elapsed}");
                        }
                        else
                        {
                            string descricaoRecinto = string.Empty;
                            string descricaoUnidade = string.Empty;

                            if (linha.Recinto != null)
                            {
                                var recintoBusca = _recintosDAO.ObterRecintos()
                                    .Where(c => c.Id == linha.Recinto.ToInt()).FirstOrDefault();

                                if (recintoBusca != null)
                                {
                                    descricaoRecinto = recintoBusca.Descricao;
                                }
                            }
                            else
                            {
                                descricaoRecinto = "Indisponível";
                            }

                            if (linha.UnidadeReceita != null)
                            {
                                var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                    .Where(c => c.Codigo.ToInt() == linha.UnidadeReceita.ToInt()).FirstOrDefault();

                                if (unidadeBusca != null)
                                {
                                    descricaoUnidade = unidadeBusca.Descricao;
                                }
                            }
                            else
                            {
                                descricaoUnidade = "Indisponível";
                            }

                            stopWatch.Reset();
                            stopWatch.Start();

                            var pesos = _estoqueDAO.ObterPesoCCT(
                                linha.Recinto,
                                linha.ResponsavelIdentificacao,
                                numeroNf,
                                linha.Registro.Substring(0, 10),
                                $"{anoNf}{mesNf}");

                            var informacoesDue = _estoqueDAO.ObterDadosDueConsCCT(chaveNf);

                            string due = "";
                            string qtdeAverbada = "";

                            if (informacoesDue != null)
                            {
                                if (informacoesDue.Any())
                                {
                                    due = string.Join("|", informacoesDue.Select(c => c.DUE));
                                    qtdeAverbada = string.Join("|", informacoesDue.Select(c => c.Quantidade));
                                }
                            }

                            stopWatch.Stop();
                            LogsService.Logar("ConsultarNFs.aspx", $"_estoqueDAO.ObterPesoCCT(): {stopWatch.Elapsed}");

                            if (pesos != null)
                            {
                                var pesoAferido = !string.IsNullOrEmpty(pesos.PesoAferido)
                                    ? pesos.PesoAferido
                                    : "Não Disponível - ";

                                if (!string.IsNullOrEmpty(pesos.MotivoNaoPesagem))
                                {
                                    pesoAferido += "Motivo não pesagem: " + pesos.MotivoNaoPesagem;
                                }

                                var notaFiscal = new NotaFiscalConsultaCCT
                                {
                                    DataRegistro = linha.Registro,
                                    SaldoCCT = linha.Saldo.ToString(),
                                    ChaveNF = chaveNf,
                                    Recinto = descricaoRecinto,
                                    UnidadeReceita = descricaoUnidade,
                                    OBS = string.Empty,
                                    Item = linha.Item,
                                    PesoEntradaCCT = pesos.PesoEntradaCCT,
                                    PesoAferido = pesoAferido,
                                    DUE = due,
                                    QtdeAverbada = qtdeAverbada
                                };

                                stopWatch.Reset();
                                stopWatch.Start();

                                _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid);

                                stopWatch.Stop();
                                LogsService.Logar("ConsultarNFs.aspx", $"Ln 489 - _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid) {stopWatch.Elapsed}");

                            }
                            else
                            {
                                var informacoesDues = _estoqueDAO.ObterDadosDueConsCCT(chaveNf);

                                string nrdue = "";
                                string qtdeAverbadas = "";

                                if (informacoesDues != null)
                                {
                                    if (informacoesDues.Any())
                                    {
                                        due = string.Join("|", informacoesDues.Select(c => c.DUE));
                                        qtdeAverbadas = string.Join("|", informacoesDues.Select(c => c.Quantidade));
                                    }
                                }

                                var notaFiscal = new NotaFiscalConsultaCCT
                                {
                                    DataRegistro = linha.Registro,
                                    SaldoCCT = linha.Saldo.ToString(),
                                    ChaveNF = chaveNf,
                                    Recinto = descricaoRecinto,
                                    UnidadeReceita = descricaoUnidade,
                                    OBS = string.Empty,
                                    Item = linha.Item,
                                    PesoAferido = "Não Disponível",
                                    PesoEntradaCCT = "Não Disponível",
                                    DUE = nrdue,
                                    QtdeAverbada = qtdeAverbadas
                                };

                                stopWatch.Reset();
                                stopWatch.Start();

                                _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid);

                                stopWatch.Stop();
                                LogsService.Logar("ConsultarNFs.aspx", $"Ln 513 - _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid) {stopWatch.Elapsed}");

                            }
                        }
                    }
                    catch (Exception)
                    {

                        logger.Info($"{DateTime.Now} - Erro ao consultar NF {notaFiscalChave}");
                    }
                }
            }
            else
            {
                var notaFiscal = new NotaFiscalConsultaCCT
                {
                    ChaveNF = chaveNf,
                    OBS = "Indisponibilidade Siscomex. Por favor, tente novamente após alguns minutos."
                };

                stopWatch.Reset();
                stopWatch.Start();

                _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid);

                stopWatch.Stop();
                LogsService.Logar("ConsultarNFs.aspx", $"Ln 539 - _notaFiscalDAO.CadastrarNFImportacaoConsulta(notaFiscal, guid); {stopWatch.Elapsed}");

            }
        }

        protected void btnGerarExcelParcial_Click(object sender, EventArgs e)
        {
            var guid = hddnGUID.Value;
            if (string.IsNullOrEmpty(guid))
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um problema ao exportar arquivo");
            }

            if (!ModelState.IsValid)
                return;

            var _notaFiscalDAO = new NotaFiscalDAO();

            var notas = _notaFiscalDAO.ObterNFsImportadasPorGUID(guid);

            ExcelPackage epackage = new ExcelPackage();
            ExcelWorksheet excel = epackage.Workbook.Worksheets.Add("CCT");

            excel.Cells["A1"].LoadFromCollection(notas.Select(c => new
            {
                c.DataRegistro,
                c.ChaveNF,
                c.SaldoCCT,
                c.PesoEntradaCCT,
                c.PesoAferido,
                c.Observacoes,
                c.Recinto,
                c.UnidadeReceita,
                c.Item,
                c.DUE,
                c.QtdeAverbada
            }), true);

            string attachment = $"attachment; filename=ArquivoNotas.xlsx";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            HttpContext.Current.Response.BinaryWrite(epackage.GetAsByteArray());

            HttpContext.Current.Response.End();
            epackage.Dispose();
        }

        [WebMethod]
        public static string ExcluirGUID(string guid)
        {
            try
            {
                var _notaFiscalDAO = new NotaFiscalDAO();

                _notaFiscalDAO.ExcluiNFImportacaoConsulta(guid);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível importar NFs");
            }

        }
    }
}