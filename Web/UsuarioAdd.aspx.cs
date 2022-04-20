using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;
using PI4Sem.Infra;
using System.Collections.Generic;
using System.Web.UI.WebControls;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class UsuarioAdd : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        
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
            CarregarComboEmpresa();
            CarregarComboFuncionario();
        }

        private void CarregarComboEmpresa()
        {
            AdmEmpresa admEmpresa = new AdmEmpresa();
            List<Model.Empresa> listEmpresa = admEmpresa.SelectRows();

            foreach (Model.Empresa empresa in listEmpresa)
            {
                ListItem li = new ListItem(empresa.Nome, empresa.IdEmpresa.ToString());
                cboEmpresa.Items.Add(li);
            }

            if (cboEmpresa.Items.Count > 0)
            {
                cboEmpresa.SelectedIndex = 0;
            }
        }

        private void CarregarComboFuncionario()
        {
            AdmFuncionario admFuncionario = new AdmFuncionario();
            List<Model.Funcionario> listFuncionario = admFuncionario.SelectRowsSemUsuario();

            foreach (Model.Funcionario funcionario in listFuncionario)
            {
                ListItem li = new ListItem(funcionario.Nome, funcionario.IdFuncionario.ToString());
                cboFuncionario.Items.Add(li);
            }

            if (cboFuncionario.Items.Count > 0)
            {
                cboFuncionario.SelectedIndex = 0;
            }
        }

        protected void BtnCadastro_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {

                Model.Usuario oUsuario = new Model.Usuario
                {
                    IdEmpresa = Convert.ToInt32(cboEmpresa.Value),
                    IdFuncionario = Convert.ToInt32(cboFuncionario.Value),
                    Login = txtUsuario.Text,
                    Senha = txtSenha.Text
                };

                AdmUsuario oAdmUsuario = new AdmUsuario();
                int i = oAdmUsuario.Insert(oUsuario);

                if (i > 0)
                {
                    Response.Redirect("~/Usuario.aspx");
                }
                else
                {
                    AppProgram.SetAlert(this, "Ocorreu um erro. Por favor, tente novamente.");
                }
            }
        }

        private bool ValidarEntrada()
        {
            return ValidarPreenchimento();
        }

        private bool ValidarPreenchimento()
        {
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                AppProgram.SetAlert(this, "O nome do usuário é obrigatório. Operação cancelada.");
                return false;
            }

            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                AppProgram.SetAlert(this, "A senha do usuário é obrigatório. Operação cancelada.");
                return false;
            }

            if (txtSenha.Text != txtConfirmeSenha.Text)
            {
                AppProgram.SetAlert(this, "A senha e a confirmação da senha não conferem. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}