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
    public partial class ProcedimentoEdit : Page
    {
        public UserLoggedInfo oUserLoggedInfo;
        protected string sIdEmpresa = string.Empty;
        protected string sIdProcedimento = string.Empty;

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
            sIdProcedimento = Request["IdProcedimento"];

            if (!Page.IsPostBack)
            {
                CarregarDados();
            }
        }

        private void CarregarDados()
        {
            if (!string.IsNullOrEmpty(sIdEmpresa) && (!string.IsNullOrEmpty(sIdProcedimento)))
            {
                AdmProcedimento oAdmProcedimento = new AdmProcedimento();
                Model.Procedimento oProcedimento = oAdmProcedimento.SelectRowByID(sIdEmpresa, sIdProcedimento);

                lblEmpresa.Text = oProcedimento.NomeEmpresa;
                txtNome.Text = oProcedimento.Nome;
                txtDescricao.Text = oProcedimento.Descricao;
                txtMaterial.Text = oProcedimento.MaterialNecessario;
                txtTempoPrevisto.Text = oProcedimento.TempoPrevisto.ToString();
            }
        }

        protected void BtnEdicao_Click(object sender, EventArgs e)
        {
            if (ValidarEntrada())
            {
                Model.Procedimento oProcedimento = new Model.Procedimento
                {
                    IdEmpresa = Convert.ToInt32(sIdEmpresa),
                    IdProcedimento = Convert.ToInt32(sIdProcedimento),
                    Nome = txtNome.Text,
                    Descricao = txtDescricao.Text,
                    MaterialNecessario = txtMaterial.Text,
                    TempoPrevisto = !string.IsNullOrEmpty(txtTempoPrevisto.Text) ? Convert.ToInt32(txtTempoPrevisto.Text) : int.MinValue,
                    EhIntercalavel = 1,
                    QtdSimultaneo = 1
                };

                AdmProcedimento oAdmProcedimento = new AdmProcedimento();
                oAdmProcedimento.Update(oProcedimento);

                Response.Redirect("~/Procedimento.aspx");
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
                AppProgram.SetAlert(this, "O nome do procedimento é obrigatório. Operação cancelada.");
                return false;
            }

            return true;
        }
    }
}