<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="ValidarXmlDUE.aspx.cs" Inherits="Sistema.DUE.Web.ValidarXmlDUE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS" runat="server">
    <link href="Content/css/smart_wizard.css" rel="stylesheet" />
    <link href="Content/css/smart_wizard_theme_dots.css" rel="stylesheet" />
    <link href="Content/css/select2.css" rel="stylesheet" />

    <style>
        #xml {
            padding-left: 20pt;
            background-color: #ffffb3;
            color: #990000;
            font-size: 14px;
            font-family: Courier New, Lucida Sans Typewriter, Lucida Typewriter, monospace;
            font-weight: 600;
            white-space: pre;
        }

        code {
            display: block !important;
            white-space: pre !important;
            font-family: Consolas, Courier, monospace;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <div class="row">
        <div class="col-sm-8 col-sm-offset-2">

            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title">Validação de XML DUE</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-10">
                            <label>Selecione o arquivo .XML:</label>
                            <asp:FileUpload ID="txtUpload" runat="server" CssClass="btn btn-default" />
                        </div>
                        <div class="col-md-2">
                            <label>&nbsp;</label>
                            <asp:Button ID="btnValidarDUE" runat="server" Text="Validar XML DUE" CssClass="btn btn-warning btn-block" OnClick="btnValidarDUE_Click" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="row">
                        <div class="col-md-12">

                            <% if (ViewState["RetornoSucesso"] != null)
                                {
                                    if ((bool)ViewState["RetornoSucesso"] == true)
                                    {%>
                            <div class="alert alert-success">
                                <strong>Sucesso!</strong> DUE pronta para ser enviada              
                            </div>
                            <% }
                                else
                                {%>
                            <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />
                            <% }
                                }%>

                            <% if (ViewState["Debug"] != null)
                                {%>

                            <div class="alert alert-warning">
                                <strong>Retorno real Siscomex:</strong>
                                <br />
                                <br />
                                <textarea id="ta" style="height: 400px; width: 100%;" oninput="beautify()">
                                <%= ViewState["Debug"].ToString() %>
                            </textarea>
                            </div>

                            <% } %>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
    <script src="Content/js/xmlPrettify.js"></script>

    <script>

        $(document).ready(function () {
            beautify();
        });

        function beautify() {

            var ta = document.getElementById('ta'),
                cp = '';

            if (ta != null) {

                cp = cp.replace(/\'/g, '').replace(/\"/g, '');

                if (!isNaN(parseInt(cp))) {  // argument is integer
                    cp = parseInt(cp);
                } else {
                    cp = cp ? cp : 4;
                }

                ta.value = vkbeautify.xml(ta.value, cp);
            }
        }

    </script>

</asp:Content>
