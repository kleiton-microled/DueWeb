<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarSenha.aspx.cs" Inherits="Sistema.DUE.Web.RecuperarSenha" %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/signup.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">

    <!-- Google Fonts -->
    <link href='https://fonts.googleapis.com/css?family=Passion+One' rel='stylesheet' type='text/css'>

    <title>Login</title>
</head>
<body>

    <form runat="server">

        <div class="container">
            <div class="row main">
                <div class="panel-heading">
                    <div class="panel-title text-center">
                        <h1 class="title logo">
                            <img src="Content/imagens/ldc-logo.png" /></h1>
                    </div>
                </div>

                <div class="main-login main-center-login">

                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                    <div ID="lblInfo" class="alert alert-info" runat="server" visible="true">
                        Informe seu nome de usuário ou email
                    </div>

                    <div ID="lblSucessoInfo" class="alert alert-success" runat="server" visible="false">
                        <asp:Label ID="lblSucessoMsg" runat="server" Text=""></asp:Label>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtUsuario" class="control-label">Usuário / Email:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-user fa" aria-hidden="true"></i></span>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnRecuperarSenha" runat="server" Text="Recuperar Senha" CssClass="btn btn-primary btn-lg btn-block login-button" OnClick="btnRecuperarSenha_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="login-registro">
                                <a href="Login.aspx">voltar</a>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>

    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>

</body>
</html>

