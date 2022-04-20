using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PI4Sem.DataBase
{
    /// <summary>
    /// Responsável pela conexão e operações no SQL Server
    /// </summary>
    public class SqlServerHelper
    {
        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="SqlServerHelper"/>.
        /// </summary>
        protected SqlServerHelper()
        {
        }

        /// <summary>
        /// Retorna uma conexão com SQL Server.
        /// </summary>
        /// <returns>A SqlConnection. Ou nulo, se erro</returns>
        public static SqlConnection GetSqlConnection()
        {
            PI4Sem.Config.Config oConfig = new Config.Config();
            SqlConnection oSqlConnection = new SqlConnection
            {
                ConnectionString = oConfig?.Conn?.SQLServer
            };

            try
            {
                oSqlConnection.Open();
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oSqlConnection = null;
            }

            return oSqlConnection;
        }

        /// <summary>
        /// Executa um comando do tipo Non Query no SQL Server
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

            SqlCommand oSqlCommand = new SqlCommand
            {
                Connection = (SqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oSqlCommand.Transaction = (SqlTransaction)oIDbTransaction;

            if (lstParameters != null)
                foreach (IDbDataParameter oParameter in lstParameters)
                    _ = oSqlCommand.Parameters.Add((SqlParameter)oParameter);

            try
            {
                iRowsAffected = oSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                iRowsAffected = int.MinValue;
            }
            finally
            {
                oSqlCommand?.Dispose();
            }

            return iRowsAffected;
        }

        /// <summary>
        /// Executa um comando do tipo Scalar (que retorna o ID de um registro inserido).
        /// </summary>
        /// <param name="oIDbConnection">Conexão com SQL Server.</param>
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

            SqlCommand oSqlCommand = new SqlCommand
            {
                Connection = (SqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oSqlCommand.Transaction = (SqlTransaction)oIDbTransaction;

            foreach (IDbDataParameter oParameter in lstParameters)
                _ = oSqlCommand.Parameters.Add((SqlParameter)oParameter);

            try
            {
                rowID = Convert.ToInt32(oSqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                rowID = int.MinValue;
            }
            finally
            {
                oSqlCommand.Dispose();
            }

            return rowID;
        }

        /// <summary>
        /// Retorna um objeto Parameter do SQL Server.
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
        /// <returns>A SqlParameter.</returns>
        public static SqlParameter GetSqlParameter(string parameterName = "", object value = null, SqlDbType? dbType = null, int? size = null, ParameterDirection? direction = null, bool? isNullable = null, byte? precision = null, byte? scale = null, string sourceColumn = null, DataRowVersion? sourceVersion = null)
        {
            if (string.IsNullOrEmpty(parameterName))
                return new SqlParameter();

            if (value == null)
                value = DBNull.Value;

            return dbType == null
                ? new SqlParameter(parameterName, value)
                : new SqlParameter(parameterName, (SqlDbType)dbType, (int)size, (ParameterDirection)direction, (bool)isNullable, (byte)precision, (byte)scale, sourceColumn, (DataRowVersion)sourceVersion, value);
        }

        /// <summary>
        /// Executa um comando que retorna um SqlDataReader.
        /// </summary>
        /// <param name="oIDbConnection">conexão com banco de dados.</param>
        /// <param name="sCommandText">texto do comando.</param>
        /// <param name="oCommandType">tipo do comando, se nulo, será texto.</param>
        /// <param name="oCommandBehavior">CommandBehavior, se nulo será default.</param>
        /// <param name="oIDbTransaction">transação.</param>
        /// <param name="lstParameters">lista de parâmetros.</param>
        /// <returns>SqlDataReader ou nulo.</returns>
        public static SqlDataReader GetDataReader(IDbConnection oIDbConnection = null, string sCommandText = "", CommandType? oCommandType = null, System.Data.CommandBehavior? oCommandBehavior = null, IDbTransaction oIDbTransaction = null, List<IDbDataParameter> lstParameters = null)
        {
            SqlDataReader oSqlDataReader = null;

            if (string.IsNullOrEmpty(sCommandText))
                return oSqlDataReader;

            if (oCommandBehavior == null)
                oCommandBehavior = System.Data.CommandBehavior.Default;

            SqlCommand oSqlCommand = new SqlCommand
            {
                Connection = (SqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            if (oIDbTransaction?.Connection != null)
                oSqlCommand.Transaction = (SqlTransaction)oIDbTransaction;

            if (lstParameters != null)
                foreach (IDbDataParameter oParameter in lstParameters)
                    _ = oSqlCommand.Parameters.Add((SqlParameter)oParameter);

            try
            {
                oSqlDataReader = oSqlCommand.ExecuteReader((CommandBehavior)oCommandBehavior);
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oSqlDataReader = null;
            }
            finally
            {
                oSqlCommand.Dispose();
            }

            return oSqlDataReader;
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

            SqlCommand oSqlCommand = new SqlCommand
            {
                Connection = (SqlConnection)oIDbConnection ?? GetSqlConnection(),
                CommandType = oCommandType ?? System.Data.CommandType.Text,
                CommandText = sCommandText
            };

            foreach (IDbDataParameter oParameter in lstParameters)
                oSqlCommand.Parameters.Add((SqlParameter)oParameter);

            SqlDataAdapter oSqlDataAdapter = new SqlDataAdapter
            {
                SelectCommand = oSqlCommand
            };

            try
            {
                oDataTable = new DataTable();
                _ = oSqlDataAdapter.Fill(oDataTable);
            }
            catch (Exception ex)
            {
                EventLog.LogErrorEvent(ex);
                oDataTable = null;
            }
            finally
            {
                oSqlCommand.Dispose();
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

            SqlConnection Connection = GetSqlConnection();
            try
            {
                oDataTable = Connection.GetSchema("Columns", new[] { Connection.Database, null, nomeTabela });
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