<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaHistoricoAverbadas.aspx.cs" Inherits="Sistema.DUE.Web.ConsultaHistoricoAverbadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/select2.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary" style="position: relative;">
                
                <div style="width: 100%;  position: absolute; z-index: 2; text-align: right;">
                    <img src="Content/imagens/beta-tag.png" />
                </div>


                <div class="panel-heading">
                    <h3 class="panel-title">Histórico de Notas Averbadas</h3>
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
                                <asp:TextBox ID="txtCNPJResponsavel" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                                        <asp:BoundField DataField="DataAverbacao" HeaderText="Data Averbação" />
                                        <asp:BoundField DataField="Sobra" HeaderText="Quebra/Sobra" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <tr>
                                                    <td colspan="13">

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
                            <asp:Button ID="btnGerarCsv" Text="Gerar .CSV" CssClass="btn btn-info btn-block" runat="server" OnClick="btnGerarCsv_Click" />
                        </div>
                    </div>

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

</asp:Content>
