<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Procedimento.aspx.cs" Inherits="PI4Sem.AgendaFacil.Procedimento" MasterPageFile="~/MenuMain.master" Theme="PI4SemTheme" %>

<asp:Content ID="ContentPage" ContentPlaceHolderID="ContentBody" runat="Server">

    <script language="javascript" type="text/javascript" src="../Templates/JScript.js"></script>

    <script language="javascript" type="text/javascript">
        function OpenAdd() {
            window.location = "ProcedimentoAdd.aspx";
        }

        function OpenEdit(IdEmpresa, IdProcedimento) {
            window.location = "ProcedimentoEdit.aspx?IdEmpresa=" + IdEmpresa + "&IdProcedimento=" + IdProcedimento;
        }
    </script>

    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Procedimentos</h1>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="Default.aspx">Home</a></li>
                            <li class="breadcrumb-item active">Procedimentos</li>
                        </ol>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="row justify-content-md-center">
                <div class="col-2 ">
                    <button type="button" class="btn btn-block btn-outline-primary" onclick="OpenAdd()">Adicionar</button>
                </div>
            </div>
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <!-- /.row -->
                <div class="row">
                    <div class="col-12">
                        <div class="card">
                            <div class="card-header">
                                <h3 class="card-title">Procedimentos cadastrados</h3>
                            </div>
                            <!-- /.card-header -->
                            <div class="card-body table-responsive p-0">
                                <table class="table table-hover text-nowrap">
                                    <thead>
                                        <tr>
                                            <th>Função</th>
                                            <th>ID</th>
                                            <th>Nome</th>
                                            <th>Descrição</th>
                                            <th>Tempo Previsto</th>
                                            <th>Empresa</th>
                                            <th>Data de Cadastro</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%= listaProcedimento %>
                                    </tbody>
                                </table>
                            </div>
                            <!-- /.card-body -->
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