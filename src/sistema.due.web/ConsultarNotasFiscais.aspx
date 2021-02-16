<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultarNotasFiscais.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarNotasFiscais" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <br />

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Consulta de Notas Fiscais de Exportação</h3>
                </div>
                <div class="panel-body">

                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="form-inline">
                                <div class="form-group">
                                    <label for="exampleInputName2">Data:&nbsp;</label>
                                    <asp:TextBox ID="txtData" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>&nbsp;&nbsp;
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail2">Número NF:&nbsp;</label>
                                    <asp:TextBox ID="txtNumeroNF" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>&nbsp;&nbsp;
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputEmail2">Chave:&nbsp;</label>
                                    <asp:TextBox ID="txtChave" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnFiltrar" runat="server" CssClass="btn btn-primary" Text="  Filtrar  " OnClick="btnFiltrar_Click" />
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
                                    ShowHeaderWhenEmpty="True">
                                    <Columns>
                                        <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF" />
                                        <asp:BoundField DataField="NumeroNF" HeaderText="Número" />
                                        <asp:BoundField DataField="CNPJNF" HeaderText="CNPJ" />
                                        <asp:BoundField DataField="QuantidadeNF" HeaderText="Quantidade" />
                                        <asp:BoundField DataField="UnidadeNF" HeaderText="Unidade" />
                                        <asp:BoundField DataField="NCM" HeaderText="NCM" />
                                        <asp:BoundField DataField="DUE" HeaderText="DUE" />
                                        <asp:BoundField DataField="Login" HeaderText="Usuário" />
                                        <asp:BoundField DataField="DataNF" HeaderText="Data Cadastro" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <a href="#" onclick="carregarNotasReferenciadas('<%# Eval("ChaveNF") %>')" class="btn btn-info btn-sm" data-toggle="modal" data-target="#modalNotasRef"><i class="fa fa-edit"></i>&nbsp;Notas Referenciadas</a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <div class="modal fade" id="modalNotasRef" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Notas Referenciadas</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <iframe id="frameNotasRef" width="100%" height="350" frameborder="0"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">

    <script>
        function carregarNotasReferenciadas(chaveNf) {
            $('#frameNotasRef').attr('src', 'ConsultarNotasReferenciadas.aspx?chaveNF=' + chaveNf);
        }
    </script>

</asp:Content>
