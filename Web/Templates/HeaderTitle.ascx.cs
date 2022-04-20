using System;
using System.Collections;
using System.Configuration;
using System.Data;
//using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using PI4Sem.Infra;

public partial class Templates_HeaderTitle : System.Web.UI.UserControl
{
    public string ImagePath = "";
    private string _SubTitle = "";
    private string _FullTitle = "";

    public string SubTitle
    {
        get
        {
            return _SubTitle;
        }
        set
        {
            _SubTitle = value;

            _FullTitle = AppProgram.GetFunctionName() + " > " + _SubTitle;
        }
    }
    
    public string FullTitle
    {
        get
        {
            return _FullTitle;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Recupera diretório de imagens

        ImagePath = AppProgram.GetAppImagesPath();

        #endregion

        if (!Page.IsPostBack)
        {
            _FullTitle = AppProgram.GetFunctionName();

            if (!string.IsNullOrEmpty(_SubTitle))
            {
                _FullTitle = _FullTitle + " > " + _SubTitle;
            }

            lblTitle.Text = _FullTitle;
        }

    }
}
