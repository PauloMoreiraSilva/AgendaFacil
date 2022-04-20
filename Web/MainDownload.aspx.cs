using System;
using System.IO;
using System.Web.UI;
using PI4Sem.Infra;

/// <summary>
/// Agenda Fácil by PI4Sem
/// </summary>
namespace PI4Sem.AgendaFacil
{
    /// <summary>
    /// Página que efetua download de arquivos
    /// </summary>
    public partial class MainDownload : System.Web.UI.Page
    {
        private string sFullPath = "";
        private string sFileName = "";
        private string sExtension = "";
        private string sContentType = "";

        /// <summary>
        /// Evento executado quando inicia a página
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = AppProgram.AppName + " - Download";

            if (Session["MainDownloadFullPath"] != null)
            {
                sFullPath = Session["MainDownloadFullPath"].ToString();
            }

            if (string.IsNullOrEmpty(sFullPath))
            {
                sFullPath = Request.QueryString["file"];
            }

            try
            {
                if (!File.Exists(sFullPath))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "close", "javascript: window.close();", true);
                    throw new Exception(string.Format("Atenção: O arquivo não foi encontrado no diretório {0}", sFullPath));
                }

                sExtension = System.IO.Path.GetExtension(sFullPath);

                if (Session["MainDownloadFileName"] != null)
                {
                    sFileName = Session["MainDownloadFileName"].ToString();

                    if (sFileName.IndexOf(".") <= 0)
                    {
                        sFileName += ".txt";
                    }
                }
                else
                {
                    sFileName = System.IO.Path.GetFileName(sFullPath);
                }

                sContentType = GetContentType(sExtension);

                Session.Remove("MainDownloadFullPath");

                SetDocResponse();
            }
            catch (Exception ex)
            {
                Session["ObjException"] = ex;
                Session["URLException"] = Request.Url.ToString();

                AppProgram.OpenErrorWindow(this);
            }
        }

        /// <summary>
        /// Retorna o ContentType da extensão informada
        /// </summary>
        /// <param name="Extension">Extensão do arquivo.</param>
        /// <returns>A string.</returns>
        private string GetContentType(string Extension)
        {
            Microsoft.Win32.RegistryKey Reg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Extension);

            return Reg?.GetValue("Content Type") != null ?
                   Reg.GetValue("Content Type").ToString() :
                   "text/plain";
        }

        /// <summary>
        /// Configura o envio do arquivo ao browser.
        /// </summary>
        private void SetDocResponse()
        {
            int chunkSize = (1024 * 100);
            FileStream oFile = File.OpenRead(sFullPath);

            oFile.Position = 0;

            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + sFileName);
            Response.AppendHeader("Content-Length", oFile.Length.ToString());
            Response.ContentType = sContentType != "" ? sContentType : "text/plain";

            while (oFile.Position < oFile.Length)
            {
                if ((oFile.Length - oFile.Position) < chunkSize)
                {
                    chunkSize = (int)(oFile.Length - oFile.Position);
                }

                byte[] chunk = new byte[chunkSize];

                oFile.Read(chunk, 0, chunkSize);
                oFile.Flush();

                Response.BinaryWrite(chunk);
                Response.Flush();
            }

            oFile.Close();

            Response.End();
        }
    }
}