using System;
using System.IO;
using System.Web;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Protege as requisições http do site.
    /// </summary>
    public class FileProtectionHandler : IHttpHandler
    {
        /// <summary>
        /// Processa as requisições
        /// </summary>
        /// <param name="context">contexto.</param>
        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":

                    if (!context.User.Identity.IsAuthenticated)
                    {
                        context.Response.Redirect("~/PageNotFound.aspx");
                        return;
                    }

                    if (context.Request.UrlReferrer != null)
                    {
                        //Se UrlReferrer for null o usuário requisitou direto por URL, se tiver conteúdo veio de link.
                        string requestedFile = context.Server.MapPath(context.Request.FilePath);
                        SendContentTypeAndFile(context, requestedFile);
                    }
                    else
                    {
                        context.Response.Redirect("~/PageNotFound.aspx");
                        return;
                    }

                    break;

                case "POST":
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Indica se a instância Http poe ser reutilizada
        /// </summary>
        bool IHttpHandler.IsReusable => true;

        /// <summary>
        /// Configura o contexto para envio do arquivo e tipo.
        /// </summary>
        /// <param name="context">contexto.</param>
        /// <param name="strFile">path do arquivo.</param>
        /// <returns>HttpContext.</returns>
        private void SendContentTypeAndFile(HttpContext context, String strFile)
        {
            string sExtension = Path.GetExtension(strFile);

            context.Response.ContentType = GetContentType(sExtension);
            context.Response.TransmitFile(strFile);
            context.Response.End();
        }

        /// <summary>
        /// Retorna o tipo do arquivo por extensão
        /// </summary>
        /// <param name="Extension">extensão do arquivo.</param>
        /// <returns>tipo do arquivo.</returns>
        private string GetContentType(string Extension)
        {
            Microsoft.Win32.RegistryKey Reg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Extension);

            string sContentType = (Reg?.GetValue("Content Type") != null) ?
                                    Reg.GetValue("Content Type").ToString() :
                                    "";

            return sContentType;
        }
    }
}