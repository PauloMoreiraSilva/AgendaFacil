using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class AgendamentoEdit : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        
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

        }

    }
}