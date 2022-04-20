using System;
using System.Diagnostics;
using System.IO;
using System.Web;

namespace PI4Sem.DataBase
{
    /// <summary>
    /// Efetua o Log no arquivo LogErros.
    /// </summary>
    public class EventLog
    {
        /// <summary>
        /// inicializa uma instância da classe.
        /// </summary>
        public EventLog()
        {
        }

        /// <summary>
        /// Loga um evento de erro.
        /// </summary>
        /// <param name="ex">A Exceção.</param>
        /// <param name="Funcao">nome da função.</param>
        /// <param name="sPath">caminho do arquivo.</param>
        /// <param name="linha">linha onde ocorreu.</param>
        public static void LogErrorEvent(Exception ex = null, string Funcao = "", string sPath = "", int linha = 0)
        {
            if (ex == null)
            { return; }

            EventLog oLog = new EventLog();
            oLog.Gravar(ex.Message + " - " +
                        ex.Source +
                        " (" + Funcao +
                        " - " + sPath +
                        " : " + linha.ToString() + ")");
        }

        /// <summary>
        /// Grava o texto no arquivo de Log.
        /// </summary>
        /// <param name="Texto">texto.</param>
        private void Gravar(string Texto)
        {
            try
            {
                Config.Config WebConfig = new Config.Config();

                string sTextoLog = "(" + DateTime.Now.ToString("hh:mm:ss") + ") " + Texto;

                string sPathFisico = (HttpContext.Current != null) ?
                              HttpContext.Current.Server.MapPath(GetAppPath() + WebConfig.Key.PathLogErro) :
                              WebConfig.Key.PathFisicoLogErro;

                if (!Directory.Exists(sPathFisico))
                    Directory.CreateDirectory(sPathFisico);

                if (!sPathFisico.EndsWith("\\"))
                    sPathFisico += "\\";

                string sFileName = "LOG-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                using StreamWriter oArqLog = File.AppendText(sPathFisico + sFileName);
                oArqLog.WriteLine(sTextoLog);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
        }

        /// <summary>
        /// Retorna o path do aplicativo.
        /// </summary>
        /// <returns>path do aplicativo.</returns>
        private string GetAppPath()
        {
            string sPath = HttpContext.Current.Request.ApplicationPath;

            if (!sPath.EndsWith("/"))
                sPath += "/";

            return sPath;
        }
    }
}