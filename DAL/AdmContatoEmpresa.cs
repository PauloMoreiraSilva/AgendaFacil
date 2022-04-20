using System;
using System.Collections.Generic;
using System.Data;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Contato da Empresa
    /// </summary>
    public class AdmContatoEmpresa : AdmBase
    {
        /// <summary>
        /// Tabela de Contato da Empresas no banco de dados
        /// </summary>
        private const string Tabela = "ContatoEmpresa";

        /// <summary>
        /// Retorna lista de Contatos da Empresa
        /// </summary>
        public List<ContatoEmpresa> LstContatoEmpresa { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmContatoEmpresa()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            UsuarioLogado ??= oAdmUsuario.GetUser();
        }

        /// <summary>
        /// Retorna um objeto ContatoEmpresa com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto ContatoEmpresa.</returns>
        private ContatoEmpresa PopulateMC(IDataReader oDR)
        {
            return (ContatoEmpresa)AdmPopulateMC(oDR: oDR, tipo: "ContatoEmpresa");
        }

        /// <summary>
        /// Seleciona Contatos da Empresa baseado em parâmetros passados no objeto ContatoEmpresa.
        /// </summary>
        /// <param name="oContatoEmpresa">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de ContatoEmpresas.</returns>
        public List<ContatoEmpresa> SelectRows(ContatoEmpresa oContatoEmpresa = null)
        {
            List<ContatoEmpresa> oColl = new List<ContatoEmpresa>();
            if (oContatoEmpresa == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContatoEmpresa, tipoObjeto: Tabela);

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetContatoEmpresa", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    ContatoEmpresa oMC = PopulateMC(oDR: oDR);
                    oColl.Add(item: oMC);
                    if (CountRegistro == 0 && oContatoEmpresa.CountAutomatico == 1)
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
        /// <param name="oContatoEmpresa"></param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(ContatoEmpresa oContatoEmpresa)
        {
            if (oContatoEmpresa.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContatoEmpresa, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetContatoEmpresa", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna ContatoEmpresa por ID
        /// </summary>
        /// <param name="IdContato">Identificação do contato.</param>
        /// <returns>objeto ContatoEmpresa.</returns>
        public ContatoEmpresa SelectRowByID(int IdContato = 0)
        {
            if (IdContato == 0)
                return null;

            ContatoEmpresa oContatoEmpresaSel = new ContatoEmpresa
            {
                IdContato = IdContato
            };
            List<ContatoEmpresa> lstContatoEmpresa = SelectRows(oContatoEmpresaSel);
            return lstContatoEmpresa?.Count > 0 ? lstContatoEmpresa?[0] : null;
        }

        /// <summary>
        /// Insere um novo Contato da Empresa
        /// </summary>
        /// <param name="oContatoEmpresa">Objeto ContatoEmpresa.</param>
        /// <returns>Id do Contato da Empresa inserido.</returns>
        public int Insert(ContatoEmpresa oContatoEmpresa = null)
        {
            if (oContatoEmpresa == null)
            {
                return 0;
            }
            if (JaExiste(out int IdContatoEmpresa, oContatoEmpresa: oContatoEmpresa))
            {
                oContatoEmpresa.IdContato = IdContatoEmpresa;
                Update(oContatoEmpresa);
                return oContatoEmpresa.IdContato;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContatoEmpresa, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: false);
                oContatoEmpresa.IdContato = DBHelper.ExecuteScalar(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return oContatoEmpresa.IdContato;
        }

        /// <summary>
        /// Altera a ContatoEmpresa
        /// </summary>
        /// <param name="oContatoEmpresa">objeto ContatoEmpresa.</param>
        public void Update(ContatoEmpresa oContatoEmpresa = null)
        {
            if (oContatoEmpresa == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContatoEmpresa, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdContatoEmpresa");

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
        /// Exclui uma ContatoEmpresa
        /// </summary>
        /// <param name="oContatoEmpresa">objeto ContatoEmpresa.</param>
        public void Delete(ContatoEmpresa oContatoEmpresa = null)
        {
            if (oContatoEmpresa == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdContato", value: oContatoEmpresa.IdContato)
            };

            SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdContato");

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
        /// <param name="IdContato">identificação do Contato da Empresa.</param>
        /// <param name="oContatoEmpresa">objeto ContatoEmpresa.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdContato, ContatoEmpresa oContatoEmpresa = null)
        {
            IdContato = 0;
            if (oContatoEmpresa == null)
            {
                return false;
            }

            ContatoEmpresa oContatoEmpresaSel = new ContatoEmpresa
            {
                Tipo = oContatoEmpresa.Tipo,
                Valor = oContatoEmpresa.Valor
            };

            List<ContatoEmpresa> lstContatoEmpresa = SelectRows(oContatoEmpresaSel);
            if (lstContatoEmpresa?.Count > 0)
            {
                IdContato = lstContatoEmpresa?[0].IdContato ?? 0;
            }

            return IdContato > 0;
        }
    }
}