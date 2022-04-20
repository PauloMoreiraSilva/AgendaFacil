using System;
using System.Collections.Generic;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class Usuario : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string listaUsuario = "";

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
            CarregarListaUsuario();
        }

        private void CarregarListaUsuario()
        {
            AdmUsuario admUsuario = new AdmUsuario();
            List<Model.Usuario> listUsuario = admUsuario.SelectRows();

            foreach (Model.Usuario Usuario in listUsuario)
            {
                listaUsuario += "<tr>" +
                                "   <td>" +
                                "       <button type =\"button\" class=\"btn btn-default\" onclick=\"OpenEdit(" + Usuario.IdEmpresa + ", " + Usuario.IdFuncionario + ")\"><i class=\"fas fa-edit\"></i></button> " +
                                "   </td> " +
                                "   <td>" + Usuario.IdFuncionario.ToString() + "</td>" +
                                "   <td>" + Usuario.NomeFuncionario + "</td>" +
                                "   <td>" + Usuario.Login + "</td>" +
                                "   <td>" + Usuario.Telefone + "</td>" +
                                "   <td>" + Usuario.Email + "</td>" +
                                "   <td>" + (Usuario.Situacao == 0 ? "Ativo" : "Bloqueado") + "</td>" +
                                "   <td>" + Usuario.NomeEmpresa + "</td>" +
                                "   <td>" + Usuario.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss") + "</td>" +
                                "</tr>";
            }
        }
    }
}