<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastrarDUE.aspx.cs" Inherits="Sistema.DUE.Web.CadastrarDUE" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/smart_wizard.css" rel="stylesheet" />
    <link href="Content/css/smart_wizard_theme_dots.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <asp:UpdateProgress ID="UpdateProgress1" runat="server"
        AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="10">
        <ProgressTemplate>
            <div class="updateModal">
                <div align="center" class="updateModalBox">
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row">
        <div class="col-sm-8 col-sm-offset-2">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Cadastrar DUE</h3>
                </div>
                <div class="panel-body">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="txtDueID" runat="server" />
                            <asp:HiddenField ID="txtDueCompleta" runat="server" />
                            <asp:HiddenField ID="txtDueSiscomex" runat="server" />
                            <asp:HiddenField ID="txtDueEnviadoSiscomex" runat="server" />

                            <div class="row no-gutter">
                                <div class="col-sm-12">
                                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />
                                </div>
                            </div>

                            <div class="row no-gutter">
                                <div class="col-sm-12">

                                    <div id="smartwizard">
                                        <ul>
                                            <li><a href="#step-1">Passo 1<br />
                                                <small>Importação</small></a></li>
                                            <li><a href="#step-2">Passo 2<br />
                                                <small>Informações Iniciais</small></a></li>
                                            <li><a href="#step-3">Passo 3<br />
                                                <small>Local de Despacho</small></a></li>
                                            <li><a href="#step-4">Passo 4<br />
                                                <small>Local de Embarque</small></a></li>
                                            <li><a href="#step-5">Passo 5<br />
                                                <small>Observações</small></a></li>
                                            <li><a href="#step-6">Passo 6<br />
                                                <small>Lançamento dos Itens</small></a></li>
                                        </ul>

                                        <div>

                                            <div id="step-1" class="">

                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Cadastrar DUE</h3>
                                                <hr />

                                                <div class="row no-gutter">
                                                    <div class="col-sm-12">
                                                        <div class="alert alert-info">
                                                            <strong>Atenção: </strong>Esta opção é somente para cadastro de uma nova DUE. Caso queira começar o cadastro a partir dos dados de uma DUE existente, utilize os botões abaixo:
                                                            <br />
                                                            <br />
                                                            Para edição de uma DUE existente, utilize a opção Consultar DUEs Cadastradas ou <strong><a href="<%= ResolveUrl("ConsultarDUE.aspx") %>">clique aqui</a></strong>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row no-gutter">
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label class="control-label">Número da DUE:</label>
                                                            <asp:TextBox ID="txtDUEImportacao" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <div class="form-group">
                                                            <label class="control-label">&nbsp;</label>
                                                            <asp:Button ID="btnImportarSiscomex" runat="server" CssClass="btn btn-warning btn-block" Text="Importar do Siscomex" OnClick="btnImportarSiscomex_Click" />
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label class="control-label">&nbsp;</label>
                                                            <asp:Button ID="btnImportarBancoDeDados" runat="server" CssClass="btn btn-warning btn-block" Text="Importar do Banco de Dados" OnClick="btnImportarBancoDeDados_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div id="step-2" class="">

                                                <div id="InformacoesIniciaisMsg" class="alert alert-danger invisivel">
                                                    <ul>
                                                    </ul>
                                                </div>

                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Cadastro das Informações Iniciais</h3>
                                                <hr />
                                                <div class="row no-gutter">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label class="control-label">CPF/CNPJ Declarante:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:TextBox ID="txtDocumentoDeclarante" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label class="control-label">Moeda Negociação:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbMoedaNegociacao" runat="server" CssClass="form-control" DataValueField="CodigoISO" DataTextField="Descricao" Font-Size="11px">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <div class="form-group">
                                                            <label class="control-label">Referência única de carga (RUC):</label>
                                                            <asp:TextBox ID="txtRUC" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row no-gutter">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label class="control-label">Situação Especial:</label>
                                                            <asp:DropDownList ID="cbSituacaoEspecial" runat="server" CssClass="form-control" Font-Size="11px">
                                                                <asp:ListItem Text="EMBARQUE NORMAL" Value="0" />
                                                                <asp:ListItem Text="DU-E A POSTERIORI" Value="2001" />
                                                                <asp:ListItem Text="EMBARQUE ANTECIPADO" Value="2002" Selected="True" />
                                                                <asp:ListItem Text="EXPORTAÇÃO SEM SAÌDA DA MERCADORIA DO PAÍS" Value="2003" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label class="control-label">Formas de Exportação:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbFormaExportacao" runat="server" CssClass="form-control" Font-Size="11px">
                                                                <asp:ListItem Text="POR CONTA PRÓPRIA" Value="1001" Selected="True" />
                                                                <asp:ListItem Text="POR CONTA E ORDEM DE TERCEIROS" Value="1002" />
                                                                <asp:ListItem Text="POR OPERADOR DE REMESSA POSTAL OU EXPRESSA" Value="1003" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-5">
                                                        <div class="form-group">
                                                            <label class="control-label">Via especial de Transporte:</label>
                                                            <asp:DropDownList ID="cbViaEspecialTransporte" runat="server" CssClass="form-control" Font-Size="11px">
                                                                <asp:ListItem Text="" Value="0" />
                                                                <asp:ListItem Text="MEIOS PRÓPRIOS / POR REBOQUE" Value="4001" />
                                                                <asp:ListItem Text="DUTOS" Value="4002" />
                                                                <asp:ListItem Text="LINHAS DE TRANSMISSÃO" Value="4003" />
                                                                <asp:ListItem Text="EM MÃOS" Value="4004" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="step-3" class="">

                                                <div id="InformacoesLocalDespachoMsg" class="alert alert-danger invisivel">
                                                    <ul></ul>
                                                </div>

                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Informações do Local de Despacho</h3>
                                                <hr />
                                                <div class="row no-gutter">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label class="control-label">Unidade RFB:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbUnidadeDespachoRFB" runat="server" CssClass="form-control" DataValueField="Codigo" DataTextField="Descricao" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="cbUnidadeDespachoRFB_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label class="control-label">Tipo recinto aduaneiro:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbTipoRecintoAduaneiroDespacho" runat="server" CssClass="form-control" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="cbTipoRecintoAduaneiroDespacho_SelectedIndexChanged">
                                                                <asp:ListItem Text="RECINTO ALFANDEGADO" Value="281" Selected="True" />
                                                                <asp:ListItem Text="FORA DO RECINTO ALFANDEGADO (DOMICILIAR)" Value="19" />
                                                                <asp:ListItem Text="FORA DO RECINTO ALFANDEGADO (NÃO DOMICILIAR)" Value="22" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-5">
                                                        <div class="form-group">
                                                            <label class="control-label">Recinto Aduaneiro:</label>
                                                            <span id="spnRecintoAduaneiroDespObrigatorio" runat="server" class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbRecintoAduaneiroDespacho" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="Descricao" Font-Size="11px">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:Panel runat="server" ID="pnlNaoRecintoAduaneiroDespacho" Visible="false">
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-3">
                                                            <div class="form-group">
                                                                <label class="control-label">CNPJ/CPF do responsável:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:TextBox ID="txtDocumentoResponsavelLocalDespacho" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">Latitude:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:TextBox ID="txtLatitudeLocalDespacho" runat="server" CssClass="form-control inteiro"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <div class="form-group">
                                                                <label class="control-label">Longitude:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:TextBox ID="txtLongitudeLocalDespacho" runat="server" CssClass="form-control inteiro"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-sm-5">
                                                            <div class="form-group">
                                                                <label class="control-label">Endereço:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:TextBox ID="txtEnderecoLocalDespacho" runat="server" CssClass="form-control moeda"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div id="step-4" class="">

                                                <div id="InformacoesLocalEmbarqueMsg" class="alert alert-danger invisivel">
                                                </div>

                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Informações do Local de Embarque</h3>
                                                <hr />
                                                <div class="row no-gutter">
                                                    <div class="col-sm-3">
                                                        <div class="form-group">
                                                            <label class="control-label">Unidade RFB:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbUnidadeEmbarqueRFB" runat="server" CssClass="form-control" DataValueField="Codigo" DataTextField="Descricao" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="cbUnidadeEmbarqueRFB_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-4">
                                                        <div class="form-group">
                                                            <label class="control-label">Tipo recinto aduaneiro:</label>
                                                            <span class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbTipoRecintoAduaneiroEmbarque" runat="server" CssClass="form-control" Font-Size="11px" AutoPostBack="true" OnSelectedIndexChanged="cbTipoRecintoAduaneiroEmbarque_SelectedIndexChanged">
                                                                <asp:ListItem Text="RECINTO ALFANDEGADO" Value="281" Selected="True" />
                                                                <asp:ListItem Text="FORA DO RECINTO ALFANDEGADO (DOMICILIAR)" Value="19" />
                                                                <asp:ListItem Text="FORA DO RECINTO ALFANDEGADO (NÃO DOMICILIAR)" Value="22" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-5">
                                                        <div class="form-group">
                                                            <label class="control-label">Recinto Aduaneiro:</label>
                                                            <span id="spnRecintoAduaneiroEmbObrigatorio" runat="server" class="text-danger">*</span>
                                                            <asp:DropDownList ID="cbRecintoAduaneiroEmbarque" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="Descricao" Font-Size="11px">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <asp:Panel runat="server" ID="pnlNaoRecintoAduaneiroEmbarque" Visible="false">
                                                    <div class="row no-gutter">
                                                        <div class="col-sm-12">
                                                            <div class="form-group">
                                                                <label class="control-label">Referência de Endereço:</label>
                                                                <span class="text-danger">*</span>
                                                                <asp:TextBox ID="txtReferenciaEndereco" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            <div id="step-5" class="">
                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Informações Complementares</h3>
                                                <hr />
                                                <div class="row no-gutter">
                                                    <div class="col-sm-12">
                                                        <div class="form-group">
                                                            <label class="control-label">Observações:</label>
                                                            <asp:TextBox ID="txtInformacoesComplementares" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>
                                            <div id="step-6" class="">
                                                <h3 class="border-bottom border-gray pb-2"><i class="fa fa-edit"></i>&nbsp;Informações Padrão</h3>
                                                <hr />

                                                <div id="InformacoesPadraoMsg" class="alert alert-danger invisivel">
                                                </div>

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
                                                            <asp:TextBox ID="txtLPCO_Default" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
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

                                                            <label>Tipo do AC</label>
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

                                                    <div class="col-sm-12">
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

                                                <div class="row no-gutter">
                                                    <div class="col-sm-3 pull-right">
                                                        <div class="form-group">
                                                            <asp:Button ID="btnCadastrarDUE" runat="server" CssClass="btn btn-primary btn-block" Text="Gravar DUE e Continuar" OnClick="btnCadastrarDUE_Click" OnClientClick="javascript: return ValidarDUE();" />
                                                        </div>
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




