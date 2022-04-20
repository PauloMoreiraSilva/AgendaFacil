using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Base para todas as páginas do site. Variáveis e funções comuns
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// Tamanho máximo da requisição (50MB).
        /// </summary>
        public int MaxRequestLength { get; set; } = 51200000; //bytes (50 MB)

        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public BasePage() : base()
        {
        }

        /// <summary>
        /// OnLoad base para todas as páginas.
        /// </summary>
        /// <param name="e">evento</param>
        protected override void OnLoad(EventArgs e)
        {
            AppProgram.SetCulture("pt-BR");

            HttpContext oHttpContext = HttpContext.Current;

            if (oHttpContext?.Session != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (oHttpContext?.Session?.IsNewSession == true)
                {
                    string cookie = Request.Headers["Cookie"];
                    if (cookie?.IndexOf("ASP.NET_SessionId") >= 0)
                    {
                        //uma vez que é uma nova sessão mas existe um cookie ASP.Net, sabemos que a sessão expirou, então redirecionamos
                        Response.Redirect("~/MainLogin.aspx?ReturnUrl=Default.aspx");
                    }
                }
                else if (oHttpContext?.Session["UserLoggedInfo"] != null)
                {
                    bool bAccess = AppProgram.GetProgramAccess();

                    if (!bAccess)
                    {
                        string sFunction = AppProgram.GetFunctionName();
                        string sPage = "~/AccessDenied.aspx";

                        oHttpContext.Session["FunctionAccessDenied"] = sFunction;

                        if (oHttpContext.Session["FunctionAccessDeniedPopUp"]?.ToString() == "1")
                        {
                            oHttpContext.Session["FunctionAccessDeniedPopUp"] = "";
                            sPage = "~/AccessDeniedPopUp.aspx";
                        }

                        Response.Redirect(sPage);
                    }
                }
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Busca no controle informado, um controle com o ID.
        /// </summary>
        /// <param name="root">O Controle inicial</param>
        /// <param name="id">o ID a ser procurado</param>
        /// <returns>Controle com ID, ou nulo.</returns>
        protected Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Ajuste para incluir Compatibilidade com IE através de MetaTag
        /// </summary>
        protected void WorkaroundIEedge()
        {
            foreach (Control oCtrl in Page.Header.Controls)
            {
                if (oCtrl is HtmlMeta htmlMeta)
                {
                    HtmlMeta oMeta = htmlMeta;

                    if (oMeta.HttpEquiv == "X-UA-Compatible")
                    {
                        oMeta.Content = "IE=edge";
                    }
                }
            }
        }
    }
}
