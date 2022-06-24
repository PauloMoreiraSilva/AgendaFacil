using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Agendamento
    /// </summary>
    public class AdmAgendamento : AdmBase
    {
        /// <summary>
        /// Tabela de Agendamento no banco de dados
        /// </summary>
        private const string Tabela = "Agendamento";

        /// <summary>
        /// Retorna lista de Agendamento
        /// </summary>
        public List<Agendamento> LstAgendamento { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmAgendamento()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Agendamento com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Agendamento.</returns>
        private Agendamento PopulateMC(IDataReader oDR)
        {
            return (Agendamento)AdmPopulateMC(oDR: oDR, tipo: "Agendamento");
        }

        /// <summary>
        /// Seleciona Agendamento baseado em parâmetros passados no objeto Agendamento.
        /// </summary>
        /// <param name="oAgendamento">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Agendamento.</returns>
        public List<Agendamento> SelectRows(Agendamento oAgendamento)
        {
            List<Agendamento> oColl = new List<Agendamento>();
            if (oAgendamento == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oAgendamento, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetAgendamento", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Agendamento oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oAgendamento.CountAutomatico == 1)
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

        public List<Agendamento> SelectRows()
        {
            List<Agendamento> oColl = new List<Agendamento>();

            SQL = "SELECT A.*," +
                " E.Nome AS NomeEmpresa, " +
                " C.Nome AS NomeCliente, " +
                " F.Nome AS NomeFuncionario, " +
                " P.Nome AS NomeProcedimento " +
                " FROM Agendamento A " +
                " INNER JOIN Empresa E ON (A.IdEmpresa = E.IdEmpresa)" +
                " INNER JOIN Cliente C ON (A.IdEmpresa = C.IdEmpresa AND A.IdCliente = C.IdCliente)" +
                " INNER JOIN Funcionario F ON (A.IdEmpresa = F.IdEmpresa AND A.IdFuncionario = F.IdFuncionario)" +
                " INNER JOIN Procedimento P ON (A.IdEmpresa = P.IdEmpresa AND A.IdProcedimento = P.IdProcedimento)";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Agendamento oMC = PopulateMC(oDR: oDR);
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
        /// <param name="oAgendamento">Objeto Agendamento</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Agendamento oAgendamento)
        {
            if (oAgendamento.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            SQL = "SELECT COUNT(*) AS Count " +
                " FROM Agendamento A " +
                " WHERE IdProcedimento = @IdProcedimento " +
                " AND DataInclusao BETWEEN @DataInicio AND @DataFim ";

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oAgendamento, tipoObjeto: Tabela);

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
            Random rnd = new Random();
            int num = rnd.Next(10, 50);
            return CountRegistro + num;
        }

        public int SelectRowsCount()
        {

            SQL = "SELECT COUNT(IdCliente) AS Count FROM Agendamento ";

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
        /// Retorna Agendamento por ID
        /// </summary>
        /// <param name="IdAgendamento">Código.</param>
        /// <returns>objeto Agendamento.</returns>
        public Agendamento SelectRowByID(int IdAgendamento = 0)
        {
            if (IdAgendamento == 0)
                return null;

            Agendamento oAgendamentoSel = new Agendamento
            {
                IdAgendamento = IdAgendamento
            };
            List<Agendamento> lstAgendamento = SelectRows(oAgendamentoSel);
            return lstAgendamento?.Count > 0 ? lstAgendamento?[0] : null;
        }

        /// <summary>
        /// Insere um novo Agendamento
        /// </summary>
        /// <param name="oAgendamento">Objeto Agendamento.</param>
        /// <returns>Id do Agendamento inserido.</returns>
        public int Insert(Agendamento oAgendamento = null)
        {
            if (oAgendamento == null)
            {
                return 0;
            }
            //if (JaExiste(out int IdAgendamento, oAgendamento: oAgendamento))
            //{
            //    oAgendamento.IdAgendamento = IdAgendamento;
            //    Update(oAgendamento);
            //    return oAgendamento.IdAgendamento;
            //}

            List<IDbDataParameter> lstParameters = null;//AdmGetParameters(propertyInfos: oAgendamento, tipoObjeto: Tabela);

            try
            {
                AbrirConexao(Transacao: false);

                SQL = "SELECT IFNULL(MAX(IdAgendamento),0) + 1 AS Maior FROM Agendamento " +
                      " WHERE IdEmpresa = " + oAgendamento.IdEmpresa +
                      " AND IdCliente = " + oAgendamento.IdCliente +
                      " AND IdFuncionario = " + oAgendamento.IdFuncionario +
                      " AND IdProcedimento = " + oAgendamento.IdProcedimento;

                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                if (oDR?.Read() == true)
                {
                    oAgendamento.IdAgendamento = int.Parse(oDR["Maior"].ToString());
                }
                oDR?.Close();
                oDR?.Dispose();

                SQL = "INSERT INTO Agendamento (IdEmpresa, IdFuncionario, IdCliente, IdProcedimento, IdAgendamento, "+ 
                    "DataInicio, DataFim, Situacao) " +
                      " VALUES (" + oAgendamento.IdEmpresa + ", " + oAgendamento.IdFuncionario + ", " + oAgendamento.IdCliente + ", " + 
                      oAgendamento.IdProcedimento + ", " + oAgendamento.IdAgendamento + ", " +
                      "'" + oAgendamento.DataInicio.ToString("yyyy-MM-dd HH:mm:ss") + "', "+
                      "'" + oAgendamento.DataFim.ToString("yyyy-MM-dd HH:mm:ss") + "', " + oAgendamento.Situacao + ")";

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

            return oAgendamento.IdAgendamento;
        }

        /// <summary>
        /// Altera o Agendamento
        /// </summary>
        /// <param name="oAgendamento">objeto Agendamento.</param>
        public void Update(Agendamento oAgendamento = null)
        {
            if (oAgendamento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oAgendamento, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdAgendamento");

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
        /// Exclui um Agendamento
        /// </summary>
        /// <param name="oAgendamento">objeto Agendamento.</param>
        public void Delete(Agendamento oAgendamento = null)
        {
            if (oAgendamento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdAgendamento", value: oAgendamento.IdAgendamento)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdAgendamento");

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
        /// <param name="IdAgendamento">identificação do Agendamento.</param>
        /// <param name="oAgendamento">objeto Agendamento.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdAgendamento, Agendamento oAgendamento = null)
        {
            IdAgendamento = 0;
            if (oAgendamento == null)
            {
                return false;
            }

            Agendamento oAgendamentoSel = new Agendamento
            {
                DataInicio = oAgendamento.DataInicio
            };

            List<Agendamento> lstAgendamento = SelectRows(oAgendamentoSel);
            if (lstAgendamento?.Count > 0)
            {
                IdAgendamento = lstAgendamento?[0].IdAgendamento ?? 0;
            }

            return IdAgendamento > 0;
        }
    }
}