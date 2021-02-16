<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastrarItensDUE.aspx.cs" Inherits="Sistema.DUE.Web.CadastrarItensDUE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/smart_wizard.css" rel="stylesheet" />
    <link href="Content/css/smart_wizard_theme_dots.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hddnExisteXML" Value="0" />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server"
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updateModal">
                <div align="center" class="updateModalBox">
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <br />
    <div class="row">
        <div class="col-sm-10 col-sm-offset-1">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Cadastro de DUE / Lançamento de Itens</h3>
                </div>
                <div class="panel-body">
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive dropdown-menu-scroll">

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <asp:HiddenField ID="txtDueID" runat="server" />
                                        <asp:HiddenField ID="txtDUEItemID" runat="server" />
                                        <asp:HiddenField ID="txtDUEDetalheItemID" runat="server" />
                                        <asp:HiddenField ID="txtSituacaoEspecial" runat="server" />

                                        <div class="row no-gutter">
                                            <div class="col-sm-12">

                                                <% if (ViewState["RetornoSucesso"] != null)
                                                    {
                                                        if ((bool)ViewState["RetornoSucesso"] == true)
                                                        {%>
                                                <% if ((bool)ViewState["RetornoWarnings"] == true)
                                                    {%>
                                                <div class="alert alert-warning">
                                                    <strong>Sucesso!</strong> Registro recebido e processado pelo Siscomex. Abaixo detalhes da operação:
                                                    <br />
                                                    <br />
                                                    <strong>Detalhes:</strong><br />
                                                    <ul>
                                                        <li><strong>Mensagem:</strong>  <%=ViewState["RetornoMessage"].ToString() %></li>
                                                        <li><strong>DUE:</strong>  <%=ViewState["RetornoDUE"].ToString() %></li>
                                                        <li><strong>RUC:</strong>  <%=ViewState["RetornoRUC"].ToString() %></li>
                                                        <li><strong>Chave:</strong>  <%=ViewState["ChaveDeAcesso"].ToString() %></li>
                                                    </ul>
                                                </div>
                                                <% }
                                                else
                                                { %>

                                                <div class="alert alert-success">                                                    
                                                    <strong>Sucesso!</strong> Registro recebido e processado pelo Siscomex. Abaixo detalhes da operação:
                                                    <br />
                                                    <br />
                                                    <strong>Detalhes:</strong><br />
                                                    <ul>
                                                        <li><strong>Mensagem:</strong>  <%=ViewState["RetornoMessage"].ToString() %></li>
                                                        <li><strong>DUE:</strong>  <%=ViewState["RetornoDUE"].ToString() %></li>
                                                        <li><strong>RUC:</strong>  <%=ViewState["RetornoRUC"].ToString() %></li>
                                                        <li><strong>Chave:</strong>  <%=ViewState["ChaveDeAcesso"].ToString() %></li>
                                                    </ul>
                                                </div>

                                                <% } %>
                                                <% }%>
                                                <% }%>

                                                <% if (ViewState["DueAverbada"] != null)
                                                    {
                                                        if ((bool)ViewState["DueAverbada"] == true)
                                                        {%>

                                                <div class="alert alert-warning">
                                                    Atenção! A DUE <%=ViewState["DueAverbadaNumero"].ToString() %> já foi <strong>Averbada</strong>
                                                </div>
                                                <%}
                                                    } %>

                                                <div id="smartwizard">
                                                    <ul>
                                                        <li><a href="#step-1">Passo 1<br />
                                                            <small>Informações Iniciais</small></a></li>
                                                        <li><a href="#step-2">Passo 2<br />
                                                            <small>Local de Despacho</small></a></li>
                                                        <li><a href="#step-3">Passo 3<br />
                                                            <small>Local de Embarque</small></a></li>
                                                        <li><a href="#step-4">Passo 4<br />
                                                            <small>Informações Complementares</small></a></li>
                                                        <li><a href="#step-5">Passo 5<br />
                                                            <small>Lançamento dos Itens</small></a></li>
                                                    </ul>

                                                    <div>
                                                        <div id="step-1" class=""></div>
                                                        <div id="step-2" class=""></div>
                                                        <div id="step-3" class=""></div>
                                                        <div id="step-4" class=""></div>

                                                        <div id="step-5">

                                                            <div id="ItemDUEMsg" class="alert alert-danger invisivel">
                                                                <ul>
                                                                </ul>
                                                            </div>

                                                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                                                            <asp:Panel ID="pnlItemMaster" runat="server">

                                                                <div class="row no-gutter">
                                                                  <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Nome do Exportador:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtItemExportador" runat="server" CssClass="form-control form-control-sm" MaxLength="250"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">CNPJ Exportador:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtItemDocumentoExportador" runat="server" CssClass="form-control form-control-sm" MaxLength="18"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Endereço:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtItemExportadorEndereco" runat="server" CssClass="form-control form-control-sm" MaxLength="250"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">UF:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtItemExportadorUF" runat="server" CssClass="form-control form-control-sm" MaxLength="2"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">País:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:DropDownList ID="cbExportadorPais" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Nome do Importador:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtImportadorNome" runat="server" CssClass="form-control form-control-sm" MaxLength="250"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Endereço do Importador:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtImportadorEndereco" runat="server" CssClass="form-control form-control-sm" MaxLength="250"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">País:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:DropDownList ID="cbImportadorPais" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Condição de Venda:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:DropDownList ID="cbCondicaoVenda" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                <asp:ListItem Text="C+F - COST PLUS FREIGHT" Value="C+F" />
                                                                                <asp:ListItem Text="C+I - COST PLUS INSURANCE" Value="C+I" />
                                                                                <asp:ListItem Text="CFR - COST AND FREIGHT" Value="CFR" />
                                                                                <asp:ListItem Text="CIF - COST, INSURANCE AND FREIGHT" Value="CIF" />
                                                                                <asp:ListItem Text="CIP - CARRIAGE AND INSURANCE PAID TO" Value="CIP" />
                                                                                <asp:ListItem Text="CPT - CARRIAGE PAID TO" Value="CPT" />
                                                                                <asp:ListItem Text="DAP - DELIVERED AT PLACE" Value="DAP" />
                                                                                <asp:ListItem Text="DAT - DELIVERED AT TERMINAL" Value="DAT" />
                                                                                <asp:ListItem Text="DDP - DELIVERED DUTY PAID" Value="DDP" />
                                                                                <asp:ListItem Text="EXW - EX WORKS" Value="EXW" />
                                                                                <asp:ListItem Text="FAS - FREE ALONGSIDE SHIP" Value="FAS" />
                                                                                <asp:ListItem Text="FCA - FREE CARRIER" Value="FCA" />
                                                                                <asp:ListItem Text="FOB - FREE ON BOARD" Value="FOB" Selected="True" />
                                                                                <asp:ListItem Text="OCV - OUTRA CONDICAO DE VENDA" Value="OCV" />
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Valor Unit. VMLE (TON):</label>
                                                                            <asp:TextBox ID="txtValorUnitVMLE" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True" OnTextChanged="txtValorUnitVMLE_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Valor Unit. VMCV (TON):</label>
                                                                            <asp:TextBox ID="txtValorUnitVMCV" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True" OnTextChanged="txtValorUnitVMCV_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                            <span class="text-danger">*</span>
                                                                            <label class="control-label">Motivo Dispensa da NF:</label>
                                                                            <asp:DropDownList ID="cbMotivoDispensaNotaFiscal" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                <asp:ListItem Text="" Value="0" />
                                                                                <asp:ListItem Text="BAGAGEM DESACOMPANHADA" Value="3001" />
                                                                                <asp:ListItem Text="BENS DE VIAJANTE NÃO INCLUÍDOS NO CONCEITO DE BAGAGEM" Value="3002" />
                                                                                <asp:ListItem Text="RETORNO DE MERCADORIA AO EXTERIOR ANTES DO REGISTRO DA DI" Value="3003" />
                                                                                <asp:ListItem Text="EMBARQUE ANTECIPADO" Value="3004" Selected="True" />
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-8">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Chave NF:</label>
                                                                            <asp:TextBox ID="txtChaveNFExp" runat="server" CssClass="form-control form-control-sm" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <label class="control-label">&nbsp;</label>
                                                                        <asp:Button ID="btnLimparItem" Text="Novo" CssClass="btn btn-primary btn-block" runat="server" OnClick="btnLimparItem_Click" />
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <label class="control-label">&nbsp;</label>
                                                                        <asp:Button ID="btnCadastrarMaster" Text="Gravar" CssClass="btn btn-primary btn-block" runat="server" OnClick="btnCadastrarMaster_Click" />
                                                                    </div>
                                                                </div>
                                                                <br />
                                                                <div class="row no-gutter">
                                                                    <div class="col-md-12">
                                                                        <div class="table-responsive">
                                                                            <asp:GridView
                                                                                ID="gvItensDUE"
                                                                                runat="server"
                                                                                Width="100%"
                                                                                CssClass="table table-striped table-bordered"
                                                                                GridLines="None"
                                                                                AutoGenerateColumns="False"
                                                                                Font-Size="12px"
                                                                                ShowHeaderWhenEmpty="True"
                                                                                DataKeyNames="Id"
                                                                                OnRowCommand="gvItensDUE_RowCommand">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="NroItem" HeaderText="#" />
                                                                                    <asp:BoundField DataField="Exportador" HeaderText="Exportador" />
                                                                                    <asp:BoundField DataField="Importador" HeaderText="Importador" />
                                                                                    <asp:BoundField DataField="MotivoDispensaNF" HeaderText="Motivo Dispensa NF" />
                                                                                    <asp:BoundField DataField="CondicaoVenda" HeaderText="Condição Venda" />
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton runat="server" ID="cmdLancarItens" ImageUrl="~/Content/imagens/check.png" CommandName="LANCAR_ITENS"
                                                                                                CommandArgument="<%# Container.DataItemIndex %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton runat="server" ID="cmdExcluirItem" ImageUrl="~/Content/imagens/excluir.png" CommandName="EXCLUIR_ITEM"
                                                                                                OnClientClick="return confirm('Confirma a exclusão do Item?');" CommandArgument="<%# Container.DataItemIndex %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnkReplicar" runat="server" CommandArgument="<%# Container.DataItemIndex %>" CommandName="REPLICAR_ITEM">Replicar</asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>

                                                            <div class="row no-gutter">
                                                                <div class="col-sm-12">
                                                                    <div class="alert alert-info">
                                                                        Clique em
                                                                        <img src="Content/imagens/check.png" />
                                                                        para incluir os detalhes do item
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <asp:Panel ID="pnlDetalhesItens" runat="server" Visible="false">

                                                                <h4 class="border-bottom border-gray pb-2"><i class="fa fa-list"></i>&nbsp;Detalhes do Item 
                                                                    <asp:Label ID="lblItemSelecionado" runat="server" Text="" Font-Bold="true"></asp:Label></h4>
                                                                <hr />

                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Sub Item:</label>
                                                                            <asp:TextBox ID="txtItem" runat="server" CssClass="form-control form-control-sm inteiro" Enabled="false"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Qtde:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtQtdeItem" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">VMLE:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtValorMercadoriaLocalEmbarque" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">País Destino:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:DropDownList ID="cbPaisDestino" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Prioridade da Carga:</label>
                                                                            <asp:DropDownList ID="cbPrioridadeCarga" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                <asp:ListItem Text="" Value="0" />
                                                                                <asp:ListItem Text="CARGA VIVA" Value="5001" />
                                                                                <asp:ListItem Text="CARGA PERECÍVEL" Value="5002" />
                                                                                <asp:ListItem Text="CARGA PERIGOSA" Value="5003" />
                                                                                <asp:ListItem Text="PARTES / PEÇAS DE AERONAVE" Value="5006" />
                                                                                <%--5006 através do link portalunico.siscomex.gov.br/docs/visual-xml/--%>
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Hora Limite:</label>
                                                                            <asp:TextBox ID="txtDataHoraLimite" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Descrição Complementar:</label>
                                                                            <asp:TextBox ID="txtDescricaoComplementarItem" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">NCM:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtNCM" runat="server" CssClass="form-control form-control-sm" MaxLength="10" AutoPostBack="true" OnTextChanged="txtNCM_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Qtde Un.:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtQtdUnidades" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">VMCV:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtValorMercadoriaCondicaoVenda" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Cód Produto:</label>
                                                                            <asp:TextBox ID="txtCodProduto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-3">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Descrição da Mercadoria:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtDescricaoMercadoria" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>




                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Descr Un.:</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:DropDownList ID="txtDescricaoUnidade" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                <asp:ListItem Text="" Value="" />
                                                                                <asp:ListItem Text="TON" Value="TON" />
                                                                                <asp:ListItem Text="KG" Value="KG" />
                                                                                <asp:ListItem Text="UN" Value="UN" />
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-2">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Peso Líq. Total (Kg):</label>
                                                                            <span class="text-danger">*</span>
                                                                            <asp:TextBox ID="txtPesoLiquidoTotal" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True" OnTextChanged="txtPesoLiquidoTotal_TextChanged"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-6">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Enquadramento 1:</label>
                                                                            <asp:DropDownList ID="cbEnquadramento1" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Enquadramento 2:</label>
                                                                            <asp:DropDownList ID="cbEnquadramento2" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="row no-gutter">
                                                                    <div class="col-sm-6">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Enquadramento 3:</label>
                                                                            <asp:DropDownList ID="cbEnquadramento3" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-6">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Enquadramento 4:</label>
                                                                            <asp:DropDownList ID="cbEnquadramento4" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <asp:Panel ID="pnlAtributosNCM" runat="server" Visible="false">
                                                                    <div class="row no-gutter">
                                                                        <div class="col-sm-6">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Padrão de Qualidade:</label>
                                                                                <span class="text-danger">*</span>
                                                                                <asp:DropDownList ID="cbAttrPadraoQualidade" runat="server" Enabled="false" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Embarcado Em:</label>
                                                                                <span class="text-danger">*</span>
                                                                                <asp:DropDownList ID="cbAttrEmbarcadoEm" runat="server" Enabled="false" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Tipo:</label>
                                                                                <span class="text-danger">*</span>
                                                                                <asp:DropDownList ID="cbAttrTipo" runat="server" Enabled="false" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Método Proc:</label>
                                                                                <span class="text-danger">*</span>
                                                                                <asp:DropDownList ID="cbAttrMetodoProcessamento" Enabled="false" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row no-gutter">
                                                                        <div class="col-sm-6">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Característica Especial:</label>
                                                                                <asp:DropDownList ID="cbAttrCaracteristicaEspecial" Enabled="false" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-4">
                                                                            <div class="form-group">
                                                                                <label class="control-label">Outra Característica Especial:</label>
                                                                                <asp:TextBox ID="txtAttrOutraCaracteristicaEspecial" runat="server" Enabled="false" CssClass="form-control form-control-sm" MaxLength="100"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-sm-2">
                                                                            <label class="control-label">&nbsp;</label>
                                                                            <div class="checkbox">
                                                                                <label>
                                                                                    <asp:CheckBox ID="chkAttrEmbalagemFinal" runat="server" Enabled="false" />
                                                                                    <asp:Label Text="Embalagem Final?" runat="server" />
                                                                                </label>
                                                                            </div>
                                                                        </div>
                                                                    </div>

                                                                </asp:Panel>

                                                                <div class="row no-gutter">

                                                                    <div class="col-sm-3">
                                                                        <div class="form-group">
                                                                            <label class="control-label">&nbsp;</label>
                                                                            <a id="btnAtoConcessorio" runat="server" href="#" onclick="abrirModalAc();" class="btn btn-primary btn-block disabled" data-toggle="modal" role="button" data-target="#modalAtoConcessorio">Cadastrar Ato Concessório</a>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-3">
                                                                        <div class="form-group">
                                                                            <label class="control-label">&nbsp;</label>
                                                                            <a id="btnLPCO" href="#" runat="server" onclick="abrirModalLPCO();" class="btn btn-primary btn-block disabled" data-toggle="modal" role="button" data-target="#modalLPCO">Cadastrar LPCO</a>
                                                                        </div>
                                                                    </div>

                                                                    <div class="col-sm-4">
                                                                        <div class="form-group">
                                                                            <label class="control-label">Comissão Agente:</label>
                                                                            <asp:TextBox ID="txtComissaoAgente" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">&nbsp;</label>
                                                                            <asp:Button ID="btnLimparItemDetalhe" Text="Limpar" CssClass="btn btn-default btn-block" runat="server" OnClick="btnLimparItemDetalhe_Click" />
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-sm-1">
                                                                        <div class="form-group">
                                                                            <label class="control-label">&nbsp;</label>
                                                                            <asp:Button ID="btnAdicionarItemDetalhe" Text="Adicionar" CssClass="btn btn-primary btn-block" runat="server" OnClick="btnAdicionarItemDetalhe_Click" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <br />
                                                                <div class="row no-gutter">
                                                                    <div class="col-md-12">
                                                                        <div class="table-responsive">
                                                                            <asp:GridView
                                                                                ID="gvDetalhesItem"
                                                                                runat="server"
                                                                                Width="100%"
                                                                                CssClass="table table-striped table-bordered"
                                                                                GridLines="None"
                                                                                AutoGenerateColumns="False"
                                                                                Font-Size="12px"
                                                                                ShowHeaderWhenEmpty="True"
                                                                                DataKeyNames="Id" OnRowCommand="gvDetalhesItem_RowCommand">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Item" HeaderText="Sub Item" />
                                                                                    <asp:BoundField DataField="DescricaoMercadoria" HeaderText="Descrição Mercadoria" />
                                                                                    <asp:BoundField DataField="QuantidadeUnidades" HeaderText="Qtd. Unidades" />
                                                                                    <asp:BoundField DataField="PesoLiquidoTotal" HeaderText="Peso Liquido Total" />
                                                                                    <asp:BoundField DataField="ValorMercadoriaLocalEmbarque" HeaderText="VMLE" />
                                                                                    <asp:BoundField DataField="ValorMercadoriaCondicaoVenda" HeaderText="VMCV" />
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton runat="server" ID="cmdEditarDetalheItem" ImageUrl="~/Content/imagens/editar.gif" CommandName="EDITAR_DETALHE_ITEM"
                                                                                                CommandArgument="<%# Container.DataItemIndex %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:ImageButton runat="server" ID="cmdExcluirDetalheItem" ImageUrl="~/Content/imagens/excluir.png" CommandName="EXCLUIR_DETALHE_ITEM"
                                                                                                CommandArgument="<%# Container.DataItemIndex %>" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </asp:Panel>

                                                            <div class="row no-gutter">
                                                                <div class="col-sm-2">
                                                                    <asp:Button ID="btnRetornar" Text="Retornar" CssClass="btn btn-primary btn-block" runat="server" OnClick="btnRetornar_Click" />
                                                                </div>
                                                                <div class="col-sm-2 col-sm-offset-1">
                                                                    <asp:Panel runat="server" ID="btnSalvarXML">
                                                                        <div class="dropdown ">
                                                                            <button class="btn btn-info btn-block dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                                                Baixar XML
                                                                        <span class="caret"></span>
                                                                            </button>
                                                                            <ul class="dropdown-menu dropdown-menu-bg text-center dropdown-menu-scroll-referencia" style="" aria-labelledby="dropdownMenu1">
                                                                                 <asp:Panel runat="server" ID="btnXMLEnviado"><li><a href="ArquivoXML.aspx?id=<%= this.txtDueID.Value %>&dest=SALVAR&tipo=ENVIADO">Enviado</a></li> </asp:Panel>
                                                                                <asp:Panel runat="server" ID="btnXMLRetorno"><li><a href="ArquivoXML.aspx?id=<%= this.txtDueID.Value %>&dest=SALVAR&tipo=RETORNO">Retorno</a></li> </asp:Panel>
                                                                            </ul>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <asp:Button ID="btnIrNotas" Text="Ir para Notas" CssClass="btn btn-warning btn-block" runat="server" OnClick="btnIrParaNotas_Click" />
                                                                </div>
                                                                <div class="col-sm-2">
                                                                    <a href="ResumoDUE.aspx?due=<%= this.txtDueID.Value %>" target="_blank" class="btn btn-info btn-block">Resumo</a>
                                                                </div>
                                                                <div class="col-sm-3">
                                                                    <asp:Button ID="btnFinalizarDUE" Text="Finalizar e enviar ao siscomex" CssClass="btn btn-warning btn-block" runat="server" OnClick="btnFinalizarDUE_Click" />
                                                                </div>
                                                            </div>

                                                      
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>



                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <div class="modal fade" id="modalAtoConcessorio" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Dados do Ato Concessório (AC)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="InformacoesAtoConcessorio" class="alert alert-danger invisivel">
                        <ul>
                        </ul>
                    </div>

                    <div class="row no-gutter">

                        <asp:HiddenField ID="txtAtoConcessorioId" runat="server" />

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">Número do AC:</label>
                                <asp:TextBox ID="txtNumeroAC" runat="server" CssClass="form-control form-control-sm" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">CNPJ do beneficiário do AC:</label>
                                <asp:TextBox ID="txtCNPJBeneficiarioAC" runat="server" CssClass="form-control form-control-sm" MaxLength="14"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">Número item AC:</label>
                                <asp:TextBox ID="txtNumeroItemAC" runat="server" CssClass="form-control form-control-sm" MaxLength="3"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">NCM item AC:</label>
                                <asp:TextBox ID="txtNCMItemAC" runat="server" CssClass="form-control form-control-sm" MaxLength="8"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="row no-gutter">

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">Quantidade Utilizada:</label>
                                <asp:TextBox ID="txtQuantidadeUtilizadaAC" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">VMLE com cobertura cambial:</label>
                                <asp:TextBox ID="txtVMLEComCoberturaCambialAC" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label">VMLE sem cobertura cambial:</label>
                                <asp:TextBox ID="txtVMLESemCoberturaCambialAC" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-1">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <button id="btnLimparAC" type="button" class="btn btn-warning btn-block">Limpar</button>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <button id="btnAdicionarAC" type="button" class="btn btn-warning btn-block">Adicionar</button>
                            </div>
                        </div>

                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-12">

                            <div id="tbAtosConcessorios" class="table-responsive">
                            </div>

                        </div>
                    </div>


                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="modalLPCO" tabindex="-1" role="dialog" aria-labelledby="exampleModalLPCOLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLPCOLabel">Dados de LPCO</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="InformacoesLPCO" class="alert alert-danger invisivel">
                        <ul>
                        </ul>
                    </div>

                    <div class="row no-gutter">

                        <asp:HiddenField ID="txtLPCOId" runat="server" />

                        <div class="col-sm-8">
                            <div class="form-group">
                                <label class="control-label">Número da LPCO:</label>
                                <asp:TextBox ID="txtNumeroLPCO" runat="server" CssClass="form-control form-control-sm" MaxLength="11"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <button id="btnLimparLPCO" type="button" class="btn btn-warning btn-block">Limpar</button>
                            </div>
                        </div>

                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <button id="btnAdicionarLPCO" type="button" class="btn btn-warning btn-block">Adicionar</button>
                            </div>
                        </div>


                    </div>

                    <div class="row no-gutter">
                        <div class="col-sm-12">

                            <div id="tbLPCO" class="table-responsive">
                            </div>

                        </div>
                    </div>


                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/jquery.smartWizard.js"></script>
    <script src="Content/js/select2.min.js"></script>

    <script>

        $('#btnLimparAC').click(function () {

            event.preventDefault();

            $('#MainContent_txtAtoConcessorioId').val('');
            $('#MainContent_txtNumeroAC').val('');
            $('#MainContent_txtCNPJBeneficiarioAC').val('');
            $('#MainContent_txtNumeroItemAC').val('');
            $('#MainContent_txtNCMItemAC').val('');
            $('#MainContent_txtQuantidadeUtilizadaAC').val('');
            $('#MainContent_txtVMLEComCoberturaCambialAC').val('');
            $('#MainContent_txtVMLESemCoberturaCambialAC').val('');

            $('#btnAdicionarAC').text('Adicionar');
        });

        $('#btnAdicionarAC').click(function () {

            event.preventDefault();

            var idAc = $("#<%=txtAtoConcessorioId.ClientID%>").val();
            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();
            var numeroAc = $("#<%=txtNumeroAC.ClientID%>").val();
            var cnpjAc = $("#<%=txtCNPJBeneficiarioAC.ClientID%>").val();
            var numeroItemAc = $("#<%=txtNumeroItemAC.ClientID%>").val();
            var ncmAc = $("#<%=txtNCMItemAC.ClientID%>").val();
            var qtdeUtilizadaAc = $("#<%=txtQuantidadeUtilizadaAC.ClientID%>").val();
            var vmleComCobertura = $("#<%=txtVMLEComCoberturaCambialAC.ClientID%>").val();
            var vmleSemCobertura = $("#<%=txtVMLESemCoberturaCambialAC.ClientID%>").val();

            $('#InformacoesAtoConcessorio ul li').remove();
            $('#InformacoesAtoConcessorio').addClass('invisivel');

            var existemErros = false;

            if (numeroAc === '') {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O número do Ato Concessório é obrigatório.</li>');

                existemErros = true;
            }

            if (cnpjAc.length !== 14) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O CNPJ do beneficiário é inválido. Verifique se o valor digitado corresponde a um número CNPJ.</li>');

                existemErros = true;
            }

            if (!isNumero(numeroItemAc)) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O número do item do Ato Concessório é inválido.</li>');

                existemErros = true;
            }

            if (ncmAc.length !== 8) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O NCM do Ato Concessório é inválido.</li>');

                existemErros = true;
            }

            if (!isNumero(qtdeUtilizadaAc)) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>A Quantidade utilizada do Ato Concessório é inválida.</li>');

                existemErros = true;
            }


            if (!isNumero(vmleComCobertura)) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O VMLE com cobertura cambial é inválido.</li>');

                existemErros = true;
            }


            if (!isNumero(vmleSemCobertura)) {
                $('#InformacoesAtoConcessorio > ul')
                    .append('<li>O VMLE sem cobertura cambial é inválido.</li>');

                existemErros = true;
            }

            if (existemErros) {
                $('#InformacoesAtoConcessorio').removeClass('invisivel');
                existemErros = false;
                return false;
            }

            if (idAc === undefined)
                idAc = '';

            var dadosForm = '{ detalheItemId: "' + detalheItemId + '", numeroAc:"' + numeroAc + '",cnpjAc:"' + cnpjAc + '",numeroItemAc:"' + numeroItemAc + '",ncmAc:"' + ncmAc + '",qtdeUtilizadaAc:"' + qtdeUtilizadaAc + '",vmleComCobertura:"' + vmleComCobertura + '",vmleSemCobertura:"' + vmleSemCobertura + '", id:"' + idAc + '"  }';

            $.ajax({
                type: "POST",
                url: "CadastrarItensDUE.aspx/AdicionarAtoConcessorio",
                data: dadosForm,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });

            function OnSuccess(response) {

                var dados = JSON.parse(response.d);
                $("#tbAtosConcessorios").html(montarTabelaAtosConcessorios(dados));

                $("#<%=txtNumeroAC.ClientID%>").val('');
                $("#<%=txtCNPJBeneficiarioAC.ClientID%>").val('');
                $("#<%=txtNumeroItemAC.ClientID%>").val('');
                $("#<%=txtNCMItemAC.ClientID%>").val('');
                $("#<%=txtQuantidadeUtilizadaAC.ClientID%>").val('');
                $("#<%=txtVMLEComCoberturaCambialAC.ClientID%>").val('');
                $("#<%=txtVMLESemCoberturaCambialAC.ClientID%>").val('');

            }

        });

        function abrirModalAc() {

            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();

            var enquadramento1 = parseInt($('#MainContent_cbEnquadramento1 option:selected').val());
            var enquadramento2 = parseInt($('#MainContent_cbEnquadramento2 option:selected').val());
            var enquadramento3 = parseInt($('#MainContent_cbEnquadramento3 option:selected').val());
            var enquadramento4 = parseInt($('#MainContent_cbEnquadramento4 option:selected').val());

            if (enquadramento1 === 81101 || enquadramento2 === 81101 || enquadramento3 === 81101 || enquadramento4 === 81101) {

                var qtdeItem = $('#MainContent_txtQtdeItem').val();
                var vmleItem = $('#MainContent_txtValorMercadoriaLocalEmbarque').val();

                $('#MainContent_txtQuantidadeUtilizadaAC').val(qtdeItem);
                $('#MainContent_txtVMLEComCoberturaCambialAC').val(vmleItem);

            } else {
                $('#MainContent_txtQuantidadeUtilizadaAC').val('');
                $('#MainContent_txtVMLEComCoberturaCambialAC').val('');
            }

            $.ajax({
                type: "GET",
                url: "CadastrarItensDUE.aspx/ObterAtosConcessoriosJson?detalheItemId=" + detalheItemId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });

            function OnSuccess(response) {

                var dados = JSON.parse(response.d);
                $("#tbAtosConcessorios").html(montarTabelaAtosConcessorios(dados));
            }
        }

        function montarTabelaAtosConcessorios(dados) {

            var tabela =
                '<table id="tbAtosConcessorios" class="table table-sm">' +
                '   <thead>' +
                '       <tr>' +
                '           <th>Número AC</th>' +
                '           <th>CNPJ Benef. AC</th>' +
                '           <th>Número item AC</th>' +
                '           <th>NCM item AC</th>' +
                '           <th>Qtde Utilizada</th>' +
                '           <th>VMLE c/ Cob. Cambial</th>' +
                '           <th>VMLE s/ Cob. Cambial</th>' +
                '           <th>&nbsp;</td>' +
                '           <th>&nbsp;</td>' +
                '       </tr>' +
                '   </thead>' +
                '<tbody>';

            dados.forEach(function (linha) {
                tabela = tabela +
                    '   <tr>' +
                    '       <td>' + linha.Numero + '</td>' +
                    '       <td>' + linha.CNPJBeneficiario + '</td>' +
                    '       <td>' + linha.NumeroItem + '</td>' +
                    '       <td>' + linha.NCMItem + '</td>' +
                    '       <td>' + linha.QuantidadeUtilizada + '</td>' +
                    '       <td>' + linha.VMLEComCoberturaCambial + '</td>' +
                    '       <td>' + linha.VMLESemCoberturaCambial + '</td>' +
                    '       <td><a href="#" onclick="atualizarAtoConcessorio(' + linha.Id + ')"><img src="Content/imagens/editar.gif" /></a></td>' +
                    '       <td><a href="#" onclick="removerAtoConcessorio(' + linha.Id + ')"><img src="Content/imagens/excluir.png" /></a></td>' +
                    '   </tr>';
            });

            tabela = tabela + '</tbody>' +
                '</table>';

            return tabela;
        }

        function atualizarAtoConcessorio(id) {

            event.preventDefault();

            $.ajax({
                type: "GET",
                url: "CadastrarItensDUE.aspx/ObterAtosConcessoriosPorIdJson?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var dados = JSON.parse(response.d);

                    if (dados) {

                        $('#MainContent_txtAtoConcessorioId').val(dados.Id);
                        $('#MainContent_txtNumeroAC').val(dados.Numero);
                        $('#MainContent_txtCNPJBeneficiarioAC').val(dados.CNPJBeneficiario);
                        $('#MainContent_txtNumeroItemAC').val(dados.NumeroItem);
                        $('#MainContent_txtNCMItemAC').val(dados.NCMItem);
                        $('#MainContent_txtQuantidadeUtilizadaAC').val(dados.QuantidadeUtilizada);
                        $('#MainContent_txtVMLEComCoberturaCambialAC').val(dados.VMLEComCoberturaCambial);
                        $('#MainContent_txtVMLESemCoberturaCambialAC').val(dados.VMLESemCoberturaCambial);

                        $('#btnAdicionarAC').text('Atualizar');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });

        }

        function removerAtoConcessorio(id) {

            event.preventDefault();

            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();

            $.ajax({
                type: "POST",
                url: "CadastrarItensDUE.aspx/RemoverAtoConcessorio",
                data: '{ id: "' + id + '", detalheItemId: "' + detalheItemId + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });

            function OnSuccess(response) {
                var dados = JSON.parse(response.d);
                $("#tbAtosConcessorios").html(montarTabelaAtosConcessorios(dados));
            }
        }

        var isNumero = function (numero) {
            return (numero !== null && !isNaN(parseFloat(numero)));
        };

        function habilitarBotoesAtoConcessorioLPCO() {

            $('#MainContent_btnAtoConcessorio').addClass('disabled');
            $('#MainContent_btnAtoConcessorio').prop('disabled', true);

            $('#MainContent_btnLPCO').addClass('disabled');
            $('#MainContent_btnLPCO').prop('disabled', true);

            var enquadramento1 = parseInt($('#MainContent_cbEnquadramento1 option:selected').val());
            var enquadramento2 = parseInt($('#MainContent_cbEnquadramento2 option:selected').val());
            var enquadramento3 = parseInt($('#MainContent_cbEnquadramento3 option:selected').val());
            var enquadramento4 = parseInt($('#MainContent_cbEnquadramento4 option:selected').val());

            var detalheItemId = parseInt($("#<%=txtDUEDetalheItemID.ClientID%>").val());

            if (detalheItemId > 0) {

                if (enquadramento1 === 81101 || enquadramento2 === 81101 || enquadramento3 === 81101 || enquadramento4 === 81101) {
                    $('#MainContent_btnAtoConcessorio').removeClass('disabled');
                    $('#MainContent_btnAtoConcessorio').prop('disabled', false);
                }

                if (enquadramento1 === 80380 || enquadramento2 === 80380 || enquadramento3 === 80380 || enquadramento4 === 80380) {
                    $('#MainContent_btnLPCO').removeClass('disabled');
                    $('#MainContent_btnLPCO').prop('disabled', false);
                }
            }
        }

        $('#btnLimparLPCO').click(function () {

            event.preventDefault();

            $('#MainContent_txtLPCOId').val('');
            $('#MainContent_txtNumeroLPCO').val('');

            $('#btnAdicionarLPCO').text('Adicionar');
        });

        $('#btnAdicionarLPCO').click(function () {

            event.preventDefault();

            var idLPCO = $("#<%=txtLPCOId.ClientID%>").val();
            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();
            var numeroLPCO = $("#<%=txtNumeroLPCO.ClientID%>").val();

            $('#InformacoesLPCO ul li').remove();
            $('#InformacoesLPCO').addClass('invisivel');

            var existemErros = false;

            if (numeroLPCO === '') {
                $('#InformacoesLPCO > ul')
                    .append('<li>O número da LPCO é obrigatório.</li>');

                existemErros = true;
            }

            if (existemErros) {
                $('#InformacoesAtoConcessorio').removeClass('invisivel');
                existemErros = false;
                return false;
            }

            if (idLPCO === undefined)
                idLPCO = '';

            var dadosForm = '{ detalheItemId: "' + detalheItemId + '", numeroLPCO:"' + numeroLPCO + '", id:"' + idLPCO + '"  }';

            $.ajax({
                type: "POST",
                url: "CadastrarItensDUE.aspx/AdicionarLPCO",
                data: dadosForm,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);

                    $("#tbLPCO").html(montarTabelaLPCO(dados));

                    $("#<%=txtLPCOId.ClientID%>").val('');
                    $("#<%=txtNumeroLPCO.ClientID%>").val('');

                    if (parseInt(idLPCO) > 0)
                        $('#btnAdicionarLPCO').text('Adicionar');

                },
                failure: function (response) {
                    alert(response.d);
                }
            });

        });

        function abrirModalLPCO() {

            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();

            var dadosForm = '{ detalheItemId: "' + detalheItemId + '"  }';

            $.ajax({
                type: "GET",
                url: "CadastrarItensDUE.aspx/ObterLPCOJson?detalheItemId=" + detalheItemId,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);
                    $("#tbLPCO").html(montarTabelaLPCO(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function montarTabelaLPCO(dados) {

            var tabela =
                '<table id="tbLPCO" class="table table-sm">' +
                '   <thead>' +
                '       <tr>' +
                '           <th>Número LPCO</th>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '           <th style="width:50px;">&nbsp;</td>' +
                '       </tr>' +
                '   </thead>' +
                '<tbody>';

            dados.forEach(function (linha) {
                tabela = tabela +
                    '   <tr>' +
                    '       <td>' + linha.Numero + '</td>' +
                    '       <td><a href="#" onclick="atualizarLPCO(' + linha.Id + ')"><img src="Content/imagens/editar.gif" /></a></td>' +
                    '       <td><a href="#" onclick="removerLPCO(' + linha.Id + ')"><img src="Content/imagens/excluir.png" /></a></td>' +
                    '   </tr>';
            });

            tabela = tabela + '</tbody>' +
                '</table>';

            return tabela;
        }

        function atualizarLPCO(id) {

            event.preventDefault();

            $.ajax({
                type: "GET",
                url: "CadastrarItensDUE.aspx/ObterLPCOPorIdJson?id=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    var dados = JSON.parse(response.d);

                    if (dados) {

                        $('#MainContent_txtLPCOId').val(dados.Id);
                        $('#MainContent_txtNumeroLPCO').val(dados.Numero);

                        $('#btnAdicionarLPCO').text('Atualizar');
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });

        }

        function removerLPCO(id) {

            event.preventDefault();

            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();

            $.ajax({
                type: "POST",
                url: "CadastrarItensDUE.aspx/RemoverLPCO",
                data: '{ id: "' + id + '", detalheItemId: "' + detalheItemId + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var dados = JSON.parse(response.d);
                    $("#tbLPCO").html(montarTabelaLPCO(dados));
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        var inicializar = function () {

            $('#MainContent_cbExportadorPais').select2();
            $('#MainContent_cbImportadorPais').select2();
            $('#MainContent_cbPaisDestino').select2();
            $('#MainContent_cbEnquadramento1').select2();
            $('#MainContent_cbEnquadramento2').select2();
            $('#MainContent_cbEnquadramento3').select2();
            $('#MainContent_cbEnquadramento4').select2();

            $('#MainContent_cbEnquadramento1, #MainContent_cbEnquadramento2, #MainContent_cbEnquadramento3, #MainContent_cbEnquadramento4').on('select2:select', function (e) {

                habilitarBotoesAtoConcessorioLPCO();
            });

            var detalheItemId = $("#<%=txtDUEDetalheItemID.ClientID%>").val();

            habilitarBotoesAtoConcessorioLPCO();

            $('#smartwizard').smartWizard({
                selected: 4,
                theme: 'dots',
                transitionEffect: 'fade',
                showStepURLhash: true,
                keyNavigation: false,
                showNextButton: false,
                showPreviousButton: false,
                lang: {
                    next: 'Próximo',
                    previous: 'Anterior'
                },
                toolbarSettings: {
                    toolbarPosition: 'bottom',
                    toolbarButtonPosition: 'right'
                },
                anchorSettings: {
                    anchorClickable: false
                },
            });

            $('#btnNext').hide();
            $('#btnPrevious').hide();

            $("#MainContent_btnCadastrarMaster").click(function () {

                $('#ItemDUEMsg ul li').remove().hide();

                var existemErros = false;

                var exportador = $("#MainContent_txtItemExportador").val();
                var exportadorDocumento = $("#MainContent_txtItemDocumentoExportador").val();
                var exportadorEndereco = $("#MainContent_txtItemExportadorEndereco").val();
                var exportadorUF = $("#MainContent_txtItemExportadorUF").val();
                var exportadorPais = $("#MainContent_cbExportadorPais option:selected").text();

                var importador = $("#MainContent_txtImportadorNome").val();
                var importadorEndereco = $("#MainContent_txtImportadorEndereco").val();
                var importadorPais = $("#MainContent_cbImportadorPais option:selected").text();

                var condicaoVenda = $("#MainContent_cbCondicaoVenda option:selected").val();
                var motivoDispensaNF = $("#MainContent_cbMotivoDispensaNotaFiscal option:selected").val();

                var exportadorDocumentoClear = exportadorDocumento.replace(/[^0-9]/g, '');

                var isEmbarqueNormal = parseInt($('#MainContent_txtSituacaoEspecial').val()) === 0;

                if (!isEmbarqueNormal) {

                    if (exportadorDocumentoClear.length !== 14) {
                        $('#ItemDUEMsg > ul')
                            .append('<li>O CNPJ do Exportador é inválido. Verifique se o valor digitado corresponde a um número de CNPJ.</li>');

                        existemErros = true;
                    }

                    if (exportador === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o Nome do Exportador</li>');

                        existemErros = true;
                    }

                    if (exportadorEndereco === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o Endereço do Exportador</li>');

                        existemErros = true;
                    }

                    if (exportadorUF === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o Estado (UF) do Exportador</li>');

                        existemErros = true;
                    }

                    if (exportadorPais === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o País do Exportador</li>');

                        existemErros = true;
                    }

                    if (importador === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o Nome do Importador</li>');

                        existemErros = true;
                    }

                    if (importadorEndereco === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o Endereço do Importador</li>');

                        existemErros = true;
                    }

                    if (importadorPais === '') {
                        $('#ItemDUEMsg > ul')
                            .append('<li>Informe o País do Importador</li>');

                        existemErros = true;
                    }
                }

                if (condicaoVenda === '') {
                    $('#ItemDUEMsg > ul')
                        .append('<li>Selecione a Condição de Venda</li>');

                    existemErros = true;
                }

                if (motivoDispensaNF === '') {
                    $('#ItemDUEMsg > ul')
                        .append('<li>Selecione o Motivo de Dispensa da NF</li>');

                    existemErros = true;
                }

                if (existemErros) {
                    event.preventDefault();
                    $('#ItemDUEMsg').show();
                    existemErros = false;
                    return false;
                }
            });
        }

        Sys.Application.add_load(inicializar);

    </script>

</asp:Content>
