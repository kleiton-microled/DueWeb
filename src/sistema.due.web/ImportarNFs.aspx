<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="ImportarNFs.aspx.cs" Inherits="Sistema.DUE.Web.ImportarNFs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <div class="row no-gutter">
                <div class="col-md-12">

                    <% if (ViewState["Sucesso"] != null)
                        {
                            if ((bool)ViewState["Sucesso"] == true && (int)ViewState["QuantidadeImportada"] > 0)
                            {%>
                    <div class="alert alert-success">
                        <strong>Sucesso!</strong>
                        <%=ViewState["QuantidadeImportada"] %> Notas Fiscais de um total de <%=ViewState["TotalNotasFiscais"] %> importadas com sucesso. O conteúdo pode ser visualizado na lista abaixo
                
            
                <asp:Panel ID="pnlDueCriada" runat="server" Visible="false">

                    <br />

                    <img src="Content/imagens/seta.png" />&nbsp;<span style="margin-top: 2px;">A DUE foi criada com sucesso! Clique <a href="CadastrarDUE.aspx?id=<%= ViewState["DueId"] %>#step-2">aqui</a> para completar as informações</span>
                </asp:Panel>
                    </div>

                    <% }
                        else
                        { %>
                    <div class="alert alert-info">
                        <strong><%=ViewState["QuantidadeImportada"] %></strong> notas fiscais foram importadas. O Conteúdo deste arquivo já foi importado.
                    </div>
                    <% } %>
                    <% }%>

                    <div class="row">
                        <div class="col-md-12">

                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Importar Arquivo de Notas Fiscais</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <label for="txtValorTotalNota" class="control-label">Arquivo:</label>
                                                <asp:FileUpload ID="txtUpload" runat="server" CssClass="btn btn-default" />
                                                <p class="help-block">
                                                    Clique no botão "Escolher arquivo" e selecione o arquivo .csv ou .txt de notas fiscais
                                                </p>
                                                <div class="checkbox">
                                                    <label>
                                                        <asp:CheckBox ID="chkCriarItensDUE" runat="server" />
                                                        <asp:Label Text="Desejo cadastrar automaticamente os itens da DUE" runat="server" />
                                                    </label>
                                                </div>

                                            </div>

                                            <div class="col-md-8">

                                                <asp:Panel ID="pnlInformacoesaDefault" runat="server" CssClass="invisivel">

                                                    <div class="alert alert-info">
                                                        Opcional. Se informados, os valores inseridos nos campos abaixo serão replicados para todos os itens da DUE.
                                                    </div>

                                                    <div class="row no-gutter">
                                                        <div class="col-sm-3">
                                                            <div class="form-group">
                                                                <label class="control-label">Valor Unit. VMLE (TON):</label>
                                                                <asp:TextBox ID="txtValorUnitVMLE_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <div class="form-group">
                                                                <label class="control-label">Valor Unit. VMCV (TON):</label>
                                                                <asp:TextBox ID="txtValorUnitVMCV_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">NCM:</label>
                                                                <asp:TextBox ID="txtNCM_Default" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True" OnTextChanged="txtNCM_Default_TextChanged"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <div class="form-group">
                                                                <label class="control-label">País Destino:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:DropDownList ID="cbPaisDestino_Default" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label">Enquadramento 1:</label>
                                                                <asp:DropDownList ID="cbEnquadramento1_Default" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label">Enquadramento 2:</label>
                                                                <asp:DropDownList ID="cbEnquadramento2_Default" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label">Enquadramento 3:</label>
                                                                <asp:DropDownList ID="cbEnquadramento3_Default" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label">Enquadramento 4:</label>
                                                                <asp:DropDownList ID="cbEnquadramento4_Default" runat="server" DataTextField="Descricao" DataValueField="Codigo" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-4">
                                                            <div class="form-group">
                                                                <label class="control-label">Condição Venda:</label>&nbsp;<small class="text-danger">(obrigatório)</small>
                                                                <asp:DropDownList ID="cbCondicaoVenda_Default" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                    <asp:ListItem Text="-- Selecione --" Value="" />
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
                                                                    <asp:ListItem Text="FOB - FREE ON BOARD" Value="FOB" />
                                                                    <asp:ListItem Text="OCV - OUTRA CONDICAO DE VENDA" Value="OCV" />
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">Prioridade:</label>
                                                                <asp:DropDownList ID="cbPrioridadeCarga_Default" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                    <asp:ListItem Text="" Value="0" />
                                                                    <asp:ListItem Text="CARGA VIVA" Value="5001" />
                                                                    <asp:ListItem Text="CARGA PERECÍVEL" Value="5002" />
                                                                    <asp:ListItem Text="CARGA PERIGOSA" Value="5003" />
                                                                    <asp:ListItem Text="PARTES / PEÇAS DE AERONAVE" Value="5006" />
                                                                    <%--5006 através do link portalunico.siscomex.gov.br/docs/visual-xml/--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-4">
                                                            <div class="form-group">
                                                                <label class="control-label">Descrição Complementar:</label>
                                                                <asp:TextBox ID="txtDescrComplementar_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">Comissão Agente:</label>
                                                                <asp:TextBox ID="txtComissaoAgenteDefault" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                                <label class="control-label">LPCO:</label>
                                                                <small>(Informe quantos LPCO's desejar, separando com vírgula)</small>
                                                                <asp:TextBox ID="txtLPCO_Default" runat="server" CssClass="form-control disabled" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <asp:Panel ID="pnlAtributosNCM" runat="server" Visible="false">

                                                        <div class="col-sm-12">
                                                            <br />
                                                            <label class="control-label">Atributos Café:</label>
                                                            <hr style="margin-top: 2px;" />
                                                        </div>

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

                                                        <div class="col-sm-12">
                                                            <br />
                                                            <label class="control-label">Ato Concessório:</label>
                                                            <hr style="margin-top: 2px;" />
                                                        </div>

                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                                <label class="control-label">Tipo do AC:</label>
                                                                <asp:DropDownList ID="cbTipoAC_Default" runat="server" CssClass="form-control form-control-sm" Font-Size="11px">
                                                                    <asp:ListItem Value="AC">COMUM</asp:ListItem>
                                                                    <asp:ListItem Value="DBSI">INTERMEDIÁRIO</asp:ListItem>
                                                                    <asp:ListItem Value="DSIG">INTERMEDIÁRIO GENÉRICO</asp:ListItem>
                                                                    <asp:ListItem Value="DSEC">EMBARCAÇÃO COMUM</asp:ListItem>
                                                                    <asp:ListItem Value="DSEG">EMBARCAÇÃO GENÉRICO</asp:ListItem>
                                                                    <asp:ListItem Value="DSG">GENÉRICO</asp:ListItem>
                                                                    <asp:ListItem Value="DSMC">FORNECIMENTO NO MERCADO INTERNO COMUM</asp:ListItem>
                                                                    <asp:ListItem Value="DSMG">FORNECIMENTO NO MERCADO INTERNO GENÉRICO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-12 invisivel">
                                                            <div class="form-group">
                                                                <label class="control-label">Exportador é o beneficiário do AC?</label>
                                                                <br />
                                                                <asp:RadioButton ID="rbAcBeneficiarioSim" runat="server" Text="   Sim" GroupName="rbAcBeneficiario" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                 <asp:RadioButton ID="rbAcBeneficiarioNao" runat="server" Text="   Não" GroupName="rbAcBeneficiario" Checked="true" />
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">Número AC:</label>
                                                                <asp:TextBox ID="txtNumeroAC_Default" runat="server" CssClass="form-control form-control-sm" MaxLength="11"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">CNPJ Beneficiário:</label>
                                                                <asp:TextBox ID="txtCNPJBeneficiarioAC_Default" runat="server" CssClass="form-control form-control-sm" MaxLength="14"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <div class="form-group">
                                                                <label class="control-label">Item:</label>
                                                                <asp:TextBox ID="txtNumeroItemAC_Default" runat="server" CssClass="form-control form-control-sm" MaxLength="3"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">NCM:</label>
                                                                <asp:TextBox ID="txtNCMItemAC_Default" runat="server" CssClass="form-control form-control-sm" MaxLength="8"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <div class="form-group">
                                                                <label class="control-label">Qtde:</label>
                                                                <asp:TextBox ID="txtQuantidadeUtilizadaAC_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">VMLE c/ cob. cambial:</label>
                                                                <asp:TextBox ID="txtVMLEComCoberturaCambialAC_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">VMLE s/ cob. cambial:</label>
                                                                <asp:TextBox ID="txtVMLESemCoberturaCambialAC_Default" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                    </div>

                                                </asp:Panel>

                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-6">
                                                <asp:Button ID="btnSair" runat="server" Text="Sair" CssClass="btn btn-default" OnClick="btnSair_Click" />
                                                <asp:Button ID="btnImportar" runat="server" Text="Importar" CssClass="btn btn-warning" OnClick="btnImportar_Click" OnClientClick="MostrarProgresso();" />
                                            </div>

                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>

                    <div id="pnlLegenda" class="row" runat="server" visible="false">
                        <div class="col-md-10">
                            <h5><strong>Legenda:</strong></h5>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="MistyRose">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                        &nbsp;[1] Notas não encontradas no CCT 
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;<asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="LightYellow">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                        &nbsp;[2] Notas com divergências no saldo
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;<asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="LightGreen">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                        &nbsp;[3] Notas sem divergências
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-2 pull-right">
                            <asp:ImageButton ID="btnGerarExcel" runat="server" OnClick="btnGerarExcel_Click" ImageUrl="~/Content/imagens/btnExcel.png" />
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
                                        <asp:BoundField DataField="TipoNF" HeaderText="Tipo" />
                                        <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF" />
                                        <asp:BoundField DataField="NumeroNF" HeaderText="Número" />
                                        <asp:BoundField DataField="CNPJNF" HeaderText="CNPJ" />
                                        <asp:BoundField DataField="QuantidadeNF" HeaderText="Quantidade" />
                                        <asp:BoundField HeaderText="Saldo CCT" />
                                        <asp:BoundField HeaderText="Saldo outras DUES" />
                                        <asp:BoundField DataField="UnidadeNF" HeaderText="Unidade" />
                                        <asp:BoundField DataField="NCM" HeaderText="NCM" />
                                        <asp:BoundField DataField="ChaveNFReferencia" HeaderText="Chave Referência" />
                                        <asp:BoundField DataField="Arquivo" HeaderText="Arquivo" />
                                        <asp:BoundField HeaderText="Cód. Situação" />
                                        <asp:BoundField DataField="Item" HeaderText="Item" />
                                        <asp:BoundField HeaderText="Recinto" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportar" runat="server" />
            <asp:PostBackTrigger ControlID="btnGerarExcel" runat="server" />
            <asp:PostBackTrigger ControlID="chkCriarItensDUE" runat="server" />
        </Triggers>



    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>
        var inicializar = function () {
            $(document).ready(function () {

                $('#MainContent_txtLPCO_Default').prop('readonly', true);
                $('#MainContent_txtNumeroAC_Default').prop('readonly', true);
                $('#MainContent_txtCNPJBeneficiarioAC_Default').prop('readonly', true);
                $('#MainContent_txtNumeroItemAC_Default').prop('readonly', true);
                $('#MainContent_txtNCMItemAC_Default').prop('readonly', true);
                $('#MainContent_txtQuantidadeUtilizadaAC_Default').prop('readonly', true);
                $('#MainContent_txtVMLEComCoberturaCambialAC_Default').prop('readonly', true);
                $('#MainContent_txtVMLESemCoberturaCambialAC_Default').prop('readonly', true);

                $('#MainContent_cbEnquadramento1_Default, #MainContent_cbEnquadramento2_Default, #MainContent_cbEnquadramento3_Default, #MainContent_cbEnquadramento4_Default').change(function () {
                    habilitarCamposAtoConcessorioLPCO();
                });

                $('#MainContent_chkCriarItensDUE').change(function () {
                    if ($(this).is(':checked')) {
                        $('#MainContent_pnlInformacoesaDefault').show();
                    } else {

                        $('#MainContent_txtValorUnitVMLE_Default').val('');
                        $('#MainContent_txtValorUnitVMCV_Default').val('');
                        $('#MainContent_cbPaisDestino_Default').val('');
                        $('#MainContent_cbEnquadramento1_Default').val('');
                        $('#MainContent_cbEnquadramento2_Default').val('');
                        $('#MainContent_cbEnquadramento3_Default').val('');
                        $('#MainContent_cbEnquadramento4_Default').val('');
                        $('#MainContent_cbCondicaoVenda_Default').val('');
                        $('#MainContent_txtLPCO_Default').val('');
                        $('#MainContent_cbPrioridadeCarga_Default').val('');
                        $('#MainContent_txtDescrComplementar_Default').val('');
                        $('#MainContent_txtComissaoAgenteDefault').val('');

                        $('#MainContent_pnlInformacoesaDefault').hide();
                    }
                });
            });
        }

        function habilitarCamposAtoConcessorioLPCO() {

            $('#MainContent_txtLPCO_Default').prop('readonly', true);
            $('#MainContent_txtNumeroAC_Default').prop('readonly', true);
            $('#MainContent_txtCNPJBeneficiarioAC_Default').prop('readonly', true);
            $('#MainContent_txtNumeroItemAC_Default').prop('readonly', true);
            $('#MainContent_txtNCMItemAC_Default').prop('readonly', true);
            $('#MainContent_txtQuantidadeUtilizadaAC_Default').prop('readonly', true);
            $('#MainContent_txtVMLEComCoberturaCambialAC_Default').prop('readonly', true);
            $('#MainContent_txtVMLESemCoberturaCambialAC_Default').prop('readonly', true);

            var enquadramento1 = parseInt($('#MainContent_cbEnquadramento1_Default').val());
            var enquadramento2 = parseInt($('#MainContent_cbEnquadramento2_Default').val());
            var enquadramento3 = parseInt($('#MainContent_cbEnquadramento3_Default').val());
            var enquadramento4 = parseInt($('#MainContent_cbEnquadramento4_Default').val());

            if (enquadramento1 === 81101 || enquadramento2 === 81101 || enquadramento3 === 81101 || enquadramento4 === 81101) {

                $('#MainContent_txtNumeroAC_Default').prop('readonly', false);
                $('#MainContent_txtCNPJBeneficiarioAC_Default').prop('readonly', false);
                $('#MainContent_txtNumeroItemAC_Default').prop('readonly', false);
                $('#MainContent_txtNCMItemAC_Default').prop('readonly', false);
                $('#MainContent_txtQuantidadeUtilizadaAC_Default').prop('readonly', false);
                $('#MainContent_txtVMLEComCoberturaCambialAC_Default').prop('readonly', false);
                $('#MainContent_txtVMLESemCoberturaCambialAC_Default').prop('readonly', false);
            } else {

                $('#MainContent_txtNumeroAC_Default').val('');
                $('#MainContent_txtCNPJBeneficiarioAC_Default').val('');
                $('#MainContent_txtNumeroItemAC_Default').val('');
                $('#MainContent_txtNCMItemAC_Default').val('');
                $('#MainContent_txtQuantidadeUtilizadaAC_Default').val('');
                $('#MainContent_txtVMLEComCoberturaCambialAC_Default').val('');
                $('#MainContent_txtVMLESemCoberturaCambialAC_Default').val('');
            }

            if (enquadramento1 === 80380 || enquadramento2 === 80380 || enquadramento3 === 80380 || enquadramento4 === 80380) {
                $('#MainContent_txtLPCO_Default').prop('readonly', false);
            } else {
                $('#MainContent_txtLPCO_Default').val('');
            }

        }

        Sys.Application.add_load(inicializar);

        function MostrarProgresso() {
            document.getElementById('MainContent_UpdateProgress1').style.display = "inline";
        }
    </script>
</asp:Content>
