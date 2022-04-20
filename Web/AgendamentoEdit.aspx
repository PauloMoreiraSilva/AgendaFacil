<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgendamentoEdit.aspx.cs" Inherits="PI4Sem.AgendaFacil.AgendamentoEdit" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

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
                        <h1>Editar Agendamento</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item"><a href="Agendamento.aspx">Agendamento</a></li>
                            <li class="breadcrumb-item active">Editar Agendamento</li>
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
                                <h3 class="card-title">Editar Agendamento</h3>
                            </div>
                            <!-- /.card-header -->
                            <!-- form start -->
                            <form class="form-horizontal">
                                <div class="card-body">

                                    <div class="form-group row">
                                        <label for="cboEmpresa" class="col-sm-2 col-form-label">Empresa:</label>
                                        <div class="col-sm-10">
                                            <select class="form-control" id="cboEmpresa" runat="server"></select>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="cboCliente" class="col-sm-2 col-form-label">Funcionario:</label>
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
                                            <input type="text" class="form-control" id="txtDataInicio" placeholder="Início:">
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="txtDataFim" class="col-sm-2 col-form-label">Final:</label>
                                        <div class="col-sm-10">
                                            <input type="text" class="form-control" id="txtDataFim" placeholder="Final previsto:">
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card-body -->
                                <div class="card-footer">
                                    <button type="submit" class="btn btn-info">Alterar</button>
                                    <button type="submit" class="btn btn-default float-right" onclick="Voltar()">Cancelar</button>
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