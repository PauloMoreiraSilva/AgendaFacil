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
    public partial class FuncionarioEdit : Page
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
                AdmFuncionario oAdmFuncionario = new AdmFuncionario();
                Model.Funcionario oFuncionario = oAdmFuncionario.SelectRowByID(sIdEmpresa, sIdFuncionario);

                lblEmpresa.Text = oFuncionario.NomeEmpresa;
                txtNome.Text = oFuncionario.Nome;
                txtTelefone.Text = oFuncionario.Telefone;
                txtEmail.Text = oFuncionario.Email;
                txtCpf.Text = oFuncionario.Cpf;
            }
        }

        protected void BtnEdicao_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Funcionario oFuncionario = new Model.Funcionario
                {
                    IdEmpresa = Convert.ToInt32(sIdEmpresa),
                    IdFuncionario = Convert.ToInt32(sIdFuncionario),
                    Nome = txtNome.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text,
                    Cpf = txtCpf.Text
                };

                AdmFuncionario oAdmFuncionario = new AdmFuncionario();
                oAdmFuncionario.Update(oFuncionario);

                Response.Redirect("~/Funcionario.aspx");
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
                AppProgram.SetAlert(this, "O nome do funcionário é obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}