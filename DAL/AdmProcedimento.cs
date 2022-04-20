using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Procedimento
    /// </summary>
    public class AdmProcedimento : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário no banco de dados
        /// </summary>
        private const string Tabela = "Procedimento";

        /// <summary>
        /// Retorna lista de Funcionários
        /// </summary>
        public List<Procedimento> LstProcedimento { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmProcedimento()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Procedimento com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Procedimento.</returns>
        private Procedimento PopulateMC(IDataReader oDR)
        {
            return (Procedimento)AdmPopulateMC(oDR: oDR, tipo: "Procedimento");
        }

        /// <summary>
        /// Seleciona Funcionários baseado em parâmetros passados no objeto Procedimento.
        /// </summary>
        /// <param name="oProcedimento">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<Procedimento> SelectRows(Procedimento oProcedimento)
        {
            List<Procedimento> oColl = new List<Procedimento>();
            if (oProcedimento == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oProcedimento, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetProcedimento", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Procedimento oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oProcedimento.CountAutomatico == 1)
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

        public List<Procedimento> SelectRows()
        {
            List<Procedimento> oColl = new List<Procedimento>();

            SQL = "SELECT P.*," +
                " E.Nome AS NomeEmpresa  " +
                "FROM Procedimento P " +
                "INNER JOIN Empresa E ON (P.IdEmpresa = E.IdEmpresa)";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Procedimento oMC = PopulateMC(oDR: oDR);
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
        /// <param name="oProcedimento">Objeto Procedimento</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Procedimento oProcedimento)
        {
            if (oProcedimento.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oProcedimento, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetProcedimento", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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

            SQL = "SELECT COUNT(IdProcedimento) AS Count FROM Procedimento ";

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
        /// Retorna Procedimento por ID
        /// </summary>
        /// <param name="IdProcedimento">Código.</param>
        /// <returns>objeto Procedimento.</returns>
        public Procedimento SelectRowByID(int IdProcedimento = 0)
        {
            if (IdProcedimento == 0)
                return null;

            Procedimento oProcedimentoSel = new Procedimento
            {
                IdProcedimento = IdProcedimento
            };
            List<Procedimento> lstProcedimento = SelectRows(oProcedimentoSel);
            return lstProcedimento?.Count > 0 ? lstProcedimento?[0] : null;
        }

        public Procedimento SelectRowByID(string sIdEmpresa, string sIdProcedimento)
        {
            SQL = "SELECT P.*, " +
                " E.Nome AS NomeEmpresa " +
                " FROM Procedimento P " +
                " INNER JOIN Empresa E ON (P.IdEmpresa = E.IdEmpresa)" +
                " WHERE P.IdEmpresa = " + sIdEmpresa +
                " AND P.IdProcedimento = " + sIdProcedimento;
            Procedimento oMC = null;

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
        /// Insere um novo Procedimento
        /// </summary>
        /// <param name="oProcedimento">Objeto Procedimento.</param>
        /// <returns>Id do Procedimento inserido.</returns>
        public int Insert(Procedimento oProcedimento = null)
        {
            if (oProcedimento == null)
            {
                return 0;
            }
            //if (JaExiste(out int IdProcedimento, oProcedimento: oProcedimento))
            //{
            //    oProcedimento.IdProcedimento = IdProcedimento;
            //    Update(oProcedimento);
            //    return oProcedimento.IdProcedimento;
            //}

            List<IDbDataParameter> lstParameters = null;//AdmGetParameters(propertyInfos: oProcedimento, tipoObjeto: Tabela);

            try
            {
                AbrirConexao(Transacao: false);

                SQL = "SELECT IFNULL(MAX(IdProcedimento),0) + 1 AS Maior FROM Procedimento " +
                      " WHERE IdEmpresa = " + oProcedimento.IdEmpresa;
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                if (oDR?.Read() == true)
                {
                    oProcedimento.IdProcedimento = int.Parse(oDR["Maior"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();

                SQL = "INSERT INTO Procedimento (IdEmpresa, IdProcedimento, Nome, Descricao, MaterialNecessario, TempoPrevisto, EhIntercalavel, QtdSimultaneo, TempoIntercalado ) " +
                      " VALUES (" + oProcedimento.IdEmpresa + ", " + oProcedimento.IdProcedimento + ", " +
                      "'" + oProcedimento.Nome + "', '" + oProcedimento.Descricao + "', " +
                      "'" + oProcedimento.MaterialNecessario + "', " +
                      (oProcedimento.TempoPrevisto == int.MinValue ? "null": oProcedimento.TempoPrevisto.ToString()) + ", " +
                      (oProcedimento.EhIntercalavel == int.MinValue ? "null" : oProcedimento.EhIntercalavel.ToString()) + ", " +
                      (oProcedimento.QtdSimultaneo == int.MinValue ? "null" : oProcedimento.QtdSimultaneo.ToString()) + ", " +
                      (oProcedimento.TempoIntercalado == int.MinValue ? "null" : oProcedimento.TempoIntercalado.ToString()) + " " +
                      ")";

                int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                InserirFuncionario(oProcedimento);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oProcedimento.IdProcedimento;
        }

        private void InserirFuncionario(Procedimento oProcedimento)
        {
            AdmFuncionario oAdmFuncionario = new AdmFuncionario();
            List<Funcionario> listFuncionario = oAdmFuncionario.SelectRows();

            StringBuilder sb = new StringBuilder();
            _ = sb.Append("INSERT INTO FuncionarioProcedimento (IdEmpresa, IdFuncionario, IdProcedimento ) Values ");

            foreach (Funcionario funcionario in listFuncionario)
            {
                _ = sb.Append(" (")
                      .Append(oProcedimento.IdEmpresa)
                      .Append(", ")
                      .Append(funcionario.IdFuncionario)
                      .Append(", ")
                      .Append(oProcedimento.IdProcedimento)
                      .Append("), ");
            }

            _ = sb.Remove(sb.Length - 2, 2)
                  .Append("; ");

            SQL = sb.ToString();

            AbrirConexao(Transacao: false);
            int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: null);

        }

        /// <summary>
        /// Altera o Procedimento
        /// </summary>
        /// <param name="oProcedimento">objeto Procedimento.</param>
        public void Update(Procedimento oProcedimento = null)
        {
            if (oProcedimento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oProcedimento, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdProcedimento");

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
        /// Exclui um Procedimento
        /// </summary>
        /// <param name="oProcedimento">objeto Procedimento.</param>
        public void Delete(Procedimento oProcedimento = null)
        {
            if (oProcedimento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdProcedimento", value: oProcedimento.IdProcedimento)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdProcedimento");

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
        /// <param name="IdProcedimento">identificação do Procedimento.</param>
        /// <param name="oProcedimento">objeto Procedimento.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdProcedimento, Procedimento oProcedimento = null)
        {
            IdProcedimento = 0;
            if (oProcedimento == null)
            {
                return false;
            }

            Procedimento oProcedimentoSel = new Procedimento
            {
                Nome = oProcedimento.Nome
            };

            List<Procedimento> lstProcedimento = SelectRows(oProcedimentoSel);
            if (lstProcedimento?.Count > 0)
            {
                IdProcedimento = lstProcedimento?[0].IdProcedimento ?? 0;
            }

            return IdProcedimento > 0;
        }
    }
}