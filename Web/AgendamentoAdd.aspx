<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgendamentoAdd.aspx.cs" Inherits="PI4Sem.AgendaFacil.AgendamentoAdd" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript">
        function Voltar() {
            window.location = "Agendamento.aspx";
        }
    </script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Adicionar Agendamento</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Agendamento.aspx">Agendamento</a></li>
                            <li class="breadcrumb-item active">Novo Agendamento</li>
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
                                <h3 class="card-title">Adicionar Agendamento</h3>
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
                                    <div class="form-group row">
                                        <label for="cboCliente" class="col-sm-2 col-form-label">Cliente:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboCliente" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="cboFuncionario" class="col-sm-2 col-form-label">Funcionario:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboFuncionario" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="cboProcedimento" class="col-sm-2 col-form-label">Procedimento:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboProcedimento" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtDataInicio" class="col-sm-2 col-form-label">Início:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control datahora" Text="" MaxLength="200" placeholder="dd/mm/yyyy hh:mm:ss" data-inputmask-alias="datetime" data-inputmask-inputformat="dd/mm/yyyy" data-mask="" inputmode="numeric"/>
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Informe a data e hora de início" ControlToValidate="txtDataInicio">*</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtDataFim" class="col-sm-2 col-form-label">Final:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control" Text="" MaxLength="200" placeholder="dd/mm/yyyy hh:mm:ss:" />
                                        </div>
                                    </div>
                                    <div style="display:none">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Informe a data e hora prevista para o final" ControlToValidate="txtDataFim">*</asp:RequiredFieldValidator>
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
    <!-- InputMask -->
<script src="Content/plugins/moment/moment.min.js"></script>
<script src="Content/plugins/inputmask/jquery.inputmask.min.js"></script>
<script>
    //Datemask dd/mm/yyyy
    $('.datahora').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyys' })
</script>

</asp:Content>