using System;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Apresenta mensagem informativa para página em construção
    /// </summary>
    public partial class UnderConstruction : System.Web.UI.Page
    {
        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFncName = Server.UrlDecode(Request.QueryString["Fnc"]);

            lblMsg.Text = "Funcionalidade em construção: " + sFncName + ". Em caso de dúvidas contacte o administrador do sistema.";
        }
    }
}