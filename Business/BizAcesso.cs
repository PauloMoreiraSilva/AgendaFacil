using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using PI4Sem.DAL;
using PI4Sem.Infra;
using PI4Sem.Model;

namespace PI4Sem.Business
{
    /// <summary>
    /// Classe para regras de negócios relacionadas a acesso à aplicação.
    /// </summary>
    public class BizAcesso : BusinessBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Acesso"/> class.
        /// </summary>
        public BizAcesso()
        {
        }

        /// <summary>
        /// Se o token for válido, autentica o usuário pela url criptografado
        /// </summary>
        /// <param name="sQuery">querystring.</param>
        /// <param name="sMensagemAlerta">retorna mensagem alerta.</param>
        /// <returns>True: autenticado.</returns>
        public UserLoggedInfo AutenticarPelaUrl(string sQuery, out string sMensagemAlerta)
        {
            sMensagemAlerta = "";
            try
            {
                string param = Formats.Descriptografar(sQuery.Substring(0, sQuery.Length - 1));
                string[] sDados = param.Split(';');

                if (sDados.Length == 2)
                {
                    AdmUsuario oAdmUsuario = new AdmUsuario();
                    string sResultado = "1"; // oAdmUsuario.ValidarToken(sDados[1], sDados[0]);

                    if (!(sResultado == "1" || sResultado == "2" || sResultado == "3"))
                    {
                        return Autenticar(sDados[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                sMensagemAlerta = "O sistema não conseguiu detectar que você é um humano. Por favor tente novamente: " + ex.Message;
                return null;
            }
            return null;
        }

        /// <summary>
        /// Autentica o usuário pelo login.
        /// </summary>
        /// <param name="sUsuario">usuario.</param>
        /// <returns>UserLoggedInfo.</returns>
        public UserLoggedInfo Autenticar(string sUsuario)
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            Usuario oUsuario = oAdmUsuario.SelectRowByLogin(sLogin: sUsuario);

            UserLoggedInfo oUserLoggedInfo = ConverterUsuarioLogado(oUsuario);
            if (oUserLoggedInfo != null)
            {
                oAdmUsuario.UsuarioLogado = oUserLoggedInfo;
            }
            return oUserLoggedInfo;
        }

        /// <summary>
        /// Efetua a autenticação do usuário no sistema.
        /// </summary>
        /// <param name="sUsername">login / username.</param>
        /// <param name="sSenha">senha.</param>
        /// <returns>1 = autenticado / 2 = trocar a senha / 3 = senha inválida / 4 = usuário não encontrado ou excluído ou bloqueado ou sem acesso (banco) / 5 = senha expirada.</returns>
        public int Authenticate(string sUsername, string sSenha)
        {
            AdmUsuario oAdmUsuario = new AdmUsuario();
            Usuario oUsuario = oAdmUsuario.SelectRowByLogin(sLogin: sUsername);

            if (oUsuario == null)
            {
                return 3; // usuário não encontrado
            }

            if (oUsuario.Deleted == 1)
            {
                return 4; // usuário excluído
            }

            //string sSenhaCryp = Formats.CalculateMD5(sSenha);
            if (oUsuario.Senha != sSenha)
            {
                return 3; // usuário encontrado mas senha inválida
            }

            if (oUsuario.Situacao == 2)
            {
                return 4; //usuário bloqueado
            }

            if (oUsuario.Situacao == 1)
            {
                return 2; //usuário encontrado, mas precisa trocar a senha
            }

            return 1; //Foi autenticado, pode logar
        }

        /// <summary>
        /// Converte um usuário em UserLoggedInfo, preenchendo as demais informações.
        /// </summary>
        /// <param name="oUsuario">usuario.</param>
        /// <returns>UserLoggedInfo.</returns>
        public UserLoggedInfo ConverterUsuarioLogado(Usuario oUsuario)
        {
            return oUsuario.Cast<UserLoggedInfo>(oUsuario);
        }

        /// <summary>
        /// Envia por e-mail o username associado para acesso ao sistema
        /// </summary>
        /// <param name="sEmail">email.</param>
        /// <returns>True: enviado / False: não enviado.</returns>
        public bool EnviarUsuarioAcesso(string sEmail = "")
        {
            if (string.IsNullOrEmpty(sEmail))
            {
                return false;
            }

            Usuario oUsuarioPesquisa = new Usuario
            {
                MaxRows = int.MaxValue,
                Email = sEmail
            };

            AdmUsuario oAdmUsuario = new AdmUsuario();
            List<Usuario> lstUsuario = oAdmUsuario.SelectRows(oUsuarioPesquisa);

            if (lstUsuario?.Count > 0)
            {
                Email oEmail = new Email();

                string sTemplateEmail = WebConfig.Key.PathPortal + "\\Templates\\Email\\email.html";
                string sUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;

                string sConteudo = File.Exists(sTemplateEmail) ?
                                   System.IO.File.ReadAllText(sTemplateEmail) :
                                   "";

                StringBuilder sMensagem = new StringBuilder();
                _ = sMensagem.Append("Nomes de usuário para o acesso:<br /><br />");

                foreach (Usuario oUsuario in lstUsuario)
                {
                    _ = sMensagem.Append("<font style=\"font-size:14px\">Login : ").Append(oUsuario.Login);

                    //if (!string.IsNullOrEmpty(oUsuario.UsuarioAcesso))
                    //{
                    //    _ = sMensagem.Append(" ou ").Append(oUsuario.UsuarioAcesso);
                    //}
                    _ = sMensagem.Append("</font><br />")
                                 .Append("<font style=\"font-size:14px\">Usuário : ").Append(oUsuario.Nome).Append(" </font><br /><br />");
                }

                _ = sMensagem.Append("<br /><b style=\"font-size:16px\"><u>Acesse o portal:</u></b><br /><br />")
                             .Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                             .Append("<font style=\"font-size:14px; color:blue\"><u>{sUrl}</u></font><br/>");

                sConteudo = string.IsNullOrEmpty(sConteudo) ? sMensagem.ToString() :
                                sConteudo.Replace("#URL#", sUrl)
                                         .Replace("#titulo#", "Dados de Acesso")
                                         .Replace("#mensagem#", sMensagem.ToString());

                return oEmail.Enviar(sEmail, AppProgram.AppName + " - Usuários para o Acesso", sConteudo);
            }

            return false;
        }

        /// <summary>
        /// Processa a requisição de uma nova senha a ser enviada ao usuário
        /// </summary>
        /// <param name="sUsuario">usuario.</param>
        /// <param name="sEmail">email.</param>
        /// <returns>True: senha enviada por e-mail com sucesso.</returns>
        public bool RequisitarSenha(string sUsuario = "", string sEmail = "")
        {
            if (string.IsNullOrEmpty(sUsuario) || string.IsNullOrEmpty(sEmail))
            {
                return false;
            }

            Usuario oUsuarioPesquisa = new Usuario
            {
                MaxRows = int.MaxValue,
                Login = sUsuario,
                Email = sEmail
            };

            AdmUsuario oAdmUsuario = new AdmUsuario();

            List<Usuario> lstUsuario = oAdmUsuario.SelectRows(oUsuario: oUsuarioPesquisa);
            if (lstUsuario?.Count == 0)
            {
                oUsuarioPesquisa = new Usuario
                {
                    MaxRows = int.MaxValue,
                    //UsuarioAcesso = sUsuario,
                    Email = sEmail
                };
                lstUsuario = oAdmUsuario.SelectRows(oUsuario: oUsuarioPesquisa);
            }

            Usuario oUsuario = lstUsuario?.Count > 0 ? lstUsuario?[0] : null;

            //if (oUsuario?.IdFuncionario != int.MinValue && oAdmUsuario.ZerarSenhaAdministrador(ref oUsuario))
            //{
            //    oAdmUsuario.RegistrarAuditoriaZerarSenha(oUsuario);
            //    return EnviarEmailNovaSenhaAdministrador(oUsuario: oUsuario);
            //}

            return false;
        }

        /// <summary>
        /// Envia email com nova senha temporária ao usuário.
        /// </summary>
        /// <param name="oUsuario">usuario.</param>
        /// <returns>True: e-mail enviado.</returns>
        public bool EnviarEmailNovaSenhaAdministrador(Usuario oUsuario = null)
        {
            if (oUsuario == null)
            {
                return false;
            }

            Email oEmail = new Email();

            string sTemplateEmail = WebConfig.Key.PathPortal + "\\Templates\\Email\\email.html";
            string sUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath;

            string sConteudo = File.Exists(sTemplateEmail) ?
                               System.IO.File.ReadAllText(sTemplateEmail) :
                               "";

            StringBuilder sMensagem = new StringBuilder();
            _ = sMensagem.Append("Login e Senha para o acesso:<br /><br />")
                         .Append("<font style=\"font-size:14px\">Login : ").Append(oUsuario.Login);

            //if (!string.IsNullOrEmpty(oUsuario.UsuarioAcesso))
            //{
            //    _ = sMensagem.Append(" ou ").Append(oUsuario.UsuarioAcesso);
            //}

            _ = sMensagem.Append("</font><br />")
                         .Append("<font style=\"font-size:14px\">Senha : ").Append(oUsuario.Senha).Append(" </font><br /><br />")
                         .Append("<font style=\"font-size:14px\">Usuário : ").Append(oUsuario.Nome).Append(" </font><br />");
                         //.Append("<font style=\"font-size:14px\">Módulo : ").Append(oUsuario.DescModulo).Append(" </font><br />")
                         //.Append("<font style=\"font-size:14px\">Perfil : ").Append(oUsuario.DescPerfil).Append(" </font><br />");

            //if (oUsuario.CodigoModulo == 2)
            //{
            //    _ = sMensagem.Append("<font style=\"font-size:14px\">Beneficiário : ").Append(oUsuario.NomeCedente).Append(" </font><br />");
            //}
            //else if (oUsuario.CodigoModulo == 3)
            //{
            //    _ = sMensagem.Append("<font style=\"font-size:14px\">Cliente : ").Append(oUsuario.NomePagador).Append(" </font><br /><br />");
            //}

            _ = sMensagem.Append("<b style=\"font-size:16px\"><u>Acesse o portal:</u></b><br /><br />")
                         .Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
                         .Append("<font style=\"font-size:14px; color:blue\"><u>").Append(sUrl).Append("</u></font><br/>");

            sConteudo = string.IsNullOrEmpty(sConteudo) ? sMensagem.ToString() :
                            sConteudo.Replace("#URL#", sUrl)
                                     .Replace("#titulo#", "Dados de Acesso")
                                     .Replace("#mensagem#", sMensagem.ToString());

            return oEmail.Enviar(oUsuario.Email, AppProgram.AppName + " - Login de Acesso", sConteudo);
        }

        /// <summary>
        /// Efetua o cadastro inicial
        /// </summary>
        /// <param name="Cadastro">The cadastro.</param>
        /// <returns>A bool.</returns>
        public bool Registrar(Object Cadastro = null)
        {
            var propRazaoSocial = Cadastro.GetType().GetProperty("Razao_Social");
            var propInscricao = Cadastro.GetType().GetProperty("Inscricao");
            var propContato = Cadastro.GetType().GetProperty("Contato");
            var propEmail = Cadastro.GetType().GetProperty("Email");
            var propUsuario = Cadastro.GetType().GetProperty("Usuario");
            var propSenha = Cadastro.GetType().GetProperty("Senha");

            Empresa oEmpresa = new Empresa
            {
                Nome = propRazaoSocial.GetValue(Cadastro, null).ToString(),
                TipoInscricao = 2,
                NumInscricao = propInscricao.GetValue(Cadastro, null).ToString()
            };

            AdmEmpresa oAdmEmpresa = new AdmEmpresa();
            int IdEmpresa = oAdmEmpresa.Insert(oEmpresa: oEmpresa);

            if (IdEmpresa <= 0)
            {
                return false;
            }

            Funcionario oFuncionario = new Funcionario
            {
                IdEmpresa = IdEmpresa,
                Nome = propContato.GetValue(Cadastro, null).ToString(),
                Email = propEmail.GetValue(Cadastro, null).ToString(),
                EhProprietario = 1
            };

            AdmFuncionario oAdmFuncionario = new AdmFuncionario();
            int IdFuncionario = oAdmFuncionario.Insert(oFuncionario: oFuncionario);

            if (IdFuncionario <= 0)
            {
                return false;
            }

            Usuario oUsuario = new Usuario
            {
                IdEmpresa = IdEmpresa,
                IdFuncionario = IdFuncionario,
                Login = propUsuario.GetValue(Cadastro, null).ToString(),
                Senha = propSenha.GetValue(Cadastro, null).ToString()
            };

            AdmUsuario oAdmUsuario = new AdmUsuario();
            int IdUsuario = oAdmUsuario.Insert(oUsuario: oUsuario);

            return IdUsuario > 0;
        }
    }
}