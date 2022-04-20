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
    public partial class Empresa : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string listaEmpresa = "";

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
            CarregarListaEmpresa();
        }

        private void CarregarListaEmpresa()
        {
            AdmEmpresa admEmpresa = new AdmEmpresa();
            List<Model.Empresa> listEmpresa = admEmpresa.SelectRows();

            foreach (Model.Empresa Empresa in listEmpresa)
            {
                listaEmpresa += "<tr>" +
                                "   <td>" +
                                "       <button type =\"button\" class=\"btn btn-default\" onclick=\"OpenEdit(" + Empresa.IdEmpresa + ")\"><i class=\"fas fa-edit\"></i></button> " +
                                "   </td> " +
                                "   <td>" + Empresa.IdEmpresa.ToString() + "</td>" +
                                "   <td>" + Empresa.Nome + "</td>" +
                                "   <td>" + Empresa.Nome + "</td>" +
                                "   <td>" + Empresa.Inscricao + "</td>" +
                                "   <td>" + Empresa.Bairro + "</td>" +
                                "   <td>" + Empresa.Cidade + "</td>" +
                                "   <td>" + Empresa.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss") + "</td>" +
                                "</tr>";
            }

        }
    }
}