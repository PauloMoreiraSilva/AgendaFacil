<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsuarioAdd.aspx.cs" Inherits="PI4Sem.AgendaFacil.UsuarioAdd" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function Voltar() {
            window.location = "Usuario.aspx";
        }
    </script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Adicionar Usuario</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Funcionario.aspx">Funcionário</a></li>
                            <li class="breadcrumb-item"><a href="Usuario.aspx">Usuário</a></li>
                            <li class="breadcrumb-item active">Novo Usuario</li>
                        </ol>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <!-- /.row -->
                <div class="row">
                    <div class="col-12">
                        <div class="card card-info">
                            <div class="card-header">
                                <h3 class="card-title">Adicionar Usuário</h3>
                            </div>
                            <!-- /.card-header -->
                            <!-- form start -->
                            <form id="form1" runat="server" class="form-horizontal">
                                <asp:ScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true" AsyncPostBackTimeout="300">
                                    <Scripts>
                                        <asp:ScriptReference Path="~/Scripts/AjaxControlToolkit/Bundle" />
                                    </Scripts>
                                </asp:ScriptManager>
                                <div class="card-body">
                                    
                                    <p><asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Verifique os valores inválidos:" /></p>
                                    
                                    <div class="form-group row">
                                        <label for="cboEmpresa" class="col-sm-2 col-form-label">Empresa:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboEmpresa" runat="server"></select>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Informe a Empresa" ControlToValidate="cboEmpresa">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="cboFuncionario" class="col-sm-2 col-form-label">Funcionário:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboFuncionario" runat="server"></select>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe a Funcionário" ControlToValidate="cboFuncionario">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtUsuario" class="col-sm-2 col-form-label">Login:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" Text="" MaxLength="14" placeholder="Usuário:"/>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Informe o login" ControlToValidate="txtUsuario">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtSenha" class="col-sm-2 col-form-label">Senha:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" TextMode="Password" Text="" MaxLength="14" placeholder="Senha:"/>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Informe a senha" ControlToValidate="txtSenha">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtConfirmeSenha" class="col-sm-2 col-form-label">Confirme a Senha:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtConfirmeSenha" runat="server" CssClass="form-control" TextMode="Password" Text="" MaxLength="14" placeholder="Confirmar senha:"/>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Confirme a senha" ControlToValidate="txtConfirmeSenha">*</asp:RequiredFieldValidator>
                                        <asp:CompareValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="txtConfirmeSenha" ControlToCompare="txtSenha" Text="A senha não confere" />
                                    </div>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer">
                                    <asp:Button ID="btnCadastro" runat="server" type="button" CssClass="btn btn-info" OnClick="BtnCadastro_Click" Text="Cadastrar" />
                                    <button type="button" class="btn btn-default float-right" onclick="Voltar()">Cancelar</button>
                                </div>
                                <!-- /.card-footer -->
                            </form>
                        </div>
                        <!-- /.card -->
                    </div>
                </div>
                <!-- /.row -->
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
</asp:Content>

<asp:Content ID="ContentJs" ContentPlaceHolderID="ContentJs" runat="Server">



</asp:Content>