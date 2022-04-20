using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace PI4Sem.DataBase
{
    /// <summary>
    /// Responsável pela conexão e operações no MySQL
    /// </summary>
    public class MySqlHelper
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="MySqlHelper"/>.
        /// </summary>
        protected MySqlHelper()
        {
        }

        /// <summary>
        /// Retorna uma conexão com MySQL.
        /// </summary>
        /// <returns>A SqlConnection. Ou nulo, se erro</returns>
        public static MySqlConnection GetSqlConnection()
        {
            PI4Sem.Config.Config oConfig = new Config.Config();
            MySqlConnection oMySqlConnection = new MySqlConnection
            {
                ConnectionString = oConfig?.Conn?.MySQL
            };

            try
            {
                oMySqlConnection.Open();
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oMySqlConnection = null;
            }

            return oMySqlConnection;
        }

        /// <summary>
        /// Executa um comando do tipo Non Query no MySQL
        /// </summary>
        /// <param name="oIDbConnection">Conexão aberta com banco de dados.</param>
        /// <param name="sCommandText">texto do comando.</param>
        /// <param name="oCommandType">tipo de comando, ou nulo. se nulo, CommandType.Text.</param>
        /// <param name="oIDbTransaction">Uma transação iniciada, ou nulo.</param>
        /// <param name="lstParameters">Lista de parâmetros, ou nulo.</param>
        /// <returns>Um inteiro. Se int.MinValue = erro.</returns>
        public static int ExecuteNonQuery(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            int iRowsAffected = int.MinValue;

            if (string.IsNullOrEmpty(sCommandText))
                return iRowsAffected;

            MySqlCommand oMySqlCommand = new MySqlCommand
            {
                Connection = (MySqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oMySqlCommand.Transaction = (MySqlTransaction)oIDbTransaction;

            if (lstParameters != null)
            {
                foreach (IDbDataParameter oParameter in lstParameters)
                    _ = oMySqlCommand.Parameters.Add((MySqlParameter)oParameter);
            }

            try
            {
                iRowsAffected = oMySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                iRowsAffected = int.MinValue;
            }
            finally
            {
                oMySqlCommand?.Dispose();
            }

            return iRowsAffected;
        }

        /// <summary>
        /// Executa um comando do tipo Scalar (que retorna o ID de um registro inserido).
        /// </summary>
        /// <param name="oIDbConnection">Conexão com MySQL.</param>
        /// <param name="sCommandText">O comando a ser executado.</param>
        /// <param name="oCommandType">O tipo do comando, se nulo = texto.</param>
        /// <param name="oIDbTransaction">a transação, ou nulo.</param>
        /// <param name="lstParameters">a lista de parâmetros, ou nulo.</param>
        /// <returns>um inteiro ou int.MinValue se erro.</returns>
        public static int ExecuteScalar(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            int rowID = int.MinValue;

            if (string.IsNullOrEmpty(sCommandText))
                return rowID;

            MySqlCommand oMySqlCommand = new MySqlCommand
            {
                Connection = (MySqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oMySqlCommand.Transaction = (MySqlTransaction)oIDbTransaction;

            foreach (IDbDataParameter oParameter in lstParameters)
                _ = oMySqlCommand.Parameters.Add((MySqlParameter)oParameter);

            try
            {
                rowID = Convert.ToInt32(oMySqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                rowID = int.MinValue;
            }
            finally
            {
                oMySqlCommand.Dispose();
            }

            return rowID;
        }

        /// <summary>
        /// Retorna um objeto Parameter do MySQL.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="isNullable"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <returns>A MySqlParameter.</returns>
        public static MySqlParameter GetSqlParameter(string parameterName = "", object value = null, MySqlDbType? dbType = null, int? size = null, ParameterDirection? direction = null, bool? isNullable = null, byte? precision = null, byte? scale = null, string sourceColumn = null, DataRowVersion? sourceVersion = null)
        {
            if (string.IsNullOrEmpty(parameterName))
                return new MySqlParameter();

            if (value == null)
                value = DBNull.Value;

            return dbType == null
                ? new MySqlParameter(parameterName, value)
                : new MySqlParameter(parameterName, (MySqlDbType)dbType, (int)size, (ParameterDirection)direction, (bool)isNullable, (byte)precision, (byte)scale, sourceColumn, (DataRowVersion)sourceVersion, value);
        }

        /// <summary>
        /// Executa um comando que retorna um MySqlDataReader.
        /// </summary>
        /// <param name="oIDbConnection">conexão com banco de dados.</param>
        /// <param name="sCommandText">texto do comando.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="oCommandBehavior">CommandBehavior, se nulo será default.</param>
        /// <param name="oIDbTransaction">transação.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>MySqlDataReader ou nulo.</returns>
        public static MySqlDataReader GetDataReader(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, System.Data.CommandBehavior? oCommandBehavior = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            MySqlDataReader oMySqlDataReader = null;

            if (string.IsNullOrEmpty(sCommandText))
                return oMySqlDataReader;

            if (oCommandBehavior == null)
                oCommandBehavior = System.Data.CommandBehavior.Default;

            MySqlCommand oMySqlCommand = new MySqlCommand
            {
                Connection = (MySqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oMySqlCommand.Transaction = (MySqlTransaction)oIDbTransaction;

            if (lstParameters != null)
            {
                foreach (IDbDataParameter oParameter in lstParameters)
                    _ = oMySqlCommand.Parameters.Add((MySqlParameter)oParameter);
            }

            try
            {
                oMySqlDataReader = oMySqlCommand.ExecuteReader((CommandBehavior)oCommandBehavior);
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oMySqlDataReader = null;
            }
            finally
            {
                oMySqlCommand.Dispose();
            }

            return oMySqlDataReader;
        }

        /// <summary>
        /// Executa um comando que retorna um DataTable.
        /// </summary>
        /// <param name="oIDbConnection">Conexão com banco de dados.</param>
        /// <param name="sCommandText">comando.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetDataTable(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, List<IDbDataParameter> lstParameters = null)
        {
            DataTable oDataTable = null;

            if (string.IsNullOrEmpty(sCommandText))
                return oDataTable;

            MySqlCommand oMySqlCommand = new MySqlCommand
            {
                Connection = (MySqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            foreach (IDbDataParameter oParameter in lstParameters)
                oMySqlCommand.Parameters.Add((MySqlParameter)oParameter);

            MySqlDataAdapter oMySqlDataAdapter = new MySqlDataAdapter
            {
                SelectCommand = oMySqlCommand
            };

            try
            {
                oDataTable = new DataTable();
                _ = oMySqlDataAdapter.Fill(oDataTable);
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oDataTable = null;
            }
            finally
            {
                oMySqlCommand.Dispose();
            }

            return oDataTable;
        }

        /// <summary>
        /// Retorna o schema da tabela informada
        /// </summary>
        /// <param name="nomeTabela">nome da tabela.</param>
        /// <returns>DataTable.</returns>
        public static DataTable GetSchema(string nomeTabela = "")
        {
            DataTable oDataTable = null;

            if (string.IsNullOrEmpty(nomeTabela))
                return oDataTable;

            MySqlConnection Connection = GetSqlConnection();
            try
            {
                oDataTable = Connection.GetSchema("Columns", new[] { null, Connection.Database, nomeTabela });
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oDataTable = null;
            }
            finally
            {
                Connection?.Close();
                Connection?.Dispose();
            }

            return oDataTable;
        }
    }
}