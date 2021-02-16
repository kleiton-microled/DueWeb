<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="ConsultarNFs.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarNFs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/consultar-nf.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdateProgress ID="UpdateProgress1" ClientIDMode="Static" runat="server"
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updateModal">
                <div align="center" class="updateModalBox">
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="hddnGUID" ClientIDMode="Static" Value="" runat="server" />
            <div class="row no-gutter">
                <div class="col-md-12">

                    <div class="row">
                        <div class="col-md-12">

                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Pesquisa notas fiscais diretamente no Siscomex. ATENÇÃO: Pode ser muito morosa, o tempo de pesquisa depende do Siscomex. </h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-10">
                                                <label for="txtValorTotalNota" class="control-label">Arquivo:</label>
                                                <asp:FileUpload ID="txtUpload" ClientIDMode="Static" runat="server" CssClass="btn btn-default" />
                                                <p class="help-block">
                                                    Clique no botão "Escolher arquivo" e selecione o arquivo .csv ou .txt de notas fiscais
                                                </p>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="pull-right">
                                                    <a href="Consulta_CCT.pdf" target="_blank">
                                                        <img src="Content/imagens/btnInstrucoes.png" /></a>
                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                    <br />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Button ID="btnSair" runat="server" Text="Sair" CssClass="btn btn-default" OnClick="btnSair_Click" />
                                                <asp:Button ID="btnImportar" Visible="false" runat="server" Text="Importar" CssClass="btn btn-warning" OnClick="btnImportar_Click" OnClientClick="MostrarProgresso();" />
                                                <input type='button' id='btnLoad' value='Importar' class="btn btn-warning" onclick='handleFileSelect();'>
                                            </div>

                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12 pull-right text-center">
                                                <label for="btnGerarExcel" style="font-size: 20px!important;" class="control-label invisivel gerar-excel">O resultado da consulta pode ser obtido através do botão:</label>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12 pull-right text-center">
                                                <%--<asp:ImageButton ID="btnGerarExcel"  ClientIDMode="Static" CssClass="invisivel" runat="server" OnClick="btnGerarExcel_Click" ImageUrl="~/Content/imagens/btnExcel.png" />--%>
                                                <asp:ImageButton ID="btnGerarExcel" ClientIDMode="Static" CssClass="gerar-excel invisivel" runat="server" OnClientClick="SetGUID();" OnClick="btnGerarExcelParcial_Click" ImageUrl="~/Content/imagens/btnExcel.png" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <br />



                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView
                                    ID="gvNotasFiscais"
                                    runat="server"
                                    Width="100%"
                                    CssClass="table"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="13px"
                                    ShowHeaderWhenEmpty="True"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <asp:BoundField HeaderText="Registro" DataField="DataRegistro" />
                                        <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF" />
                                        <asp:BoundField DataField="SaldoCCT" HeaderText="Saldo CCT" />
                                        <asp:BoundField DataField="PesoEntradaCCT" HeaderText="Peso Entrada CCT" />
                                        <asp:BoundField DataField="PesoAferido" HeaderText="Peso Aferido" />
                                        <asp:BoundField DataField="Observacoes" HeaderText="Observações" />
                                        <asp:BoundField HeaderText="Recinto" DataField="Recinto" />
                                        <asp:BoundField HeaderText="Unidade Receita" DataField="UnidadeReceita" />
                                        <asp:BoundField HeaderText="Item" DataField="Item" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="modal-importacao" class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <h2>Consultando NFs</h2>
                            <p>Por favor, aguarde...</p>
                            <div class="progress">
                                <span class="progress-value" id="barra-progresso-porcentagem">0%</span>
                                <div id="barra-progresso" class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                                </div>
                            </div>
                            <p id="label-consulta" class="gerar-excel invisivel">O resultado da consulta pode ser obtido através do botão <b>Exportar para Excel</b></p>


                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportar" runat="server" />
            <asp:PostBackTrigger ControlID="btnGerarExcel" runat="server" />
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>

        function handleFileSelect() {
            if (!window.File || !window.FileReader || !window.FileList || !window.Blob) {
                alert('Funcionalidade não suportada neste Browser');
                return;
            }

            LimparGUID();
            input = document.getElementById('txtUpload');
            file = input.files[0];
            fr = new FileReader();
            fr.onload = receivedText;
            fr.readAsText(file);
            localStorage.clear();
            localStorage.setItem('GUID', getGUID());

        }
        var totalImportados = 0;

        function receivedText() {

            $('.gerar-excel').addClass('invisivel');
            $('#modal-importacao').modal('show');
            var qtdeArquivosVez = 10;
            var text = fr.result;
            var lines = text.split(/[\r\n]+/g);
            var linesArr = [];

            for (var i = 0; i < lines.length; i++) {
                if (lines[i] !== '') {
                    linesArr.push(lines[i]);
                }
            }

            var chunks = chunk(linesArr, qtdeArquivosVez);

            var total = linesArr.length;
            totalImportados = 0;
            var guid = localStorage.getItem('GUID');

            for (var i = 0; i < chunks.length; i++) {

                //ImportarNFs(JSON.stringify(chunks[i]), totalImportados);
                ImportarNFs(chunks[i], total, guid);

            }

            //console.log(chunks);

            // document.getElementById('editor').innerHtml = JSON.stringify();
        }

        function chunk(arr, chunkSize) {
            var R = [];
            for (var i = 0, len = arr.length; i < len; i += chunkSize)
                R.push(arr.slice(i, i + chunkSize));
            return R;
        }

        function ImportarNFs(lista, total, guidUsuario) {
            var dataToSend = [{ chaves: "'" + lista + "'" }, { fieldname: 'DEF' }];
            var postData = { chaves: lista, guid: guidUsuario };
            $.ajax({
                type: "POST",
                url: "ConsultarNFs.aspx/ImportarNFs",
                data: JSON.stringify(postData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    totalImportados += lista.length;
                    var valorPerc = (totalImportados / (total / 100)).toFixed(2);
                    var perc = valorPerc + '%';
                    console.log(perc + 'de NFs importadas');
                    $('#barra-progresso').css('width', perc);
                    $('#barra-progresso-porcentagem').html(perc);

                    if (valorPerc >= 100) {

                        setTimeout(function () {

                            $('#label-consulta').fadeIn(2000);
                            setTimeout(function () {
                                //setTimeout(function () {
                                //    $('.gerar-excel').removeClass('invisivel').fadeIn(1500);
                                //    $('#label-consulta').fadeIn(1500);
                                //},2000);
                                $('#modal-importacao').modal('hide');
                                $('.gerar-excel').removeClass('invisivel');
                                perc = '0%';
                                $('#barra-progresso').css('width', perc);
                                $('#barra-progresso-porcentagem').html(perc);
                            }, 500);



                        }, 1000);
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
    </script>
</asp:Content>
