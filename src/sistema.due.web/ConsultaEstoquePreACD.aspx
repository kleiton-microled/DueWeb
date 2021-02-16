<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaEstoquePreACD.aspx.cs" Inherits="Sistema.DUE.Web.ConsultaEstoquePreACD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hddnGUID" ClientIDMode="Static" Value="" runat="server" />

    <br /><br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Consultar Estoque Pré-ACD</h3>
                </div>
                <div class="panel-body">

                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Entrada Estoque (De):</label>
                                <asp:TextBox ID="txtEntradaDe" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Entrada Estoque (Até):</label>
                                <asp:TextBox ID="txtEntradaAte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Unidade RFB:</label>
                                <span class="text-danger">*</span>
                                <asp:DropDownList ID="cbUnidadeRFB" runat="server" CssClass="form-control" DataValueField="Codigo" DataTextField="Descricao" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="cbUnidadeRFB_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">Recinto Aduaneiro:</label>
                                <span class="text-danger">*</span>
                                <asp:DropDownList ID="cbRecintoAduaneiroDespacho" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="Descricao" Font-Size="11px">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Responsável:</label>
                                <asp:TextBox ID="txtCNPJResponsavel" runat="server" CssClass="form-control form-control-sm cnpj"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">NCM:</label>
                                <asp:TextBox ID="txtNCM" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label class="control-label">País Destinatário:</label>
                                <span class="text-danger">*</span>
                                <asp:DropDownList ID="cbPaisDestinatario" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="Descricao" Font-Size="11px">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Emissão NF (De):</label>
                                <asp:TextBox ID="txtEmissaoNFDe" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Emissão NF (Até):</label>
                                <asp:TextBox ID="txtEmissaoNFAte" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label class="control-label">CNPJ Destinatário:</label>
                                &nbsp; <small>(Informe 1 ou mais CNPJ's separados por vírgula. Ex: 00.000.000/0000-00, 11.111.111/1111-11)</small>
                                <asp:TextBox ID="txtCNPJDestinatario" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Emitente:</label>
                                <asp:TextBox ID="txtCNPJEmitente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Número NF:</label>
                                <asp:TextBox ID="txtNumeroNF" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Modelo NF:</label>
                                <asp:TextBox ID="txtModeloNF" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label">Série NF:</label>
                                <asp:TextBox ID="txtSerieNF" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <div class="form-group">
                                <label class="control-label">Item:</label>
                                <asp:TextBox ID="txtItem" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnLimparFiltro" runat="server" CssClass="btn btn-default btn-block" Text="Limpar" OnClick="btnLimparFiltro_Click" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-block" Text="Filtrar" OnClick="btnFiltrar_Click" />
                            </div>
                        </div>
                    </div>

                    <br />
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView
                                    ID="gvEstoque"
                                    runat="server"
                                    Width="100%"
                                    CssClass="table table-striped"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="13px"
                                    ShowHeaderWhenEmpty="True" DataKeyNames="Id" AllowPaging="True" PageSize="6" OnPageIndexChanging="gvEstoque_PageIndexChanging" OnRowDataBound="gvEstoque_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <a href="JavaScript:divexpandcollapse('div<%# Eval("Id") %>');">
                                                    <img id="imgdiv<%# Eval("Id") %>" border="0" src="Content/imagens/plus.png" alt="" /></a>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                        <asp:BoundField DataField="CnpjDestinatario" HeaderText="CNPJ Destinatário" />
                                        <asp:BoundField DataField="DataHoraEntradaEstoque" HeaderText="Entrada Estoque" />
                                        <asp:BoundField DataField="NumeroNF" HeaderText="NF" />
                                        <asp:BoundField DataField="DescricaoURF" HeaderText="Unidade RFB" />
                                        <asp:BoundField DataField="DescricaoRA" HeaderText="Recinto Alfandegado" />
                                        <asp:BoundField DataField="CodigoNCM" HeaderText="NCM" />
                                        <asp:BoundField DataField="NomeResponsavel" HeaderText="Responsável" />
                                        <asp:BoundField DataField="NomePaisDestinatario" HeaderText="Pais Destinatário" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <tr>
                                                    <td colspan="11">

                                                        <div class="col-md-12">
                                                            <div id="div<%# Eval("Id") %>" style="display: none; position: relative; left: 15px; top: 10px; white-space: nowrap;">

                                                                <div class="table-responsive">
                                                                    <asp:GridView ID="gvEstoqueDetalhes" runat="server" AutoGenerateColumns="False"
                                                                        ForeColor="#333333" CssClass="table table-striped" EmptyDataText="Nenhum registro encontrado.">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="IdentificacaoCondutor" HeaderText="Doc. Condutor" />
                                                                            <asp:BoundField DataField="NomeCondutor" HeaderText="Nome Condutor" />
                                                                            <asp:BoundField DataField="DataHoraEntradaEstoque" HeaderText="Entrada Estoque" />
                                                                            <asp:BoundField DataField="DescricaoAvarias" HeaderText="Avarias" />
                                                                            <asp:BoundField DataField="LocalRaCodigo" HeaderText="Recinto Alf." />
                                                                            <asp:BoundField DataField="LocalUrfCodigo" HeaderText="URF" />
                                                                            <asp:BoundField DataField="NcmCodigo" HeaderText="NCM" />
                                                                            <asp:BoundField DataField="NotaFiscalDestinatarioNome" HeaderText="NF Destinatário" />
                                                                            <asp:BoundField DataField="NotaFiscalDestinatarioPais" HeaderText="NF País Destino" />
                                                                            <asp:BoundField DataField="NotaFiscalEmissao" HeaderText="NF Emissão (Ano/Mês)" />
                                                                            <asp:BoundField DataField="NotaFiscalEmitenteIdentificacao" HeaderText="NF Doc Emitente" />
                                                                            <asp:BoundField DataField="NotaFiscalEmitenteNome" HeaderText="NF Nome Emitente" />
                                                                            <asp:BoundField DataField="NotaFiscalEmitentePais" HeaderText="NF País Emitente" />
                                                                            <asp:BoundField DataField="NotaFiscalModelo" HeaderText="NF Modelo" />
                                                                            <asp:BoundField DataField="NotaFiscalNumero" HeaderText="NF Número" />
                                                                            <asp:BoundField DataField="NotaFiscalSerie" HeaderText="NF Série" />
                                                                            <asp:BoundField DataField="NotaFiscalUf" HeaderText="NF UF" />
                                                                            <asp:BoundField DataField="NumeroDue" HeaderText="DUE" />
                                                                            <asp:BoundField DataField="NumeroItem" HeaderText="Item" />
                                                                            <asp:BoundField DataField="PaisDestino" HeaderText="País Destino" />
                                                                            <asp:BoundField DataField="PesoAferido" HeaderText="Peso Aferido" />
                                                                            <asp:BoundField DataField="QuantidadeExportada" HeaderText="Qtd. Exp." />
                                                                            <asp:BoundField DataField="ResponsavelIdentificacao" HeaderText="Doc. Responsável" />
                                                                            <asp:BoundField DataField="ResponsavelNome" HeaderText="Nome Responsável" />
                                                                            <asp:BoundField DataField="ResponsavelPais" HeaderText="Responsável País" />
                                                                            <asp:BoundField DataField="Saldo" HeaderText="Saldo" />
                                                                            <asp:BoundField DataField="UnidadeEstatistica" HeaderText="Unid. Estatística" />
                                                                            <asp:BoundField DataField="TransportadorIdentificacao" HeaderText="Doc. Transportador" />
                                                                            <asp:BoundField DataField="TransportadorNome" HeaderText="Nome Transportador" />
                                                                            <asp:BoundField DataField="TransportadorPais" HeaderText="Transportador País" />
                                                                            <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                                        </Columns>

                                                                    </asp:GridView>
                                                                </div>

                                                            </div>
                                                        </div>


                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>


                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <%--<asp:Button ID="btnGerarCsv" Text="Gerar .CSV" CssClass="btn btn-info btn-block" runat="server" OnClick="btnGerarCsv_Click" />--%>

                            <button id="btnImportar" class="btn btn-warning" onclick="Consultar();">Gerar .CSV</button>

                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div id="modal-importacao" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-body">
                    <h2>Gerando CSV</h2>
                    <p>Por favor, aguarde...</p>
                    <div class="progress">
                        <span class="progress-value" id="barra-progresso-porcentagem">0%</span>
                        <div id="barra-progresso" class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                        </div>
                    </div>
                    <center>
                    <p id="label-consulta" class="gerar-excel invisivel">O resultado da consulta pode ser obtido através do botão <b>Exportar para CSV</b></p>

                    <asp:ImageButton ID="btnGerarCsv" ClientIDMode="Static" CssClass="gerar-excel invisivel" runat="server" OnClientClick="SetGUID();" OnClick="btnGerarCSVParcial_Click" ImageUrl="~/Content/imagens/btnCsv.png" />
                    </center>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/select2.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>

        $('#MainContent_cbUnidadeRFB').select2();
        $('#MainContent_cbRecintoAduaneiroDespacho').select2();
        $('#MainContent_cbPaisDestinatario').select2();
        $('#MainContent_txtEntradaDe').mask('00/00/0000');
        $('#MainContent_txtEntradaAte').mask('00/00/0000');
        $('#MainContent_txtEmissaoNFDe').mask('00/00/0000');
        $('#MainContent_txtEmissaoNFAte').mask('00/00/0000');
        $('#MainContent_txtCNPJResponsavel').mask('00.000.000/0000-00');
        $('#MainContent_txtCNPJEmitente').mask('00.000.000/0000-00');

        function divexpandcollapse(divname) {

            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display === "none") {
                div.style.display = "inline";
                img.src = "Content/imagens/minus.png";
            } else {
                div.style.display = "none";
                img.src = "Content/imagens/plus.png";
            }
        }

        var paginasProcessadas = 1;
        var totalRegistrosDoBancoDeDados = 0;
        function Consultar() {

            event.preventDefault();

            var filtro = {
                entradaDe: $('#MainContent_txtEntradaDe').val(),
                entradaAte: $('#MainContent_txtEntradaAte').val(),
                unidadeRFB: $('#MainContent_cbUnidadeRFB').val(),
                recintoDespacho: $('#MainContent_cbRecintoAduaneiroDespacho').val(),
                cnpjResponsavel: $('#MainContent_txtCNPJResponsavel').val(),
                ncm: $('#MainContent_txtNCM').val(),
                paisDestinatario: $('#MainContent_cbPaisDestinatario').val(),
                emissaoNFDe: $('#MainContent_txtEmissaoNFDe').val(),
                emissaoNFAte: $('#MainContent_txtEmissaoNFAte').val(),
                cnpjDestinatario: $('#MainContent_txtCNPJDestinatario').val(),
                cnpjEmitente: $('#MainContent_txtCNPJEmitente').val(),
                numeroNF: $('#MainContent_txtNumeroNF').val(),
                modeloNF: $('#MainContent_txtModeloNF').val(),
                serieNF: $('#MainContent_txtSerieNF').val(),
                item: $('#MainContent_txtItem').val(),
            };

            var postData = { filtro: JSON.stringify(filtro) };

            $.ajax({
                type: "POST",
                url: "ConsultaEstoquePreACD.aspx/ObterTotalItens",
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    if (response) {

                        $('#modal-importacao').modal('show');

                


                            var guid = getGUID();

                            localStorage.clear();
                            localStorage.setItem('GUID', guid);

                            var registrosPorPagina = 50000;
                            totalRegistrosDoBancoDeDados = response.d;

                            var totalPaginas = 0;

                            if (totalRegistrosDoBancoDeDados % registrosPorPagina === 0) {
                                totalPaginas = (parseInt(totalRegistrosDoBancoDeDados) / parseInt(registrosPorPagina));
                            } else {
                                // ele está achando que é string... aí somou + 1... dando 111....
                                // coloquei um parseInt
                                totalPaginas = (parseInt(totalRegistrosDoBancoDeDados) / parseInt(registrosPorPagina)) + 1;
                            }

                            totalPaginas = parseInt(totalPaginas);

                            console.log('total panco: ' + totalRegistrosDoBancoDeDados);
                            console.log('total paginas: ' + totalPaginas);

                            

                                //console.log('Página: ' + pagina);
                                    
                        GerarCSV(guid, filtro, totalRegistrosDoBancoDeDados);
                                    
                       
                            

                                            
                    }
                },
                error: function (response) {

                    var json = JSON.parse(response.responseText);

                    if (json != null) {
                        console.log(json.Message, 'DUE');
                    }
                }
            });            
        }

        function GerarCSV(guid, filtro, totalRegistrosDoBancoDeDados) {

            
            var postData = {guid: guid, filtro: JSON.stringify(filtro) };

            $.ajax({
                type: "POST",
                url: "ConsultaEstoquePreACD.aspx/ConsultarEstoquePreACD",
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {

                    console.log('Terminou de gerar o arquivo de verdade!');
                    clearInterval(i);
                    setTimeout(function () {

                        $('#label-consulta').fadeIn(2000);
                        setTimeout(function () {
                            var percFinal = '100%'
                            $('.gerar-excel').removeClass('invisivel');
                            $('#barra-progresso').css('width', percFinal);
                            $('#barra-progresso-porcentagem').html(percFinal);


                        }, 500);
                    }, 1000);
                   
                },
                error: function (response) {

                    var json = JSON.parse(response.responseText);

                    if (json != null) {
                        console.log(json.Message, 'DUE');
                    }
                }
            });

            console.log('Verificar se o status');

         
        }

        var counterProcessados = 0;
        var i = setInterval(function () {
            var guid = localStorage.getItem('GUID');
            // do your thing
            var postData2 = { guid: guid };

            $.ajax({
                type: "POST",
                url: "ConsultaEstoquePreACD.aspx/ObterStatus",
                data: JSON.stringify(postData2),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (resultado) {

                    counterProcessados = resultado.d.TotalRegistrosProcessados;

                    console.log(counterProcessados);

                    var valorPerc = (counterProcessados / (totalRegistrosDoBancoDeDados / 100)).toFixed(2);
                    var perc = valorPerc + '%';
                    console.log(perc + '%');
                    console.log('Processado: ' + valorPerc);
                    $('#barra-progresso').css('width', perc);
                    $('#barra-progresso-porcentagem').html(perc);

                    if (counterProcessados === totalRegistrosDoBancoDeDados) {
                        console.log('parar processamento');
                        clearInterval(i);
                        console.log('Última Verificação de status!');
                    }

                },
                error: function (response) {

                    var json = JSON.parse(response.responseText);

                    if (json != null) {
                        console.log(json.Message, 'DUE');
                    }
                }
            });


        }, 30000);

        function ObterTotalNFsInseridas(guidUsuario) {

            var postData = { guid: guidUsuario };
            $.ajax({
                type: "POST",
                url: "ConsultarNFs.aspx/ObterTotalNFsImportadas",
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.data('Total') > 0) {
                        $('.gerar-excel').removeClass('invisivel');
                    }


                },
                error: function (response) {

                    var json = JSON.parse(response.responseText);

                    if (json != null) {
                        console.log(json.Message, 'DUE');
                    }
                }
            });
        }
        function MostrarProgresso() {
            document.getElementById('UpdateProgress1').style.display = "inline";
        }

        function getGUID() {
            return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
                (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
            );
        }

        function createGuid() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }

        function SetGUID() {
            $('#hddnGUID').val(localStorage.getItem('GUID'));
        }

        function LimparGUID() {
            var guidUsuario = localStorage.getItem('GUID');

            if (guidUsuario !== "") {

                var postData = { guid: guidUsuario };
                $.ajax({
                    type: "POST",
                    url: "ConsultarNFs.aspx/ExcluirGUID",
                    data: JSON.stringify(postData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function () {
                        console.log('GUID removido com sucesso');
                    },
                    error: function (response) {

                        var json = JSON.parse(response.responseText);

                        if (json != null) {
                            console.log(json.Message, 'DUE');
                        }
                    }
                });

            }
        }

        //function ObterTotalItens() {

        //    var filtro = {
        //        entradaDe: $('#MainContent_txtEntradaDe').val(),
        //        entradaAte: $('#MainContent_txtEntradaAte').val(),
        //        unidadeRFB: $('#MainContent_cbUnidadeRFB').val(),
        //        recintoDespacho: $('#MainContent_cbRecintoAduaneiroDespacho').val(),
        //        cnpjResponsavel: $('#MainContent_txtCNPJResponsavel').val(),
        //        ncm: $('#MainContent_txtNCM').val(),
        //        paisDestinatario: $('#MainContent_cbPaisDestinatario').val(),
        //        emissaoNFDe: $('#MainContent_txtEmissaoNFDe').val(),
        //        emissaoNFAte: $('#MainContent_txtEmissaoNFAte').val(),
        //        cnpjDestinatario: $('#MainContent_txtCNPJDestinatario').val(),
        //        cnpjEmitente: $('#MainContent_txtCNPJEmitente').val(),
        //        numeroNF: $('#MainContent_txtNumeroNF').val(),
        //        modeloNF: $('#MainContent_txtModeloNF').val(),
        //        serieNF: $('#MainContent_txtSerieNF').val(),
        //        item: $('#MainContent_txtItem').val(),
        //    };
        //    var postData = { filtro: JSON.stringify(filtro) };
        //    $.ajax({
        //        type: "POST",
        //        url: "ConsultaEstoquePreACD_New3.aspx/ObterTotalItens",
        //        data: JSON.stringify(postData),
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        success: function (response) {
        //            var resultAsString = response.d;
        //            totalRegistrosDoBancoDeDados = parseInt(resultAsString);

        //        },
        //        error: function (response) {

        //            var json = JSON.parse(response.responseText);

        //            if (json != null) {
        //                console.log(json.Message, 'DUE');
        //            }
        //        }
        //    });
        //}

    </script>

</asp:Content>
