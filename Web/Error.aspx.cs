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
    /// Página que apresenta mensagem de erro
    /// </summary>
    public partial class Error : System.Web.UI.Page
    {
        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName;

            Exception ex = null;
            string sURL = "";
            StringBuilder sb = new StringBuilder(2000);

            if (Context.Handler != null)
            {
                if (Session["ObjException"] != null)
                {
                    ex = (Exception)Session["ObjException"];
                }
            }
            else
            {
                ex = (Exception)Context.Items["ObjException"];
            }

            if (ex?.GetBaseException() != null)
            {
                ex = ex.GetBaseException();
            }

            if (Context.Handler != null)
            {
                if (Session["URLException"] != null)
                {
                    sURL = Session["URLException"].ToString();
                }
            }
            else
            {
                sURL = Context.Items["URLException"].ToString();
            }

            _ = sb.AppendFormat("Ocorreu um erro na aplicação {0}.", AppProgram.AppName)
                  .Append("<BR/><BR/>Se o problema persistir, por favor entre em contato com o suporte.");

            if (sURL != "")
            {
                _ = sb.Append("<BR/><BR/>Página: ")
                      .Append(HttpUtility.HtmlEncode(sURL));
            }

            if (ex != null)
            {
                _ = sb.Append("<BR/><BR/>Mensagem: ")
                      .Append(HttpUtility.HtmlEncode(ex.Message))
                      .Append("<BR/><BR/>Trilha: ")
                      .Append(ex.StackTrace.Replace(" at ", "<BR/> em "));
            }

            lblMessage.Text = sb.ToString();

            if (Context.Handler != null)
            {
                //store the error for later
                Session["ObjException"] = ex;
                Session["URLException"] = sURL;
            }
        }
    }
}