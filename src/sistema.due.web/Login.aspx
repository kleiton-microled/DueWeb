<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Sistema.DUE.Web.Login" %>

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
                        <h1 class="title logo"><img src="Content/imagens/logo.png" /></h1>
                    </div>
                </div>

                <div class="main-login main-center-login">

                    <asp:ValidationSummary ID="Validacoes" runat="server" ShowModelStateErrors="true" CssClass="alert alert-danger" />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtUsuario" class="control-label">Usuário:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-user fa" aria-hidden="true"></i></span>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Nome de usuário"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <label for="txtSenha" class="control-label">Senha:</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-key fa" aria-hidden="true"></i></span>
                                    <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" placeholder="Senha" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>                      
                    </div>              

                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-lg btn-block login-button" OnClick="btnLogin_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="login-registro">
                                <a href="Registrar.aspx">Não possui cadastro? clique aqui e registre-se!</a>
                                <a href="RecuperarSenha.aspx">Esqueci minha senha</a>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>
    <div id="footer" class="container">
        <nav>
            <div class="navbar-inner navbar-content-center">
                <div class="text-muted text-center">
                     <img src="Content/imagens/SP_logo.png" /> <br/>
                    <asp:label runat="server" ID="lblInfo" style="color:#0a48a6; font-size:17px" Text="Sistemas e Consultorias"/>
                </div>
                
            </div>
        </nav>
    </div>





    <script src="Content/js/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>

</body>
</html>

