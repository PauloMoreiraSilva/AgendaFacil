using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using FluentAssertions;
using PI4Sem.Cache;

namespace PI4Sem.Config
{
    /// <summary>
    /// Faz a leitura do Web.Config / App.Config e retorna os elementos como propriedades do item Key.
    /// </summary>
    public sealed class Config : DynamicObject
    {
        /// <summary>
        /// Chave no cache para armazenar a sessão appSettings do web.config.
        /// </summary>
        private const string sKey = "WebConfig_Key";

        /// <summary>
        /// Chave no cache para armazenar a sessão ConnectionString do web.config.
        /// </summary>
        private const string sConn = "WebConfig_Conn";

        private readonly NameValueCollection _applicationSettings;
        private readonly ConnectionStringSettingsCollection _stringConnections;

        /// <summary>
        /// Exibe a chave do App.Config para a aplicação.
        /// </summary>
        public dynamic Key { get; set; } = new ExpandoObject();

        /// <summary>
        /// Exibe a string de conexão do App.Config para a aplicação.
        /// </summary>
        public dynamic Conn { get; set; } = new ExpandoObject();

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public Config()
        {
            if (CacheAdmin.Exists(sKey))
            {
                Key = CacheAdmin.GetValue(sKey);
            }
            else
            {
                _applicationSettings = (_applicationSettings ?? ConfigurationManager.GetSection("appSettings")) as NameValueCollection;
#if DEBUG
                _ = _applicationSettings.Count.Should().BeGreaterOrEqualTo(5, "Não leu a sessão keys do app/web.config corretamente");
#endif
                PopulateClassKeys();
            }

            if (CacheAdmin.Exists(sConn))
            {
                Conn = CacheAdmin.GetValue(sConn);
            }
            else
            {
                _stringConnections ??= ConfigurationManager.ConnectionStrings;
#if DEBUG
                _ = _stringConnections.Count.Should().BeGreaterOrEqualTo(1, "Não leu a ConnectionString do app/web.config corretamente");
#endif
                PopulateClassConn();
            }
        }

        /// <summary>
        /// Cria dinamicamente os membros da classe Config de acordo com a sessão appSettings do web.config / app.config
        /// </summary>
        private void PopulateClassKeys()
        {
            if (!CacheAdmin.Exists(sKey))
            {
                foreach (string item in _applicationSettings.AllKeys)
                {
                    var expandoDict = Key as IDictionary<string, object>;
                    if (expandoDict.ContainsKey(item))
                        expandoDict[item] = _applicationSettings[item];
                    else
                        expandoDict.Add(item, _applicationSettings[item]);
                }
                CacheAdmin.SetValue(CacheName: sKey, CacheValue: Key);
            }
            else
            {
                Key = CacheAdmin.GetValue(sKey);
            }
        }

        /// <summary>
        /// Cria dinamicamente os membros da classe Config de acordo com a sessão conectionString web.config / app.config
        /// </summary>
        private void PopulateClassConn()
        {
            if (!CacheAdmin.Exists(sConn))
            {
                for (int i = 0; i < _stringConnections.Count; i++)
                {
                    IDictionary<string, object> expandoDict = Conn as IDictionary<string, object>;
                    if (expandoDict.ContainsKey(_stringConnections[i].Name))
                        expandoDict[_stringConnections[i].Name] = _stringConnections[i].ConnectionString;
                    else
                        expandoDict.Add(_stringConnections[i].Name, _stringConnections[i].ConnectionString);
                }
                CacheAdmin.SetValue(CacheName: sConn, CacheValue: Conn);
            }
            else
            {
                Conn = CacheAdmin.GetValue(sConn);
            }
        }
    }
}