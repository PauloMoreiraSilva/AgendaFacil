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
    public partial class Procedimento : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string listaProcedimento = "";

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
            CarregarListaProcedimento();
        }
        private void CarregarListaProcedimento()
        {
            AdmProcedimento admProcedimento = new AdmProcedimento();
            List<Model.Procedimento> listProcedimento = admProcedimento.SelectRows();

            foreach (Model.Procedimento Procedimento in listProcedimento)
            {
                listaProcedimento += "<tr>" +
                                "   <td>" +
                                "       <button type =\"button\" class=\"btn btn-default\" onclick=\"OpenEdit("+ Procedimento.IdEmpresa + "," + Procedimento.IdProcedimento + ")\"><i class=\"fas fa-edit\"></i></button> " +
                                "   </td> " +
                                "   <td>" + Procedimento.IdProcedimento.ToString() + "</td>" +
                                "   <td>" + Procedimento.Nome + "</td>" +
                                "   <td>" + Procedimento.Descricao + "</td>" +
                                "   <td>" + Procedimento.TempoPrevisto + "</td>" +
                                "   <td>" + Procedimento.NomeEmpresa + "</td>" +
                                "   <td>" + Procedimento.DataInclusao.ToString("dd/MM/yyyy HH:mm:ss") + "</td>" +
                                "</tr>";
            }

        }
    }
}