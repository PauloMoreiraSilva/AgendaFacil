<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MainLogin.aspx.cs" Inherits="PI4Sem.AgendaFacil.MainLogin" Theme="PI4SemTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html lang="pt-br">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>

    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Content/plugins/fontawesome-free/css/all.min.css" />
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="Content/plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="Content/dist/css/adminlte.min.css" />
</head>

<body class="hold-transition login-page">
    <form id="form1" runat="server">
        <div class="container">
            <asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true" AsyncPostBackTimeout="300">
                <Scripts>
                    <asp:ScriptReference Path="~/Scripts/AjaxControlToolkit/Bundle" />
                </Scripts>
            </asp:ScriptManager>

            <div class="login-box">
                <div class="login-logo">
                    <a href="/"><b>Agenda</b>FÁCIL</a>
                </div>

                <div class="card">
                    <div class="card-body login-card-body">
                        <p class="login-box-msg">Acessar ao sistema</p>

                        <asp:Login ID="LoginCtrl" runat="server" OnAuthenticate="LoginCtrl_Authenticate" OnLoggedIn="LoginCtrl_OnLoggedIn" FailureText="Sua tentativa de acesso não obteve sucesso. Por favor tente novamente.">
                            <LayoutTemplate>
                                <div class="row">
                                    <div class="col-12">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Verifique os valores inválidos:" ValidationGroup="LoginCtrl" />
                                    </div>
                                </div>
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="Usuário:"></asp:TextBox>
                                    <div class="input-group-append">
                                        <div class="input-group-text">
                                            <span class="fas fa-user"></span>
                                        </div>
                                    </div>
                                </div>
                                <div style="display:none">
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Informe o usuário" ToolTip="Informe o usuário" ValidationGroup="LoginCtrl">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="Password" runat="server" CssClass="TextBox form-control" TextMode="Password" placeholder="Senha:"></asp:TextBox>
                                    <div class="input-group-append">
                                        <div class="input-group-text">
                                            <span class="fas fa-lock"></span>
                                        </div>
                                    </div>
                                </div>
                                <div style="display:none">
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Informe a senha" ToolTip="Informe a senha" ValidationGroup="LoginCtrl">*</asp:RequiredFieldValidator>
                                </div>
                                <div class="row">
                                    <div class="col-12" style="color: red; text-align: center">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        <br />
                                        <asp:Image ID="imgCaptcha" runat="server" ImageUrl="~/Captcha.aspx" />
                                        <span style="margin: 0 0 5px 10px;">
                                            <asp:ImageButton Text="text" ID="BntRefresh" runat="server" ImageUrl="~/Images/Reload.png" Width="16px" Height="16px" ImageAlign="Bottom" OnClick="BntRefresh_Click" ToolTip="Gerar outra imagem" /></span>
                                        <br />
                                        <br />
                                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" placeholder="Digite os caracteres da imagem:" />
                                    </div>
                                    <div class="col-12">
                                        <br />
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="Button btn btn-primary btn-block" Text="Entrar" ValidationGroup="LoginCtrl" />
                                        <br />
                                    </div>
                                </div>
                            </LayoutTemplate>
                        </asp:Login>
                        <div class="row">
                            <div class="col-6">
                                <p class="mb-1">
                                    <a href="forgot-password.aspx">Esqueci minha senha</a>
                                </p>
                            </div>
                            <div class="col-6">
                                <p class="mb-0">
                                    <a href="register.aspx" class="text-right">Quero me cadastrar</a>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="footer">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="text-align: center;">© Copyright <%= DateTime.Now.Year %>. Todos os direitos reservados.</td>
                    </tr>
                </table>
            </div>

            <asp:NoBot ID="NoBot" runat="server" CutoffMaximumInstances="5" CutoffWindowSeconds="60" ResponseMinimumDelaySeconds="2" />
        </div>
    </form>
    <!-- jQuery -->
    <script src="Content/plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="Content/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- AdminLTE Aplicativo -->
    <script src="Content/dist/js/adminlte.min.js"></script>
</body>
</html>