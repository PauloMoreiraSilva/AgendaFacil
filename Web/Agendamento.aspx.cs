using System;
using System.Collections.Generic;
using System.Text;
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
    public partial class Agendamento : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        public string listaAgendamento = "";
        
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

            CarregarAgendamento();
        }

        private void CarregarAgendamento()
        {
            AdmAgendamento oAdmAgendamento = new AdmAgendamento();
            List<Model.Agendamento> listAgendamento = oAdmAgendamento.SelectRows();

            StringBuilder sb = new StringBuilder();

            foreach (Model.Agendamento oAgendamento in listAgendamento)
            {

                sb.Append("{")
                  .Append("title: '" + oAgendamento.NomeCliente + "- " + oAgendamento.NomeProcedimento + " com " + oAgendamento .NomeFuncionario+ "',")
                  .Append("start: new Date("+ oAgendamento.DataInicio.ToString("yyyy") + ", "+ (oAgendamento.DataInicio.Month - 1).ToString() + ", "+ oAgendamento.DataInicio.ToString("dd") + ", "+ oAgendamento.DataInicio.ToString("HH") + ", "+ oAgendamento.DataInicio.ToString("mm") + "),")
                  .Append("end: new Date(" + oAgendamento.DataFim.ToString("yyyy") + ", " + (oAgendamento.DataFim.Month -1).ToString() + ", " + oAgendamento.DataFim.ToString("dd") + ", " + oAgendamento.DataFim.ToString("HH") + ", " + oAgendamento.DataFim.ToString("mm") + "),")
                  .Append("allDay: false,")
                  .Append("backgroundColor: '#00a65a',")
                  .Append("borderColor: '#00a65a'")
                  .Append("}, ");
            }
            //sb.Remove(sb.Length - 2, 2);
            listaAgendamento = sb.ToString();
        }
    }
}