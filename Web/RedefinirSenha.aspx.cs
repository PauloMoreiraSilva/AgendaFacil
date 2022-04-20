using System;
using System.Web.UI;
using PI4Sem.Business;
using PI4Sem.DAL;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Solicita uma nova senha para o e-mail informado
    /// </summary>
    public partial class RedefinirSenha : System.Web.UI.Page
    {
        public string ImagePath = string.Empty;
        private AdmUsuario oAdmUsuario = null;
        private Config.Config oConfig;

        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">evento.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName;
            ImagePath = AppProgram.GetAppImagesPath();
            oConfig = new Config.Config();

            if (Session["UserLoggedInfo"] != null)
            {
                Response.Redirect("~/");
            }

            if (!Page.IsPostBack)
            {
                lblAmbiente.Text = oConfig.Key.Ambiente;
                lblVersao.Text = AdmGlobal.Versao;

                txtEmail.Focus();
            }

            ctrlGoogleReCaptcha.PublicKey = oConfig.Key.PkPublicaReCaptcha;
            ctrlGoogleReCaptcha.PrivateKey = oConfig.Key.PkPrivadaReCaptcha;
        }

        /// <summary>
        /// Evento executado ao clicar no botão solicitando nova senha
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            ProcessarRequisicao();
        }

        /// <summary>
        /// Evento executado ao clicar no botão cancelar
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MainLogin.aspx");
        }

        /// <summary>
        /// Processa a requisição para receber a nova senha de acesso no e-mail cadastrado
        /// </summary>
        private void ProcessarRequisicao()
        {
            string sUsuario = txtUsuario.Text;
            string sEmail = txtEmail.Text.ToUpper();

            if (!ValidarRequisicao(sUsuario, sEmail))
            {
                return;
            }

            if (!ctrlGoogleReCaptcha.Validate())
            {
                AppProgram.SetAlert(this, "Autenticação pelo ReCaptcha falhou. Tente novamente.");
                return;
            }

            BizAcesso oBizAcesso = new BizAcesso();
            bool bZerado = oBizAcesso.RequisitarSenha(sUsuario: sUsuario, sEmail: sEmail);

            if (bZerado)
            {
                AppProgram.SetAlert(this, "Senha zerada com sucesso! Os dados iniciais de acesso foram enviados novamente a este email.");
                Response.Redirect("~/MainLogin.aspx");
            }
            else
            {
                AppProgram.SetAlert(this, "Os dados inseridos não são válidos. Por favor, verifique se o usuário e e-mail foram digitados corretamente.");
            }
        }

        /// <summary>
        /// Valida a entrada do usuário.
        /// </summary>
        /// <param name="sUsuario">usuario.</param>
        /// <param name="sEmail">email.</param>
        /// <returns>True: válido / False: inválido.</returns>
        private bool ValidarRequisicao(string sUsuario, string sEmail)
        {
            if (string.IsNullOrEmpty(sUsuario) || sUsuario.Length < 4 || string.IsNullOrEmpty(sEmail))
            {
                AppProgram.SetAlert(this, "Os dados inseridos não são válidos. Por favor, verifique se o usuário e e-mail foram digitados corretamente.");
                return false;
            }

            if (oConfig.Key.PortalAdministrativo == "1")
            {
                if (sUsuario.Length > 3 && sUsuario.Substring(0, 3) != "BAN")
                {
                    AppProgram.SetAlert(this, "Os dados inseridos não são válidos. Por favor, verifique se o usuário e e-mail foram digitados corretamente.");
                    return false;
                }
            }
            else
            {
                if (sUsuario.Length > 3 && sUsuario.Substring(0, 3) == "BAN")
                {
                    AppProgram.SetAlert(this, "Os dados inseridos não são válidos. Por favor, verifique se o usuário e e-mail foram digitados corretamente.");
                    return false;
                }
            }
            return true;
        }
    }
}