<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="ConsultarNFsDUE.aspx.cs" Inherits="Sistema.DUE.Web.ConsultarNFsDUE" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <div class="row no-gutter">
        <div class="col-md-12">

            <div class="row">
                <div class="col-md-10">
                    <h5><strong>Legenda:</strong></h5>
                    <table>
                        <tr>
                            <td>
                                <asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="MistyRose">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                &nbsp;Notas não encontradas no CCT 
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;<asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="LightYellow">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                &nbsp;Notas com divergências no saldo
                            </td>
                            <td>&nbsp;&nbsp;&nbsp;<asp:Label BorderStyle="Solid" BorderWidth="1" runat="server" BackColor="LightGreen">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                &nbsp;Notas sem divergências
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
                                <asp:BoundField DataField="Item" HeaderText="Item" />
                                <asp:BoundField HeaderText="Recinto" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
