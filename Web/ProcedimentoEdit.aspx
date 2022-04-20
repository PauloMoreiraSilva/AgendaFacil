<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcedimentoEdit.aspx.cs" Inherits="PI4Sem.AgendaFacil.ProcedimentoEdit" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function Voltar() {
            window.location = "Procedimento.aspx";
        }
    </script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Editar Procedimento</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Procedimento.aspx">Procedimento</a></li>
                            <li class="breadcrumb-item active">Editar Procedimento</li>
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
                                <h3 class="card-title">Editar Procedimento</h3>
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
                                            <asp:Label ID="lblEmpresa" runat="server" Text="" class="col-sm-2 col-form-label"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtnome" class="col-sm-2 col-form-label">Nome:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Nome:" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe o nome do procedimento" ControlToValidate="txtNome">*</asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group row">
                                        <label for="txtDescricao" class="col-sm-2 col-form-label">Descrição:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Descrição:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtMaterial" class="col-sm-2 col-form-label">Material necessário:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtMaterial" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Material necessário:" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtTempoPrevisto" class="col-sm-2 col-form-label">Tempo previsto:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtTempoPrevisto" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="Tempo previsto (em minutos):" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Informe o tempo previsto para o procedimento" ControlToValidate="txtTempoPrevisto">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtTempoPrevisto" ErrorMessage="Somente números são permitidosno campo Tempo Previsto" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer">
                                    <asp:Button ID="btnEdicao" runat="server" type="button" CssClass="btn btn-info" OnClick="BtnEdicao_Click" Text="Alterar" />
                                    <button type="button" class="btn btn-danger align-content-center" onclick="alert('Em desenvolvimento')">Excluir</button>
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