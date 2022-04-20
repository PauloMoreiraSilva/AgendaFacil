using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Funcionario
    /// </summary>
    public class AdmFuncionario : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário no banco de dados
        /// </summary>
        private const string Tabela = "Funcionario";

        /// <summary>
        /// Retorna lista de Funcionários
        /// </summary>
        public List<Funcionario> LstFuncionario { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmFuncionario()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Funcionario com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Funcionario.</returns>
        private Funcionario PopulateMC(IDataReader oDR)
        {
            return (Funcionario)AdmPopulateMC(oDR: oDR, tipo: "Funcionario");
        }

        /// <summary>
        /// Seleciona Funcionários baseado em parâmetros passados no objeto Funcionario.
        /// </summary>
        /// <param name="oFuncionario">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<Funcionario> SelectRows(Funcionario oFuncionario)
        {
            List<Funcionario> oColl = new List<Funcionario>();
            if (oFuncionario == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionario, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFuncionario", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Funcionario oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oFuncionario.CountAutomatico == 1)
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

        public List<Funcionario> SelectRows()
        {
            List<Funcionario> oColl = new List<Funcionario>();

            SQL = "SELECT F.*," +
                " E.Nome AS NomeEmpresa " +
                " FROM Funcionario F " +
                " INNER JOIN Empresa E ON (F.IdEmpresa = E.IdEmpresa)";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Funcionario oMC = PopulateMC(oDR: oDR);
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

        public List<Funcionario> SelectRowsSemUsuario()
        {
            List<Funcionario> oColl = new List<Funcionario>();

            SQL = "SELECT F.*," +
                " E.Nome AS NomeEmpresa, " +
                " U.Login " +
                " FROM Funcionario F " +
                " INNER JOIN Empresa E ON (F.IdEmpresa = E.IdEmpresa)" +
                " LEFT JOIN Usuario U ON (F.IdEmpresa = U.IdEmpresa AND F.IdFuncionario = U.IdFuncionario)" +
                " WHERE U.Login IS NULL";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Funcionario oMC = PopulateMC(oDR: oDR);
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
        /// <param name="oFuncionario">Objeto Funcionario</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Funcionario oFuncionario)
        {
            if (oFuncionario.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionario, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFuncionario", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna Funcionario por ID
        /// </summary>
        /// <param name="IdFuncionario">Código.</param>
        /// <returns>objeto Funcionario.</returns>
        public Funcionario SelectRowByID(int IdFuncionario = 0)
        {
            if (IdFuncionario == 0)
                return null;

            Funcionario oFuncionarioSel = new Funcionario
            {
                IdFuncionario = IdFuncionario
            };
            List<Funcionario> lstFuncionario = SelectRows(oFuncionarioSel);
            return lstFuncionario?.Count > 0 ? lstFuncionario?[0] : null;
        }

        public Funcionario SelectRowByID(string sIdEmpresa, string sIdFuncionario)
        {
            SQL = "SELECT F.*, " +
                " E.Nome AS NomeEmpresa" +
                " FROM Funcionario F " +
                " INNER JOIN Empresa E ON (F.IdEmpresa = E.IdEmpresa)" +
                " WHERE F.IdEmpresa = " + sIdEmpresa +
                " AND F.IdFuncionario = " + sIdFuncionario;
            Funcionario oMC = null;

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
        /// Insere um novo Funcionario
        /// </summary>
        /// <param name="oFuncionario">Objeto Funcionario.</param>
        /// <returns>Id do Funcionario inserido.</returns>
        public int Insert(Funcionario oFuncionario = null)
        {
            if (oFuncionario == null)
            {
                return 0;
            }
            //if (JaExiste(out int IdFuncionario, oFuncionario: oFuncionario))
            //{
            //    oFuncionario.IdFuncionario = IdFuncionario;
            //    //Update(oFuncionario);
            //    return oFuncionario.IdFuncionario;
            //}

            List<IDbDataParameter> lstParameters = null; //AdmGetParameters(propertyInfos: oFuncionario, tipoObjeto: Tabela);

            try
            {
                AbrirConexao(Transacao: false);

                SQL = "SELECT IFNULL(MAX(IdFuncionario),0) + 1 AS Maior FROM Funcionario " +
                      " WHERE IdEmpresa = " + oFuncionario.IdEmpresa;
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                if (oDR?.Read() == true)
                {
                    oFuncionario.IdFuncionario = int.Parse(oDR["Maior"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();

                SQL = "INSERT INTO Funcionario (IdEmpresa, IdFuncionario, Nome, Email, EhProprietario) " +
                      " VALUES ("+oFuncionario.IdEmpresa+", "+oFuncionario.IdFuncionario+", " +
                      "'"+oFuncionario.Nome+"', '"+oFuncionario.Email+"', "+oFuncionario.EhProprietario+")";

                int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                InserirProcedimento(oFuncionario);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oFuncionario.IdFuncionario;
        }

        private void InserirProcedimento(Funcionario oFuncionario)
        {
            AdmProcedimento oAdmProcedimento = new AdmProcedimento();
            List<Procedimento> listProcedimento = oAdmProcedimento.SelectRows();

            StringBuilder sb = new StringBuilder();
            _ = sb.Append( "INSERT INTO FuncionarioProcedimento (IdEmpresa, IdFuncionario, IdProcedimento ) Values ");

            foreach (Procedimento procedimento in listProcedimento)
            {
                _ = sb.Append(" (")
                      .Append(oFuncionario.IdEmpresa)
                      .Append(", ")
                      .Append(oFuncionario.IdFuncionario)
                      .Append(", ")
                      .Append(procedimento.IdProcedimento)
                      .Append("), ");
            }

            _ = sb.Remove(sb.Length - 2, 2)
                  .Append("; ");

            SQL = sb.ToString();

            AbrirConexao(Transacao: false);
            int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: null);

        }

        /// <summary>
        /// Altera o Funcionario
        /// </summary>
        /// <param name="oFuncionario">objeto Funcionario.</param>
        public void Update(Funcionario oFuncionario = null)
        {
            if (oFuncionario == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionario, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFuncionario");

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
        /// Exclui um Funcionario
        /// </summary>
        /// <param name="oFuncionario">objeto Funcionario.</param>
        public void Delete(Funcionario oFuncionario = null)
        {
            if (oFuncionario == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdFuncionario", value: oFuncionario.IdFuncionario)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFuncionario");

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
        /// <param name="IdFuncionario">identificação do Funcionario.</param>
        /// <param name="oFuncionario">objeto Funcionario.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdFuncionario, Funcionario oFuncionario = null)
        {
            IdFuncionario = 0;
            if (oFuncionario == null)
            {
                return false;
            }

            Funcionario oFuncionarioSel = new Funcionario
            {
                Nome = oFuncionario.Nome,
                Cpf = oFuncionario.Cpf,
                Telefone = oFuncionario.Telefone,
                Email = oFuncionario.Email
            };

            List<Funcionario> lstFuncionario = SelectRows(oFuncionarioSel);
            if (lstFuncionario?.Count > 0)
            {
                IdFuncionario = lstFuncionario?[0].IdFuncionario ?? 0;
            }

            return IdFuncionario > 0;
        }
    }
}