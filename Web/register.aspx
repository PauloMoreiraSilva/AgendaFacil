<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="PI4Sem.AgendaFacil.Register" Theme="PI4SemTheme" %>

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
                        <p class="login-box-msg">Efetue seu cadastro.</p>

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Verifique os valores inválidos:" />
                        
                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtRazaoSocial" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Razão Social:" />
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="far fa-building"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Informe a Razão Social" ControlToValidate="txtRazaoSocial">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtInscricao" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="CNPJ/CPF:" />
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-money-check"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Informe o CNPJ ou CPF" ControlToValidate="txtInscricao">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Nome do responsável:" />
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="far fa-address-card"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Informe o nome do responsável" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="E-mail:" />
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-envelope"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Informe o Email" ControlToValidate="txtEmail">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ControlToValidate="txtEmail" ErrorMessage="Email em formato inválido">*</asp:RegularExpressionValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" Text="" MaxLength="14" placeholder="Usuário:"/>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-user"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe o login" ControlToValidate="txtUsuario">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" Text="" MaxLength="14" placeholder="Senha:"/>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock-open"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Informe a senha" ControlToValidate="txtSenha">*</asp:RequiredFieldValidator>
                        </div>

                        <div class="input-group mb-3">
                            <asp:TextBox ID="txtConfirmeSenha" runat="server" CssClass="form-control" Text="" MaxLength="14" placeholder="Confirmar senha:"/>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock-open"></span>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Confirme a senha" ControlToValidate="txtConfirmeSenha">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtSenha" ControlToCompare="txtConfirmeSenha" Text="A senha não confere" />
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
                                <asp:Button ID="btnCadastro" runat="server" type="button" CssClass="btn btn-primary btn-block" OnClick="BtnCadastro_Click" Text="Cadastrar" />
                                <br />
                            </div>
                            <!-- /.col -->
                        </div>

                        <div class="row">
                            <div class="col-6">
                                <p class="mb-1">
                                    <a href="MainLogin.aspx">Já sou cadastrado</a>
                                </p>
                            </div>
                            <div class="col-6">
                                <p class="mb-0">
                                    <a href="forgot-password.aspx" class="text-right">Esqueci minha senha</a>
                                </p>
                            </div>
                        </div>

                    </div>
                    <!-- /.login-card-body -->
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
    <!-- AdminLTE App -->
    <script src="Content/dist/js/adminlte.min.js"></script>
</body>
</html>