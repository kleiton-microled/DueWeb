<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrar.aspx.cs" Inherits="Sistema.DUE.Web.Registrar" %>

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/signup.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">

    <!-- Google Fonts -->
    <link href='https://fonts.googleapis.com/css?family=Passion+One' rel='stylesheet' type='text/css'>

    <title>Registrar</title>
</head>
<body>

    <form runat="server">

        <div class="container">
            <div class="row main">
                <div class="panel-heading">
                    <div class="panel-title text-center">
                        <h1 class="title"><img src="Content/imagens/ldc-logo.png" /></h1>
                    </div>
                </div>

                <div class="main-login main-center-registro">

                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtNome" class="control-label">Nome:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-text-width"></span></span>
                                    <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" placeholder="Nome completo"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                         <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtEmail" class="control-label">Email:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-comment"></span></span>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="E-mail"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtCPF" class="control-label">CPF:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-credit-card"></span></span>
                                    <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" placeholder="CPF (somente números)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtLogin" class="control-label">Login:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                                    <asp:TextBox ID="txtLogin" runat="server" CssClass="form-control" placeholder="Nome de usuário"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSenha" class="control-label">Senha:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>
                                    <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" placeholder="Senha" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSenhaConfirmada" class="control-label">Confirmar Senha:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>
                                    <asp:TextBox ID="txtSenhaConfirmada" runat="server" CssClass="form-control" placeholder="Senha" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnRegistrar" runat="server" Text="Registrar" CssClass="btn btn-primary btn-lg btn-block registrar-button" OnClick="btnRegistrar_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="login-registro">
                                <a href="Login.aspx">Já possui cadastro? clique aqui e faça o login!</a>
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
