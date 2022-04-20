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
    public partial class Cliente : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string listaCliente = "";
        
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

            CarregarListaCliente();
        }

        private void CarregarListaCliente()
        {
            AdmCliente admCliente = new AdmCliente();
            List<Model.Cliente> listCliente = admCliente.SelectRows();

            foreach (Model.Cliente cliente in listCliente)
            {
                listaCliente += "<tr>" +
                                "   <td>" +
                                "       <button type =\"button\" class=\"btn btn-default\" onclick=\"OpenEdit("+cliente.IdEmpresa+","+cliente.IdCliente+")\"><i class=\"fas fa-edit\"></i></button> " +
                                "   </td> " +
                                "   <td>"+cliente.IdCliente.ToString()+"</td>" +
                                "   <td>"+cliente.Nome+"</td>" +
                                "   <td>"+cliente.Telefone+"</td>" +
                                "   <td>"+cliente.Email+"</td>" +
                                "   <td>"+cliente.Bairro+"</td>" +
                                "   <td>" + cliente.NomeEmpresa + "</td>" +
                                "   <td>" +cliente.DataInclusao.ToString("dd/MM/YYYY")+"</td>" +
                                "</tr>";
            }

        }

    }
}