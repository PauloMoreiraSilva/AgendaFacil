using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de FuncionarioProcedimento
    /// </summary>
    public class AdmFuncionarioProcedimento : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário e procedimentos no banco de dados
        /// </summary>
        private const string Tabela = "FuncionarioProcedimento";

        /// <summary>
        /// Retorna lista de Funcionários e procedimentos
        /// </summary>
        public List<FuncionarioProcedimento> LstFuncionarioProcedimento { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmFuncionarioProcedimento()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto FuncionarioProcedimento com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto FuncionarioProcedimento.</returns>
        private FuncionarioProcedimento PopulateMC(IDataReader oDR)
        {
            return (FuncionarioProcedimento)AdmPopulateMC(oDR: oDR, tipo: "FuncionarioProcedimento");
        }

        /// <summary>
        /// Seleciona Funcionários baseado em parâmetros passados no objeto FuncionarioProcedimento.
        /// </summary>
        /// <param name="oFuncionarioProcedimento">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<FuncionarioProcedimento> SelectRows(FuncionarioProcedimento oFuncionarioProcedimento = null)
        {
            List<FuncionarioProcedimento> oColl = new List<FuncionarioProcedimento>();
            if (oFuncionarioProcedimento == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionarioProcedimento, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFuncionarioProcedimento", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    FuncionarioProcedimento oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oFuncionarioProcedimento.CountAutomatico == 1)
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

        /// <summary>
        /// Retorna a quantidade de registros encontrados
        /// </summary>
        /// <param name="oFuncionarioProcedimento">Objeto FuncionarioProcedimento</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(FuncionarioProcedimento oFuncionarioProcedimento)
        {
            if (oFuncionarioProcedimento.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionarioProcedimento, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFuncionarioProcedimento", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna FuncionarioProcedimento por ID
        /// </summary>
        /// <param name="IdFuncionarioProcedimento">Código.</param>
        /// <returns>objeto FuncionarioProcedimento.</returns>
        public FuncionarioProcedimento SelectRowByID(int IdFuncionarioProcedimento = 0)
        {
            if (IdFuncionarioProcedimento == 0)
                return null;

            FuncionarioProcedimento oFuncionarioProcedimentoSel = new FuncionarioProcedimento
            {
                IdFuncionario = IdFuncionarioProcedimento
            };
            List<FuncionarioProcedimento> lstFuncionarioProcedimento = SelectRows(oFuncionarioProcedimentoSel);
            return lstFuncionarioProcedimento?.Count > 0 ? lstFuncionarioProcedimento?[0] : null;
        }

        /// <summary>
        /// Insere um novo FuncionarioProcedimento
        /// </summary>
        /// <param name="oFuncionarioProcedimento">Objeto FuncionarioProcedimento.</param>
        /// <returns>Id do FuncionarioProcedimento inserido.</returns>
        public int Insert(FuncionarioProcedimento oFuncionarioProcedimento = null)
        {
            if (oFuncionarioProcedimento == null)
            {
                return 0;
            }
            if (JaExiste(out int IdFuncionarioProcedimento, oFuncionarioProcedimento: oFuncionarioProcedimento))
            {
                oFuncionarioProcedimento.IdFuncionario = IdFuncionarioProcedimento;
                Update(oFuncionarioProcedimento);
                return oFuncionarioProcedimento.IdFuncionario;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionarioProcedimento, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: false);
                oFuncionarioProcedimento.IdFuncionario = DBHelper.ExecuteScalar(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oFuncionarioProcedimento.IdFuncionario;
        }

        /// <summary>
        /// Altera o FuncionarioProcedimento
        /// </summary>
        /// <param name="oFuncionarioProcedimento">objeto FuncionarioProcedimento.</param>
        public void Update(FuncionarioProcedimento oFuncionarioProcedimento = null)
        {
            if (oFuncionarioProcedimento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFuncionarioProcedimento, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFuncionarioProcedimento");

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
        /// Exclui um FuncionarioProcedimento
        /// </summary>
        /// <param name="oFuncionarioProcedimento">objeto FuncionarioProcedimento.</param>
        public void Delete(FuncionarioProcedimento oFuncionarioProcedimento = null)
        {
            if (oFuncionarioProcedimento == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdFuncionario", value: oFuncionarioProcedimento.IdFuncionario)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFuncionarioProcedimento");

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
        /// <param name="IdFuncionarioProcedimento">identificação do FuncionarioProcedimento.</param>
        /// <param name="oFuncionarioProcedimento">objeto FuncionarioProcedimento.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdFuncionario, FuncionarioProcedimento oFuncionarioProcedimento = null)
        {
            IdFuncionario = 0;
            if (oFuncionarioProcedimento == null)
            {
                return false;
            }

            FuncionarioProcedimento oFuncionarioProcedimentoSel = new FuncionarioProcedimento
            {
                IdFuncionario = oFuncionarioProcedimento.IdFuncionario,
                IdProcedimento = oFuncionarioProcedimento.IdProcedimento
            };

            List<FuncionarioProcedimento> lstFuncionarioProcedimento = SelectRows(oFuncionarioProcedimentoSel);
            if (lstFuncionarioProcedimento?.Count > 0)
            {
                IdFuncionario = lstFuncionarioProcedimento?[0].IdFuncionario ?? 0;
            }

            return IdFuncionario > 0;
        }
    }
}