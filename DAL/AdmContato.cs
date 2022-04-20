using System;
using System.Collections.Generic;
using System.Data;
using Guasti.DataBase;
using Guasti.Infra;
using Guasti.Model;

namespace Guasti.DAL
{
    public class AdmContato : AdmBase
    {
        private int iCount;
        public List<Contato> lstContato;
        private const string Tabela = "CONTATO";

        public AdmContato() : base()
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            oUserLoggedInfo ??= oAdmUsuario.GetUser();
        }

        private Contato PopulateMC(IDataReader oDR)
        {
            return (Contato)AdmPopulateMC(oDR: oDR, tipo: "Contato");
        }

        public List<Contato> SelectRows(int MaxRows = 0, int StartRowIndex = 0, string SortField = "",
                                        int Codigo = 0, int CodigoSacado = 0, string CodigoSAP = "",
                                        string Nome = "", string NomePagador = "", string NumInscricao = "",
                                        int CountAutomatico = 0)
        {
            List<Contato> oColl = new List<Contato>();
            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: new
            {
                MaxRows,
                StartRowIndex,
                SortField,
                Codigo,
                CodigoSacado,
                CodigoSAP,
                NomePagador,
                NumInscricao,
                Nome
            });

            if (oUserLoggedInfo?.CodigoModulo == 3)
            {
                lstParameters.Add(DBHelper.getParameter(parameterName: "CodigoCliente", value: oUserLoggedInfo.CodigoSacado));

                if (oUserLoggedInfo?.CodigoNucleo != 0)
                {
                    lstParameters.Add(DBHelper.getParameter(parameterName: "CodigoNucleo", value: oUserLoggedInfo.CodigoNucleo));
                }
            }
            
