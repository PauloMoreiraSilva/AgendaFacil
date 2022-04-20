using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Infra;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Feriado
    /// </summary>
    public class AdmFeriado : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário no banco de dados
        /// </summary>
        private const string Tabela = "Feriado";

        /// <summary>
        /// Retorna lista de Funcionários
        /// </summary>
        public List<Feriado> LstFeriado { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmFeriado()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Feriado com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Feriado.</returns>
        private Feriado PopulateMC(IDataReader oDR)
        {
            return (Feriado)AdmPopulateMC(oDR: oDR, tipo: "Feriado");
        }

        /// <summary>
        /// Seleciona Funcionários baseado em parâmetros passados no objeto Feriado.
        /// </summary>
        /// <param name="oFeriado">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<Feriado> SelectRows(Feriado oFeriado = null)
        {
            List<Feriado> oColl = new List<Feriado>();
            if (oFeriado == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFeriado, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFeriado", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Feriado oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oFeriado.CountAutomatico == 1)
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
        /// <param name="oFeriado">Objeto Feriado</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Feriado oFeriado)
        {
            if (oFeriado.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFeriado, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetFeriado", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna Feriado por ID
        /// </summary>
        /// <param name="IdFeriado">Código.</param>
        /// <returns>objeto Feriado.</returns>
        public Feriado SelectRowByID(int IdFeriado = 0)
        {
            if (IdFeriado == 0)
                return null;

            Feriado oFeriadoSel = new Feriado
            {
                IdFeriado = IdFeriado
            };
            List<Feriado> lstFeriado = SelectRows(oFeriadoSel);
            return lstFeriado?.Count > 0 ? lstFeriado?[0] : null;
        }

        /// <summary>
        /// Insere um novo Feriado
        /// </summary>
        /// <param name="oFeriado">Objeto Feriado.</param>
        /// <returns>Id do Feriado inserido.</returns>
        public int Insert(Feriado oFeriado = null)
        {
            if (oFeriado == null)
            {
                return 0;
            }
            if (JaExiste(out int IdFeriado, oFeriado: oFeriado))
            {
                oFeriado.IdFeriado = IdFeriado;
                Update(oFeriado);
                return oFeriado.IdFeriado;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFeriado, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: false);
                oFeriado.IdFeriado = DBHelper.ExecuteScalar(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oFeriado.IdFeriado;
        }

        /// <summary>
        /// Altera o Feriado
        /// </summary>
        /// <param name="oFeriado">objeto Feriado.</param>
        public void Update(Feriado oFeriado = null)
        {
            if (oFeriado == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oFeriado, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFeriado");

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
        /// Exclui um Feriado
        /// </summary>
        /// <param name="oFeriado">objeto Feriado.</param>
        public void Delete(Feriado oFeriado = null)
        {
            if (oFeriado == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdFeriado", value: oFeriado.IdFeriado)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdFeriado");

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
        /// <param name="IdFeriado">identificação do Feriado.</param>
        /// <param name="oFeriado">objeto Feriado.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdFeriado, Feriado oFeriado = null)
        {
            IdFeriado = 0;
            if (oFeriado == null)
            {
                return false;
            }

            Feriado oFeriadoSel = new Feriado
            {
                Dia = oFeriado.Dia,
                Mes = oFeriado.Mes,
                Data = oFeriado.Data
            };

            List<Feriado> lstFeriado = SelectRows(oFeriadoSel);
            if (lstFeriado?.Count > 0)
            {
                IdFeriado = lstFeriado?[0].IdFeriado ?? 0;
            }

            return IdFeriado > 0;
        }

        /// <summary>
        /// Verifica se a data informada é feriado (não-útil).
        /// </summary>
        /// <param name="dtBase">Data.</param>
        /// <returns>True: feriado / false: não feriado (dia útil).</returns>
        public bool VerificarFeriado(DateTime dtBase)
        {
            if (!Formats.IsDate(Data: dtBase.ToString(), out _))
            {
                return false;
            }

            bool bReturn = VerificarFeriadoFixo(dtBase);

            if (!bReturn)
            {
                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("Data", dtBase.Date)
                };

                const string sSQL = "SELECT Codigo FROM FERIADO WHERE Data = @Data";

                try
                {
                    AbrirConexao(Transacao: false);
                    IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: sSQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                    bReturn = oDR?.Read() == true;

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
            }
            return bReturn;
        }

        /// <summary>
        /// Verifica se a data informada é feriado fixo (não-útil).
        /// </summary>
        /// <param name="dtBase">Data.</param>
        /// <returns>True: feriado / false: não feriado (dia útil).</returns>
        public bool VerificarFeriadoFixo(DateTime dtBase)
        {
            if (!Formats.IsDate(Data: dtBase.ToString(), out _))
            {
                return false;
            }

            bool bReturn = false;

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter("Dia", dtBase.Day),
                DBHelper.GetParameter("Mes", dtBase.Month)
            };

            const string sSQL = "SELECT IdFeriado FROM Feriado WHERE Dia = @Dia AND Mes = @Mes";

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: sSQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                bReturn = oDR?.Read() == true;

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
            return bReturn;
        }
    }
}