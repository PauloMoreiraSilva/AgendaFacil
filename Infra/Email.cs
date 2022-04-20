using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using PI4Sem.DataBase;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Executa o envio de emails da aplicação
    /// </summary>
    public class Email
    {
        private readonly Config.Config oConfig;
        private StringBuilder sbMsgErros;

        /// <summary>
        /// Retorna as mensagens de erro no processamento.
        /// </summary>
        public string MsgErros
        {
            get
            {
                string sMsg = sbMsgErros.ToString();

                if (sMsg.Length > 2 && sMsg.Substring(startIndex: sMsg.Length - 2) == Environment.NewLine)
                {
                    sMsg = sMsg.Substring(0, sMsg.Length - 2);
                }

                return sMsg;
            }
        }

        /// <summary>
        /// inicializa a instância da classe email
        /// </summary>
        public Email()
        {
            oConfig = new Config.Config();
        }

        /// <summary>
        /// Envia o email
        /// </summary>
        /// <param name="sEmailsDestinatarios">destinatários</param>
        /// <param name="sAssunto">Assunto</param>
        /// <param name="sConteudo">Conteúdo</param>
        /// <param name="sPathAnexos">Anexos</param>
        /// <param name="sEmailCCAdicionais">Email cópia adicional</param>
        /// <param name="lstLinkedRes">Recursos</param>
        /// <returns>True: Enviado / False: Falhou.</returns>
        public bool Enviar(string sEmailsDestinatarios = "",
                            string sAssunto = "",
                            string sConteudo = "",
                            string sPathAnexos = "",
                            string sEmailCCAdicionais = "",
                            List<LinkedResource> lstLinkedRes = null)
        {
            sbMsgErros = new StringBuilder();
            bool bReturn = false;

            if (string.IsNullOrEmpty(sEmailsDestinatarios))
            {
                _ = sbMsgErros.Append("E-mail sem destinatários.");
                return bReturn;
            }

            MailMessage objEmail = null;
            Encoding oEncoding = Encoding.GetEncoding("ISO-8859-1");

            try
            {
                objEmail = new MailMessage();

                ConfigurarMailMessage(ref objEmail, sEmailsDestinatarios, sEmailCCAdicionais);
                ConfigurarAnexos(ref objEmail, sPathAnexos);
                ConfigurarLogo(ref objEmail, ref sConteudo);

                objEmail.Priority = MailPriority.Normal;
                objEmail.Subject = sAssunto;
                objEmail.SubjectEncoding = oEncoding;
                objEmail.BodyEncoding = oEncoding;

                sConteudo = sConteudo + "\n\nCopyright " + DateTime.Today.Year.ToString() + " " + oConfig.Key.EmailCopyright;

                AlternateView plainView = AlternateView.CreateAlternateViewFromString(sConteudo, oEncoding, "text/plain");
                objEmail.AlternateViews.Add(plainView);

                sConteudo = sConteudo.Replace("\n", "<br/>");
                sConteudo = "<html><body>" + sConteudo + "</body></html>";

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(sConteudo, oEncoding, "text/html");

                if (lstLinkedRes != null)
                {
                    foreach (LinkedResource oLR in lstLinkedRes)
                    {
                        htmlView.LinkedResources.Add(oLR);
                    }
                }

                objEmail.AlternateViews.Add(htmlView);

                objEmail.IsBodyHtml = true;
                objEmail.Body = sConteudo;

                objEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay;

                string sEmailMessageID = "<" + Guid.NewGuid().ToString() + "@" + objEmail.From.Host + ">"; //Padrão RFC 2822

                objEmail.Headers.Add("Message-ID", sEmailMessageID);
                objEmail.Headers.Add("X-Mailer", "Microsoft Outlook 16.0");

                string sLocalIP = GetLocalIP();

                if (!string.IsNullOrEmpty(sLocalIP))
                {
                    objEmail.Headers.Add("X-Sender-IP", sLocalIP);
                }

                ConfigurarAWSSES(ref objEmail);

                //SSL é obrigatório para AWS SES
                SmtpClient objSmtp = new SmtpClient
                {
                    Credentials = new NetworkCredential(oConfig.Key.EMailUsuario, oConfig.Key.EMailSenha),
                    Host = oConfig.Key.SMTPServer,
                    Port = string.IsNullOrEmpty(oConfig.Key.SmtpPort) ?
                               587 :
                               Convert.ToInt32(oConfig.Key.SmtpPort),
                    EnableSsl = true
                };

                objSmtp.Send(objEmail);

                bReturn = true;
            }
            catch (Exception ex)
            {
                sbMsgErros.Append(ex.Message).Append(Environment.NewLine);
                EventLog.LogErrorEvent(ex);
            }
            finally
            {
                objEmail?.Dispose();
            }

            return bReturn;
        }

        /// <summary>
        /// Configuração inicial do objeto MailMessage
        /// </summary>
        /// <param name="objEmail">o objeto retornado.</param>
        /// <param name="sEmailsDestinatarios">destinatários</param>
        /// <param name="sEmailCCAdicionais">cópias adicionais</param>
        private void ConfigurarMailMessage(ref MailMessage objEmail, string sEmailsDestinatarios, string sEmailCCAdicionais)
        {
            string sEmailReply = string.IsNullOrEmpty(oConfig.Key.EmailReply) ?
                                 oConfig.Key.EmailRemetente :
                                 oConfig.Key.EmailReply;

            objEmail.From = new MailAddress(oConfig.Key.EmailRemetente, oConfig.Key.EmailDisplay);
            objEmail.ReplyToList.Add(new MailAddress(sEmailReply, oConfig.Key.EmailDisplay));

            foreach (string sEmail in sEmailsDestinatarios.Split(';'))
            {
                if (!string.IsNullOrEmpty(sEmail))
                {
                    objEmail.To.Add(sEmail.ToLower());
                }
            }

            foreach (string sEmail in oConfig.Key.EmailCC.Split(';'))
            {
                if (!string.IsNullOrEmpty(sEmail))
                {
                    objEmail.CC.Add(sEmail.ToLower());
                }
            }

            foreach (string sEmail in sEmailCCAdicionais.Split(';'))
            {
                if (!string.IsNullOrEmpty(sEmail))
                {
                    objEmail.CC.Add(sEmail.ToLower());
                }
            }
        }

        /// <summary>
        /// inclui os anexos na mensagem de e-mail
        /// </summary>
        /// <param name="objEmail">o objeto email.</param>
        /// <param name="sPathAnexos">path dos anexos.</param>
        private void ConfigurarAnexos(ref MailMessage objEmail, string sPathAnexos)
        {
            if (!string.IsNullOrEmpty(sPathAnexos))
            {
                foreach (string sAnexo in sPathAnexos.Split(';'))
                {
                    if ((sAnexo != "") && (File.Exists(sAnexo)))
                    {
                        objEmail.Attachments.Add(new Attachment(sAnexo));
                    }
                }
            }
        }

        /// <summary>
        /// Configura o Logotipo na mensagem
        /// </summary>
        /// <param name="objEmail">objeto email.</param>
        /// <param name="sConteudo">conteúdo adicionado.</param>
        private void ConfigurarLogo(ref MailMessage objEmail, ref string sConteudo)
        {
            if (!string.IsNullOrEmpty(oConfig.Key.PathLogo))
            {
                const string contentID = "Image";
                Attachment inlineLogo = new Attachment(oConfig.Key.PathLogo.ToString())
                {
                    ContentId = contentID
                };
                inlineLogo.ContentDisposition.Inline = true;
                inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;

                objEmail.Attachments.Add(inlineLogo);

                sConteudo = sConteudo + "\n\n<img width=\"80px\" height=\"40px\" align=\"left\" src=\"cid:" + contentID + "\"/>\n\n\n";
            }
        }

        /// <summary>
        /// Retorna o IP local
        /// </summary>
        /// <returns>A string.</returns>
        private string GetLocalIP()
        {
            string sReturn = "";

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    sReturn = addr.ToString();
                    break;
                }
            }

            return sReturn;
        }

        /// <summary>
        /// Permite sempre aceitar o certificado do servidor
        /// </summary>
        /// <param name="sender">emissor.</param>
        /// <param name="certificate">certificado.</param>
        /// <param name="chain">autoridade.</param>
        /// <param name="policyErrors">política de erros.</param>
        /// <returns>True.</returns>
        private static bool AllwaysGoodCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        /// <summary>
        /// Configurações específicas necessárias para utilização da AWS SES
        /// </summary>
        /// <param name="objEmail">objeto E-Mail.</param>
        private void ConfigurarAWSSES(ref MailMessage objEmail)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AllwaysGoodCertificate);
            //Aceita, Ignora certificado do servidor

            if (!string.IsNullOrEmpty(oConfig.Key.SESConfigSet))
            {
                //Define o Configuration Set para captura de eventos
                objEmail.Headers.Add("X-SES-CONFIGURATION-SET", oConfig.Key.SESConfigSet);
            }

            if (!string.IsNullOrEmpty(oConfig.Key.SESMsgtags))
            {
                //Define a tag dos logs gerados pela captura de eventos
                objEmail.Headers.Add("X-SES-MESSAGE-TAGS", oConfig.Key.SESMsgtags);
            }

            //SES Limite de Envios (padrão após sair da Sandbox)
            //Quota: de Envios 50.000 emails por um período de 24
            //Taxa máxima de envio: 14 emails/segundo

            if (!string.IsNullOrEmpty(oConfig.Key.SESDelaySleep))
            {
                //Forçar um delay para evitar atingir o limite por segundo quando tiver vários serviços paralelamente
                int iDelaySleep = Convert.ToInt32(oConfig.Key.SESDelaySleep);

                if (iDelaySleep > 0)
                {
                    Thread.Sleep(iDelaySleep); //milissegundos
                }
            }
        }
    }
}