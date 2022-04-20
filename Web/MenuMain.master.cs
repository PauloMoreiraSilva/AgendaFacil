using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PI4Sem.Infra;
using PI4Sem.Model;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Cria o menu para todas as páginas, conforme permissões do usuário
    /// </summary>
    public partial class MenuMain : System.Web.UI.MasterPage
    {
        public string ImagePath = "";
        private UserLoggedInfo oUserLoggedInfo = null;
        private Config.Config oConfig = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (FormsAuthentication.RequireSSL == true)
            {
                if ((Request.IsSecureConnection == false) && (Request.IsLocal == false))
                {
                    string sRedirect = "https://" + Request.ServerVariables["HTTP_HOST"] + HttpContext.Current.Request.RawUrl;

                    Response.Redirect(sRedirect);
                }
            }

            if ((Request.UserAgent.IndexOf("AppleWebKit") > 0) || (Request.UserAgent.IndexOf("Unknown") > 0) || (Request.UserAgent.IndexOf("Chrome") > 0))
            {
                Request.Browser.Adapters.Clear();
            }

            string sNomeCedente = "";

            ImagePath = AppProgram.GetAppImagesPath();

            if (Session["UserLoggedInfo"] != null) //&& (Session["GAuthToken"]) != null && (Request.Cookies["GAuthToken"] != null))
            {
                //if (Session["GAuthToken"].ToString().Equals(Request.Cookies["GAuthToken"].Value))
                //{
                oUserLoggedInfo = (UserLoggedInfo)Session["UserLoggedInfo"];
                //}
                //else
                //{
                //    //Houve manipulação de cookie. Session Fixation.
                //    ConcluirLogOut();
                //    Response.Redirect("~/MainLogin.aspx");
                //}
            }
            else
            {
                ConcluirLogOut();
                Response.Redirect("~/MainLogin.aspx");
            }

            oConfig = new Config.Config();
            //CarregaMenu();

            if (!Page.IsPostBack)
            {
                lblAmbiente.Text = oConfig.Key.Ambiente;
                lblVersao.Text = AdmGlobal.Versao;
                lblNomeUsuario.Text = oUserLoggedInfo.NomeFuncionario;
            }

            AddHead();

            #region Define título da janela

            string sTitle = AppProgram.AppName;

            if (oUserLoggedInfo != null)
            {
                sTitle = "Agenda Fácil > " + oUserLoggedInfo.NomeFuncionario;
            }

            HeadMaster.Title = sTitle;

            #endregion Define título da janela

            //if ((oUserLoggedInfo != null) && (oUserLoggedInfo.TrocaSenha == 1))
            //{
            //    mnuMain.Visible = false;
            //}
        }

        private void CarregaMenu()
        {
            if (Session["MenuXmlSource"] == null)
            {
                PI4Sem.Infra.MenuMain oMenuMain = new PI4Sem.Infra.MenuMain();
                Session["MenuXmlSource"] = oMenuMain.GetXmlSource();
            }

            XmlDataSource xdsMenuSource = new XmlDataSource();

            xdsMenuSource.EnableCaching = false;
            xdsMenuSource.XPath = "Menu/MenuItem";
            xdsMenuSource.Data = Session["MenuXmlSource"].ToString();

            //mnuMain.DataSource = xdsMenuSource;

            MenuItemBinding mibMenuBind = new MenuItemBinding();

            mibMenuBind.ValueField = "Value";
            mibMenuBind.TextField = "Text";
            mibMenuBind.NavigateUrlField = "NavigateUrl";
            mibMenuBind.ImageUrl = "~/Images/bullet.png";

            //mnuMain.DataBindings.Add(mibMenuBind);

            // mnuMain.DataBind();
        }

        protected void btnHome_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void btnHelpInfo_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Help/HelpIndex.aspx");
        }

        protected void LoginStatus1_LoggedOut(Object sender, System.EventArgs e)
        {
            ConcluirLogOut();
        }

        protected void mnuMain_MenuItemDataBound(Object sender, MenuEventArgs e)
        {
            if (e.Item.Text.Contains("<hr/>"))
            {
                string sSeparador = "<hr style=\"color: #a7c0dc; background-color: #a7c0dc; height: 1px;\"/>";

                e.Item.Text = sSeparador;
                e.Item.ImageUrl = "";
                e.Item.Selectable = false;
            }
        }

        private void ConcluirLogOut()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            //Remove cookie para evitar Session Fixation
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-2);
            }

            if (Request.Cookies["__LOGINCOOKIE__"] != null)
            {
                Response.Cookies["__LOGINCOOKIE__"].Value = string.Empty;
                Response.Cookies["__LOGINCOOKIE__"].Expires = DateTime.Now.AddYears(-2);
            }

            if (Request.Cookies["GAuthToken"] != null)
            {
                Response.Cookies["GAuthToken"].Value = string.Empty;
                Response.Cookies["GAuthToken"].Expires = DateTime.Now.AddYears(-2);
            }
        }

        private void AddHead()
        {
            HtmlGenericControl scriptTag = new HtmlGenericControl("script");

            scriptTag.Attributes.Add("type", "text/javascript");
            scriptTag.Attributes.Add("language", "javascript");
            scriptTag.Attributes.Add("src", Page.ResolveUrl("~/Scripts/jquery-3.1.1.min.js"));

            //HeadMaster.Controls.Add(scriptTag);
        }
    }
}