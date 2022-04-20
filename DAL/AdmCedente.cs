using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Guasti.DataBase;
using Guasti.Model;

namespace Guasti.DAL
{
    /// <summary>
    /// Acesso aos dados de Cedentes
    /// </summary>
    public class AdmCedente : AdmBase
    {
        private int iCount;

        /// <summary>
        /// Tabela de Cedentes no banco de dados
        /// </summary>
        private const string Tabela = "CEDENTE";

        /// <summary>
        /// Retorna lista de cedentes
        /// </summary>
        public List<Cedente> LstCedente { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmCedente()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Cedente com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Cedente.</returns>
        private Cedente PopulateMC(IDataReader oDR)
        {
            return (Cedente)AdmPopulateMC(oDR: oDR, tipo: "Cedente");
        }

        /// <summary>
        /// Retorna lista de cedentes
        /// </summary>
        /// <param name="MaxRows">Quantidade máxima de linhas para paginação</param>
        /// <param name="StartRowIndex">Número da linha para iniciar a paginação.</param>
        /// <param name="SortField">Ordenar pelo campo.</param>
        /// <param name="Codigo">código do cedente.</param>
        /// <param name="Nome">nome do cedente.</param>
        /// <param name="NumInscricao">inscrição do cedente.</param>
        /// <param name="CountAutomatico">Flag para retornar a quantidade de registro: 1 = sim / 0 = não.</param>
        /// <returns>A list of Cedentes.</returns>
        public List<Cedente> SelectRows(int MaxRows = int.MinValue,
                                        int StartRowIndex = int.MinValue,
                                        string SortField = "",
                                        int Codigo = int.MinValue,
                                        string Nome = "",
                                        string NumInscricao = "",
                                        int CountAutomatico = 0)
        {
            var myParam = new
            {
                MaxRows,
                StartRowIndex,
                SortField,
                Nome,
                NumInscricao
            };

            iCount = 0;
            List<Cedente> oColl = new List<Cedente>();
            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: myParam, tipoObjeto: Tabela);

            if (UsuarioLogado?.CodigoModulo == 3)
            {
                lstParameters.Add(DBHelper.GetParameter(parameterName: nameof(Codigo), value: UsuarioLogado.CodigoCedente));
            }
            else if (Codigo != 0)
            {
                lstParameters.Add(DBHelper.GetParameter(parameterName: nameof(Codigo), value: Codigo));
            }

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetCedente", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                while (oDR?.Read() == true)
                {
                    Cedente oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (iCount == 0 && CountAutomatico == 1)
                    {
                        iCount = int.Parse(oDR["Count"].ToString());
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
        /// <param name="Codigo">código.</param>
        /// <param name="Nome">nome.</param>
        /// <param name="NumInscricao">inscrição.</param>
        /// <param name="CountAutomatico">Flag se retorna a quantidade encontrada pela SelectRows.</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(int Codigo = int.MinValue,
                                   string Nome = "",
                                   string NumInscricao = "",
                                   int CountAutomatico = 0)
        {
            if (CountAutomatico == 1)
            {
                return iCount;
            }
            var myParam = new
            {
                Codigo,
                Nome,
                NumInscricao
            };

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: myParam, tipoObjeto: Tabela);

            if (UsuarioLogado?.CodigoModulo == 3)
            {
                lstParameters.Add(DBHelper.GetParameter(parameterName: nameof(Codigo), value: UsuarioLogado.CodigoCedente));
            }
            else if (Codigo != 0)
            {
                lstParameters.Add(DBHelper.GetParameter(parameterName: nameof(Codigo), value: Codigo));
            }

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetCedente", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                if (oDR?.Read() == true)
                {
                    iCount = int.Parse(oDR["Count"].ToString());
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
            return iCount;
        }

        /// <summary>
        /// Retorna Cedente por ID
        /// </summary>
        /// <param name="Codigo">Código.</param>
        /// <returns>objeto Cedente.</returns>
        public Cedente SelectRowByID(int Codigo = 0)
        {
            if (Codigo == 0)
                return null;

            List<Cedente> lstCedente = SelectRows(Codigo: Codigo);
            return lstCedente?.Count > 0 ? lstCedente?[0] : null;
        }

        /// <summary>
        /// Seleciona o cedente pelo CNPJ
        /// </summary>
        /// <param name="CNPJ">cnpj.</param>
        /// <returns>Cedente.</returns>
        public Cedente SelectRowByCNPJ(string CNPJ = "")
        {
            if (string.IsNullOrEmpty(CNPJ))
            {
                return null;
            }
            List<Cedente> lstCedente = SelectRows(NumInscricao: CNPJ);
            return lstCedente?.Count > 0 ? lstCedente?[0] : null;
        }

        public int SelectRowCountByIDs(List<string> lstIDs = null, string sNome = "", string sNumInscricao = "")
        {
            if (lstIDs == null)
            {
                return 0;
            }
            int iCount = 0;
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("SELECT COUNT(Codigo) AS Count FROM CEDENTE").Append(" WHERE (");

            foreach (string sID in lstIDs)
            {
                sbSQL.Append(" Codigo = ").Append(sID).Append(" OR ");
            }

            sbSQL.Remove(sbSQL.Length - 4, 4).Append(")");

            if (!string.IsNullOrEmpty(sNome))
            {
                sbSQL.Append(" AND Nome LIKE '%").Append(sNome).Append("%'");
            }

            if (!string.IsNullOrEmpty(sNumInscricao))
            {
                sbSQL.Append(" AND NumInscricao LIKE '%").Append(sNumInscricao).Append("%'");
            }

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sbSQL.ToString(), CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

                if (oDR?.Read() == true)
                {
                    iCount = int.Parse(oDR["Count"].ToString());
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
            return iCount;
        }

        public List<Cedente> SelectRowsByIDs(List<string> lstIDs = null, string sNome = "", string sNumInscricao = "")
        {
            if (lstIDs == null)
            {
                return null;
            }

            List<Cedente> oColl = new List<Cedente>();
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("SELECT * FROM CEDENTE").Append(" WHERE (");

            foreach (string sID in lstIDs)
            {
                sbSQL.Append("Codigo = " + sID + " OR ");
            }

            sbSQL.Remove(sbSQL.Length - 4, 4).Append(")");

            if (!string.IsNullOrEmpty(sNome))
            {
                sbSQL.Append(" AND Nome LIKE '%" + sNome + "%'");
            }

            if (!string.IsNullOrEmpty(sNumInscricao))
            {
                sbSQL.Append(" AND NumInscricao LIKE '%" + sNumInscricao + "%'");
            }

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sbSQL.ToString(), CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

                while (oDR?.Read() == true)
                {
                    Cedente oMC = PopulateMC(oDR);
                    oColl.Add(oMC);
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

        

        public int Insert(Cedente oCedente)
        {
            if (oCedente == null)
            {
                return 0;
            }
            if (JaExiste(out int Codigo, oCedente: oCedente))
            {
                oCedente.Codigo = Codigo;
                Update(oCedente);
                return oCedente.Codigo;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oCedente, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: true);
                oCedente.Codigo = DBHelper.ExecuteScalar(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                string sTxt = "ID: " + oCedente.Codigo.ToString() +
                              ", Nome: " + oCedente.Nome +
                              ", Inscrição: " + oCedente.NumInscricao;

                AdmAuditoria oAdmAuditoria = new AdmAuditoria();
                oAdmAuditoria.Insert(OConn: OConn, oTrans: OTrans, iCodigoUsuario: UsuarioLogado.Codigo, iCodigoFuncao: 1, iCodigoFuncionalidade: 1, sOcorrencia: sTxt);

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oCedente.Codigo;
        }

        public void Update(Cedente oCedente)
        {
            if (oCedente == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oCedente, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

            try
            {
                AbrirConexao(Transacao: true);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                string sTxt = "ID: " + oCedente.Codigo.ToString() +
                              ", Nome: " + oCedente.Nome +
                              ", Inscrição: " + oCedente.NumInscricao;

                AdmAuditoria oAdmAuditoria = new AdmAuditoria();
                oAdmAuditoria.Insert(OConn, OTrans, UsuarioLogado.Codigo, 1, 2, sTxt);

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
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

        public void Delete(Cedente oCedente)
        {
            if (oCedente == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "Codigo", value: oCedente.Codigo)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

            try
            {
                AbrirConexao(Transacao: true);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, lstParameters: lstParameters);

                string sTxt = "ID: " + oCedente.Codigo.ToString() +
                              ", Nome: " + oCedente.Nome +
                              ", Inscrição: " + oCedente.NumInscricao;

                AdmAuditoria oAdmAuditoria = new AdmAuditoria();
                oAdmAuditoria.Insert(OConn: OConn, oTrans: OTrans, iCodigoUsuario: UsuarioLogado.Codigo, iCodigoFuncao: 1, iCodigoFuncionalidade: 3, sOcorrencia: sTxt);

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
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

        public void InserirArquivo()
        {
            int iSeq = 0;

            try
            {
                foreach (Cedente oCedente in LstCedente)
                {
                    iSeq++;
                    RaiseBoletoEvent(iSeq.ToString(), LstCedente.Count.ToString());

                    _ = Insert(oCedente);
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
        }

        private bool JaExiste(out int Codigo, Cedente oCedente = null)
        {
            Codigo = 0;
            if (oCedente == null)
            {
                return false;
            }

            bool bAchou = false;

            foreach (Cedente oSelecionado in SelectRows(NumInscricao: oCedente.NumInscricao))
            {
                Codigo = oSelecionado.Codigo;
                bAchou = true;
                break;
            }

            return bAchou;
        }

        /// <summary>
        /// Gera o Nosso número sequencial conforme o cedente
        /// </summary>
        /// <param name="CodigoCedente">codigo cedente.</param>
        /// <param name="CodCarteira">retorna código da carteira.</param>
        /// <returns>Nosso Número.</returns>
        public long GerarNossoNumero(int CodigoCedente, out string CodCarteira)
        {
            long iNN = 0;

            CodCarteira = "00";

            try
            {
                AbrirConexao(Transacao: true);

                #region Pega a sequência atual

                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("Codigo", CodigoCedente)
                };

                SQL = "SELECT SequenciaNN, CodCarteira FROM CEDENTE WHERE Codigo = @Codigo";

                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                if (oDR?.Read() == true)
                {
                    if (!string.IsNullOrEmpty(oDR["SequenciaNN"].ToString()))
                    {
                        iNN = Convert.ToInt64(oDR["SequenciaNN"]);
                    }
                    if (!string.IsNullOrEmpty(oDR["CodCarteira"].ToString()))
                    {
                        CodCarteira = oDR["CodCarteira"].ToString();
                    }
                }

                oDR?.Close();
                oDR?.Dispose();

                #endregion Pega a sequência atual

                iNN++;

                #region Atualiza o NN

                lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("Codigo", CodigoCedente),
                    DBHelper.GetParameter("SequenciaNN", iNN)
                };

                SQL = "UPDATE CEDENTE SET SequenciaNN = @SequenciaNN WHERE Codigo = @Codigo";
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, SQL, CommandType.Text, OTrans, lstParameters: lstParameters);

                #endregion Atualiza o NN

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }
            return iNN;
        }

        /// <summary>
        /// Gera o próximo número do arquivo de remessa (sequencial)
        /// </summary>
        /// <param name="CodigoCedente">código do cedente.</param>
        /// <returns>Número sequencial do arquivo de remessa.</returns>
        public int GerarNumeroRemessa(int CodigoCedente)
        {
            int iNumRem = 0;

            try
            {
                AbrirConexao(Transacao: true);

                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter(parameterName: "Codigo", value: CodigoCedente)
                };

                SQL = "SELECT UltimaRemessa FROM CEDENTE WHERE Codigo = @Codigo";

                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

                if (oDR?.Read() == true && oDR["UltimaRemessa"] != DBNull.Value)
                {
                    iNumRem = Convert.ToInt32(oDR["UltimaRemessa"]);
                }

                oDR?.Close();
                oDR?.Dispose();

                iNumRem++;

                if (iNumRem > 999999) //6 posições no header de arquivo
                {
                    iNumRem = 1;
                }

                #region Atualiza o NN

                lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter(parameterName: "Codigo",value: CodigoCedente),
                    DBHelper.GetParameter(parameterName: "UltimaRemessa", value: iNumRem)
                };

                SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                #endregion Atualiza o NN

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return iNumRem;
        }

        public int GerarNumeroRetorno(int CodigoCedente)
        {
            int iNumRet = 0;

            try
            {
                AbrirConexao(Transacao: true);

                #region Pega a sequência atual

                List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("Codigo", CodigoCedente)
                };

                SQL = "SELECT UltimoRetorno FROM CEDENTE WHERE Codigo = @Codigo";

                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, OTrans, lstParameters: lstParameters);

                if (oDR?.Read() == true && oDR["UltimoRetorno"] != DBNull.Value)
                {
                    iNumRet = Convert.ToInt32(oDR["UltimoRetorno"]);
                }

                oDR?.Close();
                oDR?.Dispose();

                #endregion Pega a sequência atual

                iNumRet++;

                #region Atualiza o NN

                lstParameters = new List<IDbDataParameter>
                {
                    DBHelper.GetParameter("Codigo", CodigoCedente),
                    DBHelper.GetParameter("UltimoRetorno", iNumRet)
                };

                SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

                _ = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, SQL, CommandType.Text, OTrans, lstParameters: lstParameters);

                #endregion Atualiza o NN

                if (!ManterTransacao)
                {
                    CommitTransacao();
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return iNumRet;
        }

        public bool PodeInserir(string NumInscricao)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();

            lstParameters.Add(DBHelper.GetParameter("NumInscricao", NumInscricao));

            SQL = "SELECT COUNT(Codigo) AS Count FROM CEDENTE";
            SQL = SQL + " WHERE NumInscricao = @NumInscricao";

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();
            IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

            bool bReturn = false;

            if (oDR.Read() && int.Parse(oDR["Count"].ToString()) == 0)
            {
                bReturn = true;
            }

            oDR?.Close();
            oDR?.Dispose();

            return bReturn;
        }

        public bool PodeAlterar(int CodigoCedente, string NumInscricao)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();

            lstParameters.Add(DBHelper.GetParameter("CodigoCedente", CodigoCedente));
            lstParameters.Add(DBHelper.GetParameter("NumInscricao", NumInscricao));

            SQL = "SELECT COUNT(Codigo) AS Count FROM CEDENTE";
            SQL = SQL + " WHERE Codigo <> @CodigoCedente AND NumInscricao = @NumInscricao";

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();
            IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

            bool bReturn = false;

            if (oDR.Read() && int.Parse(oDR["Count"].ToString()) == 0)
            {
                bReturn = true;
            }

            oDR?.Close();
            oDR?.Dispose();

            return bReturn;
        }

        public bool PodeExcluir(int CodigoCedente, ref string Motivo)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();
            bool bReturn = true;

            #region Usuários

            lstParameters.Clear();
            lstParameters.Add(DBHelper.GetParameter("CodigoCedente", CodigoCedente));

            //_SQL = "SELECT COUNT(Codigo) AS Count FROM USUARIO";
            //_SQL = _SQL + " WHERE CodigoCedente = @CodigoCedente";

            SQL = "SELECT TOP 1 Codigo FROM USUARIO WITH (NOLOCK) ";
            SQL = SQL + " WHERE CodigoCedente = @CodigoCedente";

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();
            IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

            if (oDR.Read())
            {
                //if (int.Parse(oDR["Count"].ToString()) > 0)
                //{
                bReturn = false;
                Motivo = Motivo + "Existe(m) usuário(s) cadastrado(s) para este Cedente. ";
                //}
            }

            oDR?.Close();
            oDR?.Dispose();

            #endregion Usuários

            #region Boletos

            lstParameters.Clear();
            lstParameters.Add(DBHelper.GetParameter("CodigoCedente", CodigoCedente));

            //_SQL = "SELECT COUNT(Codigo) AS Count FROM BOLETO";
            //_SQL = _SQL + " WHERE CodigoCedente = @CodigoCedente";

            SQL = "SELECT TOP 1 Codigo FROM BOLETO WITH (NOLOCK) ";
            SQL = SQL + " WHERE CodigoCedente = @CodigoCedente";

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();
            oDR = DBHelper.GetDataReader(oIDbConnection: OConn, SQL, CommandType.Text, oCommandBehavior: CommandBehavior.Default, lstParameters: lstParameters);

            if (oDR?.Read() == true)
            {
                //if (int.Parse(oDR["Count"].ToString()) > 0)
                //{
                bReturn = false;
                Motivo = Motivo + "Existe(m) boleto(s) cadastrado(s) para este Cedente. ";
                //}
            }

            oDR?.Close();
            oDR?.Dispose();

            #endregion Boletos

            return bReturn;
        }

        public bool UpdateNSU(int CodigoCedente, long NSU)
        {
            bool bReturn = false;

            if (CodigoCedente == 0)
            {
                return bReturn;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();
            lstParameters.Add(DBHelper.GetParameter("Codigo", CodigoCedente));
            lstParameters.Add(DBHelper.GetParameter("UltimoNSU", NSU));

            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append("UPDATE CEDENTE");
            sbSQL.Append(" SET UltimoNSU = @UltimoNSU");
            sbSQL.Append(" WHERE");
            sbSQL.Append(" Codigo = @Codigo");

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();

            try
            {
                #region Atualiza registros

                int iRowsAffected = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: sbSQL.ToString(), oCommandType: CommandType.Text, lstParameters: lstParameters);

                if (iRowsAffected > 0)
                {
                    bReturn = true;
                }

                #endregion Atualiza registros
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

        public void AtivarBloqueioEmDolar(int CodigoCedente)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();

            OTrans = OTrans?.Connection == null ? OConn.BeginTransaction() : OTrans;

            try
            {
                #region Ativar

                lstParameters.Clear();
                lstParameters.Add(DBHelper.GetParameter("Codigo", CodigoCedente));
                lstParameters.Add(DBHelper.GetParameter("TravarEmDolar", 1));

                SQL = "UPDATE CEDENTE SET";
                SQL = SQL + " TravarEmDolar = @TravarEmDolar";
                SQL = SQL + " WHERE Codigo = @Codigo";

                int iRowsAffected = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, SQL, CommandType.Text, OTrans, lstParameters: lstParameters);

                #endregion Ativar

                if (!ManterTransacao)
                {
                    OTrans.Commit();
                }
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

        public void DesativarBloqueioEmDolar(int CodigoCedente)
        {
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();

            OConn = OConn?.State == ConnectionState.Open ? OConn : DBHelper.GetConnection();

            OTrans = OTrans?.Connection == null ? OConn.BeginTransaction() : OTrans;

            try
            {
                #region Desativar

                lstParameters.Clear();
                lstParameters.Add(DBHelper.GetParameter("Codigo", CodigoCedente));
                lstParameters.Add(DBHelper.GetParameter("TravarEmDolar", DBNull.Value));

                SQL = "UPDATE CEDENTE SET";
                SQL = SQL + " TravarEmDolar = @TravarEmDolar";
                SQL = SQL + " WHERE Codigo = @Codigo";

                int iRowsAffected = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, SQL, CommandType.Text, OTrans, lstParameters: lstParameters);

                #endregion Desativar

                if (!ManterTransacao)
                {
                    OTrans.Commit();
                }
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
    }
}