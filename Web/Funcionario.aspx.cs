using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;
using PI4Sem.Infra;
using System.Collections.Generic;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class Funcionario : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string listaFuncionario = "";

        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserLoggedInfo"] != null)
            {
                oUserLoggedInfo = (UserLoggedInfo)Session["UserLoggedInfo"];
            }
            CarregarListaFuncionario();
        }

        private void CarregarListaFuncionario()
        {
            AdmFuncionario admFuncionario = new AdmFuncionario();
            List<Model.Funcionario> listFuncionario = admFuncionario.SelectRows();

            foreach (Model.Funcionario Funcionario in listFuncionario)
            {
                listaFuncionario += "<tr>" +
                                "   <td>" +
                                "       <button type =\"button\" class=\"btn btn-default\" onclick=\"OpenEdit("+ Funcionario.IdEmpresa+"," + Funcionario.IdFuncionario + ")\"><i class=\"fas fa-edit\"></i></button> " +
                                "   </td> " +
                                "   <td>" + Funcionario.IdFuncionario.ToString() + "</td>" +
                                "   <td>" + Funcionario.Nome + "</td>" +
                                "   <td>" + Funcionario.Cpf + "</td>" +
                                "   <td>" + Funcionario.Telefone + "</td>" +
                                "   <td>" + Funcionario.Email + "</td>" +
                                "   <td>" + Funcionario.NomeEmpresa + "</td>" +
                                "   <td>" + (Funcionario.EhProprietario == 1 ? "Sim" : "Não") + "</td>" +
                                "   <td>" + Funcionario.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss") + "</td>" +
                                "</tr>";
            }

        }
    }
}