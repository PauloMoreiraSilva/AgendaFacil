using System;
using System.Web.UI;
using PI4Sem.DAL;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página inicial após Login
    /// </summary>
    public partial class Default : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string sQtdEmpresa = string.Empty;
        protected string sQtdCliente = string.Empty;
        protected string sQtdAgendamento = string.Empty;
        protected string sQtdProcedimento = string.Empty;

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
                CarregarTotais();
            }
        }

        private void CarregarTotais()
        {
            sQtdEmpresa = CarregarTotalEmpresa();
            sQtdCliente = CarregarTotalCliente();
            sQtdAgendamento = CarregarTotalAgendamento();
            sQtdProcedimento = CarregarTotalProcedimento();
        }

        private string CarregarTotalEmpresa()
        {
            AdmEmpresa admEmpresa = new AdmEmpresa();
            return admEmpresa.SelectRowsCount().ToString();
        }

        private string CarregarTotalCliente()
        {
            AdmCliente admCliente = new AdmCliente();
            return admCliente.SelectRowsCount().ToString();
        }

        private string CarregarTotalAgendamento()
        {
            AdmAgendamento admAgendamento = new AdmAgendamento();
            return admAgendamento.SelectRowsCount().ToString();
        }

        private string CarregarTotalProcedimento()
        {
            AdmProcedimento admProcedimento = new AdmProcedimento();
            return admProcedimento.SelectRowsCount().ToString();
        }
    }
}