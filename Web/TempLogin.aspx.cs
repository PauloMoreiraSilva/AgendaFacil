using System;
using System.Web;
using System.Web.Security;
using PI4Sem.Business;
using PI4Sem.DAL;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Efetua login automático para desenvolvimento
    /// </summary>
    public partial class TempLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Evento ao iniciar a página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
#pragma warning disable S125 // Não deve conter sessões de código comentado
            /*
            //Login automático sem aparecer o formulário
            Autenticar("GUA0001");
            Autenticar("PAG0027");
            Autenticar("PAG0024");
            GravarAuthToken();
            */
#pragma warning restore S125 // Não deve conter sessões de código comentado
            RequerSLL(true);
        }

        /// <summary>
        /// Se SSL é requerido, redireciona o usuário para a versão SSL do site
        /// Não permite o acesso fora do ambiente de desenvolvimento
        /// </summary>
        /// <param name="requerido">If true, requerido.</param>
        private void RequerSLL(bool requerido = false)
        {
            if (requerido && FormsAuthentication.RequireSSL && !(Request.IsSecureConnection && Request.IsLocal))
            {
                string sRedirect = "http" + (Request.ServerVariables["HTTPS"] == "ON" ? "s://" : "://") + Request.ServerVariables["HTTP_HOST"] + "/MainLogin.aspx";
                Response.Redirect(sRedirect);
            }
        }

        /// <summary>
        /// Evento ao clicar no botão
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            Autenticar(TextBox1.Text);
            GravarAuthToken();
        }

        /// <summary>
        /// Autentica o usuário sem gravar log auditoria ou data do último login
        /// </summary>
        /// <param name="sUsuario">The s usuario.</param>
        private void Autenticar(string sUsuario)
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            Usuario oUsuario = null;
            //oAdmUsuario.SelectRowByLogin(sLoginAcesso: sUsuario) ??
              //                 oAdmUsuario.SelectRowByUsername(sUsername: sUsuario);

            BizAcesso oBizAcesso = new BizAcesso();
            UserLoggedInfo oUserLoggedInfo = null;
            //oBizAcesso.ConverterUsuarioLogado(oUsuario);

            Session["UserLoggedInfo"] = oUserLoggedInfo;

            FormsAuthentication.RedirectFromLoginPage(oUserLoggedInfo.IdFuncionario.ToString(), false);
        }

        /// <summary>
        /// Grava o token de autenticação
        /// </summary>
        private void GravarAuthToken()
        {
            string guid = Guid.NewGuid().ToString();
            Session["GAuthToken"] = guid;
            Response.Cookies.Add(new HttpCookie("GAuthToken", guid));
        }
    }
}