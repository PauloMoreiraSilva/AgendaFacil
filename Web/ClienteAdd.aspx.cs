using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using PI4Sem.DAL;
using PI4Sem.Infra;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class ClienteAdd : Page
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
                DateTime dt;
                DateTime.TryParse(txtNascimento.Text, out dt);

                Model.Cliente oCliente = new Model.Cliente 
                {
                    IdEmpresa = Convert.ToInt32(cboEmpresa.Value),
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
                int i = oAdmCliente.Insert(oCliente);

                if (i > 0)
                {
                    Response.Redirect("~/Cliente.aspx");
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

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            using (var ws = new WSCorreios.AtendeClienteClient())
            {
                try
                {
                    var resultado = ws.consultaCEP(txtCep.Text);
                    txtEndereco.Text = resultado.end;
                    txtCidade.Text = resultado.cidade;
                    txtBairro.Text = resultado.bairro;
                    txtUf.Text = resultado.uf;
                }
                catch (Exception ex)
                {
                    AppProgram.SetAlert(this, "CEP em formato inválido ou não encontrado.");
                }
            }
        }
    }
}