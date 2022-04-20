using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados da Cliente
    /// </summary>
    public class AdmCliente : AdmBase
    {
        /// <summary>
        /// Tabela de Clientes no banco de dados
        /// </summary>
        private const string Tabela = "Cliente";

        /// <summary>
        /// Retorna lista de Clientes
        /// </summary>
        public List<Cliente> LstCliente { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmCliente()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Cliente com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Cliente.</returns>
        private Cliente PopulateMC(IDataReader oDR)
        {
            return (Cliente)AdmPopulateMC(oDR: oDR, tipo: "Cliente");
        }

        /// <summary>
        /// Seleciona Clientes baseado em parâmetros passados no objeto Cliente.
        /// </summary>
        /// <param name="oCliente">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Clientes.</returns>
        public List<Cliente> SelectRows(Cliente oCliente = null)
        {
            List<Cliente> oColl = new List<Cliente>();
            if (oCliente == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oCliente, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetCliente", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Cliente oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oCliente.CountAutomatico == 1)
                    {
                        CountRegistro = int.Parse(oDR["Count"].ToString());
                    }
                }

                oDR?.Close();
                oDR?.Dispose();
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oColl;
        }

        public List<Cliente> SelectRows()
        {
            List<Cliente> oColl = new List<Cliente>();

            SQL = "SELECT C.*, " +
                " E.Nome AS NomeEmpresa " +
                " FROM Cliente C " +
                " INNER JOIN Empresa E ON (C.IdEmpresa = E.IdEmpresa)";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Cliente oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                }
                CountRegistro = oColl.Count;
                oDR?.Close();
                oDR?.Dispose();
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oColl;
        }

        /// <summary>
        /// Retorna a quantidade de registros encontrados
        /// </summary>
        /// <param name="oCliente">Objeto cliente</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Cliente oCliente)
        {
            if (oCliente.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oCliente, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetCliente", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                if (oDR?.Read() == true)
                {
                    CountRegistro = int.Parse(oDR["Count"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }
            return CountRegistro;
        }

        public int SelectRowsCount()
        {
            SQL = "SELECT COUNT(IdCliente) AS Count FROM Cliente ";

            if (UsuarioLogado?.IdEmpresa > 0)
            {
                SQL += " WHERE IdEmpresa = " + UsuarioLogado.IdEmpresa;
            }

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                if (oDR?.Read() == true)
                {
                    CountRegistro = int.Parse(oDR["Count"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }
            return CountRegistro;
        }

        /// <summary>
        /// Retorna Cliente por ID
        /// </summary>
        /// <param name="IdCliente">Código.</param>
        /// <returns>objeto Cliente.</returns>
        public Cliente SelectRowByID(int IdCliente = 0)
        {
            if (IdCliente == 0)
                return null;

            Cliente oClienteSel = new Cliente
            {
                IdCliente = IdCliente
            };
            List<Cliente> lstCliente = SelectRows(oClienteSel);
            return lstCliente?.Count > 0 ? lstCliente?[0] : null;
        }

        public Cliente SelectRowByID(string sIdEmpresa, string sIdCliente)
        {
            SQL = "SELECT C.*, " +
                " E.Nome AS NomeEmpresa" +
                " FROM Cliente C " +
                " INNER JOIN Empresa E ON (C.IdEmpresa = E.IdEmpresa)" +
                " WHERE C.IdEmpresa = " + sIdEmpresa +
                " AND C.IdCliente = " + sIdCliente;
            Cliente oMC = null;

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                if (oDR?.Read() == true)
                {
                    oMC = PopulateMC(oDR: oDR);
                }
                oDR?.Close();
                oDR?.Dispose();
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oMC;
        }

        /// <summary>
        /// Insere uma nova Cliente
        /// </summary>
        /// <param name="oCliente">Objeto Cliente.</param>
        /// <returns>Id da Cliente inserida.</returns>
        public int Insert(Cliente oCliente = null)
        {
            if (oCliente == null)
            {
                return 0;
            }
            //if (JaExiste(out int IdCliente, oCliente: oCliente))
            //{
            //    oCliente.IdCliente = IdCliente;
            //    //Update(oCliente);
            //    return oCliente.IdCliente;
            //}

            List<IDbDataParameter> lstParameters = null;//AdmGetParameters(propertyInfos: oCliente, tipoObjeto: Tabela);

            //SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: false);

                SQL = "SELECT IFNULL(MAX(IdCliente),0) + 1 AS Maior FROM Cliente " +
                      " WHERE IdEmpresa = " + oCliente.IdEmpresa;
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                if (oDR?.Read() == true)
                {
                    oCliente.IdCliente = int.Parse(oDR["Maior"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();

                SQL = "INSERT INTO Cliente (IdEmpresa, IdCliente, Nome, Telefone, Email, DataNascimento," +
                    " Endereco, Bairro, Cidade, Uf, Cep, Notas) " +
                      " VALUES (" + oCliente.IdEmpresa + ", " + oCliente.IdCliente + ", " +
                      "'" + oCliente.Nome + "', '" + oCliente.Telefone + "', '" + oCliente.Email + "', " +
                      (oCliente.DataNascimento == DateTime.MinValue ? "null" : "'" + oCliente.DataNascimento.ToString("yyyy-MM-dd") + "'") + ", " +
                      "'"+ oCliente.Endereco + "', '"+ oCliente.Bairro + "', '"+ oCliente.Cidade + "'," +
                      "'" + oCliente.Uf + "', '"+ oCliente.Cep + "', '"+ oCliente.Notas+ "' )";

                int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oCliente.IdCliente;
        }

        /// <summary>
        /// Altera a Cliente
        /// </summary>
        /// <param name="oCliente">objeto Cliente.</param>
        public void Update(Cliente oCliente = null)
        {
            if (oCliente == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oCliente, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdCliente,IdEmpresa");

            try
            {
                AbrirConexao(Transacao: false);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }
        }

        /// <summary>
        /// Exclui uma Cliente
        /// </summary>
        /// <param name="oCliente">objeto Cliente.</param>
        public void Delete(Cliente oCliente = null)
        {
            if (oCliente == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdCliente", value: oCliente.IdCliente)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdCliente");

            try
            {
                AbrirConexao(Transacao: false);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }
        }

        /// <summary>
        /// Verifica se o registro já existe
        /// </summary>
        /// <param name="IdCliente">identificação da Cliente.</param>
        /// <param name="oCliente">objeto Cliente.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdCliente, Cliente oCliente = null)
        {
            IdCliente = 0;
            if (oCliente == null)
            {
                return false;
            }

            Cliente oClienteSel = new Cliente
            {
                Nome = oCliente.Nome,
                Telefone = oCliente.Telefone,
                Email = oCliente.Email
            };

            List<Cliente> lstCliente = SelectRows(oClienteSel);
            if (lstCliente?.Count > 0)
            {
                IdCliente = lstCliente?[0].IdCliente ?? 0;
            }

            return IdCliente > 0;
        }
    }
}