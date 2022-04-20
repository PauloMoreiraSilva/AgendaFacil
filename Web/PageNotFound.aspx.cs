using System;
using System.Text;
using System.Web;
using System.Web.UI;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Apresenta mensagem de erro para página não encontrada
    /// </summary>
    public partial class PageNotFound : System.Web.UI.Page
    {
        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName;

            string sURL = Request.QueryString["aspxerrorpath"];
            StringBuilder sb = new StringBuilder(2000);

            _ = sb.AppendFormat("Desculpe, mas a página não foi encontrada na aplicação {0}.", AppProgram.AppName);

            if (!string.IsNullOrEmpty(sURL))
            {
                _ = sb.Append("<BR/><BR/>Página procurada: ")
                      .Append(HttpUtility.HtmlEncode(sURL));
            }

            lblMessage.Text = sb.ToString();
        }
    }
}