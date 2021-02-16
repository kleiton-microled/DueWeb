<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation ="true" AutoEventWireup="true" Async="true" CodeBehind="ConsultarDUE.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarDUE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <style>
        .gvDue {
            height: expression(getScrollBottom(this.parentNode.parentNode.parentNode.parentNode));
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">DUEs cadastradas</h3>
                </div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-md-12">

                     
                            <asp:Panel runat="server" Visible="false" ID="alertaValidacao">
                                <div class="alert alert-warning">
                                    <strong>Verifique o arquivo de retorno!</strong>
                                </div>
                            </asp:Panel>
                          

                            <% if (ViewState["RetornoSucesso"] != null)
                                {
                                    if ((bool)ViewState["RetornoSucesso"] == true)
                                    {%>
                            <div class="alert alert-success">
                                <strong>Sucesso!</strong> DUE pronta para ser enviada
                            </div>
                            <% }
                                else
                                {
                                    
                            
                             %>
                                <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />
                            <% 
                                    
                                }
                            }%>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">DUE:</label>
                                <asp:TextBox ID="txtDUE" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">De:</label>
                                <asp:TextBox ID="txtDe" runat="server" CssClass="form-control form-control-sm data" placeholder="dd/mm/yyyy"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">Até:</label>
                                <asp:TextBox ID="txtAte" runat="server" CssClass="form-control form-control-sm data" placeholder="dd/mm/yyyy"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">RUC:</label>
                                <asp:TextBox ID="txtRUC" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">Status:</label>
                                <asp:DropDownList ID="cbStatus" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                    <asp:ListItem Text="- Selecione -" Value="" />
                                    <asp:ListItem Text="Pendente" Value="0" />
                                    <asp:ListItem Text="Enviado" Value="1" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Chave DUE:</label>
                                <asp:TextBox ID="txtChaveDUE" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">&nbsp</label>
                                <p><a href="ConsultarDUE.aspx" class="btn btn-sm btn-warning btn-block" data-toggle="tooltip" data-placement="top" title="Limpar"><i class="fa fa-trash"></i>&nbsp;Limpar</a></p>
                                <%--<asp:Button ID="btnLimpar" runat="server" CssClass="btn btn-warning btn-block" Text="Limpar" OnClick="btnLimpar_Click" />--%>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">&nbsp</label>
                                <%--<asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-block" OnClick="btnFiltrar_Click" />--%>
                                <p>
                                    <button runat="server" id="btnFiltrar" class="btn btn-sm btn-primary btn-block" data-toggle="tooltip" data-placement="top" title="Pesquisar" onserverclick="btnFiltrar_Click">
                                        <i class="fa fa-search"></i>&nbsp;Pesquisar
                                    </button>
                                </p>
                            </div>
                        </div>
                    </div>

                    <br />
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive dropdown-menu-sobrepor ">
                                <asp:GridView ID="gvDUE"
                                    runat="server"
                                    Width="100%"
                                    ClientIDMode="Static"
                                    CssClass="table table-sm table-hover"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="11px" CellSpacing="0" CellPadding="0"
                                    ShowHeaderWhenEmpty="True"
                                    DataKeyNames="Id" OnRowCommand="gvDUE_RowCommand" AllowPaging="True" OnPageIndexChanging="gvDUE_PageIndexChanging" PageSize="14">
                                    <Columns>

                                        <asp:BoundField DataField="DataCadastro" HeaderText="Data Cadastro">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Login" HeaderText="Usuário" />

                                        <asp:BoundField DataField="DUE" HeaderText="DUE">
                                            <ItemStyle Wrap="False" />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="RUC" HeaderText="RUC" />
                                        <asp:BoundField DataField="ChaveAcesso" HeaderText="Chave" />
                                        <asp:BoundField DataField="DocumentoDeclarante" HeaderText="Declarante" />
                                        <asp:BoundField DataField="SituacaoEspecial" HeaderText="Tipo" />
                                        <%--<asp:BoundField DataField="MoedaNegociacao" HeaderText="MOEDA" />--%>
                                        <asp:BoundField DataField="FormaExportacao" HeaderText="Forma Exp." />
                                        <asp:BoundField DataField="RecintoAduaneiroDespachoId" HeaderText="Recinto Despacho" />
                                        <asp:BoundField DataField="RecintoAduaneiroEmbarqueId" HeaderText="Recinto Embarque" />
                                        <asp:BoundField DataField="SituacaoDUE" HeaderText="Situação" />

                                       <%-- <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlStatusRetificado" runat="server" Visible='<%# Eval("RetificadoSiscomex").ToString() == "1" %>'>
                                                    <span class='label label-primary'>Retificado</span>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlStatusImportado" runat="server" Visible='<%# Eval("DueImportada").ToString() == "1" && Eval("RetificadoSiscomex").ToString() == "0" && Eval("EnviadoSiscomex").ToString() == "0" %>'>
                                                    <span class='label label-default'>Importado</span>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlStatusEnviadoSiscomex" runat="server" Visible='<%# Eval("RetificadoSiscomex").ToString() == "0" %>'>
                                                    <%# (Eval("EnviadoSiscomex").ToString() == "1" ? "<span class='label label-success'>Enviado</span>" : "<span class='label label-warning'>&nbsp;Pendente&nbsp;</span>") %>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>--%>

                                          <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Panel ID="pnlStatusRetificado" runat="server" Visible='<%# Eval("RetificadoSiscomex").ToString() == "1" %>'>
                                                    <span class='label label-primary'>Retificado</span>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlStatusImportado" runat="server" Visible='<%# Eval("ImportadoSiscomex").ToString() == "1" && Eval("RetificadoSiscomex").ToString() == "0" && Eval("EnviadoSiscomex").ToString() == "1" %>'>
                                                    <span class='label label-default'>Importado</span>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlStatusEnviadoSiscomex" runat="server" Visible='<%# Eval("RetificadoSiscomex").ToString() == "0" && Eval("ImportadoSiscomex").ToString() == "0" %>'>
                                                    <%# (Eval("EnviadoSiscomex").ToString() == "1" ? "<span class='label label-success'>Enviado</span>" : "<span class='label label-warning'>Pendente</span>") %>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>

                                                <div id="div-acoes" class="dropdown div-menu-acoes">
                                                    <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-toggle="dropdown">
                                                        <i class="fas fa-list"></i>&nbsp;&nbsp;Ações
                                                        <span class="caret"></span>
                                                    </button>
                                                    <ul class="dropdown-menu pull-right">
                                                        <li><a href="CadastrarDUE.aspx?id=<%# Eval("Id") %>&completa=<%# Eval("Completa") %>#step-2"><i class="fas fa-edit"></i>&nbsp;&nbsp;Editar</a></li>
                                                        <li><a href="ConsultarNFsDUE.aspx?id=<%# Eval("Id") %>" target="_blank"><i class="fas fa-list"></i>&nbsp;&nbsp;Consultar Notas Fiscais</a></li>
                                                        <li><a href="ResumoDUE.aspx?due=<%# Eval("Id") %>" target="_blank"><i class="fas fa-search"></i>&nbsp;&nbsp;Visualizar</a></li>
                                                        <li>
                                                            <asp:LinkButton runat="server" ID="btnGerarXML" CommandName="GerarXML" CommandArgument="<%# Container.DataItemIndex %>">
                                                                <i class="fas fa-file-code"></i>&nbsp;&nbsp;Gerar XML
                                                            </asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" ID="btnValidarXML" CommandName="ValidarXML" CommandArgument="<%# Container.DataItemIndex %>">
                                                                <i class="fas fa-file-code"></i>&nbsp;&nbsp;Validar
                                                            </asp:LinkButton>
                                                        </li>
                                                        <%--<li>
                                                            <asp:LinkButton runat="server" ID="btnEnviarSiscomex" CommandName="EnviarSiscomex" CommandArgument="<%# Container.DataItemIndex %>">
                                                                <i class="fas fa-share-square"></i>&nbsp;&nbsp;Retificar
                                                            </asp:LinkButton>
                                                        </li>--%>
                                                        <li class="<%# (Eval("EnviadoSiscomex").ToString() == "0" && Eval("RetificadoSiscomex").ToString() == "0") ? "" : "invisivel" %>"><a href="ConfirmaExclusaoDUE.aspx?id=<%# Eval("Id") %>"><i class="fas fa-trash"></i>&nbsp;&nbsp;Excluir</a></li>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="campo-acao" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        function ValidarXML() {
            $('#validacao-modal').modal('show');
        }
        //$(document).ready(function () {

        //});
        //$("#menu-acoes").click(function () {
        //    if ($('#menu-acoes').attr('aria-expanded') === "true") {
        //        $('#gvDUE').height() = gvDUE.height() + $('.div-menu-acoes').height();
        //    }

        //}
        //);

        //$('.dropdown-menu-sobrepor').on('show.bs.dropdown', function () {
        //    $('.table-responsive').css("overflow", "inherit");
        //    //$('.table-responsive').scrollTop = $('.table-responsive').height + $('div-acoes').height;
        //    //$('.table-responsive').scrollTop = $('div-acoes').scrollTop;
        //});

        //$('.dropdown-menu-sobrepor').on('hide.bs.dropdown', function () {
        //    $('.table-responsive').css("overflow", "auto");
        //    //$('.table-responsive').height - $('div-acoes').height;
        //})
    </script>
</asp:Content>