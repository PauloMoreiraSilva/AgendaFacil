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
    public partial class EmpresaEdit : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string sIdEmpresa = string.Empty;

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

            if (!Page.IsPostBack)
            {
                CarregarDados();
            }
        }

        private void CarregarDados()
        {
            if (!string.IsNullOrEmpty(sIdEmpresa))
            {
                AdmEmpresa oAdmEmpresa = new AdmEmpresa();
                Model.Empresa oEmpresa = oAdmEmpresa.SelectRowByID(sIdEmpresa);

                txtNome.Text = oEmpresa.Nome;
                txtInscricao.Text = oEmpresa.Inscricao;
                txtEndereco.Text = oEmpresa.Endereco;
                txtBairro.Text = oEmpresa.Bairro;
                txtCidade.Text = oEmpresa.Cidade;
                txtUf.Text = oEmpresa.Uf;
                txtCep.Text = oEmpresa.Cep;
            }
        }

        protected void BtnEdicao_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Empresa oEmpresa = new Model.Empresa
                {
                    IdEmpresa = Convert.ToInt32(sIdEmpresa),
                    Nome = txtNome.Text,
                    Inscricao = txtInscricao.Text,
                    Endereco = txtEndereco.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Uf = txtUf.Text,
                    Cep = txtCep.Text
                };

                AdmEmpresa oAdmEmpresa = new AdmEmpresa();
                oAdmEmpresa.Update(oEmpresa);

                Response.Redirect("~/Empresa.aspx");
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
                AppProgram.SetAlert(this, "O nome da empresa é obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}