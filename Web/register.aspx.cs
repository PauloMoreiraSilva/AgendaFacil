using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Newtonsoft.Json;
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
    public partial class Register : System.Web.UI.Page
    {
        /// <summary>
        /// Diretório de imagens do portal
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

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
                ValidarUrl();
            }

            imgCaptcha.ImageUrl = "Captcha.aspx";
            imgCaptcha.DataBind();
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

        /// <summary>
        /// Evento do click do botão de refresh: retorna outra imagem do captcha.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void BntRefresh_Click(object sender, EventArgs e)
        {
            imgCaptcha.DataBind();
        }

        /// <summary>
        /// Btns the cadastro_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void BtnCadastro_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                var Cadastro = new
                {
                    Razao_Social = txtRazaoSocial.Text,
                    Inscricao = txtInscricao.Text,
                    Contato = txtNome.Text,
                    Email = txtEmail.Text,
                    Usuario = txtUsuario.Text,
                    Senha = txtSenha.Text
                };

                BizAcesso oBizAcesso = new BizAcesso();
                if (oBizAcesso.Registrar(Cadastro))
                {
                    UserLoggedInfo oUserLoggedInfo = oBizAcesso.Autenticar(Cadastro.Usuario);
                    Session["UserLoggedInfo"] = oUserLoggedInfo;

                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        /// <summary>
        /// Valida a entrada do usuário
        /// </summary>
        /// <returns>A bool.</returns>
        private bool ValidarEntrada()
        {
            return ValidarPreenchimento();
        }

        /// <summary>
        /// Valida se os campos obrigatórios foram preenchidos corretamente
        /// </summary>
        /// <returns>A bool.</returns>
        private bool ValidarPreenchimento()
        {
            if (string.IsNullOrEmpty(txtRazaoSocial.Text))
            {
                AppProgram.SetAlert(this, "A Razão Social é obrigatória. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtInscricao.Text))
            {
                AppProgram.SetAlert(this, "O CNPJ/CPF é um campo obrigatório. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                AppProgram.SetAlert(this, "O Nome do responsável é um campo obrigatório. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                AppProgram.SetAlert(this, "O E-mail é um campo obrigatório. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                AppProgram.SetAlert(this, "O Nome de Usuário é um campo obrigatório. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                AppProgram.SetAlert(this, "A Senha é um campo obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            if (Formats.ValidaCNPJ(txtInscricao.Text))
            {
                try
                {
                    var client = new WebClient();
                    var conteudo = client.DownloadString("https://www.receitaws.com.br/v1/cnpj/@cnpj".Replace("@cnpj", txtInscricao.Text));

                    var retorno = JsonConvert.DeserializeObject<WSEmpresa>(conteudo);

                    txtRazaoSocial.Text = retorno.nome;
                    return;
                }
                catch (Exception ex)
                {
                    AppProgram.SetAlert(this, "Muitas consultas: a versão gratuita da API permite apenas 3 consultas por minuto.");
                    return;
                }
            }
            else
            {
                AppProgram.SetAlert(this, "CNPJ inválido");
            }
        }
    }
}