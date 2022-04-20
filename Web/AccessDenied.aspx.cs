using System;

/// <summary>
/// Página para acesso negado.
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página para acesso negado
    /// </summary>
    public partial class AccessDenied : System.Web.UI.Page
    {
        /// <summary>
        /// Evento ao acessar a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sFncName = Server.UrlDecode(Request.QueryString["Fnc"]);

            this.lblMsg.Text = "Seu usuário não tem acesso permitido à esta função " + sFncName + ". Em caso de dúvidas contacte o administrador do sistema.";
        }
    }
}