<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResumoDUE.aspx.cs" EnableSessionState="False" EnableViewState="false" Inherits="Sistema.DUE.Web.ResumoDUE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resumo DUE</title>

    <style>
        * {
            font-size: 12px;
            font-family: Courier New, Courier, monospace;
        }

        h1 {
            font-size: 24px;
        }

        h2 {
            font-size: 20px;
        }

        h3 {
            font-size: 16px;
        }

        table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .cinza {
            background: #F5F5F5;
            font-weight: bold;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>

            <h1>Resumo</h1>

            <h3>Informações Iniciais</h3>

            <table style="width: 100%;">
                <tr>
                    <td class="cinza" style="width: 250px;">CPF/CNPJ Declarante:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisCNPJDeclarante" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Moeda Negociação:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisMoeda" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">RUC:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisRUC" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Situação Especial:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisSitEspecial" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Forma de Exportação:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisFormaExportacao" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Via Especial de Transporte:</td>
                    <td>
                        <asp:Label ID="lblInfoIniciaisViaEspecialTransp" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

            <h3>Informações do Local de Despacho</h3>

            <table style="width: 100%;">
                <tr>
                    <td class="cinza" style="width: 250px;">Unidade RFB:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespUnidadeRFB" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Tipo Recinto Aduaneiro:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespTipoRecinto" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Recinto Aduaneiro:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespRecintoAduaneiro" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">CPF/CNPJ do Responsável:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespCNPJResponsavel" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Latitude:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespLatitude" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Longitude:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespLongitude" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Endereço:</td>
                    <td>
                        <asp:Label ID="lblInfLocalDespEndereco" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>


            <h3>Informações do Local de Embarque</h3>

            <table style="width: 100%;">
                <tr>
                    <td class="cinza" style="width: 250px;">Unidade RFB:</td>
                    <td>
                        <asp:Label ID="lblInfLocalEmbarqueUnidadeRFB" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Tipo Recinto Aduaneiro:</td>
                    <td>
                        <asp:Label ID="lblInfLocalEmbarqueTipoRecinto" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Referência de Endereço:</td>
                    <td>
                        <asp:Label ID="lblInfLocalEmbarqueRefEndereco" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td class="cinza">Recinto Aduaneiro:</td>
                    <td>
                        <asp:Label ID="lblInfLocalEmbarqueRecintoAduaneiro" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>

            <h3>Observações</h3>
            <asp:Label ID="lblObservacoes" runat="server" Text=""></asp:Label>

            <asp:Label ID="lblItens" runat="server" Text=""></asp:Label>

            <div style="clear: right; padding-top: 18px; float: right;">

                <h3>Totais</h3>

                <table style="text-align: right; border: 0px;" width="400px;" border="0">
                    <tr>
                        <td class="cinza">Quantidade Total:
                        </td>
                        <td style="text-align: left; width: 200px;">
                            <asp:Label ID="txtQuantidadeTotal" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="cinza">Preço Total CV:
                        </td>
                        <td style="text-align: left; width: 200px;">
                            <asp:Label ID="txtPrecoTotalCV" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="cinza">Preço Total LE:
                        </td>
                        <td style="text-align: left; width: 200px;">
                            <asp:Label ID="txtPrecoTotalLE" runat="server" Text="" Font-Bold="true"></asp:Label></td>
                    </tr>
                </table>
            </div>
            
        </div>
        <br />
            <br />
    </form>
</body>
</html>
