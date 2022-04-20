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
    public partial class ClienteEdit : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string sIdEmpresa = string.Empty;
        protected string sIdCliente = string.Empty;

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
            sIdCliente = Request["IdCliente"];

            if (!Page.IsPostBack)
            {
                CarregarDados();
            }

        }

        private void CarregarDados()
        {
            if (!string.IsNullOrEmpty(sIdEmpresa) && (!string.IsNullOrEmpty(sIdCliente)))
            {
                AdmCliente oAdmCliente = new AdmCliente();
                Model.Cliente oCliente = oAdmCliente.SelectRowByID(sIdEmpresa, sIdCliente);

                DateTime dt;
                string data = "";
                if (DateTime.TryParse(oCliente.DataNascimento.ToString(), out dt))
                {
                    if (dt != DateTime.MinValue)
                    {
                        data = dt.ToString("dd/MM/yyyy");
                    }
                }

                lblEmpresa.Text = oCliente.NomeEmpresa;
                txtNome.Text = oCliente.Nome;
                txtTelefone.Text = oCliente.Telefone;
                txtEmail.Text = oCliente.Email;
                txtNascimento.Text = data;
                txtEndereco.Text = oCliente.Endereco;
                txtBairro.Text = oCliente.Bairro;
                txtCidade.Text = oCliente.Cidade;
                txtUf.Text = oCliente.Uf;
                txtCep.Text = oCliente.Cep;
                txtObs.Text = oCliente.Notas;
            }
        }

        protected void BtnEdicao_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                DateTime dt;
                DateTime.TryParse(txtNascimento.Text, out dt);

                Model.Cliente oCliente = new Model.Cliente
                {
                    IdEmpresa = Convert.ToInt32(sIdEmpresa),
                    IdCliente = Convert.ToInt32(sIdCliente),
                    Nome = txtNome.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text,
                    Endereco = txtEndereco.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Uf = txtUf.Text,
                    Cep = txtCep.Text,
                    Notas = txtObs.Text,
                    DataNascimento = dt
                };

                AdmCliente oAdmCliente = new AdmCliente();
                oAdmCliente.Update(oCliente);

                Response.Redirect("~/Cliente.aspx");
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