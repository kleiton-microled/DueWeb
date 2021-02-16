<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmaExclusaoDUE.aspx.cs" Inherits="Sistema.DUE.Web.ConfirmaExclusaoDUE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-sm-8 col-sm-offset-2">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Exclusão de DUE</h3>
                </div>
                <div class="panel-body">
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <div class="alert alert-info alert-dismissible">
                                Você está prestes a excluir um registro de DUE. Após a exclusão, os dados não poderão mais ser recuperados.
                            </div>
                        </div>
                    </div>
                    <div class="row no-gutter">
                        <div class="col-md-12">
                            <a href="ConsultarDUE.aspx" class="btn btn-default">Voltar</a>
                            <asp:Button ID="btnExcluirDUE" runat="server" CssClass="btn btn-danger" Text="Sim, estou ciente e confirmo a exclusão" OnClick="btnExcluirDUE_Click" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
</asp:Content>
