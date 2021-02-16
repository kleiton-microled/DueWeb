<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compartilhar.aspx.cs" Inherits="Sistema.DUE.Web.Compartilhar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <div class="row no-gutter">
        <div class="col-sm-8 col-sm-offset-2">

            <div class="row">
                <div class="col-md-12">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Compartilhar Dados</h3>
                                </div>
                                <div class="panel-body">

                                    <% if (ViewState["Sucesso"] != null)
                                        {
                                            if ((bool)ViewState["Sucesso"] == true)
                                            {%>
                                    <div class="alert alert-success">
                                        <strong>Sucesso!</strong> dados compartilhados!
                                    </div>
                                    <% }%>
                                    <% }%>

                                    <div class="alert alert-info">
                                        Você pode compartilhar com outros usuários os dados de DUE que digitou ou importou. Marque abaixo todos os usuários com que deseja compartilhar os dados.
                                    </div>

                                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />
                                    <div class="row">
                                        <div class="col-sm-3 pull-right">
                                            <asp:Button ID="btnCompartilhar" Text="Compartilhar Dados" CssClass="btn btn-warning btn-block" runat="server" OnClick="btnCompartilhar_Click" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label class="control-label">Selecione os usuários para compartilhar dados:</label>
                                            </div>

                                            <div class="table-responsive">

                                                <asp:GridView
                                                    ID="gvUsuarios"
                                                    runat="server"
                                                    Width="100%"
                                                    CssClass="table table-striped table-bordered"
                                                    GridLines="None"
                                                    AutoGenerateColumns="False"
                                                    Font-Size="12px"
                                                    ShowHeaderWhenEmpty="True"
                                                    DataKeyNames="Id">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <input id="chkSelecionarTodosItens" type="checkbox" onclick="selecionarTodosItens(this)" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="UsuarioCheck" runat="server" CommandArgument="<%# Container.DataItemIndex %>" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="20px" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Login" HeaderText="Login" />
                                                        <asp:BoundField DataField="Nome" HeaderText="Nome" />
                                                        <asp:BoundField DataField="Email" HeaderText="Email" />
                                                    </Columns>
                                                </asp:GridView>

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

    <script src="Content/js/select2.min.js"></script>

    <script>

        $('#MainContent_cbDUEs').select2();

        function selecionarTodosItens(checkbox) {
            var grid = document.getElementById("<%=gvUsuarios.ClientID %>");
            for (i = 1; i < grid.rows.length; i++) {
                grid.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = checkbox.checked;
            }
        }

        function CheckOtherIsCheckedByGVID(rb) {
            var isChecked = rb.checked;
            var row = rb.parentNode.parentNode;

            var currentRdbID = rb.id;
            parent = document.getElementById("<%= gvUsuarios.ClientID %>");
            var items = parent.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].id != currentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }
    </script>
</asp:Content>
