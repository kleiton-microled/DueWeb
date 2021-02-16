<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HabilitarUsuarios.aspx.cs" Inherits="Sistema.DUE.Web.HabilitarUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
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

    <div class="row no-gutter">
        <div class="col-sm-8 col-sm-offset-2">

            <div class="row">
                <div class="col-md-12">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Habilitar Usuários do Sistema</h3>
                                </div>
                                <div class="panel-body">

                                    <% if (ViewState["Sucesso"] != null)
                                        {
                                            if ((bool)ViewState["Sucesso"] == true)
                                            {%>
                                    <div class="alert alert-success">
                                        <strong>Sucesso!</strong> usuário(s) atualizado(s).                                        
                                    </div>
                                    <% }%>
                                    <% }%>

                                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                                    <div class="row">
                                        <div class="col-md-2 col-md-offset-10">
                                            <asp:Button ID="btnHabilitarUsuarios" Text="Salvar Mudanças" CssClass="btn btn-warning btn-block" runat="server" OnClick="btnHabilitarUsuarios_Click" />
                                        </div>
                                    </div>

                                    <br />

                                    <div class="row">
                                        <div class="form-group">
                                            <div class="col-md-12">
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
                                                            <asp:BoundField DataField="Login" HeaderText="Login" />
                                                            <asp:BoundField DataField="Nome" HeaderText="Nome" />
                                                            <asp:BoundField DataField="Cpf" HeaderText="CPF" />
                                                            <asp:BoundField DataField="Email" HeaderText="Email" />
                                                            <asp:BoundField DataField="DataCadastro" HeaderText="Data Cadastro" />
                                                            <asp:TemplateField HeaderText="Admin?">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckAdmin" runat="server" Checked='<%# Bind("FlagAdmin") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ativo?">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CheckAtivo" runat="server" Checked='<%# Bind("FlagAtivo") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
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
</asp:Content>
