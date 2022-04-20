using System;
using System.Web.UI;
using PI4Sem.Business;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Envia o usuário para o e-mail informado
    /// </summary>
    public partial class EsqueciUsuario : System.Web.UI.Page
    {
        public string ImagePath = string.Empty;
        private Config.Config oConfig;

        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
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
        /// Evento executado ao clicar no botão enviar a senha
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
        /// Processa a requisição para receber o login de acesso no e-mail cadastrado
        /// </summary>
        private void ProcessarRequisicao()
        {
            string sEmail = txtEmail.Text;

            if (string.IsNullOrEmpty(sEmail))
            {
                return;
            }

            if (!ctrlGoogleReCaptcha.Validate())
            {
                AppProgram.SetAlert(this, "Autenticação pelo ReCaptcha falhou. Tente novamente.");
                return;
            }
            BizAcesso oBizAcesso = new BizAcesso();
            bool bEnviado = oBizAcesso.EnviarUsuarioAcesso(sEmail: sEmail);

            if (bEnviado)
            {
                AppProgram.SetAlert(this, "Nomes dos usuários enviados com sucesso!");
                Response.Redirect("~/MainLogin.aspx");
            }
            else
            {
                AppProgram.SetAlert(this, "E-mail não encontrado na base de usuários. Por favor contacte um administrador.");
            }
        }
    }
}