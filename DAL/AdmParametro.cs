using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Parametro
    /// </summary>
    public class AdmParametro : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário no banco de dados
        /// </summary>
        private const string Tabela = "Parametro";

        /// <summary>
        /// Retorna lista de Funcionários
        /// </summary>
        public List<Parametro> LstParametro { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmParametro()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Parametro com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Parametro.</returns>
        private Parametro PopulateMC(IDataReader oDR)
        {
            return (Parametro)AdmPopulateMC(oDR: oDR, tipo: "Parametro");
        }

        /// <summary>
        /// Seleciona Funcionários baseado em parâmetros passados no objeto Parametro.
        /// </summary>
        /// <param name="oParametro">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<Parametro> SelectRows(Parametro oParametro = null)
        {
            List<Parametro> oColl = new List<Parametro>();
            if (oParametro == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oParametro, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetParametro", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Parametro oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oParametro.CountAutomatico == 1)
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
        /// <param name="oParametro">Objeto Parametro</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Parametro oParametro)
        {
            if (oParametro.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oParametro, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetParametro", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna Parametro por ID
        /// </summary>
        /// <param name="IdParametro">Código.</param>
        /// <returns>objeto Parametro.</returns>
        public Parametro SelectRowByID(int IdParametro = 0)
        {
            if (IdParametro == 0)
                return null;

            Parametro oParametroSel = new Parametro
            {
                IdParametro = IdParametro
            };
            List<Parametro> lstParametro = SelectRows(oParametroSel);
            return lstParametro?.Count > 0 ? lstParametro?[0] : null;
        }

        /// <summary>
        /// Insere um novo Parametro
        /// </summary>
        /// <param name="oParametro">Objeto Parametro.</param>
        /// <returns>Id do Parametro inserido.</returns>
        public int Insert(Parametro oParametro = null)
        {
            if (oParametro == null)
            {
                return 0;
            }
            if (JaExiste(out int IdParametro, oParametro: oParametro))
            {
                oParametro.IdParametro = IdParametro;
                Update(oParametro);
                return oParametro.IdParametro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oParametro, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: false);
                oParametro.IdParametro = DBHelper.ExecuteScalar(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oParametro.IdParametro;
        }

        /// <summary>
        /// Altera o Parametro
        /// </summary>
        /// <param name="oParametro">objeto Parametro.</param>
        public void Update(Parametro oParametro = null)
        {
            if (oParametro == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oParametro, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdParametro");

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
        /// Exclui um Parametro
        /// </summary>
        /// <param name="oParametro">objeto Parametro.</param>
        public void Delete(Parametro oParametro = null)
        {
            if (oParametro == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdParametro", value: oParametro.IdParametro)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdParametro");

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
        /// <param name="IdParametro">identificação do Parametro.</param>
        /// <param name="oParametro">objeto Parametro.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdParametro, Parametro oParametro = null)
        {
            IdParametro = 0;
            if (oParametro == null)
            {
                return false;
            }

            Parametro oParametroSel = new Parametro
            {
                Nome = oParametro.Nome
            };

            List<Parametro> lstParametro = SelectRows(oParametroSel);
            if (lstParametro?.Count > 0)
            {
                IdParametro = lstParametro?[0].IdParametro ?? 0;
            }

            return IdParametro > 0;
        }
    }
}