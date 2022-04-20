using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PI4Sem.Business;
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
    public partial class MainLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Diretório de imagens do portal
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Identifica se o usuário foi autenticado com sucesso
        /// </summary>
        private int IAutenticado { get; set; } = int.MinValue;

        /// <summary>
        /// Evento inicial ao entrar na página.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">event.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName;
            ImagePath = AppProgram.GetAppImagesPath();

            RequerSLL(false);

            if (!Page.IsPostBack)
            {
                LoginCtrl.Focus();
                ValidarUrl();
            }

            Image Captcha = (Image)LoginCtrl.FindControl("imgCaptcha");
            Captcha.ImageUrl = "Captcha.aspx";
            Captcha.DataBind();
        }

        /// <summary>
        /// Processa o login do usuário
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            if (!CaptchaValido())
            {
                e.Authenticated = false;
                LoginCtrl.FailureText = "Autenticação pelo Captcha falhou.<br />Tente novamente.";
                return;
            }

            IAutenticado = Authenticate();

            if ((IAutenticado == 1) || (IAutenticado == 2))
            {
                e.Authenticated = true;
                _ = GetUserLoggedInfo();

                return;
            }

            if (IAutenticado == 3)
            {
                e.Authenticated = false;
                LoginCtrl.FailureText = "Usuário ou senha inválidos. Tente novamente.";
                return;
            }

            if (IAutenticado == 4)
            {
                e.Authenticated = false;
                LoginCtrl.FailureText = "Seu acesso está bloqueado. Entre em contato com o administrador.";
                return;
            }

            if (IAutenticado == 5)
            {
                e.Authenticated = false;
                LoginCtrl.FailureText = "Sua senha expirou. Entre em contato com o administrador.";
                return;
            }

            e.Authenticated = false;
            LoginCtrl.FailureText = "Sua tentativa de acesso não obteve sucesso. Por favor tente novamente.";
        }

        /// <summary>
        /// Ao efetuar o login com sucesso, se precisar trocar senha, redireciona o usuário
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void LoginCtrl_OnLoggedIn(object sender, EventArgs e)
        {
            if (IAutenticado == 2)
            {
                Response.Redirect("~/CAD/CAD005_TrocaSenha.aspx");
            }
        }

        /// <summary>
        /// Autentica o usuário na aplicação
        /// </summary>
        /// <returns>1 = autenticado / 2 = trocar a senha / 3 = senha inválida / 4 = usuário não encontrado ou excluído ou bloqueado ou sem acesso (banco) / 5 = senha expirada.</returns>
        private int Authenticate()
        {
            BizAcesso oBizAcesso = new BizAcesso();
            return oBizAcesso.Authenticate(sUsername: LoginCtrl.UserName, sSenha: LoginCtrl.Password);
        }

        /// <summary>
        /// Verifica se o Captcha foi preenchido corretamente.
        /// </summary>
        /// <returns>True = válido / False = inválido.</returns>
        private bool CaptchaValido()
        {
            TextBox ResCaptcha = (TextBox)LoginCtrl.FindControl("txtCaptcha");
            return Session["CaptchaImageText"] != null &&
                   string.Equals(Session["CaptchaImageText"]?.ToString(), ResCaptcha.Text, StringComparison.Ordinal);
        }

        /// <summary>
        /// Grava o usuário autenticado na session do usuário
        /// </summary>
        private UserLoggedInfo GetUserLoggedInfo()
        {
            BizAcesso oBizAcesso = new BizAcesso();
            UserLoggedInfo oUserLoggedInfo = oBizAcesso.Autenticar(LoginCtrl.UserName);

            Session["UserLoggedInfo"] = oUserLoggedInfo;
            return oUserLoggedInfo;
        }

        /// <summary>
        /// Redireciona para alterar a senha
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void LkbRedefinirSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/RedefinirSenha.aspx");
        }

        /// <summary>
        /// Redireciona o usuário para solicitar o username
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void LkbSolicitarUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EsqueciUsuario.aspx");
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
        /// Evento do click do botão de refresh: retorna outra imagem do captcha.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void BntRefresh_Click(object sender, EventArgs e)
        {
            Image Captcha = (Image)LoginCtrl.FindControl("imgCaptcha");

            Captcha.DataBind();
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