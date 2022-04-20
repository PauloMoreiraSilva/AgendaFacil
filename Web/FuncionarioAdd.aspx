<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FuncionarioAdd.aspx.cs" Inherits="PI4Sem.AgendaFacil.FuncionarioAdd" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function Voltar() {
            window.location = "Funcionario.aspx";
        }
    </script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Adicionar Funcionário</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Funcionario.aspx">Funcionário</a></li>
                            <li class="breadcrumb-item active">Novo Funcionário</li>
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
                                <h3 class="card-title">Adicionar Funcionário</h3>
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
                                        <label for="txtNome" class="col-sm-2 col-form-label">Nome:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Nome:" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe o nome do cliente" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtCpf" class="col-sm-2 col-form-label">CPF:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtCpf" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="CPF:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtTelefone" class="col-sm-2 col-form-label">Telefone:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Telefone:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtEmail" class="col-sm-2 col-form-label">EMail:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Email:" />
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="cboProprietario" class="col-sm-2 col-form-label">É Proprietário?:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboProprietario" runat="server">
                                                <option value="0">Não</option>
                                                <option value="1">Sim</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Informe a Empresa" ControlToValidate="cboEmpresa">*</asp:RequiredFieldValidator>
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