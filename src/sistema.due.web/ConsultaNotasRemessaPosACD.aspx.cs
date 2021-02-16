using Sistema.DUE.Web.DAO;
using Sistema.DUE.Web.DTO;
using Sistema.DUE.Web.Extensions;
using Sistema.DUE.Web.Helpers;
using Sistema.DUE.Web.Responses;
using Sistema.DUE.Web.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Sistema.DUE.Web
{ 
    public partial class ConsultaNotasRemessaPosACD : System.Web.UI.Page
    {
        private readonly EstoquePosACDDAO _estoqueDAO = new EstoquePosACDDAO();
        private readonly RecintosDAO _recintosDAO = new RecintosDAO();
        private readonly UnidadesReceitaDAO _unidadesReceitaDAO = new UnidadesReceitaDAO();
        private readonly MoedaDAO _moedasDAO = new MoedaDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logado"] == null)
                Response.Redirect("Login.aspx");

            if (!Page.IsPostBack)
            {
                //Consultar();
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

                        if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
                        {
                            filtro.Append($" AND B.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()})");
                        }

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

        protected async void btnGerarCsv_Click(object sender, EventArgs e)
        {
            var filtro = new StringBuilder();
            var paises = PaisesDAO.ObterPaisesSiscomex();

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

            var filtroRem = new StringBuilder();
            var listaNotasArquivo = new List<string>();
            var listaNotasConsulta = new List<string>();
            var listaNotasDiferencas = new List<string>();

            if (Session["FILTRO_CHAVES_REMESSA_UPLOAD"] != null)
            {
                filtroRem.Append($" AND B.CHAVE_ACESSO IN ({Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString()}) ");

                listaNotasArquivo.AddRange(Session["FILTRO_CHAVES_REMESSA_UPLOAD"].ToString().Split(','));
            }

            var dataTableRemessa = _estoqueDAO.ObterNotasRemessa(filtroRem.ToString());

            foreach (DataRow rowRemessa in dataTableRemessa.Rows)
            {
                listaNotasConsulta.Add(rowRemessa["CHAVE_NF"].ToString());
            }

            listaNotasConsulta = listaNotasConsulta.Distinct().ToList();

            foreach (var chaveNfArquivo in listaNotasArquivo)
            {
                var nf = chaveNfArquivo.Replace("'", "");

                if (!listaNotasConsulta.Contains(nf))
                {
                    listaNotasDiferencas.Add(nf);
                }
            }

            StringBuilder builder = new StringBuilder();
            List<string> rows = new List<string>();

            List<string> colunas = new List<string>();

            foreach (DataColumn column in dataTableRemessa.Columns)
            {
                colunas.Add(column.ColumnName);
            }

            foreach (DataRow rowRemessa in dataTableRemessa.Rows)
            {
                List<string> camposRemessa = new List<string>();

                foreach (DataColumn column in dataTableRemessa.Columns)
                {
                    object item = rowRemessa[column];
                    var texto = Regex.Replace(item.ToString(), @"\t|\n|\r", "");
                    camposRemessa.Add(texto);
                }

                rows.Add(string.Join(";", camposRemessa.ToArray()));
            }

            var duesImportadas = new List<string>();

            if (listaNotasDiferencas.Any())
            {
                rows.Add("As notas abaixo não formam encontradas no banco de dados de DUEs LDC e os dados apresentados foram consultados diretamente no SISCOMEX");

                foreach (var notaNaoEncontrada in listaNotasDiferencas)
                {
                    var dadosDue = await ServicoSiscomex2
                        .ConsultarDUEPorDanfe(notaNaoEncontrada, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                    if (dadosDue != null)
                    {
                        if (dadosDue.Sucesso)
                        {
                            foreach (var due in dadosDue?.ListaDadosDUE)
                            {
                                var retornoDue = await ServicoSiscomex2.ObterDUECompleta(due.rel, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                                if (retornoDue != null)
                                {
                                    if (!duesImportadas.Contains(due.rel))
                                    {
                                        duesImportadas.Add(due.rel);

                                        var declarante = retornoDue.declarante?.numeroDoDocumento ?? string.Empty;
                                        var declaranteNome = retornoDue.declarante?.nome ?? string.Empty;
                                        var descricaoRecinto = string.Empty;
                                        var descricaoUnidade = string.Empty;

                                        if (retornoDue.recintoAduaneiroDeDespacho != null)
                                        {
                                            var recintoBusca = _recintosDAO.ObterRecintos()
                                                .Where(c => c.Id == retornoDue.recintoAduaneiroDeDespacho?.codigo.ToInt()).FirstOrDefault();

                                            if (recintoBusca != null)
                                            {
                                                descricaoRecinto = recintoBusca.Descricao;
                                            }
                                        }
                                        else
                                        {
                                            descricaoRecinto = "Indisponível";
                                        }

                                        if (retornoDue.unidadeLocalDeDespacho != null)
                                        {
                                            var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                                .Where(c => c.Codigo.ToInt() == retornoDue.unidadeLocalDeDespacho?.codigo.ToInt()).FirstOrDefault();

                                            if (unidadeBusca != null)
                                            {
                                                descricaoUnidade = unidadeBusca.Descricao;
                                            }
                                        }
                                        else
                                        {
                                            descricaoUnidade = "Indisponível";
                                        }

                                        var ultimoEvento = string.Empty;
                                        EventosDoHistorico averbacao;
                                        var dataAverbacao = string.Empty;
                                        DateTime? dataUltimoEvento = null;

                                        if (retornoDue.eventosDoHistorico != null)
                                        {
                                            ultimoEvento = retornoDue.eventosDoHistorico.OrderByDescending(c => c.dataEHoraDoEvento).Select(c => c.evento).FirstOrDefault();
                                            dataUltimoEvento = retornoDue.eventosDoHistorico.OrderByDescending(c => c.dataEHoraDoEvento).Select(c => c.dataEHoraDoEvento).FirstOrDefault();

                                            averbacao = retornoDue.eventosDoHistorico
                                               .FirstOrDefault(c => c.evento.ToLower().Contains("averbacao") || c.evento.ToLower().Contains("averbação"));

                                            dataAverbacao = averbacao != null
                                                    ? averbacao.dataEHoraDoEvento.ToString("dd/MM/yyyy HH:mm:ss")
                                                    : string.Empty;
                                        }

                                        if (retornoDue.itens != null)
                                        {
                                            foreach (var item in retornoDue.itens)
                                            {
                                                string valorDaMercadoriaNaCondicaoDeVenda = item?.valorDaMercadoriaNaCondicaoDeVenda.ToString() ?? "0";
                                                string valorDaMercadoriaNoLocalDeEmbarque = item?.valorDaMercadoriaNoLocalDeEmbarque.ToString() ?? "0";
                                                string valorDaMercadoriaNoLocalDeEmbarqueEmReais = item?.valorDaMercadoriaNoLocalDeEmbarqueEmReais.ToString() ?? "0";

                                                string importador = item?.nomeImportador ?? string.Empty;
                                                string enderecoImportador = item?.enderecoImportador ?? string.Empty;
                                                string pesoLiquidoTotal = item?.pesoLiquidoTotal.ToString() ?? "0";
                                                pesoLiquidoTotal = pesoLiquidoTotal.Replace(".", "").Replace(",", ".");
                                                string incoterm = item?.codigoCondicaoVenda.codigo ?? string.Empty;

                                                var codigoPaisImportador = retornoDue?.paisImportador?.codigo ?? 0;
                                                string paisImportador = paises.Where(c => c.Id == codigoPaisImportador)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                var codigoPaisDestino = item?.listaPaisDestino.Select(c => c.codigo).FirstOrDefault() ?? 0;
                                                string paisDestino = paises.Where(c => c.Id == codigoPaisDestino)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                string unidadeEstatistica = item?.unidadeComercializada ?? string.Empty;
                                                string informacoesComplementares = retornoDue?.informacoesComplementares ?? string.Empty;

                                                var codigoMoeda = retornoDue?.moeda?.codigo ?? 0;
                                                string moedaNegociacao = _moedasDAO.ObterMoedas().Where(c => c.Id == codigoMoeda)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                foreach (var nfRemessa in item.itensDaNotaDeRemessa)
                                                {
                                                    if (nfRemessa != null)
                                                    {
                                                        var tipoNf = string.Empty;
                                                        var cfop = nfRemessa.cfop.ToString();

                                                        if (cfop.EndsWith("04") || cfop.EndsWith("05"))
                                                        {
                                                            tipoNf = "FDL";
                                                        }

                                                        if (cfop.EndsWith("01") || cfop.EndsWith("02"))
                                                        {
                                                            tipoNf = "REM";
                                                        }

                                                        var itemNfRef = nfRemessa.notaFiscal;
                                                        var emitente = itemNfRef.identificacaoDoEmitente;

                                                        rows.Add($";{retornoDue.dataDeRegistro.ToString("dd/MM/yyyy HH:mm:ss")};{retornoDue.numero};{retornoDue.chaveDeAcesso};{declarante};{declaranteNome};{ultimoEvento};{dataUltimoEvento?.ToString("dd/MM/yyyy HH:mm:ss")};{dataAverbacao};{nfRemessa.ncm?.codigo};{nfRemessa.numeroDoItem};{nfRemessa.cfop};{nfRemessa.descricao};{tipoNf};{itemNfRef.chaveDeAcesso};{itemNfRef.numeroDoDocumento};{itemNfRef.modelo};{itemNfRef.serie};{itemNfRef.ufDoEmissor};{emitente.numero};{retornoDue.situacao};{valorDaMercadoriaNaCondicaoDeVenda.Replace(".", "").Replace(",", ".") };{valorDaMercadoriaNoLocalDeEmbarque.Replace(".", "").Replace(",", ".") };{valorDaMercadoriaNoLocalDeEmbarqueEmReais.Replace(".", "").Replace(",", ".") };{importador};{enderecoImportador};{paisImportador};{paisDestino};{unidadeEstatistica};{pesoLiquidoTotal};{moedaNegociacao};{incoterm};{informacoesComplementares};{retornoDue.unidadeLocalDeDespacho?.codigo};{descricaoUnidade};{retornoDue.recintoAduaneiroDeDespacho?.codigo };{descricaoRecinto};{item.quantidadeNaUnidadeEstatistica.ToString("n3")};{nfRemessa.quantidadeConsumida.ToString("n3")}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var retornoDueResumida = await ServicoSiscomex2.ObterDetalhesDUE(due.rel, ConfigurationManager.AppSettings["CpfCertificado"].ToString());

                                    if (retornoDueResumida != null)
                                    {
                                        if (!duesImportadas.Contains(due.rel))
                                        {
                                            duesImportadas.Add(due.rel);

                                            var declarante = retornoDueResumida.declarante?.numero ?? string.Empty;
                                            var descricaoRecinto = string.Empty;
                                            var descricaoUnidade = string.Empty;

                                            if (retornoDueResumida.codigoRecintoAduaneiroDespacho != null)
                                            {
                                                var recintoBusca = _recintosDAO.ObterRecintos()
                                                    .Where(c => c.Id == retornoDueResumida.codigoRecintoAduaneiroDespacho.ToInt()).FirstOrDefault();

                                                if (recintoBusca != null)
                                                {
                                                    descricaoRecinto = recintoBusca.Descricao;
                                                }
                                            }
                                            else
                                            {
                                                descricaoRecinto = "Indisponível";
                                            }

                                            if (retornoDueResumida.uaDespacho != null)
                                            {
                                                var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                                    .Where(c => c.Codigo.ToInt() == retornoDueResumida.uaDespacho.ToInt()).FirstOrDefault();

                                                if (unidadeBusca != null)
                                                {
                                                    descricaoUnidade = unidadeBusca.Descricao;
                                                }
                                            }
                                            else
                                            {
                                                descricaoUnidade = "Indisponível";
                                            }

                                            rows.Add($";;{retornoDueResumida.numeroDUE};;{declarante};;;;;;;;;;{notaNaoEncontrada};;;;;;;;;;;;;;;;;;;{retornoDueResumida.uaDespacho};{descricaoUnidade};{retornoDueResumida.codigoRecintoAduaneiroDespacho };{descricaoRecinto}");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            rows.Add(dadosDue.Mensagem);
                        }
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
            //Response.End();

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
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

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
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

        private static void ConsultaPosACDAsync(List<string> notasFiscaisFiltro, string guid, int usuarioId, string sessionFiltroChavesRemessaUpload)
        {
            var _notaFiscalDAO = new NotaFiscalDAO();
            var _estoqueDAO = new EstoquePosACDDAO();
            var _recintosDAO = new RecintosDAO();
            var _unidadesReceitaDAO = new UnidadesReceitaDAO();
            var _moedasDAO = new MoedaDAO();
            var filtro = new StringBuilder();
            var paises = PaisesDAO.ObterPaisesSiscomex();

            if (notasFiscaisFiltro.Count > 0)
            {
                var filtroNfs = string.Join(",", notasFiscaisFiltro);

                if (!string.IsNullOrEmpty(filtroNfs))
                {
                    sessionFiltroChavesRemessaUpload = filtroNfs;
                    filtro.Append($" AND E.CHAVE_ACESSO IN ({filtroNfs}) ");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(sessionFiltroChavesRemessaUpload))
                {
                    filtro.Append($" AND E.CHAVE_ACESSO IN ({sessionFiltroChavesRemessaUpload}) ");
                }
            }

            var filtroRem = new StringBuilder();
            var listaNotasArquivo = new List<string>();
            var listaNotasConsulta = new List<string>();
            var listaNotasDiferencas = new List<string>();

            if (!string.IsNullOrEmpty(sessionFiltroChavesRemessaUpload))
            {
                filtroRem.Append($" AND B.CHAVE_ACESSO IN ({sessionFiltroChavesRemessaUpload}) ");

                listaNotasArquivo.AddRange(sessionFiltroChavesRemessaUpload.Split(','));
            }

            var dataTableRemessa = _estoqueDAO.ObterNotasRemessa(filtroRem.ToString());

            foreach (DataRow rowRemessa in dataTableRemessa.Rows)
            {
                listaNotasConsulta.Add(rowRemessa["CHAVE_NF"].ToString());
            }

            listaNotasConsulta = listaNotasConsulta.Distinct().ToList();

            foreach (var chaveNfArquivo in listaNotasArquivo)
            {
                var nf = chaveNfArquivo.Replace("'", "");

                if (!listaNotasConsulta.Contains(nf))
                {
                    listaNotasDiferencas.Add(nf);
                }
            }

            List<string> colunas = new List<string>();

            foreach (DataColumn column in dataTableRemessa.Columns)
            {
                colunas.Add(column.ColumnName);
            }

            foreach (DataRow rowRemessa in dataTableRemessa.Rows)
            {
                List<string> camposRemessa = new List<string>();

                foreach (DataColumn column in dataTableRemessa.Columns)
                {
                    object item = rowRemessa[column];
                    var texto = Regex.Replace(item.ToString(), @"\t|\n|\r", "");
                    camposRemessa.Add(texto);
                }

                var camposNFRemessa = new NFRemessaPosACD();
                camposNFRemessa.CamposRemessa = string.Join(";", camposRemessa.ToArray());
                camposNFRemessa.SessaoHash = guid;
                camposNFRemessa.UsuarioId = usuarioId;

                camposNFRemessa.DataDUE = camposRemessa.ElementAt(0);
                camposNFRemessa.DUE = camposRemessa.ElementAt(1);
                camposNFRemessa.ChaveDUE = camposRemessa.ElementAt(2);
                camposNFRemessa.DeclaranteCnpj = camposRemessa.ElementAt(3);
                camposNFRemessa.DeclaranteNome = camposRemessa.ElementAt(4);
                camposNFRemessa.UltimoEvento = camposRemessa.ElementAt(5);
                camposNFRemessa.DataUltimoEvento = camposRemessa.ElementAt(6);
                camposNFRemessa.DataAverbacao = camposRemessa.ElementAt(7);
                camposNFRemessa.ItemDUE = camposRemessa.ElementAt(8);
                camposNFRemessa.NCM = camposRemessa.ElementAt(9);
                camposNFRemessa.ItemNF = camposRemessa.ElementAt(10);
                camposNFRemessa.CFOP = camposRemessa.ElementAt(11);
                camposNFRemessa.DESCRICAO = camposRemessa.ElementAt(12);
                camposNFRemessa.TipoNF = camposRemessa.ElementAt(13);
                camposNFRemessa.ChaveNF = camposRemessa.ElementAt(14);
                camposNFRemessa.NUMERO = camposRemessa.ElementAt(15);
                camposNFRemessa.MODELO = camposRemessa.ElementAt(16);
                camposNFRemessa.SERIE = camposRemessa.ElementAt(17);
                camposNFRemessa.UF = camposRemessa.ElementAt(19);
                camposNFRemessa.CNPJEmitente = camposRemessa.ElementAt(20);
                camposNFRemessa.EventoAverbacao = camposRemessa.ElementAt(21);
                camposNFRemessa.VMLE = camposRemessa.ElementAt(21);
                camposNFRemessa.ValorEmReais = camposRemessa.ElementAt(22);
                camposNFRemessa.VMCV = camposRemessa.ElementAt(23);
                camposNFRemessa.IMPORTADOR = camposRemessa.ElementAt(24);
                camposNFRemessa.ImportadorEndereco = camposRemessa.ElementAt(25);
                camposNFRemessa.ImportadorPais = camposRemessa.ElementAt(26);
                camposNFRemessa.PaisDestino = camposRemessa.ElementAt(27);
                camposNFRemessa.Unidade = camposRemessa.ElementAt(28);
                camposNFRemessa.PesoLiquidoTotal = camposRemessa.ElementAt(29);
                camposNFRemessa.Moeda = camposRemessa.ElementAt(30);
                camposNFRemessa.INCOTERM = camposRemessa.ElementAt(31);
                camposNFRemessa.InformacoesComplementares = camposRemessa.ElementAt(32);
                camposNFRemessa.UnidadeRFB = camposRemessa.ElementAt(33);
                camposNFRemessa.DescricaoUnidadeRFB = camposRemessa.ElementAt(34);
                camposNFRemessa.RecintoDespacho = camposRemessa.ElementAt(35);
                camposNFRemessa.DescricaoRecintoDespacho = camposRemessa.ElementAt(36);
                camposNFRemessa.QuantidadeNF = camposRemessa.ElementAt(37);
                camposNFRemessa.QuantidadeAverbada = camposRemessa.ElementAt(38);
                camposNFRemessa.RUC = camposRemessa.ElementAt(40);

                _notaFiscalDAO.CadastrarNFPosADC(camposNFRemessa);
            }

            var duesImportadas = new List<string>();

            if (listaNotasDiferencas.Any())
            {
                foreach (var notaNaoEncontrada in listaNotasDiferencas)
                {
                    var dadosDue = AsyncHelpers.RunSync<DadosDUE>(() => ServicoSiscomex2.ConsultarDUEPorDanfe(notaNaoEncontrada, ConfigurationManager.AppSettings["CpfCertificado"].ToString()));

                    if (dadosDue != null)
                    {
                        if (dadosDue.Sucesso)
                        {
                            foreach (var due in dadosDue?.ListaDadosDUE)
                            {
                                var retornoDue = AsyncHelpers.RunSync<DueDadosCompletos>(() => ServicoSiscomex2.ObterDUECompleta(due.rel, ConfigurationManager.AppSettings["CpfCertificado"].ToString()));

                                if (retornoDue != null)
                                {
                                    if (!duesImportadas.Contains(due.rel))
                                    {
                                        duesImportadas.Add(due.rel);

                                        var declarante = retornoDue.declarante?.numeroDoDocumento ?? string.Empty;
                                        var declaranteNome = retornoDue.declarante?.nome ?? string.Empty;
                                        var descricaoRecinto = string.Empty;
                                        var descricaoUnidade = string.Empty;

                                        if (retornoDue.recintoAduaneiroDeDespacho != null)
                                        {
                                            var recintoBusca = _recintosDAO.ObterRecintos()
                                                .Where(c => c.Id == retornoDue.recintoAduaneiroDeDespacho?.codigo.ToInt()).FirstOrDefault();

                                            if (recintoBusca != null)
                                            {
                                                descricaoRecinto = recintoBusca.Descricao;
                                            }
                                        }
                                        else
                                        {
                                            descricaoRecinto = "Indisponível";
                                        }

                                        if (retornoDue.unidadeLocalDeDespacho != null)
                                        {
                                            var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                                .Where(c => c.Codigo.ToInt() == retornoDue.unidadeLocalDeDespacho?.codigo.ToInt()).FirstOrDefault();

                                            if (unidadeBusca != null)
                                            {
                                                descricaoUnidade = unidadeBusca.Descricao;
                                            }
                                        }
                                        else
                                        {
                                            descricaoUnidade = "Indisponível";
                                        }

                                        var ultimoEvento = string.Empty;
                                        EventosDoHistorico averbacao;
                                        var dataAverbacao = string.Empty;
                                        DateTime? dataUltimoEvento = null;

                                        if (retornoDue.eventosDoHistorico != null)
                                        {
                                            ultimoEvento = retornoDue.eventosDoHistorico.OrderByDescending(c => c.dataEHoraDoEvento).Select(c => c.evento).FirstOrDefault();
                                            dataUltimoEvento = retornoDue.eventosDoHistorico.OrderByDescending(c => c.dataEHoraDoEvento).Select(c => c.dataEHoraDoEvento).FirstOrDefault();

                                            averbacao = retornoDue.eventosDoHistorico
                                               .FirstOrDefault(c => c.evento.ToLower().Contains("averbacao") || c.evento.ToLower().Contains("averbação"));

                                            dataAverbacao = averbacao != null
                                                    ? averbacao.dataEHoraDoEvento.ToString("dd/MM/yyyy HH:mm:ss")
                                                    : string.Empty;
                                        }

                                        if (retornoDue.itens != null)
                                        {
                                            foreach (var item in retornoDue.itens)
                                            {
                                                string valorDaMercadoriaNaCondicaoDeVenda = item?.valorDaMercadoriaNaCondicaoDeVenda.ToString() ?? "0";
                                                string valorDaMercadoriaNoLocalDeEmbarque = item?.valorDaMercadoriaNoLocalDeEmbarque.ToString() ?? "0";
                                                string valorDaMercadoriaNoLocalDeEmbarqueEmReais = item?.valorDaMercadoriaNoLocalDeEmbarqueEmReais.ToString() ?? "0";

                                                string importador = item?.nomeImportador ?? string.Empty;
                                                string enderecoImportador = item?.enderecoImportador ?? string.Empty;
                                                string pesoLiquidoTotal = item?.pesoLiquidoTotal.ToString() ?? "0";
                                                pesoLiquidoTotal = pesoLiquidoTotal.Replace(".", "").Replace(",", ".");
                                                string incoterm = item?.codigoCondicaoVenda.codigo ?? string.Empty;

                                                var codigoPaisImportador = retornoDue?.paisImportador?.codigo ?? 0;
                                                string paisImportador = paises.Where(c => c.Id == codigoPaisImportador)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                var codigoPaisDestino = item?.listaPaisDestino.Select(c => c.codigo).FirstOrDefault() ?? 0;
                                                string paisDestino = paises.Where(c => c.Id == codigoPaisDestino)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                string unidadeEstatistica = item?.unidadeComercializada ?? string.Empty;
                                                string informacoesComplementares = retornoDue?.informacoesComplementares ?? string.Empty;

                                                var codigoMoeda = retornoDue?.moeda?.codigo ?? 0;
                                                string moedaNegociacao = _moedasDAO.ObterMoedas().Where(c => c.Id == codigoMoeda)
                                                    .Select(c => c.Descricao).FirstOrDefault() ?? string.Empty;

                                                foreach (var nfRemessa in item.itensDaNotaDeRemessa)
                                                {
                                                    if (nfRemessa != null)
                                                    {
                                                        if (listaNotasDiferencas.Contains(nfRemessa?.notaFiscal?.chaveDeAcesso))
                                                        {



                                                            var tipoNf = string.Empty;
                                                            var cfop = nfRemessa.cfop.ToString();

                                                            if (cfop.EndsWith("04") || cfop.EndsWith("05"))
                                                            {
                                                                tipoNf = "FDL";
                                                            }

                                                            if (cfop.EndsWith("01") || cfop.EndsWith("02"))
                                                            {
                                                                tipoNf = "REM";
                                                            }

                                                            var itemNfRef = nfRemessa.notaFiscal;
                                                            var emitente = itemNfRef.identificacaoDoEmitente;

                                                            var camposNFRemessa = new NFRemessaPosACD();
                                                            camposNFRemessa.CamposRemessa = $";{retornoDue.dataDeRegistro.ToString("dd/MM/yyyy HH:mm:ss")};{retornoDue.numero};{retornoDue.chaveDeAcesso};{declarante};{declaranteNome};{ultimoEvento};{dataUltimoEvento?.ToString("dd/MM/yyyy HH:mm:ss")};{dataAverbacao};{nfRemessa.ncm?.codigo};{nfRemessa.numeroDoItem};{nfRemessa.cfop};{nfRemessa.descricao};{tipoNf};{itemNfRef.chaveDeAcesso};{itemNfRef.numeroDoDocumento};{itemNfRef.modelo};{itemNfRef.serie};{itemNfRef.ufDoEmissor};{emitente.numero};{retornoDue.situacao};{valorDaMercadoriaNaCondicaoDeVenda.Replace(".", "").Replace(",", ".") };{valorDaMercadoriaNoLocalDeEmbarque.Replace(".", "").Replace(",", ".") };{valorDaMercadoriaNoLocalDeEmbarqueEmReais.Replace(".", "").Replace(",", ".") };{importador};{enderecoImportador};{paisImportador};{paisDestino};{unidadeEstatistica};{pesoLiquidoTotal};{moedaNegociacao};{incoterm};{informacoesComplementares};{retornoDue.unidadeLocalDeDespacho?.codigo};{descricaoUnidade};{retornoDue.recintoAduaneiroDeDespacho?.codigo };{descricaoRecinto};{item.quantidadeNaUnidadeEstatistica.ToString("n3")};{nfRemessa.quantidadeConsumida.ToString("n3")}";
                                                            camposNFRemessa.SessaoHash = guid;
                                                            camposNFRemessa.UsuarioId = usuarioId;
                                                            camposNFRemessa.DadosSISCOMEX = true;

                                                            camposNFRemessa.DataDUE = $"{retornoDue.dataDeRegistro.ToString("dd/MM/yyyy HH:mm:ss")}";
                                                            camposNFRemessa.DUE = $"{retornoDue.numero}";
                                                            camposNFRemessa.ChaveDUE = $"{retornoDue.chaveDeAcesso}";
                                                            camposNFRemessa.DeclaranteCnpj = declarante;
                                                            camposNFRemessa.DeclaranteNome = declaranteNome;
                                                            camposNFRemessa.UltimoEvento = ultimoEvento;
                                                            camposNFRemessa.DataUltimoEvento = $"{dataUltimoEvento?.ToString("dd/MM/yyyy HH:mm:ss")}";
                                                            camposNFRemessa.DataAverbacao = $"{dataAverbacao}";
                                                            camposNFRemessa.ItemDUE = $"{nfRemessa.ncm?.codigo}";
                                                            camposNFRemessa.NCM = $"{nfRemessa.numeroDoItem}";
                                                            camposNFRemessa.CFOP = $"{nfRemessa.cfop}";
                                                            camposNFRemessa.DESCRICAO = $"{nfRemessa.descricao}";
                                                            camposNFRemessa.TipoNF = $"{tipoNf}";
                                                            camposNFRemessa.ChaveNF = $"{itemNfRef.chaveDeAcesso}";
                                                            camposNFRemessa.NUMERO = $"{itemNfRef.numeroDoDocumento}";
                                                            camposNFRemessa.MODELO = $"{itemNfRef.modelo}";
                                                            camposNFRemessa.SERIE = $"{itemNfRef.serie}";
                                                            camposNFRemessa.UF = $"{itemNfRef.ufDoEmissor}";
                                                            camposNFRemessa.CNPJEmitente = $"{emitente.numero}";
                                                            camposNFRemessa.EventoAverbacao = $"{retornoDue.situacao}";
                                                            camposNFRemessa.VMLE = $"{valorDaMercadoriaNaCondicaoDeVenda.Replace(".", "").Replace(",", ".") }";
                                                            camposNFRemessa.ValorEmReais = $"{valorDaMercadoriaNoLocalDeEmbarque.Replace(".", "").Replace(",", ".") }";
                                                            camposNFRemessa.VMCV = $"{valorDaMercadoriaNoLocalDeEmbarqueEmReais.Replace(".", "").Replace(",", ".") }";
                                                            camposNFRemessa.IMPORTADOR = importador;
                                                            camposNFRemessa.ImportadorEndereco = enderecoImportador;
                                                            camposNFRemessa.ImportadorPais = paisImportador;
                                                            camposNFRemessa.PaisDestino = paisDestino;
                                                            camposNFRemessa.Unidade = unidadeEstatistica;
                                                            camposNFRemessa.PesoLiquidoTotal = pesoLiquidoTotal;
                                                            camposNFRemessa.Moeda = moedaNegociacao;
                                                            camposNFRemessa.INCOTERM = incoterm;
                                                            camposNFRemessa.InformacoesComplementares = informacoesComplementares;
                                                            camposNFRemessa.UnidadeRFB = retornoDue.unidadeLocalDeDespacho?.codigo;
                                                            camposNFRemessa.DescricaoUnidadeRFB = descricaoUnidade;
                                                            camposNFRemessa.RecintoDespacho = $"{retornoDue.recintoAduaneiroDeDespacho?.codigo }";
                                                            camposNFRemessa.DescricaoRecintoDespacho = descricaoRecinto;
                                                            camposNFRemessa.QuantidadeNF = $"{item.quantidadeNaUnidadeEstatistica.ToString("n3")}";
                                                            camposNFRemessa.QuantidadeAverbada = $"{nfRemessa.quantidadeConsumida.ToString("n3")}";

                                                            _notaFiscalDAO.CadastrarNFPosADC(camposNFRemessa);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var retornoDueResumida = AsyncHelpers.RunSync<ConsultaDueDadosResumidos>(() => ServicoSiscomex2.ObterDetalhesDUE(due.rel, ConfigurationManager.AppSettings["CpfCertificado"].ToString()));

                                    if (retornoDueResumida != null)
                                    {
                                        if (!duesImportadas.Contains(due.rel))
                                        {
                                            duesImportadas.Add(due.rel);

                                            var declarante = retornoDueResumida.declarante?.numero ?? string.Empty;
                                            var descricaoRecinto = string.Empty;
                                            var descricaoUnidade = string.Empty;

                                            if (retornoDueResumida.codigoRecintoAduaneiroDespacho != null)
                                            {
                                                var recintoBusca = _recintosDAO.ObterRecintos()
                                                    .Where(c => c.Id == retornoDueResumida.codigoRecintoAduaneiroDespacho.ToInt()).FirstOrDefault();

                                                if (recintoBusca != null)
                                                {
                                                    descricaoRecinto = recintoBusca.Descricao;
                                                }
                                            }
                                            else
                                            {
                                                descricaoRecinto = "Indisponível";
                                            }

                                            if (retornoDueResumida.uaDespacho != null)
                                            {
                                                var unidadeBusca = _unidadesReceitaDAO.ObterUnidadesRFB()
                                                    .Where(c => c.Codigo.ToInt() == retornoDueResumida.uaDespacho.ToInt()).FirstOrDefault();

                                                if (unidadeBusca != null)
                                                {
                                                    descricaoUnidade = unidadeBusca.Descricao;
                                                }
                                            }
                                            else
                                            {
                                                descricaoUnidade = "Indisponível";
                                            }

                                            var camposNFRemessa = new NFRemessaPosACD();
                                            camposNFRemessa.CamposRemessa = $";;{retornoDueResumida.numeroDUE};;{declarante};;;;;;;;;;{notaNaoEncontrada};;;;;;;;;;;;;;;;;;;{retornoDueResumida.uaDespacho};{descricaoUnidade};{retornoDueResumida.codigoRecintoAduaneiroDespacho };{descricaoRecinto}";
                                            camposNFRemessa.SessaoHash = guid;
                                            camposNFRemessa.UsuarioId = usuarioId;
                                            camposNFRemessa.DadosSISCOMEX = true;

                                            camposNFRemessa.DUE = $"{retornoDueResumida.numeroDUE}";
                                            camposNFRemessa.DeclaranteCnpj = $"{declarante}";
                                            camposNFRemessa.ChaveNF = $"{notaNaoEncontrada}";
                                            camposNFRemessa.UnidadeRFB = $"{retornoDueResumida.uaDespacho}";
                                            camposNFRemessa.DescricaoUnidadeRFB = $"{descricaoUnidade}";
                                            camposNFRemessa.RecintoDespacho = $"{retornoDueResumida.codigoRecintoAduaneiroDespacho}";
                                            camposNFRemessa.DescricaoRecintoDespacho = $"{descricaoRecinto}";
                                            _notaFiscalDAO.CadastrarNFPosADC(camposNFRemessa);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            string teste = dadosDue.Mensagem;
                        }
                    }
                }
            }
        }

        [WebMethod]
        public static string ConsultarNFs(List<string> chaves, string guid, string usuarioId, string sessionFiltroChavesRemessaUpload)
        {
            try
            {
                List<string> chavesConsulta = new List<string>();
                foreach (string chave in chaves)
                {
                    chavesConsulta.Add($"'{chave.Trim()}'");
                }
                ConsultaPosACDAsync(chavesConsulta, guid, Convert.ToInt32(usuarioId), sessionFiltroChavesRemessaUpload);

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível importar NFs");
            }
        }

        [WebMethod]
        public static int ObterTotalNFsConsultaRemessaPosACDPorGUID(string guid, string usuarioId)
        {
            var _notaFiscalDAO = new NotaFiscalDAO();

            return _notaFiscalDAO.ObterTotalNFsConsultaRemessaPosACDPorGUID(guid, Convert.ToInt32(usuarioId));
        }

        protected void btnGerarCsvAux_Click(object sender, EventArgs e)
        {
            var _notaFiscalDAO = new NotaFiscalDAO();
            var filtro = new StringBuilder();
            var paises = PaisesDAO.ObterPaisesSiscomex();

            var filtroRem = new StringBuilder();
            var listaNotasArquivo = new List<string>();

            StringBuilder builder = new StringBuilder();
            List<string> rows = new List<string>();

            List<string> colunas = new List<string>();

            colunas.Add("AUTONUM");
            colunas.Add("DataDUE");
            colunas.Add("DUE");
            colunas.Add("ChaveDUE");
            colunas.Add("DeclaranteCnpj");
            colunas.Add("DeclaranteNome");
            colunas.Add("UltimoEvento");
            colunas.Add("DataUltimoEvento");
            colunas.Add("DataAverbacao");
            colunas.Add("ItemDUE");
            colunas.Add("NCM");
            colunas.Add("ItemNF");
            colunas.Add("CFOP");
            colunas.Add("DESCRICAO");
            colunas.Add("TipoNF");
            colunas.Add("CHAVE_NF");
            colunas.Add("NUMERO");
            colunas.Add("MODELO");
            colunas.Add("SERIE");
            colunas.Add("UF");
            colunas.Add("CNPJ_EMITENTE");
            colunas.Add("EventoAverbacao");
            colunas.Add("VMLE");
            colunas.Add("ValorEmReais");
            colunas.Add("VMCV");
            colunas.Add("IMPORTADOR");
            colunas.Add("ImportadorEndereco");
            colunas.Add("ImportadorPais");
            colunas.Add("PaisDestino");
            colunas.Add("Unidade");
            colunas.Add("PesoLiquidoTotal");
            colunas.Add("Moeda");
            colunas.Add("INCOTERM");
            colunas.Add("InformacoesComplementares");
            colunas.Add("UnidadeRFB");
            colunas.Add("DescricaoUnidadeRFB");
            colunas.Add("RecintoDespacho");
            colunas.Add("DescricaoRecintoDespacho");
            colunas.Add("QuantidadeNF");
            colunas.Add("QuantidadeAverbada");
            colunas.Add("RUC");

            var listaResultados = _notaFiscalDAO.ObterNFsConsultaRemessaPosACDPorGUID(hddnGUID.Value, Convert.ToInt32(Session["UsuarioId"].ToString()));

            foreach (var rowRemessa in listaResultados)
            {
                rows.Add(rowRemessa.CamposRemessa);
            }

            var listaResultadosSiscomex = _notaFiscalDAO.ObterNFsConsultaRemessaPosACDPorGUIDSISCOMEX(hddnGUID.Value, Convert.ToInt32(Session["UsuarioId"].ToString()));

            if (listaResultadosSiscomex != null)
            {
                if (listaResultadosSiscomex.Count() > 0)
                {
                    rows.Add("As notas abaixo não formam encontradas no banco de dados de DUEs CARGILL e os dados apresentados foram consultados diretamente no SISCOMEX");
                }
            }
            foreach (var rowRemessa in listaResultadosSiscomex)
            {
                rows.Add(rowRemessa.CamposRemessa);
            }

            var duesImportadas = new List<string>();
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
            //Response.End();

            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}