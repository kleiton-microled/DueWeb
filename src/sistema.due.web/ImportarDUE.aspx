<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportarDUE.aspx.cs" Inherits="Sistema.DUE.Web.ImportarDUE" %>

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

                    <div class="row">
                        <div class="col-md-12">

                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Importar Arquivo de DUE'S</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-10">
                                                <label for="txtValorTotalNota" class="control-label">Arquivo:</label>
                                                <asp:FileUpload ID="txtUpload" runat="server" CssClass="btn btn-default" />
                                                <p class="help-block">
                                                    Clique no botão "Escolher arquivo" e selecione o arquivo .csv ou .txt de DUEs
                                                </p>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="pull-right">                                                    
                                                    <a href="Consulta_DUE.pdf" target="_blank"><img src="Content/imagens/btnInstrucoes.png" /></a>
                                                </div>
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

                    <br />

                    <div class="row">
                        <div class="col-md-12 pull-right">
                            <asp:ImageButton ID="btnGerarExcel" Visible="false" runat="server" OnClick="btnGerarExcel_Click" ImageUrl="~/Content/imagens/btnExcel.png" />
                        </div>
                    </div>

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
                                        <asp:BoundField DataField="DUE" HeaderText="DUE" />
                                        <asp:BoundField HeaderText="Data Situação Siscomex" />
                                        <asp:BoundField HeaderText="Situação Siscomex" />
                                        <asp:BoundField HeaderText="Status Siscomex" />
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
        </Triggers>

    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script>

        function MostrarProgresso() {
            document.getElementById('MainContent_UpdateProgress1').style.display = "inline";
        }

    </script>
</asp:Content>
