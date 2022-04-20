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
    public partial class AgendamentoAdd : Page
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
            if (!Page.IsPostBack)
            {
                CarregarComboEmpresa();
                CarregarComboCliente();
                CarregarComboFuncionario();
                CarregarComboProcedimento();
            }
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

        private void CarregarComboCliente()
        {
            AdmCliente admCliente = new AdmCliente();
            List<Model.Cliente> listCliente = admCliente.SelectRows();

            foreach (Model.Cliente cliente in listCliente)
            {
                ListItem li = new ListItem(cliente.Nome, cliente.IdCliente.ToString());
                cboCliente.Items.Add(li);
            }

            if (cboCliente.Items.Count > 0)
            {
                cboCliente.SelectedIndex = 0;
            }
        }

        private void CarregarComboFuncionario()
        {
            AdmFuncionario admFuncionario = new AdmFuncionario();
            List<Model.Funcionario> listFuncionario = admFuncionario.SelectRows();

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

        private void CarregarComboProcedimento()
        {
            AdmProcedimento admProcedimento = new AdmProcedimento();
            List<Model.Procedimento> listProcedimento = admProcedimento.SelectRows();

            foreach (Model.Procedimento procedimento in listProcedimento)
            {
                ListItem li = new ListItem(procedimento.Nome, procedimento.IdProcedimento.ToString());
                cboProcedimento.Items.Add(li);
            }

            if (cboProcedimento.Items.Count > 0)
            {
                cboProcedimento.SelectedIndex = 0;
            }
        }

        protected void BtnCadastro_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                DateTime dtInicio;
                if (!DateTime.TryParse(txtDataInicio.Text, out dtInicio))
                {
                    AppProgram.SetAlert(this, "Data e hora de início em formato inválido. Deve ser dd/mm/yyyy hh:mm.");
                }

                DateTime dtFim;
                if (!DateTime.TryParse(txtDataFim.Text, out dtFim))
                {
                    AppProgram.SetAlert(this, "Data e hora prevista para o final está em formato inválido. Deve ser dd/mm/yyyy hh:mm.");
                }

                Model.Agendamento oAgendamento = new Model.Agendamento
                {
                    IdEmpresa = Convert.ToInt32(cboEmpresa.Value),
                    IdCliente = Convert.ToInt32(cboCliente.Value),
                    IdFuncionario = Convert.ToInt32(cboFuncionario.Value),
                    IdProcedimento = Convert.ToInt32(cboProcedimento.Value),
                    DataInicio = dtInicio,
                    DataFim = dtFim,
                    Situacao = 0
                };

                AdmAgendamento oAdmAgendamento = new AdmAgendamento();
                int i = oAdmAgendamento.Insert(oAgendamento);

                if (i > 0)
                {
                    Response.Redirect("~/Agendamento.aspx");
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
            DateTime dtInicio;
            if (!DateTime.TryParse(txtDataInicio.Text, out dtInicio))
            {
                AppProgram.SetAlert(this, "Data e hora de início em formato inválido. Deve ser dd/mm/yyyy hh:mm.");
                return false;
            }

            DateTime dtFim;
            if (!DateTime.TryParse(txtDataInicio.Text, out dtFim))
            {
                AppProgram.SetAlert(this, "Data e hora prevista para o final está em formato inválido. Deve ser dd/mm/yyyy hh:mm.");
                return false;
            }

            return true;
        }

    }
}