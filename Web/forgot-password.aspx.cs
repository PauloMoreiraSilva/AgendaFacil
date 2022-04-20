using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PI4Sem.Business;
using PI4Sem.DAL;
using PI4Sem.Infra;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página de login na aplicação.
    /// </summary>
    public partial class ForgtPassword : System.Web.UI.Page
    {
        /// <summary>
        /// String de comandos SQL
        /// </summary>
        private string SQL { get; set; } = string.Empty;

        /// <summary>
        /// Diretório de imagens do portal
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Identifica se o usuário foi autenticado com sucesso
        /// </summary>
        private int IAutenticado { get; set; } = int.MinValue;

        /// <summary>
        /// Parâmetros do Web.Config
        /// </summary>
        private Config.Config WebConfig { get; set; }

        /// <summary>
        /// Evento inicial ao entrar na página.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName;
            ImagePath = AppProgram.GetAppImagesPath();
            WebConfig = new Config.Config();

            RequerSLL(false);

            if (!Page.IsPostBack)
            {
                ValidarUrl();
            }
        }

        
        /// <summary>
        /// Redireciona o usuário para a página de cadastro
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void LkbRegistro_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Registro.aspx");
        }

        /// <summary>
        /// Se SSL é requerido, redireciona o usuário para a versão SSL do site
        /// </summary>
        /// <param name="requerido">If true, requerido.</param>
        private void RequerSLL(bool requerido = false)
        {
            if (requerido && FormsAuthentication.RequireSSL && !(Request.IsSecureConnection && Request.IsLocal))
            {
                string sRedirect = "https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl;
                Response.Redirect(sRedirect);
            }
        }

        /// <summary>
        /// Valida a URL para garantir que o acesso é feito por humano
        /// </summary>
        /// <returns>True: acesso válido / False: Robot acessando.</returns>
        private void ValidarUrl()
        {
            if (!NoBot.IsValid())
            {
                AppProgram.SetAlertAjax(this, "O sistema não conseguiu detectar que você é um humano. Por favor tente novamente.", "SetAlert");
            }
        }
    }
}