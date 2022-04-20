using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Header para páginas de Insert e Update
    /// </summary>
    public partial class ADDUPD : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FormsAuthentication.RequireSSL && !(Request.IsSecureConnection && Request.IsLocal))
            {
                string sRedirect = "https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl;
                Response.Redirect(sRedirect);
            }

            UserLoggedInfo oUserLoggedInfo = null;

            if ((Session["UserLoggedInfo"] != null) && (Session["GAuthToken"]) != null && (Request.Cookies["GAuthToken"] != null))
            {
                if (Session["GAuthToken"].ToString().Equals(Request.Cookies["GAuthToken"].Value))
                {
                    oUserLoggedInfo = (UserLoggedInfo)Session["UserLoggedInfo"];
                }
                else
                {
                    //Houve manipulação de cookie. Session Fixation.
                    ConcluirLogOut();
                    //Geralmente esta master page é usada em popups de ADD e UPD, então precisa fechar a janela
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Close", "window.close();", true);
                    return;
                }
            }
            else
            {
                ConcluirLogOut();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Close", "window.close();", true);
            }

            if (!Page.IsPostBack)
            {
                string sTitle = "";
                if (string.IsNullOrEmpty(HeadMaster.Title))
                {
                    oUserLoggedInfo = oUserLoggedInfo ?? (UserLoggedInfo)Session["UserLoggedInfo"];
                    //string sNomeCedente = oUserLoggedInfo?.CodigoCedente > 0 ?
                    //                      oUserLoggedInfo?.NomeCedente :
                    //                      oUserLoggedInfo?.DescModulo;

                    //sTitle = sNomeCedente + " > " + oUserLoggedInfo?.NomeUsuario;
                }
                HeadMaster.Title = sTitle;
            }
        }

        /// <summary>
        /// Efetua logout do usuário e remove todos os cookies
        /// </summary>
        private void ConcluirLogOut()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            //Remove cookie para evitar Session Fixation
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-2);
            }

            if (Request.Cookies["GAuthToken"] != null)
            {
                Response.Cookies["GAuthToken"].Value = string.Empty;
                Response.Cookies["GAuthToken"].Expires = DateTime.Now.AddYears(-2);
            }
        }
    }
}