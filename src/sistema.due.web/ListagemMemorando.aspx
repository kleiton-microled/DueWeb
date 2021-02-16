<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListagemMemorando.aspx.cs" Inherits="Sistema.DUE.Web.ListagemMemorando" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div class="row">
        <div class="col-md-10 col-md-offset-1">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Listagem de Memorando</h3>
                </div>
                <div class="panel-body">

                    <div class="row no-gutter">
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">De:</label>
                                <asp:TextBox ID="txtDe" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Até:</label>
                                <asp:TextBox ID="txtAte" runat="server" CssClass="form-control form-control-sm data"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label class="control-label">Empresa:</label>
                                <asp:DropDownList ID="cbEmpresa" runat="server" CssClass="form-control" Font-Size="11px">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Filial:</label>
                                <asp:TextBox ID="txtFilial" runat="server" CssClass="form-control form-control-sm" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Memorando:</label>
                                <asp:TextBox ID="txtMemorando" runat="server" CssClass="form-control form-control-sm" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">CNPJ Emitente NF:</label>
                                <asp:TextBox ID="txtCNPJ" runat="server" CssClass="form-control form-control-sm" MaxLength="18"></asp:TextBox>
                            </div>
                        </div>
                    </div>


                    <div class="row no-gutter">
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">DUE:</label>
                                <asp:TextBox ID="txtDUE" runat="server" CssClass="form-control form-control-sm" MaxLength="14"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Chave DUE:</label>
                                <asp:TextBox ID="txtChaveDUE" runat="server" CssClass="form-control form-control-sm" MaxLength="14"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">Número NF:</label>
                                <asp:TextBox ID="txtNumeroNF" runat="server" CssClass="form-control form-control-sm" TextMode="Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="control-label">Chave NF:</label>
                                &nbsp; <small>permite pesquisa por parte da chave</small>
                                <asp:TextBox ID="txtChaveNF" runat="server" CssClass="form-control form-control-sm" MaxLength="44"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <div class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary btn-block" Text="  Filtrar  " OnClick="btnFiltrar_Click" />
                            </div>
                        </div>
                    </div>

                    <br />
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView
                                    ID="gvNotasFiscais"
                                    runat="server"
                                    Width="100%"
                                    CssClass="table table-striped"
                                    GridLines="None"
                                    AutoGenerateColumns="False"
                                    Font-Size="13px"
                                    ShowHeaderWhenEmpty="True" AllowPaging="True" PageSize="12" OnPageIndexChanging="gvNotasFiscais_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                                        <asp:BoundField DataField="Filial" HeaderText="Filial" />
                                        <asp:BoundField DataField="Memorando" HeaderText="Memorando" />
                                        <asp:BoundField DataField="DUE" HeaderText="DUE" />
                                        <asp:BoundField DataField="ChaveAcesso" HeaderText="Chave DUE" />
                                        <asp:BoundField DataField="NumeroNF" HeaderText="Número NF" />
                                        <asp:BoundField DataField="DataEmissao" HeaderText="Data Emissão" />
                                        <asp:BoundField DataField="QuantidadeNF" HeaderText="Quantidade NF" />
                                        <asp:BoundField DataField="CNPJNF" HeaderText="CNPJ NF" />
                                        <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF" />
                                    </Columns>
                                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="row no-gutter">
                        <div class="col-md-2">
                            <asp:Button ID="btnGerarCsv" Text="Gerar .CSV" CssClass="btn btn-info btn-block" runat="server" Visible="false" OnClick="btnGerarCsv_Click" />
                        </div>
                    </div>


                </div>
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script src="Content/js/jquery.mask.min.js"></script>

    <script>

        $('#MainContent_txtDe').mask('00/00/0000');
        $('#MainContent_txtAte').mask('00/00/0000');

    </script>

</asp:Content>
