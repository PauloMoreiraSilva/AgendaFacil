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

        protected string sUltimosMeses = string.Empty;
        protected string sDados1 = string.Empty;
        protected string sDados2 = string.Empty;

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
                CarregarGrafico();
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

        private void CarregarGrafico()
        {
            sUltimosMeses = RetornaUltimosMeses();
            sDados1 = RetornaDados1();
            sDados2 = RetornaDados2();
        }

        private string RetornaUltimosMeses()
        {
            return 
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-6).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-5).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-4).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-3).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-2).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.AddMonths(-1).Month) + "', " +
                "'" + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Today.Month) + "'";

        }

        private string RetornaDados1()
        {
            int idProcedimento = 1;

            DateTime dDataIni1 = FirstDayOfMonth(DateTime.Today.AddMonths(-6));
            DateTime dDataFim1 = LastDayOfMonth(dDataIni1);

            DateTime dDataIni2 = FirstDayOfMonth(DateTime.Today.AddMonths(-5));
            DateTime dDataFim2 = LastDayOfMonth(dDataIni2);

            DateTime dDataIni3 = FirstDayOfMonth(DateTime.Today.AddMonths(-4));
            DateTime dDataFim3 = LastDayOfMonth(dDataIni3);

            DateTime dDataIni4 = FirstDayOfMonth(DateTime.Today.AddMonths(-3));
            DateTime dDataFim4 = LastDayOfMonth(dDataIni4);

            DateTime dDataIni5 = FirstDayOfMonth(DateTime.Today.AddMonths(-2));
            DateTime dDataFim5 = LastDayOfMonth(dDataIni5);

            DateTime dDataIni6 = FirstDayOfMonth(DateTime.Today.AddMonths(-1));
            DateTime dDataFim6 = LastDayOfMonth(dDataIni6);

            DateTime dDataIni7 = FirstDayOfMonth(DateTime.Today);
            DateTime dDataFim7 = LastDayOfMonth(dDataIni7);

            return 
                RetornaDados(idProcedimento, dDataIni1, dDataFim1) + ", " +
                RetornaDados(idProcedimento, dDataIni2, dDataFim2) + ", " +
                RetornaDados(idProcedimento, dDataIni3, dDataFim3) + ", " +
                RetornaDados(idProcedimento, dDataIni4, dDataFim4) + ", " +
                RetornaDados(idProcedimento, dDataIni5, dDataFim5) + ", " +
                RetornaDados(idProcedimento, dDataIni6, dDataFim6) + ", " +
                RetornaDados(idProcedimento, dDataIni7, dDataFim7);
        }

        private string RetornaDados2()
        {
            int idProcedimento = 2;

            DateTime dDataIni1 = FirstDayOfMonth(DateTime.Today.AddMonths(-6));
            DateTime dDataFim1 = LastDayOfMonth(dDataIni1);

            DateTime dDataIni2 = FirstDayOfMonth(DateTime.Today.AddMonths(-5));
            DateTime dDataFim2 = LastDayOfMonth(dDataIni2);

            DateTime dDataIni3 = FirstDayOfMonth(DateTime.Today.AddMonths(-4));
            DateTime dDataFim3 = LastDayOfMonth(dDataIni3);

            DateTime dDataIni4 = FirstDayOfMonth(DateTime.Today.AddMonths(-3));
            DateTime dDataFim4 = LastDayOfMonth(dDataIni4);

            DateTime dDataIni5 = FirstDayOfMonth(DateTime.Today.AddMonths(-2));
            DateTime dDataFim5 = LastDayOfMonth(dDataIni5);

            DateTime dDataIni6 = FirstDayOfMonth(DateTime.Today.AddMonths(-1));
            DateTime dDataFim6 = LastDayOfMonth(dDataIni6);

            DateTime dDataIni7 = FirstDayOfMonth(DateTime.Today);
            DateTime dDataFim7 = LastDayOfMonth(dDataIni7);

            return
                RetornaDados(idProcedimento, dDataIni1, dDataFim1) + ", " +
                RetornaDados(idProcedimento, dDataIni2, dDataFim2) + ", " +
                RetornaDados(idProcedimento, dDataIni3, dDataFim3) + ", " +
                RetornaDados(idProcedimento, dDataIni4, dDataFim4) + ", " +
                RetornaDados(idProcedimento, dDataIni5, dDataFim5) + ", " +
                RetornaDados(idProcedimento, dDataIni6, dDataFim6) + ", " +
                RetornaDados(idProcedimento, dDataIni7, dDataFim7);
        }

        private DateTime FirstDayOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        private DateTime LastDayOfMonth(DateTime value)
        {
            return value
                .AddMonths(1)
                .AddMinutes(-1);
        }

        private string RetornaDados(int idProcedimento, DateTime dDataIni, DateTime dDataFim)
        {
            Model.Agendamento oAgendamento = new Model.Agendamento();

            oAgendamento.IdProcedimento = idProcedimento;
            oAgendamento.DataInicio = dDataIni;
            oAgendamento.DataFim = dDataFim;

            AdmAgendamento oAdmAgendamento = new DAL.AdmAgendamento();
            return oAdmAgendamento.SelectRowsCount(oAgendamento).ToString();
        }
    }
}