</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/jquery.smartWizard.js"></script>
    <script src="Content/js/select2.min.js"></script>

    <script>

        function IrParaItens() {

            if (parseInt($('#MainContent_txtDueID').val()) > 0) {
                document.location.href = 'CadastrarItensDUE.aspx?id=' + $('#MainContent_txtDueID').val() + '#step-5';
            }
        }

        function IrParaNotas() {

            if (parseInt($('#MainContent_txtDueID').val()) > 0) {
                document.location.href = 'CadastrarNotas.aspx?id=' + $('#MainContent_txtDueID').val() + '#step-5';
            }
        }

        var inicializar = function () {

            $(document).ready(function () {
                $('#MainContent_cbEnquadramento1_Default, #MainContent_cbEnquadramento2_Default, #MainContent_cbEnquadramento3_Default, #MainContent_cbEnquadramento4_Default').change(function () {

                    habilitarCamposAtoConcessorioLPCO();
                });
            });

            $('#MainContent_cbUnidadeDespachoRFB').select2();
            $('#MainContent_cbRecintoAduaneiroDespacho').select2();

            $('#MainContent_cbUnidadeEmbarqueRFB').select2();
            $('#MainContent_cbRecintoAduaneiroEmbarque').select2();

            var isNumero = function (numero) {
                return (numero !== null && !isNaN(parseFloat(numero)));
            }

            $('#smartwizard').smartWizard({
                selected: 0,
                theme: 'dots',
                transitionEffect: 'fade',
                showStepURLhash: true,
                keyNavigation: false,
                lang: {
                    next: 'Próximo',
                    previous: 'Anterior'
                },
                toolbarSettings: {
                    toolbarPosition: 'bottom',
                    toolbarButtonPosition: 'right'
                }
            });

            if (parseInt($('#MainContent_txtDueID').val()) > 0) {
                $('.btn-toolbar').append("<button id='btnIrParaItens' class='btn btn-warning pull-right' onclick='IrParaItens()'>Ir para Itens</button>")
            }

            if (parseInt($('#MainContent_txtDueID').val()) > 0) {
                if (parseInt($('#MainContent_txtDueCompleta').val()) >= 0) {
                    $('.btn-toolbar').append("<button id='btnIrParaNotas' class='btn btn-warning pull-right' onclick='IrParaNotas()'>Ir para Notas</button>")
                }
            }

            $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

                if (stepNumber === 1) {

                    $('#InformacoesIniciaisMsg ul li').remove().hide();

                    var existemErros = false;

                    var documentoDeclarante = $("#MainContent_txtDocumentoDeclarante").val();

                    var documentoDeclaranteClear = documentoDeclarante.replace(/[^0-9]/g, '');

                    if (documentoDeclaranteClear.length !== 11 && documentoDeclaranteClear.length !== 14) {
                        $('#InformacoesIniciaisMsg > ul')
                            .append('<li>O Documento do Declarante é inválido. Verifique se o valor digitado corresponde a um número de CPF ou CNPJ.</li>');

                        existemErros = true;
                    }

                    var numeroRUC = $("#MainContent_txtRUC").val();

                    if (numeroRUC.length > 0 && numeroRUC.length < 16) {

                        $('#InformacoesIniciaisMsg > ul')
                            .append('<li>O número da RUC informado é inválido. Verifique se o formato da RUC atende a uma recomendação da Organização Mundial de Aduanas (OMA).</li>');

                        existemErros = true;
                    }

                    var moedaNegociacao = $('#MainContent_cbMoedaNegociacao option:selected').val();

                    if (moedaNegociacao === '0') {

                        $('#InformacoesIniciaisMsg > ul')
                            .append('<li>Selecione a Moeda de Negociação.</li>');

                        existemErros = true;
                    }

                    if (existemErros) {
                        $('#InformacoesIniciaisMsg').show();
                        existemErros = false;
                        return false;
                    }
                }

                if (stepNumber === 2) {

                    $('#InformacoesLocalDespachoMsg ul li').remove().hide();
                    var existemErros = false;

                    var unidadeRFBDespacho = $('#MainContent_cbUnidadeDespachoRFB option:selected').val();
                    if (unidadeRFBDespacho === '0') {
                        $('#InformacoesLocalDespachoMsg > ul')
                            .append('<li>Selecione a Unidade da Receita Federal de Despacho.</li>');

                        existemErros = true;
                    }

                    var tipoRecintoDespacho = $('#MainContent_cbTipoRecintoAduaneiroDespacho option:selected').val();

                    if (tipoRecintoDespacho !== '281') {

                        var documentoResponsavelLocalDespacho = $('#MainContent_txtDocumentoResponsavelLocalDespacho').val();
                        var latitudeLocalDespacho = $('#MainContent_txtLatitudeLocalDespacho').val();
                        var longitudeLocalDespacho = $('#MainContent_txtLongitudeLocalDespacho').val();
                        var enderecoLocalDespacho = $('#MainContent_txtEnderecoLocalDespacho').val();

                        var documentoResponsavelLocalDespachoClear = documentoResponsavelLocalDespacho.replace(/[^0-9]/g, '');

                        if (documentoResponsavelLocalDespachoClear.length !== 11 && documentoResponsavelLocalDespachoClear.length !== 14) {
                            $('#InformacoesLocalDespachoMsg > ul')
                                .append('<li>O Documento do responsável pelo local de despacho é inválido. Verifique se o valor digitado corresponde a um número de CPF ou CNPJ</li>');

                            existemErros = true;
                        }

                        if (!isNumero(latitudeLocalDespacho)) {
                            $('#InformacoesLocalDespachoMsg > ul')
                                .append('<li>A latitude do local de despacho é inválida. Cerifique-se que o valor digitado foi informado corretamente. Exemplo: -23.000001</li>');

                            existemErros = true;
                        }

                        if (!isNumero(longitudeLocalDespacho)) {
                            $('#InformacoesLocalDespachoMsg > ul')
                                .append('<li>A longitude do local de despacho é inválida. Cerifique-se que o valor digitado foi informado corretamente. Exemplo: -23.000001</li>');

                            existemErros = true;
                        }

                        if (enderecoLocalDespacho === '') {
                            $('#InformacoesLocalDespachoMsg > ul')
                                .append('<li>Informe o endereço do local de despacho</li>');

                            existemErros = true;
                        }
                    }

                    if (existemErros) {
                        $('#InformacoesLocalDespachoMsg').show();
                        existemErros = false;
                        return false;
                    }
                }

                if (stepNumber === 3) {

                    $('#InformacoesLocalEmbarqueMsg').empty().hide();

                    var unidadeRFBEmbarque = $("#MainContent_cbUnidadeEmbarqueRFB option:selected").val();
                    if (unidadeRFBEmbarque === '0') {
                        $('#InformacoesLocalEmbarqueMsg')
                            .show()
                            .text('Selecione a Unidade da Receita Federal de Embarque.');

                        return false;
                    }

                    //var enderecoReferenciaLocalEmbarque = $('#MainContent_txtReferenciaEndereco').val();

                    //if (enderecoReferenciaLocalEmbarque === '') {
                    //    $('#InformacoesLocalEmbarqueMsg')
                    //        .show()
                    //        .text('Informe o endereço de referência do local de embarque.');

                    //    return false;
                    //}
                }

                if (stepNumber === 4) {
                    $("#btnNext").attr('disabled', 'disabled');
                } else {
                    $("#btnNext").removeAttr('disabled');
                }

                if (stepNumber === 4) {

                    //  $('#btnIrParaItens').addClass('invisivel');
                }
            });

            irParaEtapaInicial = function () {
                location.hash = "#step-2";
            }

            $('#MainContent_txtLPCO_Default').prop('readonly', true);
            $('#MainContent_txtNumeroAC_Default').prop('readonly', true);
            $('#MainContent_txtCNPJBeneficiarioAC_Default').prop('readonly', true);
            $('#MainContent_txtNumeroItemAC_Default').prop('readonly', true);
            $('#MainContent_txtNCMItemAC_Default').prop('readonly', true);
            $('#MainContent_txtQuantidadeUtilizadaAC_Default').prop('readonly', true);
            $('#MainContent_txtVMLEComCoberturaCambialAC_Default').prop('readonly', true);
            $('#MainContent_txtVMLESemCoberturaCambialAC_Default').prop('readonly', true);

            habilitarCamposAtoConcessorioLPCO();
        }

        function ValidarDUE() {


            $('#InformacoesPadraoMsg').empty().hide();

            var condicaoVendaPadrao = $('#MainContent_cbCondicaoVenda_Default').val();

            if (condicaoVendaPadrao === '') {

                $('#InformacoesPadraoMsg')
                    .show()
                    .text('O campo Condição de Venda é obrigatório');

                return false;
            }

            return true;
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

    </script>

</asp:Content>
