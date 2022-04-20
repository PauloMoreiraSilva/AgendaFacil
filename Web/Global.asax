<%@ Application Language="C#" %>

<script RunAt="server">

    /// <summary>
    /// Evento executado ao iniciar a aplicação
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
    }

    /// <summary>
    /// Evento executado ao finalizar a aplicação
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    /// <summary>
    /// Evento executado quando a aplicação detecta um erro não tratado
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        HttpException oHttpEx = null;
        string sURL = Request.Url.ToString();

        if (ex != null)
        {
            if (ex.GetBaseException() != null)
            {
                ex = ex.GetBaseException();
            }

            if (Context.Handler != null)
            {
                //Pode acontecer de ser null pois o tipo de requisição HTTP atual não está relacionado ao estado da sessão, ou seja, você não consegue acesso a ela dentro do contexto da requisição.
                Session["ObjException"] = ex;
            }
            else
            {
                Context.Items["ObjException"] = ex;
            }
        }

        if (Context.Handler != null)
        {
            Session["URLException"] = sURL;
        }
        else
        {
            Context.Items["URLException"] = sURL;
        }

        //Server.ClearError(); //Não limpa o erro para que seja executado em seguida o customErrors no web.config

        if (ex.GetType() == typeof(HttpException))
        {
            oHttpEx = (HttpException)ex;
        }

        if ((oHttpEx != null) && (oHttpEx.GetHttpCode() == 404))
        {
            //Deixa executar o customErrors
        }
        else
        {
            if (Context.Handler != null)
            {
                Response.Redirect("~/Error.aspx");
            }
            else
            {
                Server.Transfer("~/Error.aspx");
            }
        }
    }

    /// <summary>
    /// Evento executado ao iniciar a sessão do usuário
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    /// <summary>
    /// Evento executado ao finalizar a sessão do usuário
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends.
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer
        // or SQLServer, the event is not raised.

    }
</script>