            try
            {
                iCount = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.getDataReader(oIDbConnection: oConn, sCommandText: "GetContato", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: oTrans, lstParameters: lstParameters);

                while (oDR?.Read() == true)
                {
                    Contato oMC = PopulateMC(oDR: oDR);
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

        public int SelectRowsCount(int Codigo = 0, int CodigoSacado = 0, string CodigoSAP = "", string Nome = "",
                                   string NomePagador = "", string NumInscricao = "", int CountAutomatico = 0)
        {
            if (CountAutomatico == 1)
            {
                return iCount;
            }
            const string Function = "COUNT";
            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: new
            {
                Codigo,
                CodigoSacado,
                CodigoSAP,
                Nome,
                NomePagador,
                NumInscricao,
                Function
            });

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.getDataReader(oIDbConnection: oConn, sCommandText: "GetContato", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: oTrans, lstParameters: lstParameters);

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

        public Contato SelectRowByID(int Codigo = 0)
        {
            if (Codigo == 0)
            {
                return null;
            }
            List<Contato> lstContato = SelectRows(Codigo: Codigo);
            return lstContato?.Count > 0 ? lstContato?[0] : null;
        }

        public int Insert(Contato oContato = null)
        {
            if (oContato == null)
            {
                return 0;
            }

            if (JaExiste(out int Codigo, oContato: oContato))
            {
                oContato.Codigo = Codigo;
                if (oConfig.sCargaInicial == "N")
                {
                    Update(oContato);
                }
                return oContato.Codigo;
            }

            if ((oContato.EnvioDebito10 == "1" ||
                oContato.EnvioDebito25 == "1" ||
                oContato.EnvioDebitoAmbos == "1") &&
                oContato.DebitoVencido == "0")
            {
                oContato.DebitoVencido = "1";
            }

            if ((oContato.EnvioDebito10 == "0" &&
                oContato.EnvioDebito25 == "0" &&
                oContato.EnvioDebitoAmbos == "0") &&
                oContato.DebitoVencido == "1")
            {
                oContato.EnvioDebito10 = "1";
            }

            oContato.DataInclusao = DateTime.Now;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContato);

            _SQL = GerarSQL(Tipo: "INSERT", Tabela: Tabela, lstParameters: lstParameters);

            try
            {
                AbrirConexao(Transacao: true);
                oContato.Codigo = DBHelper.ExecuteScalar(oIDbConnection: oConn, sCommandText: _SQL, oCommandType: CommandType.Text, oIDbTransaction: oTrans, lstParameters: lstParameters);

                Auditoria oAuditoria = new Auditoria();
                string sTxt = "ID: " + oContato.Codigo.ToString() +
                              ", ID Sacado: " + oContato.CodigoSacado.ToString() +
                              ", Nome: " + oContato.Nome;
                oAuditoria.Insert(oConn: oConn, oTrans: oTrans, iCodigoUsuario: oUserLoggedInfo.Codigo, iCodigoFuncao: 38, iCodigoFuncionalidade: 1, sOcorrencia: sTxt);

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

            return oContato.Codigo;
        }

        public void Update(Contato oContato = null)
        {
            if (oContato == null)
            {
                return;
            }

            if ((oContato.EnvioDebito10 == "1" ||
                oContato.EnvioDebito25 == "1" ||
                oContato.EnvioDebitoAmbos == "1") &&
                oContato.DebitoVencido == "0")
            {
                oContato.DebitoVencido = "1";
            }

            if ((oContato.EnvioDebito10 == "0" &&
                oContato.EnvioDebito25 == "0" &&
                oContato.EnvioDebitoAmbos == "0") &&
                oContato.DebitoVencido == "1")
            {
                oContato.EnvioDebito10 = "1";
            }

            oContato.DataAlteracao = DateTime.Now;

            //Para estes campos são permitidos valores nulos, zeros ou vazio no update
            BDCampoZerado = oContato.CamposZerados();

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oContato);

            _SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

            try
            {
                AbrirConexao(Transacao: true);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: oConn, sCommandText: _SQL, oCommandType: CommandType.Text, oIDbTransaction: oTrans, lstParameters: lstParameters);

                Auditoria oAuditoria = new Auditoria();
                string sTxt = "ID: " + oContato.Codigo.ToString() +
                              ", ID Sacado: " + oContato.CodigoSacado.ToString() +
                              ", Nome: " + oContato.Nome;
                oAuditoria.Insert(oConn: oConn, oTrans: oTrans, iCodigoUsuario: oUserLoggedInfo.Codigo, iCodigoFuncao: 38, iCodigoFuncionalidade: 2, sOcorrencia: sTxt);

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

        public void Delete(Contato oContato = null)
        {
            if (oContato == null)
            {
                return;
            }
            oContato.DataExclusao = DateTime.Now;

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.getParameter(parameterName: "Codigo", value: oContato.Codigo)
            };

            _SQL = GerarSQL(Tipo: "DELETE", Tabela: Tabela, lstParameters: lstParameters, Where: "Codigo");

            try
            {
                AbrirConexao(Transacao: true);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: oConn, sCommandText: _SQL, oCommandType: CommandType.Text, oIDbTransaction: oTrans,  lstParameters: lstParameters);

                Auditoria oAuditoria = new Auditoria();
                string sTxt = "ID: " + oContato.Codigo.ToString() +
                              ", ID Sacado: " + oContato.CodigoSacado.ToString() +
                              ", Nome: " + oContato.Nome;
                oAuditoria.Insert(oConn: oConn, oTrans: oTrans, iCodigoUsuario: oUserLoggedInfo.Codigo, iCodigoFuncao: 38, iCodigoFuncionalidade: 3, sOcorrencia: sTxt);

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
                foreach (Contato oContato in lstContato)
                {
                    oContato.CodigoArquivo = CodigoArquivo;

                    iSeq++;
                    if (bInterface)
                    { RaiseBoletoEvent(iSeq.ToString(), lstContato.Count.ToString()); }

                    _ = Insert(oContato);
                }
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
        }

        private bool JaExiste(out int Codigo, Contato oContato = null)
        {
            Codigo = 0;
            if (oContato == null)
            {
                return false;
            }

            bool bAchou = false;

            foreach (Contato oSelecionado in SelectRows(CodigoSAP: oContato.CodigoSAP))
            {
                Codigo = oSelecionado.Codigo;
                bAchou = true;
                break;
            }

            return bAchou;
        }

        //Quando recebemos o arquivo de carga de contato mas ainda não recebemos o do cliente
        //o código do sacado fica em branco. usamos este processo para verificar se já recebemos
        //o cliente correspondente e ajustar o cadastro do contato.
        public void AjustarContato()
        {
            _SQL = " UPDATE CT " +
                   " SET CT.CodigoSacado = C.Codigo " +
                   " FROM CONTATO CT " +
                   " INNER JOIN COMPRADOR C ON (CT.CodigoSAP = C.CodigoSAP AND CT.CodigoSacado IS NULL)";
            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>();
            try
            {
                AbrirConexao(Transacao: true);
                _ = DBHelper.ExecuteNonQuery(oIDbConnection: oConn, sCommandText: _SQL, oCommandType: CommandType.Text, oIDbTransaction: oTrans, lstParameters: lstParameters);

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
    }
}
