<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultarNotasReferenciadas.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarNotasReferenciadas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:Label Text="Total Quantidade: " Font-Bold="true" runat="server" />
        <asp:Label ID="lblTotalQuantiade" Text="" runat="server" />

        <asp:GridView
            ID="gvNotasFiscaisReferenciadas"
            runat="server"
            Width="100%"
            GridLines="Vertical"
            AutoGenerateColumns="False"
            Font-Size="13px"
            DataKeyNames="Id"
            Font-Names="Arial" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" OnRowCommand="gvNotasFiscaisReferenciadas_RowCommand">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="Item" HeaderText="Item" />
                <asp:BoundField DataField="ChaveNF" HeaderText="Chave NF" />
                <asp:BoundField DataField="NumeroNF" HeaderText="Número NF">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CNPJNF" HeaderText="CNPJ">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="QuantidadeNF" HeaderText="Quantidade">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UnidadeNF" HeaderText="Unidade">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="NCM" HeaderText="NCM">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="cmdExcluirNF" ImageUrl="~/Content/imagens/excluir.png" CommandName="EXCLUIR_NF"
                            CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return confirm('Confirma a exclusão da Nota Fiscal de remessa?');" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
    </form>
</body>
</html>
