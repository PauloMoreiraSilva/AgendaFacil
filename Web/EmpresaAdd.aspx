<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmpresaAdd.aspx.cs" Inherits="PI4Sem.AgendaFacil.EmpresaAdd" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function Voltar() {
            window.location = "Empresa.aspx";
        }
    </script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Adicionar Empresa</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Empresa.aspx">Empresa</a></li>
                            <li class="breadcrumb-item active">Nova Empresa</li>
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
                                <h3 class="card-title">Adicionar Empresa</h3>
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
                                        <label for="txtNome" class="col-sm-2 col-form-label">Nome:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Nome:" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Informe o nome da Empresa" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtInscricao" class="col-sm-2 col-form-label">CPF/CNPJ:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtInscricao" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="CPF/CNPJ:" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe o CNPJ/CPF da Empresa" ControlToValidate="txtInscricao">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtEndereco" class="col-sm-2 col-form-label">Endereço:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtEndereco" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Endereço:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtBairro" class="col-sm-2 col-form-label">Bairro:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Bairro:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtCidade" class="col-sm-2 col-form-label">Cidade:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtCidade" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Cidade:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtUf" class="col-sm-2 col-form-label">UF:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtUf" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="UF:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtCep" class="col-sm-2 col-form-label">CEP:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtCep" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="CEP:" />
                                        </div>
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