using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;
using PI4Sem.Infra;
using System.Net;
using Newtonsoft.Json;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class EmpresaAdd : Page
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

        }

        protected void BtnCadastro_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Empresa oEmpresa = new Model.Empresa
                {
                    Nome = txtNome.Text,
                    NumInscricao = txtInscricao.Text,
                    Endereco = txtEndereco.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Uf = txtUf.Text,
                    Cep = txtCep.Text,
                };

                AdmEmpresa oAdmEmpresa = new AdmEmpresa();
                int i = oAdmEmpresa.Insert(oEmpresa);

                if (i > 0)
                {
                    Response.Redirect("~/Empresa.aspx");
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
            if (Formats.ValidaCNPJ(txtInscricao.Text))
            {
                try
                {
                    var client = new WebClient();
                    var conteudo = client.DownloadString("https://www.receitaws.com.br/v1/cnpj/@cnpj".Replace("@cnpj", txtInscricao.Text));

                    var retorno = JsonConvert.DeserializeObject<WSEmpresa>(conteudo);

                    txtNome.Text = retorno.nome;
                    txtEndereco.Text = retorno.logradouro + ", " + retorno.numero + " - " + retorno.complemento;
                    txtBairro.Text = retorno.bairro;
                    txtCep.Text = Formats.GetNumbers(retorno.cep);
                    txtCidade.Text = retorno.municipio;
                    txtUf.Text = retorno.uf;
                    return;
                }
                catch (Exception ex)
                {
                    AppProgram.SetAlert(this, "Muitas consultas: a versão gratuita da API permite apenas 3 consultas por minuto.");
                    return;
                }
            }
            if (!Formats.ValidaCPF(txtInscricao.Text))
            {
                AppProgram.SetAlert(this, "CPF/CNPJ inválido");
            }
        }
    }
}