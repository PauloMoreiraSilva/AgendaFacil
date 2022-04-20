using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using PI4Sem.DataBase;
using PI4Sem.Model;

namespace PI4Sem.DAL
{
    /// <summary>
    /// Acesso aos dados de Usuario
    /// </summary>
    public class AdmUsuario : AdmBase
    {
        /// <summary>
        /// Tabela de Funcionário no banco de dados
        /// </summary>
        private const string Tabela = "Usuario";

        /// <summary>
        /// Retorna lista de Funcionários
        /// </summary>
        public List<Usuario> LstUsuario { get; set; }

        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public AdmUsuario()
        {
            UsuarioLogado ??= GetUser();
        }

        /// <summary>
        /// Retorna um objeto Usuario com os dados retornados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>objeto Usuario.</returns>
        private Usuario PopulateMC(IDataReader oDR)
        {
            return (Usuario)AdmPopulateMC(oDR: oDR, tipo: "Usuario");
        }

        /// <summary>
        /// Preenche o objeto UserLoggedInfo com os dados da base de dados
        /// </summary>
        /// <param name="oDR">DataReader.</param>
        /// <returns>UserLoggedInfo.</returns>
        private UserLoggedInfo PopulateUser(IDataReader oDR)
        {
            return (UserLoggedInfo)AdmPopulateMC(oDR: oDR, tipo: "UserLoggedInfo");
        }

        /// <summary>
        /// Seleciona Usuários baseado em parâmetros passados no objeto Usuario.
        /// </summary>
        /// <param name="oUsuario">Objeto com parâmetros para pesquisa</param>
        /// <returns>Lista de Funcionários.</returns>
        public List<Usuario> SelectRows(Usuario oUsuario)
        {
            List<Usuario> oColl = new List<Usuario>();
            if (oUsuario == null)
                return oColl;

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oUsuario, tipoObjeto: Tabela);

            StringBuilder sb = new StringBuilder();
            _ = sb.Append("SELECT U.*, " +
                " F.Nome AS NomeFuncionario, " +
                " F.Email AS EmailFuncionario, " +
                " E.Nome AS NomeEmpresa" +
                " FROM Usuario U " +
                " INNER JOIN Funcionario F ON (U.IdFuncionario = F.IdFuncionario AND U.IdEmpresa = F.IdEmpresa)" +
                " INNER JOIN Empresa E ON (U.IdEmpresa = E.IdEmpresa)" +
                " WHERE " +
                "");

            foreach (IDbDataParameter oParam in lstParameters)
            {
                    _ = sb.Append(oParam.ParameterName)
                          .Append(" = @")
                          .Append(oParam.ParameterName)
                          .Append(" AND ");
            }

