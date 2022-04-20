using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using PI4Sem.Cache;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Dados gerais do aplicativo
    /// </summary>
    public class AppProgram
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        protected AppProgram()
        {
        }

        /// <summary>
        /// Retorna o nome do programa
        /// </summary>
        /// <returns>A string.</returns>
        public static string AppName { get; set; } = "Agenda Fácil";

        /// <summary>
        /// Retorna o path para imagens
        /// </summary>
        /// <returns>path.</returns>
        public static string GetAppImagesPath()
        {
            return GetAppPath() + "Images";
        }

        /// <summary>
        /// Retorna o caminho da aplicação
        /// </summary>
        /// <returns>o path.</returns>
        public static string GetAppPath()
        {
            HttpContext oHttpContext = HttpContext.Current;
            string sPath = oHttpContext.Request.ApplicationPath;

            if (!sPath.EndsWith("/"))
                sPath += "/";

            return sPath;
        }

        /// <summary>
        /// Implementa um diretório MesAno para o path informado.
        /// </summary>
        /// <param name="sPathRaiz">path raiz.</param>
        /// <returns>Novo Path com MesAno.</returns>
        public static string GetPathPorMes(string sPathRaiz)
        {
            if (!Directory.Exists(sPathRaiz))
                Directory.CreateDirectory(sPathRaiz);

            string sReturn = Path.Combine(sPathRaiz, DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0'));

            if (!Directory.Exists(sReturn))
                Directory.CreateDirectory(sReturn);

            return sReturn;
        }

        /// <summary>
        /// Retorna um diretório MesAno para o path informado
        /// </summary>
        /// <param name="sPathRaiz">path raiz.</param>
        /// <returns>Path com MesAno.</returns>
        public static string SetPathPorMes(string sPathRaiz)
        {
            if (!sPathRaiz.EndsWith("\\"))
                sPathRaiz += "\\";

            return sPathRaiz + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// Retorna o path completo do arquivo passado, buscando pelo diretório virtual de remessa
        /// </summary>
        /// <param name="sFileName">nome do arquivo.</param>
        /// <returns>path completo.</returns>
        public static string GetFullPathRemessaValid(string sFileName)
        {
            if (!VerifyFullPathValid("DirVirtualRemessa", sFileName, out string sFullPath))
            {
                _ = VerifyFullPathValid("DirVirtualEnviados", sFileName, out sFullPath);
            }

            return sFullPath;
        }

        /// <summary>
        /// Retorna o path completo do arquivo passado, buscando pelo diretório virtual de retorno
        /// </summary>
        /// <param name="sFileName">nome do arquivo.</param>
        /// <returns>path completo.</returns>
        public static string GetFullPathRetornoValid(string sFileName)
        {
            if (!VerifyFullPathValid("DirVirtualRetorno", sFileName, out string sFullPath))
            {
                _ = VerifyFullPathValid("DirVirtualLidos", sFileName, out sFullPath);
            }

            return sFullPath;
        }

        /// <summary>
        /// Verifica se o path completo informado é válido.
        /// </summary>
        /// <param name="DirVirtual">Diretório virtual.</param>
        /// <param name="sFileName">Nome do arquivo.</param>
        /// <param name="FullPath">Path completo.</param>
        /// <returns>True: path completo válido, false: inválido .</returns>
        private static bool VerifyFullPathValid(string DirVirtual, string sFileName, out string FullPath)
        {
            bool bReturn = false;
            FullPath = "";
            Config.Config oConfig = new Config.Config();

            if (oConfig.Key[DirVirtual] != null)
            {
                HttpContext oHttpContext = HttpContext.Current;
                string sDir = oHttpContext.Server.MapPath(oConfig.Key[DirVirtual]);

                if (!sDir.EndsWith("\\"))
                {
                    sDir += "\\";
                }
                FullPath = sDir + sFileName;
                bReturn = File.Exists(FullPath);
            }

            return bReturn;
        }

        /// <summary>
        /// Retorna o nome da função.
        /// </summary>
        /// <returns>nome da função.</returns>
        public static string GetFunctionName()
        {
            string sName = "";
            HttpContext oHttpContext = HttpContext.Current;
            string sPrograma = oHttpContext.Request.Url.Segments[oHttpContext.Request.Url.Segments.Length - 1];
            string sKey = "FncName_" + sPrograma;

            if (!CacheAdmin.Exists(sKey))
            {
                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("NomePrograma", sPrograma)
                };

                IDbConnection oConn = DBHelper.GetConnection();
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: oConn, sCommandText: "GetFunctionName", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.CloseConnection, lstParameters: lstParameters);

                if (oDR?.Read() == true)
                {
                    sName = oDR["Descricao"].ToString();
                }

                oDR?.Close();
                oDR?.Dispose();

                CacheAdmin.SetValue(CacheName: sKey, CacheValue: sName);
            }
            else
            {
                sName = CacheAdmin.GetValue(sKey).ToString();
            }

            return sName;
        }

        /// <summary>
        /// identifica se o usuário tem permissão para acessar a página de uma função.
        /// </summary>
        /// <returns>True: tem permissão / False: não tem permissão.</returns>
        public static bool GetProgramAccess()
        {
            HttpContext oHttpContext = HttpContext.Current;
            string sPrograma = oHttpContext?.Request?.Url?.Segments?[(oHttpContext?.Request?.Url?.Segments?.Length ?? 0) - 1];

            int iCodigoPerfil = 0;

            if (oHttpContext?.Session["UserLoggedInfo"] != null)
            {
                UserLoggedInfo oUserLoggedInfo = (UserLoggedInfo)oHttpContext?.Session["UserLoggedInfo"];
                iCodigoPerfil =  0;
            }

            Dictionary<string, bool> dicAccess = oHttpContext?.Session["ProgramAccess"] == null
                ? new Dictionary<string, bool>()
                : (Dictionary<string, bool>)oHttpContext.Session["ProgramAccess"];

            bool bAccess;
            if (!dicAccess.ContainsKey(sPrograma))
            {
                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter( parameterName: "NomePrograma", value: sPrograma),
                    DBHelper.GetParameter(parameterName: "CodigoPerfil", value: iCodigoPerfil)
                };

                IDbConnection oConn = DBHelper.GetConnection();
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: oConn, sCommandText: "GetProgramAccess", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.CloseConnection, lstParameters: lstParameters);

                bAccess = oDR?.Read() == true;

                oDR?.Close();
                oDR?.Dispose();

                dicAccess.Add(sPrograma, bAccess);

                if (oHttpContext != null)
                {
                    oHttpContext.Session["ProgramAccess"] = dicAccess;
                }
            }
            else
            {
                bAccess = dicAccess[sPrograma];
            }

            return bAccess;
        }

        /// <summary>
        /// Configura um alerta para a página.
        /// </summary>
        /// <param name="oPage">Página.</param>
        /// <param name="sMessage">texto do alerta.</param>
        /// <param name="sKey">chave para alerta</param>
        public static void SetAlert(Page oPage = null, string sMessage = "", string sKey = "SetAlert")
        {
            string sScript = "alert('" + sMessage + @"');";
            oPage.ClientScript.RegisterStartupScript(oPage.GetType(), sKey, sScript, true);
        }

        /// <summary>
        /// Configura um alerta usando Ajax
        /// </summary>
        /// <param name="oPage">Pagina.</param>
        /// <param name="sMessage">Mensagem de alerta.</param>
        /// <param name="sKey">chave.</param>
        public static void SetAlertAjax(Page oPage = null, string sMessage = "", string sKey = "")
        {
            string sScript = "alert(\"" + sMessage + "\");";
            ScriptManager.RegisterClientScriptBlock(oPage, oPage?.GetType(), sKey, sScript, true);
        }

        /// <summary>
        /// Executa script para abir uma página popup com a página de erro
        /// </summary>
        /// <param name="oPage">página.</param>
        public static void OpenErrorWindow(Page oPage = null)
        {
            string sScript = "openCenterWin('" + GetAppPath() + "Error.aspx', 'Error', 800, 600);";
            oPage.ClientScript.RegisterStartupScript(oPage?.GetType(), "Error", sScript, true);
        }

        /// <summary>
        /// Executa script para abir uma página popup com a página de erro usando Ajax
        /// </summary>
        /// <param name="oPage">Página.</param>
        public static void OpenErrorWindowAjax(Page oPage = null)
        {
            string sScript = "openCenterWin('" + GetAppPath() + "Error.aspx', 'Error', 800, 600);";
            ScriptManager.RegisterClientScriptBlock(oPage, oPage?.GetType(), "Error", sScript, true);
        }

        /// <summary>
        /// Configura a cultura usada pela aplicação
        /// </summary>
        /// <param name="Culture">Cultura.</param>
        public static void SetCulture(string Culture = "pt-BR")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Culture, true);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Culture, true);
        }
    }
}