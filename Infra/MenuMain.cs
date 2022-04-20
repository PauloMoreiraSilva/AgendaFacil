using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Responsável por montar o menu dinâmico
    /// </summary>
    public class MenuMain
    {
        private IDbConnection oConn;
        private XmlTextWriter oXml;
        private int iCodigoUsuario;

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public MenuMain()
        {
        }

        /// <summary>
        /// Retorna XML com menu dinâmico
        /// </summary>
        /// <returns>menu.</returns>
        public string GetXmlSource()
        {
            HttpContext oHttpContext = HttpContext.Current;
            UserLoggedInfo oUserLoggedInfo = (UserLoggedInfo)oHttpContext?.Session["UserLoggedInfo"];
            iCodigoUsuario = oUserLoggedInfo?.IdFuncionario ?? int.MinValue;

            string sXML = "";
            using (System.IO.MemoryStream oStream = new System.IO.MemoryStream())
            {
                oXml = new XmlTextWriter(oStream, System.Text.Encoding.UTF8)
                {
                    Formatting = Formatting.Indented
                };
                oXml?.WriteStartDocument();
                oXml?.WriteStartElement("Menu");

                CarregarItensPai();

                oXml?.WriteEndElement();
                oXml?.WriteEndDocument();
                oXml?.Flush();

                oStream.Position = 0;

                using System.IO.StreamReader oReader = new System.IO.StreamReader(oStream);
                sXML = oReader?.ReadToEnd();
                oXml?.Close();
            }

            return sXML;
        }

        /// <summary>
        /// Retorna os itens principais do Menu
        /// </summary>
        private void CarregarItensPai()
        {
            oConn = DBHelper.GetConnection();

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter( parameterName: "CodigoUsuario", value: iCodigoUsuario),
                DBHelper.GetParameter( parameterName: "CodigoPai", value: int.Parse("0"))
            };

            DataTable oDT = DBHelper.GetDataTable(oIDbConnection: oConn, sCommandText: "GetMenuItens", oCommandType: CommandType.StoredProcedure, lstParameters: lstParameters);

            foreach (DataRow oDR in oDT?.Rows)
            {
                //CodigoFuncao = null para itens agrupadores que não chamam função
                //Se for somente um agrupador sem itens permitidos, então não mostra o agrupador vazio
                if (!((oDR["CodigoFuncao"] == DBNull.Value) && (int.Parse(oDR["QtdeFilhos"].ToString()) == 0)))
                {
                    oXml?.WriteStartElement("MenuItem");

                    oXml?.WriteAttributeString("Value", oDR["Codigo"].ToString());
                    oXml?.WriteAttributeString("Text", " " + oDR["Descricao"].ToString());
                    oXml?.WriteAttributeString("NavigateUrl", oDR["URL"].ToString());

                    CarregarItensFilhos(int.Parse(oDR["Codigo"].ToString()));

                    oXml?.WriteEndElement();
                }
            }

            oDT?.Dispose();

            if (oConn?.State == ConnectionState.Open)
            {
                oConn?.Close();
                oConn?.Dispose();
            }
        }

        /// <summary>
        /// Retorna os subitens de cada item do menu
        /// </summary>
        /// <param name="iCodigoPai">código pai.</param>
        private void CarregarItensFilhos(int iCodigoPai)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter( parameterName: "CodigoUsuario", value: iCodigoUsuario),
                DBHelper.GetParameter( parameterName: "CodigoPai", value: iCodigoPai)
            };

            DataTable oDT = DBHelper.GetDataTable(oIDbConnection: oConn, sCommandText: "GetMenuItens", oCommandType: CommandType.StoredProcedure, lstParameters: lstParameters);

            foreach (DataRow oDR in oDT?.Rows)
            {
                //CodigoFuncao = null para itens agrupadores que não chamam função
                //Se for somente um agrupador sem itens permitidos, então não mostra o agrupador vazio
                if (!((oDR["CodigoFuncao"] == DBNull.Value) && (int.Parse(oDR["QtdeFilhos"].ToString()) == 0)))
                {
                    oXml?.WriteStartElement("MenuItem");
                    oXml?.WriteAttributeString("Value", oDR["Codigo"].ToString());
                    oXml?.WriteAttributeString("Text", " " + oDR["Descricao"].ToString());
                    oXml?.WriteAttributeString("NavigateUrl", oDR["URL"].ToString());

                    CarregarItensFilhos(int.Parse(oDR["Codigo"].ToString()));

                    oXml?.WriteEndElement();
                }
            }

            oDT?.Dispose();
        }
    }
}