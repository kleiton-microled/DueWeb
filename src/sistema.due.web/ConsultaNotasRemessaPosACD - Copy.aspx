<%@ Page Language="C#" AutoEventWireup="true" Async="true" EnableEventValidation="false" CodeBehind="ConsultaNotasRemessaPosACD.aspx.cs" Inherits="Cargill.DUE.Web.ConsultaNotasRemessaPosACD" %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <!--<link rel="icon" href="../../favicon.ico">-->

    <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet">
    <title>CARGILL Agricola SA</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/fontawesome-all.css" rel="stylesheet" />
    <link href="Content/css/site.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />
    <noscript>
        <meta http-equiv="refresh" content="0;url=Javascript.aspx">
    </noscript>

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>

<body>

    <nav class="navbar navbar-default navbar-fixed-top navbar-custom">
        <div class="container-fluid">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <% if (Convert.ToBoolean(Session["Redefiniu"]) == false)
                    { %>
                <a class="navbar-brand" href="Default.aspx"></a>
                <%}
                    else
                    { %>
                <a class="navbar-brand" href="#"></a>
                <%} %>
            </div>

            <% if (Convert.ToBoolean(Session["Redefiniu"]) == false)
                { %>

            <div id="navbar" class="collapse navbar-collapse">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="Default.aspx"><i class="fa fa-home"></i>&nbsp;Início</a></li>
                    <li class="active"><a href="ImportarNfs.aspx"><i class="fa fa-list"></i>&nbsp;Importar NFs</a></li>
                </ul>

                <ul class="nav navbar-nav">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"><i class="fa fa-edit"></i>&nbsp;DUE<span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="CadastrarDUE.aspx">Cadastrar DUE</a></li>
                        </ul>
                    </li>
                </ul>

                <ul class="nav navbar-nav">
                    <li class="active"><a href="Compartilhar.aspx"><i class="fa fa-share-alt"></i>&nbsp;Compartilhar</a></li>

                    <% if (HttpContext.Current.Session["FlagAdmin"] != null && (Convert.ToBoolean(HttpContext.Current.Session["FlagAdmin"]) == true))
                        { %>
                    <li><a href="HabilitarUsuarios.aspx"><i class="fa fa-user"></i>&nbsp;Usuários</a></li>
                    <%} %>
                </ul>

                <ul class="nav navbar-nav">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"><i class="fa fa-search"></i>&nbsp;Consultas<span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="ConsultarDUE.aspx">Consultar DUEs Cadastradas</a></li>
                            <li><a href="ConsultarNotasFiscais.aspx">Consultar Notas Fiscais de Exportação</a></li>
                            <li><a href="ConsultarDUESemVinculoNF.aspx">Consultar DUEs sem Vínculo de Notas Fiscais</a></li>
                            <li><a href="ListagemMemorando.aspx">Listagem Memorando</a></li>
                            <li><a href="ConsultaEstoquePreACD.aspx">Estoque Pré ACD</a></li>
                            <li><a href="ConsultaEstoquePosACD.aspx">Estoque Pós ACD</a></li>
                            <li><a href="ConsultaNotasRemessaPosACD.aspx">Consulta Notas Remessa Pós ACD</a></li> 
                        </ul>
                    </li>
                </ul>

                <ul class="nav navbar-nav">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false"><i class="fa fa-search"></i>&nbsp;Pesquisas<span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="ImportarDUE.aspx">Consultar DUE</a></li>
                            <li><a href="ConsultarNFs.aspx">Consultar CCT</a></li>
                        </ul>
                    </li>
                </ul>

                <ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Olá, <%= HttpContext.Current.Session["UsuarioLogin"] %> <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="Sair.aspx">Sair</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
            <% } %>
        </div>
    </nav>

    <div class="container-fluid">
        <form runat="server">
            <asp:ScriptManager runat="server" AsyncPostBackTimeout="360000">
                <Scripts>
                    <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                    <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                    <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                    <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                    <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                    <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                    <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                    <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                </Scripts>
            </asp:ScriptManager>

            <br />

            <div class="row">
                <div class="col-md-12">

                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title">Consultar Estoque Pós-ACD</h3>
                        </div>
                        <div class="panel-body">

                            <div class="row no-gutter">
                                <div class="col-md-10">
                                    <label for="txtUpload" class="control-label">Arquivo Notas de Remessa:</label>
                                    <asp:FileUpload ID="txtUpload" runat="server" CssClass="btn btn-default" />
                                    <p class="help-block">
                                        Clique no botão "Escolher arquivo" e selecione o arquivo .csv ou .txt de notas fiscais de remessa
                                    </p>
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
                                                <asp:BoundField DataField="DataDUE" HeaderText="Data Envio DUE" />
                                                <asp:BoundField DataField="DUE" HeaderText="Número DUE" />
                                                <asp:BoundField DataField="ChaveDUE" HeaderText="Chave DUE" />
                                                <asp:BoundField DataField="DeclaranteCnpj" HeaderText="CNPJ do Declarante" />
                                                <asp:BoundField DataField="DeclaranteNome" HeaderText="Nome do Declarante" />
                                                <asp:BoundField DataField="UltimoEvento" HeaderText="Último Evento" />
                                                <asp:BoundField DataField="DataUltimoEvento" HeaderText="Data do Último Evento" />
                                                <asp:BoundField DataField="DataAverbacao" HeaderText="Data Averbação" />
                                                <asp:BoundField DataField="StatusDUE" HeaderText="Status" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td colspan="11">

                                                                <div class="col-md-12">
                                                                    <div id="div<%# Eval("Id") %>" style="display: none; position: relative; left: 15px; top: 1px; bottom: 0px; white-space: nowrap;">

                                                                        <div class="table-responsive">
                                                                            <asp:GridView ID="gvEstoqueDetalhes" DataKeyNames="Id" runat="server" AutoGenerateColumns="False"
                                                                                ForeColor="#333333" CssClass="table table-striped table-condensed borda-collapse no-padding" EmptyDataText="Nenhum registro encontrado." OnRowDataBound="gvEstoqueDetalhes_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                                                        <ItemTemplate>
                                                                                            <a href="JavaScript:divexpandcollapse('div<%# Eval("Id") %>');">
                                                                                                <img id="imgdiv<%# Eval("Id") %>" border="0" src="Content/imagens/plus.png" alt="" /></a>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="20px" VerticalAlign="Middle"></ItemStyle>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="ITEM" HeaderText="Item" ItemStyle-Width="24px" />
                                                                                    <asp:BoundField DataField="NCM" HeaderText="NCM" ItemStyle-Width="100px" />
                                                                                    <asp:BoundField DataField="Descricao" HeaderText="Mercadoria" />
                                                                                    <asp:BoundField DataField="Quantidade" HeaderText="Quantidade" />
                                                                                    <asp:BoundField DataField="CFOP" HeaderText="CFOP" />
                                                                                    <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF Exportação" />
                                                                                    <asp:BoundField DataField="Numero" HeaderText="Número NF" />
                                                                                    <asp:BoundField DataField="Modelo" HeaderText="Modelo NF" />
                                                                                    <asp:BoundField DataField="Serie" HeaderText="Série NF" />
                                                                                    <asp:BoundField DataField="UF" HeaderText="UF NF" />
                                                                                    <asp:BoundField DataField="CnpjEmitente" HeaderText="CNPJ Emitente" />
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <tr>
                                                                                                <td colspan="13" style="padding: 0px;">

                                                                                                    <div class="col-md-12">
                                                                                                        <div id="div<%# Eval("Id") %>" style="display: none; position: relative; left: 27px; bottom: 0px; white-space: nowrap;">

                                                                                                            <div class="table-responsive">
                                                                                                                <asp:GridView ID="gvNotasRemessa" runat="server" AutoGenerateColumns="False"
                                                                                                                    ForeColor="#333333" CssClass="table table-striped table-condensed sem-margem border-bottom-zero" EmptyDataText="Nenhum registro encontrado.">
                                                                                                                    <Columns>
                                                                                                                        <asp:BoundField DataField="TipoNF" ItemStyle-Width="138px" HeaderText="Tipo NF" />
                                                                                                                        <asp:BoundField DataField="ChaveNF" HeaderText="Chave" />
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
                                    <asp:Button ID="btnGerarCsv" Text="Gerar .CSV" CssClass="btn btn-info btn-block" runat="server" OnClick="btnGerarCsv_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </form>
    </div>

    <div id="footer" class="container">
        <nav class="navbar navbar-inverse navbar-fixed-bottom navbar-custom">
            <div class="navbar-inner navbar-content-center">
                <p class="text-muted text-center">CARGILL Agricola SA <%= DateTime.Now.Year %></p>
            </div>
        </nav>
    </div>

    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script src="Content/js/site.js"></script>
    <script src="Content/js/select2.min.js"></script>
    <script src="Content/js/jquery.mask.min.js"></script>

    <script>
        var urlBase = '<%= Page.ResolveUrl("~") %>';

        $('#txtDataDUEDe').mask('00/00/0000');
        $('#txtDataDUEAte').mask('00/00/0000');
        $('#txtEmissaoNotaExpDe').mask('00/00');
        $('#txtEmissaoNotaExpAte').mask('00/00');

        $('#cbUnidadeRFB').select2();
        $('#cbRecintoAduaneiroDespacho').select2();

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

    </script>

</body>
</html>