            SQL = sb.Remove(sb.Length - 5, 5).ToString();

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);
                while (oDR?.Read() == true)
                {
                    Usuario oMC = PopulateMC(oDR: oDR);
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

        public List<Usuario> SelectRows()
        {
            List<Usuario> oColl = new List<Usuario>();

            SQL = "SELECT U.*, " +
                " F.Nome AS NomeFuncionario, " +
                " F.Email AS EmailFuncionario, " +
                " E.Nome AS NomeEmpresa" +
                " FROM Usuario U " +
                " INNER JOIN Funcionario F ON (U.IdFuncionario = F.IdFuncionario AND U.IdEmpresa = F.IdEmpresa)" +
                " INNER JOIN Empresa E ON (U.IdEmpresa = E.IdEmpresa)";

            try
            {
                CountRegistro = 0;
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: null);
                while (oDR?.Read() == true)
                {
                    Usuario oMC = PopulateMC(oDR: oDR);
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
        /// <param name="oUsuario">Objeto Usuario</param>
        /// <returns>quantidade de registros.</returns>
        public int SelectRowsCount(Usuario oUsuario)
        {
            if (oUsuario.CountAutomatico == 1)
            {
                return CountRegistro;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oUsuario, tipoObjeto: Tabela);

            lstParameters.Add(DBHelper.GetParameter(parameterName: "Function", value: "COUNT"));

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: "GetUsuario", oCommandType: CommandType.StoredProcedure, oCommandBehavior: CommandBehavior.Default, oIDbTransaction: OTrans, lstParameters: lstParameters);

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
        /// Retorna Usuario por ID
        /// </summary>
        /// <param name="IdUsuario">Código.</param>
        /// <returns>objeto Usuario.</returns>
        public Usuario SelectRowByID(int IdFuncionario = 0)
        {
            if (IdFuncionario == 0)
                return null;

            Usuario oUsuarioSel = new Usuario
            {
                IdFuncionario = IdFuncionario
            };
            List<Usuario> lstUsuario = SelectRows(oUsuarioSel);
            return lstUsuario?.Count > 0 ? lstUsuario?[0] : null;
        }

        public Usuario SelectRowByID(string sIdEmpresa, string sIdFuncionario)
        {
            SQL = "SELECT U.*, " +
                " F.Nome AS NomeFuncionario, " +
                " F.Email AS EmailFuncionario, " +
                " E.Nome AS NomeEmpresa" +
                " FROM Usuario U " +
                " INNER JOIN Funcionario F ON (U.IdFuncionario = F.IdFuncionario AND U.IdEmpresa = F.IdEmpresa)" +
                " INNER JOIN Empresa E ON (U.IdEmpresa = E.IdEmpresa)" +
                " WHERE U.IdEmpresa = " + sIdEmpresa + 
                " AND U.IdFuncionario = " + sIdFuncionario;
            Usuario oMC = null;

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
        /// Seleciona o usuário pelo login de acesso.
        /// </summary>
        /// <param name="sLoginAcesso">login acesso.</param>
        /// <returns>Usuario.</returns>
        public Usuario SelectRowByLogin(string sLogin = "")
        {
            if (sLogin?.Length == 0)
            {
                return null;
            }
            Usuario oUsuarioSel = new Usuario
            {
                Login = sLogin
            };
            List<Usuario> lstUsuario = SelectRows(oUsuario: oUsuarioSel);
            return lstUsuario?.Count > 0 ? lstUsuario?[0] : null;
        }

        /// <summary>
        /// Insere um novo Usuario
        /// </summary>
        /// <param name="oUsuario">Objeto Usuario.</param>
        /// <returns>Id do Usuario inserido.</returns>
        public int Insert(Usuario oUsuario = null)
        {
            if (oUsuario == null)
            {
                return 0;
            }
            int i = 0;
            if (JaExiste(out int IdFuncionario, oUsuario: oUsuario))
            {
                oUsuario.IdFuncionario = IdFuncionario;
                //Update(oUsuario);
                i = oUsuario.IdFuncionario;
            }

            List<IDbDataParameter> lstParameters = null; //AdmGetParameters(propertyInfos: oUsuario, tipoObjeto: Tabela);

            try
            {
                AbrirConexao(Transacao: false);

                SQL = "INSERT INTO Usuario (IdEmpresa, IdFuncionario, Login, Senha) " +
                      " VALUES (" + oUsuario.IdEmpresa + ", " + oUsuario.IdFuncionario + ", " +
                      "'"+oUsuario.Login +"', '"+ oUsuario.Senha+"')";

                i = DBHelper.ExecuteNonQuery(oIDbConnection: OConn, sCommandText: SQL, oCommandType: CommandType.Text, oIDbTransaction: OTrans, lstParameters: lstParameters);
            }
            catch (Exception ex)
            {
                AdmLogError(ex: ex);
            }
            finally
            {
                AdmFinnaly();
            }

            return i;
        }

        /// <summary>
        /// Altera o Usuario
        /// </summary>
        /// <param name="oUsuario">objeto Usuario.</param>
        public void Update(Usuario oUsuario = null)
        {
            if (oUsuario == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = AdmGetParameters(propertyInfos: oUsuario, tipoObjeto: Tabela);

            SQL = GerarSQL(Tipo: "UPDATE", Tabela: Tabela, lstParameters: lstParameters, Where: "IdEmpresa,IdFuncionario");

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
        /// Exclui um Usuario
        /// </summary>
        /// <param name="oUsuario">objeto Usuario.</param>
        public void Delete(Usuario oUsuario = null)
        {
            if (oUsuario == null)
            {
                return;
            }

            List<IDbDataParameter> lstParameters = new List<IDbDataParameter>
            {
                DBHelper.GetParameter(parameterName: "IdFuncionario", value: oUsuario.IdFuncionario)
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
        /// <param name="IdUsuario">identificação do Usuario.</param>
        /// <param name="oUsuario">objeto Usuario.</param>
        /// <returns>True: já existe / False: não existe.</returns>
        public bool JaExiste(out int IdFuncionario, Usuario oUsuario = null)
        {
            IdFuncionario = 0;
            if (oUsuario == null)
            {
                return false;
            }

            Usuario oUsuarioSel = new Usuario
            {
                IdFuncionario = oUsuario.IdFuncionario
            };

            List<Usuario> lstUsuario = SelectRows(oUsuarioSel);
            if (lstUsuario?.Count > 0)
            {
                IdFuncionario = lstUsuario?[0].IdFuncionario ?? 0;
            }

            return IdFuncionario > 0;
        }

        /// <summary>
        /// Retorna o usuário logado
        /// </summary>
        /// <returns>UserLoggedInfo.</returns>
        public UserLoggedInfo GetUser()
        {
            if (UsuarioLogado == null)
            {
                HttpContext oHttpContext = HttpContext.Current;

                UsuarioLogado = (oHttpContext?.Session?["UserLoggedInfo"] != null) ?
                                   (UserLoggedInfo)oHttpContext.Session["UserLoggedInfo"] :
                                   null;

                if ((UsuarioLogado == null) && (FlagAutomatico == 1))
                {
                    UsuarioLogado = GetUserServicoAutomatico();
                }
            }
            return UsuarioLogado;
        }

        /// <summary>
        /// Retorna o usuário padrão usado por serviços automáticos
        /// </summary>
        /// <returns>UserLoggedInfo.</returns>
        public UserLoggedInfo GetUserServicoAutomatico()
        {
            UserLoggedInfo oUser = new UserLoggedInfo();
            const string _SQL = "SELECT * FROM VW_UsuarioServicoAutomatico";

            try
            {
                AbrirConexao(Transacao: false);
                IDataReader oDR = DBHelper.GetDataReader(oIDbConnection: OConn, sCommandText: _SQL);

                if (oDR?.Read() == true)
                {
                    oUser = PopulateUser(oDR: oDR);
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
            return oUser;
        }

        /// <summary>
        /// Cria uma nova senha
        /// </summary>
        /// <param name="iTamMin">Tamanho mínimo.</param>
        /// <param name="iTamMax">Tamanho máximo.</param>
        /// <returns>senha randômica.</returns>
        public string CriarSenha(int iTamMin, int iTamMax)
        {
            string[] sTabela = {"4", "w", "j", "0", "x", "h", "6",
                                "u", "t", "i", "d", "g", "m", "c",
                                "v", "5", "b", "f", "z", "3", "r",
                                "y", "9", "p", "l", "7", "n", "e",
                                "s", "a", "k", "q", "o", "2", "8",
                                "1",
                                "4", "W", "J", "0", "X", "H", "6",
                                "U", "T", "I", "D", "G", "M", "C",
                                "V", "5", "B", "F", "Z", "3", "R",
                                "Y", "9", "P", "L", "7", "N", "E",
                                "S", "A", "K", "Q", "O", "2", "8",
                                "1",
                               };

            string sSenha = "";
            Random RandNum = new Random();
            int iTamSenha;
            if (iTamMin <= iTamMax && iTamMin > 0)
            {
                iTamSenha = RandNum.Next(iTamMin, iTamMax);
            }
            else
            {
                iTamSenha = 10;
            }

            int i;
            for (i = 1; i <= iTamSenha; i++)
            {
                sSenha += sTabela[RandNum.Next(0, sTabela.Length - 1)];
            }

            return sSenha;
        }
    }
}