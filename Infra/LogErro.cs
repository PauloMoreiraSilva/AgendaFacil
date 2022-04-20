using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using PI4Sem.Model;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Efetua o log de erros na aplicação.
    /// </summary>
    public static class LogErro
    {
        /// <summary>
        /// Grava o texto no arquivo de Log
        /// </summary>
        /// <param name="Texto">texto.</param>
        public static void Gravar(string Texto)
        {
            PI4Sem.Config.Config oConfig = new Config.Config();

            try
            {
                UserLoggedInfo oUserLoggedInfo = (HttpContext.Current != null) ?
                                                 (UserLoggedInfo)HttpContext.Current.Session["UserLoggedInfo"] :
                                                 null;

                string sPathVirtual = AppProgram.GetAppPath() + oConfig.Key.PathLogErro;
                string sPathFisico = (!string.IsNullOrEmpty(oConfig.Key.PathLogErro) ?
                                      HttpContext.Current.Server.MapPath(sPathVirtual) :
                                      oConfig.Key.PathFisicoLogErro);

                string sTextoLog = "(" + DateTime.Now.ToString("hh:mm:ss") +
                                    (oUserLoggedInfo != null ? (" - " + oUserLoggedInfo?.Login) : "") +
                                    Texto;

                if (!Directory.Exists(sPathFisico))
                    Directory.CreateDirectory(sPathFisico);

                if (!sPathFisico.EndsWith("\\"))
                    sPathFisico += "\\";

                string sFileName = "LOG" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                using StreamWriter oArqLog = File.AppendText(sPathFisico + sFileName);
                oArqLog.WriteLine(sTextoLog);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }
    }
}