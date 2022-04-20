using System;
using System.Collections.Generic;
using System.Data;

namespace PI4Sem.DataBase
{
    /// <summary>
    /// Define o tipo do banco de dados
    /// </summary>
    public enum SGBD
    {
        SQLServer = 1,
        Oracle = 2,
        MySQL = 3
    }

    /// <summary>
    /// Helper para operações no banco de dados.
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// Inicializa uma nova instância da classe.
        /// </summary>
        protected DBHelper()
        {
        }

        /// <summary>
        /// Retorna o tipo de banco de dados
        /// </summary>
        /// <returns>SGBD.</returns>
        public static SGBD GetSGBD()
        {
            Config.Config oConfig = new Config.Config();

            return oConfig.Key.SGBD switch
            {
                "SQLServer" => SGBD.SQLServer,
                "Oracle" => SGBD.Oracle,
                "MySQL" => SGBD.MySQL,
                _ => throw new Exception("Atenção: Sistema gerenciador de banco de dados (SGBD) não configurado corretamente."),
            };
        }

        /// <summary>
        /// Implementa a conexão com banco de dados.
        /// </summary>
        /// <returns>IDbConnection.</returns>
        public static IDbConnection GetConnection()
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.GetSqlConnection(),
                SGBD.Oracle => null,
                SGBD.MySQL => MySqlHelper.GetSqlConnection(),
                _ => null,
            };
        }

        /// <summary>
        /// Implementa a execução de um comando do tipo Non Query
        /// </summary>
        /// <param name="oIDbConnection">conexão com banco de dados.</param>
        /// <param name="sCommandText">comando a ser executado</param>
        /// <param name="oCommandType">tipo do comando.</param>
        /// <param name="oIDbTransaction">a transação, ou nulo.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>Int, ou int.MinValue se erro.</returns>
        public static int ExecuteNonQuery(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.ExecuteNonQuery(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                SGBD.Oracle => int.MinValue,
                SGBD.MySQL => MySqlHelper.ExecuteNonQuery(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                _ => int.MinValue,
            };
        }

        /// <summary>
        /// Implementa a execução de comando do tipo Scalar. Retorna o ID de um novo registro
        /// </summary>
        /// <param name="oIDbConnection">conexão com banco de dados.</param>
        /// <param name="sCommandText">texto do comando a ser executado.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="oIDbTransaction">a transação ou nulo.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>inteiro ou int.MinValue se erro .</returns>
        public static int ExecuteScalar(IDbConnection oIDbConnection = null, string sCommandText = "", System.Data.CommandType? oCommandType = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.ExecuteScalar(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                SGBD.Oracle => int.MinValue,
                SGBD.MySQL => MySqlHelper.ExecuteScalar(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                _ => int.MinValue,
            };
        }

        /// <summary>
        /// Retorna um Parameter.
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <returns>IDbDataParameter.</returns>
        public static IDbDataParameter GetParameter(string parameterName = "", object value = null)
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.GetSqlParameter(parameterName: parameterName, value: value),
                SGBD.Oracle => null,
                SGBD.MySQL => MySqlHelper.GetSqlParameter(parameterName: parameterName, value: value),
                _ => null,
            };
        }

        /// <summary>
        /// Implementa a execução de um comando que retorna um DataReader.
        /// </summary>
        /// <param name="oIDbConnection">conexão.</param>
        /// <param name="sCommandText">texto do comando.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="oCommandBehavior">CommandBehavior, se nulo, será default.</param>
        /// <param name="oIDbTransaction">transação.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>IDataReader ou nulo.</returns>
        public static IDataReader GetDataReader(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, CommandBehavior? oCommandBehavior = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.GetDataReader(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oCommandBehavior: oCommandBehavior, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                SGBD.Oracle => null,
                SGBD.MySQL => MySqlHelper.GetDataReader(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, oCommandBehavior: oCommandBehavior, oIDbTransaction: oIDbTransaction, lstParameters: lstParameters),
                _ => null,
            };
        }

        /// <summary>
        /// Implementa a execução de um comando que retorna um DataTable.
        /// </summary>
        /// <param name="oIDbConnection">conexão com banco de dados.</param>
        /// <param name="sCommandText">texto do comando.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="lstParameters">lista de parâmetros</param>
        /// <returns>A DataTable.</returns>
        public static DataTable GetDataTable(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, List<IDbDataParameter> lstParameters = null)
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.GetDataTable(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, lstParameters: lstParameters),
                SGBD.Oracle => null,
                SGBD.MySQL => MySqlHelper.GetDataTable(oIDbConnection: oIDbConnection, sCommandText: sCommandText, oCommandType: oCommandType, lstParameters: lstParameters),
                _ => null,
            };
        }

        /// <summary>
        /// Retorna o schema da tabela informada
        /// </summary>
        /// <param name="nomeTabela">nome da tabela.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetSchema(string nomeTabela = "'")
        {
            return GetSGBD() switch
            {
                SGBD.SQLServer => SqlServerHelper.GetSchema(nomeTabela: nomeTabela),
                SGBD.Oracle => null,
                SGBD.MySQL => MySqlHelper.GetSchema(nomeTabela: nomeTabela),
                _ => null,
            };
        }
    }
}