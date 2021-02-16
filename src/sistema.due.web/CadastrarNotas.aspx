<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastrarNotas.aspx.cs" Inherits="Sistema.DUE.Web.CadastrarNotas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="updateModal" style="display: none;">
        <div align="center" class="updateModalBox">
        </div>
    </div>

    <br />
    <div class="row">
        <div class="col-sm-10 col-sm-offset-1">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Cadastro de Notas Fiscais / Itens DUE</h3>
                </div>
                <div class="panel-body">

                    <asp:HiddenField ID="txtDueID" runat="server" />
                    <asp:HiddenField ID="txtChaveExportacaoSelecionada" runat="server" />

                    <div class="row">

                        <div class="col-sm-12">
                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                            <% if (ViewState["Sucesso"] != null)
                                {
                                    if ((bool)ViewState["Sucesso"] == true && (int)ViewState["QuantidadeImportada"] > 0)
                                    {%>
                            <div class="alert alert-success">
                                <strong>Sucesso!</strong>
                                Arquivo importado com sucesso! Os itens poderão ser visualizados na lista abaixo                                       
                            </div>

                            <% }
                                else
                                { %>
                            <div class="alert alert-info">
                                notas fiscais foram importadas. O Conteúdo deste arquivo já foi importado.
                            </div>
                            <% } %>
                            <% }%>


                            <div id="InformacoesNotas" class="alert alert-danger invisivel">
                                <ul>
                                </ul>
                            </div>
                        </div>
                    </div>


                    <div class="row no-gutter">

                        <div class="col-sm-7">

                            <div class="form-group">
                                <label class="control-label">Chave de acesso da NF-e:</label>
                                <asp:TextBox ID="txtChaveNF" runat="server" CssClass="form-control form-control-sm" AutoPostBack="true" OnTextChanged="txtChaveNF_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">Número:</label>
                                <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Exportador:</label>
                                <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row no-gutter">

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">Quantidade:</label>
                                <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">Unid:</label>
                                <asp:DropDownList ID="cbUnidade" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                    <asp:ListItem Text="TON" Value="TON" />
                                    <asp:ListItem Text="KG" Value="KG" />
                                    <asp:ListItem Text="UN" Value="UN" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">NCM:</label>
                                <asp:TextBox ID="txtNCM" runat="server" CssClass="form-control form-control-sm" MaxLength="8"></asp:TextBox>
                            </div>
                        </div>                       

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Valor Unit. VMLE (TON):</label>
                                <asp:TextBox ID="txtVMLE" runat="server" CssClass="form-control form-control-sm" Enabled="true"></asp:TextBox>
                            </div>
                        </div>

                         <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Valor Unit. VMCV (TON):</label>
                                <asp:TextBox ID="txtVMCV" runat="server" CssClass="form-control form-control-sm" Enabled="true"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">Enquadramento:</label>
                                <asp:DropDownList ID="cbEnquadramento" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <button id="btnLimpar" class="btn btn-sm btn-warning btn-block" data-toggle="tooltip" data-placement="top" title="Limpar Campos">
                                    <i class="fa fa-trash"></i>&nbsp;Limpar
                                </button>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <button id="btnAdicionarNF" class="btn btn-sm btn-primary btn-block" data-toggle="tooltip" data-placement="top" title="Adicionar Nota Fiscal">
                                    <i class="fa fa-edit"></i>&nbsp;Incluir
                                </button>
                            </div>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-12">

                            <div id="tbNotas" class="table-responsive">
                            </div>

                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-4">
                            <asp:FileUpload ID="txtUpload" runat="server" AllowMultiple="true" CssClass="btn btn-default input-100-p" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="btnImportar" runat="server" Text="Importar" CausesValidation="true" CssClass="btn btn-info" OnClick="btnImportar_Click" OnClientClick="return verificaItensDUE();" />
                        </div>
                        <div class="col-sm-6">
                            <div class="pull-right">
                                <a href="CadastrarItensDUE.aspx?id=<%=txtDueID.Value %>" class="btn btn-warning">Ir para Itens</a>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>


    <div class="modal fade" id="modalNotasRemessa" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Cadastrar Notas de Remessa</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="InformacoesNotasRemessa" class="alert alert-danger invisivel">
                        <ul>
                        </ul>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-12">

                            <span id="spNotaExportacao"></span>
                        </div>
                    </div>
                    <br />
                    <div class="row no-gutter">

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Tipo:</label>
                                <asp:DropDownList ID="cbTipoNotaRemessa" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                    <asp:ListItem Text="REM" Value="REM" />
                                    <asp:ListItem Text="FDL" Value="FDL" />
                                    <asp:ListItem Text="NFF" Value="NFF" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Item:</label>
                                <asp:TextBox ID="txtItemNotaRemessa" runat="server" CssClass="form-control form-control-sm" MaxLength="3" Text="1"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="control-label">Chave Nota Remessa:</label>
                                <asp:TextBox ID="txtChaveNotaRemessa" runat="server" CssClass="form-control form-control-sm" MaxLength="44"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Número:</label>
                                <asp:TextBox ID="txtNumeroNotaRemessa" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row no-gutter">

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Exportador:</label>
                                <asp:TextBox ID="txtCnpjNotaRemessa" runat="server" CssClass="form-control form-control-sm" MaxLength="14"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Quantidade:</label>
                                <asp:TextBox ID="txtQuantidadeNotaRemessa" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Unid:</label>
                                <asp:DropDownList ID="cbUnidadeNotaRemessa" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                    <asp:ListItem Text="TON" Value="TON" />
                                    <asp:ListItem Text="KG" Value="KG" />
                                    <asp:ListItem Text="UN" Value="UN" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">NCM:</label>
                                <asp:TextBox ID="txtNCMNotaRemessa" runat="server" CssClass="form-control form-control-sm" MaxLength="8"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <button id="btnLimparNotaRemessa" class="btn btn-sm btn-warning btn-block" data-toggle="tooltip" data-placement="top" title="Limpar Campos">
                                    <i class="fa fa-trash"></i>&nbsp;Limpar
                                </button>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <button id="btnAdicionarNotaRemessa" class="btn btn-sm btn-primary btn-block" data-toggle="tooltip" data-placement="top" title="Adicionar Nota Fiscal">
                                    <i class="fa fa-edit"></i>&nbsp;Incluir
                                </button>
                            </div>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-12">

                            <div id="tbNotasRemessa" class="table-responsive">
                            </div>

                        </div>
                    </div>


                </div>

            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/select2.min.js"></script>

    <script>

        $('#MainContent_cbEnquadramento').select2();

        $('#btnLimpar').click(function () {

            event.preventDefault();

            $("#<%=txtChaveNF.ClientID%>").val('');
            $("#<%=txtNumero.ClientID%>").val('');
            $("#<%=txtCNPJ.ClientID%>").val('');
            $("#<%=txtQuantidade.ClientID%>").val('');
            $("#<%=cbUnidade.ClientID%>").val('');
            $("#<%=txtNCM.ClientID%>").val('');
        });

        $('#btnAdicionarNF').click(function () {

            event.preventDefault();

            var chaveNF = $("#<%=txtChaveNF.ClientID%>").val();
            var dueId = $("#<%=txtDueID.ClientID%>").val();
            var numero = $("#<%=txtNumero.ClientID%>").val();
            var cnpj = $("#<%=txtCNPJ.ClientID%>").val();
            var quantidade = $("#<%=txtQuantidade.ClientID%>").val();
            var unidade = $("#<%=cbUnidade.ClientID%>").val();
            var ncm = $("#<%=txtNCM.ClientID%>").val();
            var vmcv = $("#<%=txtVMCV.ClientID%>").val();
            var vmle = $("#<%=txtVMLE.ClientID%>").val();

            $('#InformacoesNotas ul li').remove();
            $('#InformacoesNotas').addClass('invisivel');

            var existemErros = false;

            if (chaveNF === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe a Chave da NF corretamente</li>');

                existemErros = true;
            }

            if (numero === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe o número da Nota Fiscal</li>');

                existemErros = true;
            }

            if (cnpj === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe o CNPJ do Exportador da Nota Fiscal</li>');

                existemErros = true;
            }

            if (quantidade === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe a Quantidade da Nota Fiscal</li>');

                existemErros = true;
            }

            if (unidade === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe a Unidade da Nota Fiscal</li>');

                existemErros = true;
            }

            if (ncm === '') {
                $('#InformacoesNotas > ul')
                    .append('<li>Informe o NCM da Nota Fiscal</li>');

                existemErros = true;
            }

            if (parseInt(dueId) === 0) {
                $('#InformacoesNotas > ul')
                    .append('<li>DUE não informada</li>');

                existemErros = true;
            }

            if (existemErros) {
                $('#InformacoesNotas').removeClass('invisivel');
                existemErros = false;
                return false;
            }

            var dadosForm = '{ chaveNF: "' + chaveNF + '", dueId: "' + dueId + '", numero: "' + numero + '", cnpj: "' + cnpj + '", quantidade: "' + quantidade + '", unidade: "' + unidade + '", ncm: "' + ncm + '", vmcv: "' + vmcv + '", vmle: "' + vmle + '" }';

            $.ajax({
                type: "POST",
                url: "CadastrarNotas.aspx/AdicionarNotaFiscal",
                data: dadosForm,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var mensagem = response.d;

                    if ((mensagem.indexOf('Alerta') !== -1) || (mensagem.indexOf('Erro') !== -1)) {

                        $('#InformacoesNotas > ul')
                            .append('<li>' + mensagem + '</li>') /

                            $('#InformacoesNotas').removeClass('invisivel');

                        return;
                    }

                    var dados = JSON.parse(response.d);

                    $("#tbNotas").html(montarTabelaNotas(dados));

                    $("#<%=txtChaveNF.ClientID%>").val('');
                },
                error: function (response) {

                    var dados = response.responseJSON;

                    $('#InformacoesNotas > ul')
                        .append('<li>' + dados.d + '</li>') /

                        $('#InformacoesNotas').removeClass('invisivel');
                }
            });

        });

        function montarTabelaNotas(dados) {

            var tabela =
                '<table id="tbNotas" class="table table-sm">' +
                '   <thead>' +
                '       <tr>' +
                '           <th>Item</th>' +
                '           <th>Chave NF</th>' +
                '           <th style="width:160px;">&nbsp;</td>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '       </tr>' +
                '   </thead>' +
                '<tbody>';

            dados.forEach(function (linha) {

                var id = "'" + linha.Id + "'";
                var nf = "'" + linha.ChaveNF + "'";

                tabela = tabela +
                    '   <tr>' +
                    '       <td>' + linha.Item + '</td>' +
                    '       <td>' + linha.ChaveNF + '</td>' +
                    '       <td><a href="#" onclick="notasRemessa(' + nf + ')" data-toggle="modal" role="button" data-target="#modalNotasRemessa">Notas de Remessa</a></td>' +
                    '       <td><a href="#" onclick="visualizarNF(' + id + ')"><img src="Content/imagens/search.png" /></a></td>' +
                    '       <td><a href="#" onclick="removerNF(' + id + ')"><img src="Content/imagens/excluir.png" /></a></td>' +
                    '   </tr>';
            });

            tabela = tabela + '</tbody>' +
                '</table>';

            return tabela;
        }

        function removerNF(id) {

            event.preventDefault();

            var escolha = confirm('Atenção: Ao excluir a Nota, o Item vinculado também será removido da DUE. Clique em OK para confirmar.');

            if (!escolha)
                return;

            $.ajax({
                type: "POST",
                url: "CadastrarNotas.aspx/RemoverNotaFiscal",
                data: '{ id: "' + id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d.indexOf('Erro') !== -1) {
                        alert(response.d);
                        return;
                    }
                    var dados = JSON.parse(response.d);
                    $("#tbNotas").html(montarTabelaNotas(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function visualizarNF(id) {

            event.preventDefault();

            $.ajax({
                type: "GET",
                url: "CadastrarNotas.aspx/VisualizarNF?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);

                    if (dados) {

                        $("#<%=txtChaveNF.ClientID%>").val(dados.ChaveNF);
                        $("#<%=txtNumero.ClientID%>").val(dados.NumeroNF);
                        $("#<%=txtCNPJ.ClientID%>").val(dados.CnpjNF);
                        $("#<%=txtQuantidade.ClientID%>").val(dados.QuantidadeNF);
                        $("#<%=cbUnidade.ClientID%>").val(dados.UnidadeNF);
                        $("#<%=txtNCM.ClientID%>").val(dados.NCM);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function notasRemessa(nf) {

            $("#<%=txtChaveNotaRemessa.ClientID%>").val('');
            $("#<%=cbTipoNotaRemessa.ClientID%>").val('');
            $("#<%=txtItemNotaRemessa.ClientID%>").val('');
            $("#<%=txtNumeroNotaRemessa.ClientID%>").val('');
            $("#<%=txtCnpjNotaRemessa.ClientID%>").val('');
            $("#<%=txtQuantidadeNotaRemessa.ClientID%>").val('');
            $("#<%=cbUnidadeNotaRemessa.ClientID%>").val('');
            $("#<%=txtNCMNotaRemessa.ClientID%>").val('');

            $("#<%=txtChaveExportacaoSelecionada.ClientID%>").val(nf);

            $('#spNotaExportacao').html('<strong>Nota de Exportação: </strong> ' + nf);

            var dueId = $("#<%=txtDueID.ClientID%>").val();
            console.log(nf);
            $.ajax({
                type: "GET",
                url: "CadastrarNotas.aspx/ObterNotasRemessa?dueId=" + dueId + "&chaveReferenciada=" + "'" + nf + "'",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);
                    $("#tbNotasRemessa").html(montarTabelaNotasRemessa(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        $('#btnAdicionarNotaRemessa').click(function () {

            event.preventDefault();

            var chaveNF = $("#<%=txtChaveNotaRemessa.ClientID%>").val();
            var tipo = $("#<%=cbTipoNotaRemessa.ClientID%>").val();
            var item = $("#<%=txtItemNotaRemessa.ClientID%>").val();
            var chaveNFReferenciada = $("#<%=txtChaveExportacaoSelecionada.ClientID%>").val();
            var dueId = $("#<%=txtDueID.ClientID%>").val();
            var numero = $("#<%=txtNumeroNotaRemessa.ClientID%>").val();
            var cnpj = $("#<%=txtCnpjNotaRemessa.ClientID%>").val();
            var quantidade = $("#<%=txtQuantidadeNotaRemessa.ClientID%>").val();
            var unidade = $("#<%=cbUnidadeNotaRemessa.ClientID%>").val();
            var ncm = $("#<%=txtNCMNotaRemessa.ClientID%>").val();

            $('#InformacoesNotasRemessa ul li').remove();
            $('#InformacoesNotasRemessa').addClass('invisivel');

            var existemErros = false;

            if (chaveNF === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe a Chave da NF corretamente</li>');

                existemErros = true;
            }

            if (numero === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe o número da Nota Fiscal</li>');

                existemErros = true;
            }

            if (cnpj === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe o CNPJ do Exportador da Nota Fiscal</li>');

                existemErros = true;
            }

            if (quantidade === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe a Quantidade da Nota Fiscal</li>');

                existemErros = true;
            }

            if (unidade === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe a Unidade da Nota Fiscal</li>');

                existemErros = true;
            }

            if (ncm === '') {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>Informe o NCM da Nota Fiscal</li>');

                existemErros = true;
            }

            if (parseInt(dueId) === 0) {
                $('#InformacoesNotasRemessa > ul')
                    .append('<li>DUE não informada</li>');

                existemErros = true;
            }

            if (existemErros) {
                $('#InformacoesNotasRemessa').removeClass('invisivel');
                existemErros = false;
                return false;
            }

            var dadosForm = '{ tipo: "' + tipo + '", item: "' + item + '", chaveNF: "' + chaveNF + '", chaveReferenciada: "' + chaveNFReferenciada + '", dueId: "' + dueId + '", numero: "' + numero + '", cnpj: "' + cnpj + '", quantidade: "' + quantidade + '", unidade: "' + unidade + '", ncm: "' + ncm + '" }';

            $.ajax({
                type: "POST",
                url: "CadastrarNotas.aspx/AdicionarNotaFiscalRemessa",
                data: dadosForm,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);

                    $("#tbNotasRemessa").html(montarTabelaNotasRemessa(dados));

                    $("#<%=txtChaveNotaRemessa.ClientID%>").val('');
                    $("#<%=cbTipoNotaRemessa.ClientID%>").val('');
                    $("#<%=txtItemNotaRemessa.ClientID%>").val('1');
                    $("#<%=txtNumeroNotaRemessa.ClientID%>").val('');
                    $("#<%=txtCnpjNotaRemessa.ClientID%>").val('');
                    $("#<%=txtQuantidadeNotaRemessa.ClientID%>").val('');
                    $("#<%=cbUnidadeNotaRemessa.ClientID%>").val('');
                    $("#<%=txtNCMNotaRemessa.ClientID%>").val('');
                },
                error: function (response) {

                    var dados = response.responseJSON;

                    $('#InformacoesNotasRemessa > ul')
                        .append('<li>' + dados.d + '</li>');

                    $('#InformacoesNotasRemessa').removeClass('invisivel');
                }
            });

        });

        function montarTabelaNotasRemessa(dados) {

            var tabela =
                '<table id="tbNotasRemessa" class="table table-sm">' +
                '   <thead>' +
                '       <tr>' +
                '           <th>Tipo</th>' +
                '           <th>Item</th>' +
                '           <th>Chave NF</th>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '       </tr>' +
                '   </thead>' +
                '<tbody>';

            dados.forEach(function (linha) {

                tabela = tabela +
                    '   <tr>' +
                    '       <td>' + linha.TipoNF + '</td>' +
                    '       <td>' + linha.Item + '</td>' +
                    '       <td>' + linha.ChaveNF + '</td>' +
                    '       <td><a href="#" onclick="visualizarNotaFiscalRemessa(' + linha.Id + ')"><img src="Content/imagens/search.png" /></a></td>' +
                    '       <td><a href="#" onclick="removerNotaFiscalRemessa(' + linha.Id + ')"><img src="Content/imagens/excluir.png" /></a></td>' +
                    '   </tr>';
            });

            tabela = tabela + '</tbody>' +
                '</table>';

            return tabela;
        }

        function removerNotaFiscalRemessa(id) {

            event.preventDefault();

            var escolha = confirm('Confirma a Exclusão da Nota Referenciada?');

            if (!escolha)
                return;

            $.ajax({
                type: "POST",
                url: "CadastrarNotas.aspx/RemoverNotaFiscalRemessa",
                data: '{ id: "' + id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var dados = JSON.parse(response.d);
                    $("#tbNotasRemessa").html(montarTabelaNotasRemessa(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function visualizarNotaFiscalRemessa(id) {

            event.preventDefault();

            $.ajax({
                type: "GET",
                url: "CadastrarNotas.aspx/VisualizarNotaFiscalRemessa?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);

                    if (dados) {

                        $("#<%=txtChaveNotaRemessa.ClientID%>").val(dados.ChaveNF);
                        $("#<%=cbTipoNotaRemessa.ClientID%>").val(dados.TipoNF);
                        $("#<%=txtItemNotaRemessa.ClientID%>").val(dados.Item);
                        $("#<%=txtNumeroNotaRemessa.ClientID%>").val(dados.NumeroNF);
                        $("#<%=txtCnpjNotaRemessa.ClientID%>").val(dados.CnpjNF);
                        $("#<%=txtQuantidadeNotaRemessa.ClientID%>").val(dados.QuantidadeNF);
                        $("#<%=cbUnidadeNotaRemessa.ClientID%>").val(dados.UnidadeNF);
                        $("#<%=txtNCMNotaRemessa.ClientID%>").val(dados.NCM);
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        $('#btnLimparNotaRemessa').click(function () {

            event.preventDefault();

            $("#<%=txtChaveNotaRemessa.ClientID%>").val('');
            $("#<%=cbTipoNotaRemessa.ClientID%>").val('');
            $("#<%=txtNumeroNotaRemessa.ClientID%>").val('');
            $("#<%=txtCnpjNotaRemessa.ClientID%>").val('');
            $("#<%=txtQuantidadeNotaRemessa.ClientID%>").val('');
            $("#<%=cbUnidadeNotaRemessa.ClientID%>").val('');
            $("#<%=txtNCMNotaRemessa.ClientID%>").val('');
        });

        $(document).ready(function () {

            var dueId = $("#<%=txtDueID.ClientID%>").val();

            $.ajax({
                type: "GET",
                url: "CadastrarNotas.aspx/ObterNotas?dueId=" + dueId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);
                    $("#tbNotas").html(montarTabelaNotas(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        });

        function verificaItensDUE() {

            var dueId = $("#<%=txtDueID.ClientID%>").val();
            var btnID = '<%=btnImportar.UniqueID  %>';

            $('.updateModal').show();

            $.ajax({
                type: "GET",
                url: "CadastrarNotas.aspx/DueManual?dueId=" + dueId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    if (response.d) {

                        if (!confirm('Esta DUE já contém itens que não foram cadastrados manualmente ou apartir de um arquivo. Ao conformar, o sistema irá excluir e cadastrar os itens automaticamente de acordo com o arquivo a ser importado. Deseja confirmar?')) {
                            return false;
                        }
                    }

                    __doPostBack(btnID, '');

                    $('.updateModal').hide();
                }
            });

            return false;
        }

    </script>

</asp:Content>
