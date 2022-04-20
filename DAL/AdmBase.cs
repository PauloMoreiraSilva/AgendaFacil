using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using PI4Sem.Cache;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Classe de base para todas as classes do tipo Adm. Possui propriedades e métodos comuns.
    /// Data Access layer - Camada de Acesso aos Dados: Implementa CRUD na base de dados
    /// </summary>
    public abstract class AdmBase
    {
        /// <summary>
        /// Config: efetua a leitura automática do Web.Config e App.Config
        /// </summary>
        public static Config.Config WebConfig { get; set; }

        /// <summary>
        /// Flag que indica se a execução do código é automática ou via demanda (serviço ou web)
        /// </summary>
        public static int FlagAutomatico { get; set; } = int.MinValue;

        /// <summary>
        /// Usuário logado na aplicação
        /// </summary>
        public UserLoggedInfo UsuarioLogado { get; set; }

        /// <summary>
        /// Flag que indica se deve manter a conexão aberta após a execução do comando
        /// </summary>
        public bool ManterConexao { get; set; } = false;

        /// <summary>
        /// Flag que identifica se deve manter a transação ativa após a execução do comando
        /// </summary>
        public bool ManterTransacao { get; set; } = false;

        /// <summary>
        /// Flag que identifica se a execução do serviço possui interface gráfica ou não
        /// </summary>
        public bool PossuiInterface { get; set; } = true;

        /// <summary>
        /// Objeto de conexão com banco de dados
        /// </summary>
        public IDbConnection OConn { get; set; } = null;

        /// <summary>
        /// Transação da conexão com banco de dados
        /// </summary>
        public IDbTransaction OTrans { get; set; } = null;

        /// <summary>
        /// Objeto Data Reader, conjunto de dados vindos do banco de dados
        /// </summary>
        public IDataReader ODR { get; set; } = null;

        /// <summary>
        /// Lista de parâmetros usados no comando ao banco de dados
        /// </summary>
        public List<IDbDataParameter> LstParameters { get; set; } = null;

        /// <summary>
        /// instrução SQL enviada ao banco de dados
        /// </summary>
        public string SQL { get; set; } = string.Empty;

        /// <summary>
        /// Quantidade de registros retornados na consulta
        /// </summary>
        public int CountRegistro { get; set; } = int.MinValue;

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        protected AdmBase()
        {
            //WebConfig ??= new Config.Config();
        }

        /// <summary>
        /// Popula automaticamente um objeto do tipo informado com os dados do Data Reader
        /// Restrição: as propriedades do modelo precisam ter os mesmos nomes dos campos na tabela
        /// </summary>
        /// <param name="oDR">DataReader com dados extraídos da base.</param>
        /// <param name="tipo">nome do modelo do objeto.</param>
        /// <returns>Objeto com dados carregados.</returns>
        public object AdmPopulateMC(IDataReader oDR, string tipo)
        {
            var columns = Enumerable.Range(0, oDR.FieldCount).Select(oDR.GetName).ToList();

            object oMC = SelectedObject(tipo);

            if (oMC != null)
            {
                foreach (PropertyInfo oProperty in oMC.GetType().GetProperties())
                {
                    if (columns.Contains(oProperty.Name) && oDR[oProperty.Name] != DBNull.Value)
                    {
                        if (oProperty.PropertyType.FullName.IndexOf("SYSTEM.INT32", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            oMC.GetType().GetProperty(oProperty.Name).SetValue(oMC, Convert.ToInt32(oDR[oProperty.Name]));
                        }
                        else if (oProperty.PropertyType.FullName.IndexOf("SYSTEM.STRING", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            oMC.GetType().GetProperty(oProperty.Name).SetValue(oMC, oDR[oProperty.Name].ToString().Trim());
                        }
                        else if (oProperty.PropertyType.FullName.IndexOf("SYSTEM.DATETIME", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            oMC.GetType().GetProperty(oProperty.Name).SetValue(oMC, Convert.ToDateTime(oDR[oProperty.Name]));
                        }
                        else if (oProperty.PropertyType.FullName.IndexOf("SYSTEM.DOUBLE", StringComparison.CurrentCultureIgnoreCase) >= 0)
                        {
                            oMC.GetType().GetProperty(oProperty.Name).SetValue(oMC, Convert.ToDouble(oDR[oProperty.Name]));
                        }
                    }
                }
            }
            return oMC;
        }

        /// <summary>
        /// Retorna um objeto do tipo recebido
        /// </summary>
        /// <param name="tipo">nome do modelo.</param>
        /// <returns>objeto do modelo recebido.</returns>
        private object SelectedObject(string tipo)
        {
            switch (tipo)
            {
                case "Agendamento":
                    return new Agendamento();

                case "Cliente":
                    return new Cliente();

                case "ContatoEmpresa":
                    return new ContatoEmpresa();

                case "Empresa":
                    return new Empresa();

                case "Feriado":
                    return new Feriado();

                case "Funcionario":
                    return new Funcionario();

                case "FuncionarioProcedimento":
                    return new FuncionarioProcedimento();

                case "Parametro":
                    return new Parametro();

                case "Procedimento":
                    return new Procedimento();

                case "UserLoggedInfo":
                    return new UserLoggedInfo();

                case "Usuario":
                    return new Usuario();

                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Retorna a lista de parâmetros (precisa ser revisado: novos valores de inicialização / comparar com schema do banco de dados
        /// </summary>
        /// <param name="propertyInfos">objeto com lista a ser convertida em parâmetros.</param>
        /// <returns>Lista de IDbDataParameters.</returns>
        public List<IDbDataParameter> AdmGetParameters(object propertyInfos = null, string tipoObjeto = "")
        {
            string[] colunas = SelectedSchema(tipoObjeto);

            List<IDbDataParameter> listParameters = new List<IDbDataParameter>();

            foreach (PropertyInfo param in propertyInfos?.GetType()?.GetProperties())
            {
                bool campoExiste = Array.Exists(colunas, element => element == param.Name);

                if (!campoExiste)
                    continue;

                if (param.GetValue(propertyInfos, null) == null)
                    continue;

                if (param.GetValue(propertyInfos, null).GetType().FullName.ToUpper().IndexOf("SYSTEM.INT32", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (Convert.ToInt32(param.GetValue(propertyInfos, null)) != int.MinValue)
                    {
                        listParameters.Add(DBHelper.GetParameter(parameterName: param.Name, value: param.GetValue(propertyInfos, null)));
                    }
                }
                else if (param.GetValue(propertyInfos, null).GetType().FullName.ToUpper().IndexOf("SYSTEM.STRING", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (!string.IsNullOrEmpty(param.GetValue(propertyInfos, null).ToString()))
                    {
                        listParameters.Add(DBHelper.GetParameter(parameterName: param.Name, value: param.GetValue(propertyInfos, null).ToString().Trim()));
                    }
                }
                else if (param.GetValue(propertyInfos, null).GetType().FullName.ToUpper().IndexOf("SYSTEM.DATETIME", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    if (Convert.ToDateTime(param.GetValue(propertyInfos, null)) != DateTime.MinValue)
                    {
                        listParameters.Add(DBHelper.GetParameter(parameterName: param.Name, value: param.GetValue(propertyInfos, null)));
                    }
                }
                else if (param.GetValue(propertyInfos, null).GetType().FullName.ToUpper().IndexOf("SYSTEM.DOUBLE", StringComparison.CurrentCultureIgnoreCase) >= 0 && (Convert.ToDouble(param.GetValue(propertyInfos, null)) != double.MinValue))
                {
                    listParameters.Add(DBHelper.GetParameter(parameterName: param.Name, value: param.GetValue(propertyInfos, null)));
                }
            }

            return listParameters;
        }

        /// <summary>
        /// Retorna um objeto contendo o schema da tabela-base informada
        /// </summary>
        /// <param name="tipo">nome do modelo.</param>
        /// <returns>objeto com schema da tabela base.</returns>
        private string[] SelectedSchema(string tipo)
        {
            string chaveSchemaCache = "schema_" + tipo.ToUpper();

            if (CacheAdmin.Exists(chaveSchemaCache))
            {
                return (string[])CacheAdmin.GetValue(chaveSchemaCache);
            }

            DataTable dtSchema = DBHelper.GetSchema(tipo);
            string[] colunas = { "MaxRows", "StartRowIndex", "SortField" };
            List<string> list = new List<string>(colunas.ToList());

            if (dtSchema != null)
            {
                foreach (DataRow campo in dtSchema.Rows)
                {
                    list.Add(campo.ItemArray[3].ToString());
                }

                colunas = list.ToArray();

                if (colunas.Length > 0)
                {
                    CacheAdmin.SetValue(chaveSchemaCache, colunas);
                }
            }

            return colunas;
        }

        /// <summary>
        /// Retorna a data em seu segundo inicial
        /// </summary>
        /// <param name="Data">Data.</param>
        /// <returns>Data com primeiro segundo do dia .</returns>
        public DateTime AdmDataInicial(DateTime Data)
        {
            return Convert.ToDateTime(Data).Date;
        }

        /// <summary>
        /// Adiciona último segundo do dia. usado para between
        /// </summary>
        /// <param name="Data">Data.</param>
        /// <returns>Data com último segundo do dia .</returns>
        public DateTime AdmDataFinal(DateTime Data)
        {
            return Data != DateTime.MinValue ?
                   Convert.ToDateTime(Data).Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(9999) :
                   DateTime.MinValue;
        }

        /// <summary>
        /// Gera automaticamente o comando SQL, conforme o tipo
        /// </summary>
        /// <param name="Tipo">Tipo: insert / update / delete.</param>
        /// <param name="Tabela">tabela.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <param name="Where">cláusula where.</param>
        /// <returns>comando SQL.</returns>
        public string GerarSQL(string Tipo = "", string Tabela = "", List<IDbDataParameter> lstParameters = null, string Where = "", string CampoRetornado = "")
        {
            return Tipo switch
            {
                "INSERT" => GerarSQLInsert(Tabela: Tabela, lstParameters: lstParameters),
                "UPDATE" => GerarSQLUpdate(Tabela: Tabela, lstParameters: lstParameters, Where: Where),
                "DELETE" => GerarSQLDelete(Tabela: Tabela, lstParameters: lstParameters, Where: Where),
                _ => "",
            };
        }

        public int RetornarMax(string Tabela, string campo)
        {
            SQL = "SELECT ISNULL(MAX(" + campo + "),0) AS Count FROM " + Tabela + ";";

            AbrirConexao(Transacao: false);
            IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

            if (oDR?.Read() == true)
            {
                CountRegistro = int.Parse(oDR["Count"].ToString());
            }
            oDR?.Close();
            oDR?.Dispose();

            return CountRegistro;
        }

        /// <summary>
        /// Gera automaticamente o comando SQL para INSERT
        /// </summary>
        /// <param name="Tabela">tabela.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>comando SQL para Insert.</returns>
        private string GerarSQLInsert(string Tabela, List<IDbDataParameter> lstParameters)
        {
            StringBuilder sb = new StringBuilder();

            _ = sb.Append("INSERT INTO ")
                  .Append(Tabela)
                  .Append(" (");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                _ = sb.Append(oParam.ParameterName).Append(", ");
            }

            _ = sb.Remove(sb.Length - 2, 2)
                  .Append(") ")
                  .Append("  ")
                  .Append(" VALUES ")
                  .Append(" (");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                _ = sb.Append('@').Append(oParam.ParameterName).Append(", ");
            }
            _ = sb.Remove(sb.Length - 2, 2)
                  .Append(") ");

            return sb.ToString();
        }

        /// <summary>
        /// Gera automaticamente o comando SQL para Update
        /// </summary>
        /// <param name="Tabela">tabela.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <param name="Where">Cláusula where.</param>
        /// <returns>comando SQL para update.</returns>
        private string GerarSQLUpdate(string Tabela, List<IDbDataParameter> lstParameters, string Where)
        {
            StringBuilder sb = new StringBuilder();
            string[] campos = Where.Split(',');

            _ = sb.Append("UPDATE ")
                  .Append(Tabela)
                  .Append(" SET ");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                if (!campos.Contains(oParam.ParameterName))
                {
                    _ = sb.Append(oParam.ParameterName).Append(" = @")
                          .Append(oParam.ParameterName).Append(", ");
                }
            }
            _ = sb.Remove(sb.Length - 2, 2)
                  .Append(" WHERE ");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                if (campos.Contains(oParam.ParameterName))
                {
                    _ = sb.Append(oParam.ParameterName)
                          .Append(" = @").Append(oParam.ParameterName)
                          .Append(" AND ");
                }
            }

            _ = sb.Remove(sb.Length - 5, 5);

            return sb.ToString();
        }

        /// <summary>
        /// Gera automaticamente o comando SQL para Delete
        /// </summary>
        /// <param name="Tabela">tabela.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <param name="Where">cláusula where.</param>
        /// <returns>comando SQL para Delete.</returns>
        private string GerarSQLDelete(string Tabela, List<IDbDataParameter> lstParameters, string Where)
        {
            StringBuilder sb = new StringBuilder();
            string[] campos = Where.Split(',');

            _ = sb.Append("DELETE FROM ")
                  .Append(Tabela)
                  .Append(" WHERE ");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                if (campos.Contains(oParam.ParameterName))
                {
                    _ = sb.Append(oParam.ParameterName)
                          .Append(" = @")
                          .Append(oParam.ParameterName)
                          .Append(" AND ");
                }
            }

            _ = sb.Remove(sb.Length - 5, 5);

            return sb.ToString();
        }

        /// <summary>
        /// loga um evento de erro
        /// </summary>
        /// <param name="ex">a exceção que ocorreu.</param>
        /// <param name="callingMethod">o método onde ocorreu.</param>
        /// <param name="callingFilePath">o path do arquivo onde ocorreu.</param>
        /// <param name="callingLineNumber">o número da linha no código.</param>
        protected void AdmLogError(Exception ex,
                                [CallerMemberName] string callingMethod = "",
                                [CallerFilePath] string callingFilePath = "",
                                [CallerLineNumber] int callingLineNumber = 0)
        {
            OTrans?.Rollback();

            EventLog.LogErrorEvent(ex: ex, Funcao: callingMethod, sPath: callingFilePath, linha: callingLineNumber);

            ManterConexao = false;
            ManterTransacao = false;
        }

        /// <summary>
        /// Abre uma conexão com o banco de dados
        /// </summary>
        /// <param name="Transacao">If true, inicia uma transação.</param>
        public void AbrirConexao(bool Transacao = false)
        {
            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();

            if (Transacao)
            {
                OTrans = OTrans?.Connection == null ? OConn?.BeginTransaction() : OTrans;
            }
            else
            {
                if (!ManterTransacao)
                {
                    OTrans?.Dispose();
                    OTrans = null;
                }
            }
        }

        /// <summary>
        /// Confirma uma transação
        /// </summary>
        public void CommitTransacao()
        {
            try
            {
                if (OTrans?.Connection != null)
                {
                    OTrans?.Commit();
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                OTrans?.Dispose();
                OTrans = null;
            }
        }

        /// <summary>
        /// Finaliza transação e conexão conforme definido em ManterTransacao e ManterConexao
        /// </summary>
        public void AdmFinnaly()
        {
            if (!ManterTransacao)
            {
                if (OTrans?.Connection != null)
                {
                    OTrans?.Commit();
                }
                OTrans?.Dispose();
                OTrans = null;
            }

            if (!ManterConexao)
            {
                if (OConn?.State == ConnectionState.Open)
                {
                    OConn?.Close();
                }
                OConn?.Dispose();
                OConn = null;
            }
        }
    }
}