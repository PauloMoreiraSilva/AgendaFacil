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
    public partial class FuncionarioAdd : Page
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

        protected void BtnCadastro_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Funcionario oFuncionario = new Model.Funcionario
                {
                    IdEmpresa = Convert.ToInt32(cboEmpresa.Value),
                    Nome = txtNome.Text,
                    Cpf = txtCpf.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text,
                    EhProprietario = Convert.ToInt32(cboProprietario.Value)
                };

                AdmFuncionario oAdmFuncionario = new AdmFuncionario();
                int i = oAdmFuncionario.Insert(oFuncionario);

                if (i > 0)
                {
                    Response.Redirect("~/Funcionario.aspx");
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
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                AppProgram.SetAlert(this, "O nome do cliente é obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}