using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class UsuarioEdit : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string sIdEmpresa = string.Empty;
        protected string sIdFuncionario = string.Empty;
        
        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserLoggedInfo"] != null)
            {
                oUserLoggedInfo = (UserLoggedInfo)Session["UserLoggedInfo"];
            }
            
            sIdEmpresa = Request["IdEmpresa"];
            sIdFuncionario = Request["IdFuncionario"];
            
            if (!Page.IsPostBack)
            {
                CarregarDados();
            }
        }

        private void CarregarDados()
        {
            if (!string.IsNullOrEmpty(sIdEmpresa) && (!string.IsNullOrEmpty(sIdFuncionario)))
            {
                AdmUsuario oAdmUsuario = new AdmUsuario();
                Model.Usuario oUsuario = oAdmUsuario.SelectRowByID(sIdEmpresa, sIdFuncionario);

                lblEmpresa.Text = oUsuario.NomeEmpresa;
                lblFuncionario.Text = oUsuario.NomeFuncionario;
                txtLogin.Text = oUsuario.Login;
            }
        }

        protected void BtnEdicao_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Usuario oUsuario = new Model.Usuario
                {
                    IdEmpresa = Convert.ToInt32(sIdEmpresa),
                    IdFuncionario = Convert.ToInt32(sIdFuncionario),
                    Login = txtLogin.Text
                };

                AdmUsuario oAdmUsuario = new AdmUsuario();
                oAdmUsuario.Update(oUsuario);

                Response.Redirect("~/Usuario.aspx");
            }
        }

        private bool ValidarEntrada()
        {
            return ValidarPreenchimento();
        }

        private bool ValidarPreenchimento()
        {
            if (string.IsNullOrEmpty(txtLogin.Text))
            {
                AppProgram.SetAlert(this, "O login do usuário é obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}