using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados da Empresa
    /// </summary>
    public class AdmEmpresa : AdmBase
    {
        /// <summary>
        /// Tabela de Empresas no banco de dados
        /// </summary>
        private const string Tabela = "Empresa";

        /// <summary>
        /// Retorna lista de Empresas
        /// </summary>
        public List<Empresa> LstEmpresa { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmEmpresa()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto Empresa com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Empresa.</returns>
        private Empresa PopulateMC(IDataReader oDR)
        {
            return (Empresa)AdmPopulateMC(oDR: oDR, tipo: "Empresa");
        }

        /// <summary>
        /// Seleciona empresas baseado em parâmetros passados no objeto Empresa.
        /// </summary>
        /// <param name="oEmpresa">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Empresas.</returns>
        public List<Empresa> SelectRows(Empresa oEmpresa)
        {
            List<Empresa> oColl = new List<Empresa>();
            if (oEmpresa == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oEmpresa, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetEmpresa", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Empresa oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oEmpresa.CountAutomatico == 1)
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

        public List<Empresa> SelectRows()
        {
            List<Empresa> oColl = new List<Empresa>();

            SQL = "SELECT * FROM Empresa";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Empresa oMC = PopulateMC(oDR: oDR);
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
        /// <param name="Codigo">código.</param>
        /// <param name="Nome">nome.</param>
        /// <param name="NumInscricao">inscrição.</param>
        /// <param name="CountAutomatico">Flag se retorna a quantidade encontrada pela SelectRows.</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Empresa oEmpresa)
        {
            if (oEmpresa.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oEmpresa, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetEmpresa", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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

            SQL = "SELECT COUNT(IdEmpresa) AS Count FROM Empresa";
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
        /// Retorna Empresa por ID
        /// </summary>
        /// <param name="Codigo">Código.</param>
        /// <returns>objeto Empresa.</returns>
        public Empresa SelectRowByID(int IdEmpresa = 0)
        {
            if (IdEmpresa == 0)
                return null;

            Empresa oEmpresaSel = new Empresa
            {
                IdEmpresa = IdEmpresa
            };
            List<Empresa> lstEmpresa = SelectRows(oEmpresaSel);
            return lstEmpresa?.Count > 0 ? lstEmpresa?[0] : null;
        }

        public Empresa SelectRowByID(string sIdEmpresa)
        {
            SQL = "SELECT * " +
                " FROM Empresa " +
                " WHERE IdEmpresa = " + sIdEmpresa;
            Empresa oMC = null;

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
        /// Seleciona o Empresa pelo CNPJ
        /// </summary>
        /// <param name="CNPJ">cnpj.</param>
        /// <returns>Empresa.</returns>
        public Empresa SelectRowByCNPJ(string CNPJ = "")
        {
            if (string.IsNullOrEmpty(CNPJ))
            {
                return null;
            }
            Empresa oEmpresaSel = new Empresa
            {
                NumInscricao = CNPJ
            };
            List<Empresa> lstEmpresa = SelectRows(oEmpresaSel);
            return lstEmpresa?.Count > 0 ? lstEmpresa?[0] : null;
        }

        /// <summary>
        /// Insere uma nova empresa
        /// </summary>
        /// <param name="oEmpresa">Objeto empresa.</param>
        /// <returns>Id da empresa inserida.</returns>
        public int Insert(Empresa oEmpresa = null)
        {
            if (oEmpresa == null)
            {
                return 0;
            }
            //if (JaExiste(out int IdEmpresa, oEmpresa: oEmpresa))
            //{
            //    oEmpresa.IdEmpresa = IdEmpresa;
            //    //Update(oEmpresa);
            //    return oEmpresa.IdEmpresa;
            //}

            List<IDbDataParameter> lstParameters = null; //AdmGetParameters(propertyInfos: oEmpresa, tipoObjeto: Tabela);

            //SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            SQL = "INSERT INTO Empresa (Nome, TipoInscricao, Inscricao, Endereco, "+ 
                "Bairro, Cidade, Uf, CEP) " +
                  " VALUES ('" + oEmpresa.Nome + "', " + oEmpresa.TipoInscricao + ", " +
                  "'" + oEmpresa.NumInscricao + "', '" + oEmpresa.Endereco + "', " +
                  "'" + oEmpresa.Bairro + "', '"+ oEmpresa.Cidade +"', "+
                  "'"+ oEmpresa.Uf +"', '"+ oEmpresa.Cep+"')";

            try
            {
                AbrirConexao(Transacao: false);
                int i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);

                if (i > 0)
                {
                    SQL = "SELECT IFNULL(MAX(IdEmpresa),0) AS Maior FROM Empresa";
                    IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);

                    if (oDR?.Read() == true)
                    {
                        oEmpresa.IdEmpresa = int.Parse(oDR["Maior"].ToString());
                    }
                    oDR?.Close();
                    oDR?.Dispose();
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

            return oEmpresa.IdEmpresa;
        }

        /// <summary>
        /// Altera a empresa
        /// </summary>
        /// <param name="oEmpresa">objeto empresa.</param>
        public void Update(Empresa oEmpresa = null)
        {
            if (oEmpresa == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oEmpresa, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdEmpresa");

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
        /// Exclui uma empresa
        /// </summary>
        /// <param name="oEmpresa">objeto empresa.</param>
        public void Delete(Empresa oEmpresa = null)
        {
            if (oEmpresa == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdEmpresa", value: oEmpresa.IdEmpresa)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdEmpresa");

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
        /// <param name="IdEmpresa">identificação da empresa.</param>
        /// <param name="oEmpresa">objeto empresa.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdEmpresa, Empresa oEmpresa = null)
        {
            IdEmpresa = 0;
            if (oEmpresa == null)
            {
                return false;
            }

            Empresa oEmpresaSel = new Empresa
            {
                NumInscricao = oEmpresa.NumInscricao
            };

            List<Empresa> lstEmpresa = SelectRows(oEmpresaSel);
            if (lstEmpresa?.Count > 0)
            {
                IdEmpresa = lstEmpresa?[0].IdEmpresa ?? 0;
            }

            return IdEmpresa > 0;
        }
    }
}