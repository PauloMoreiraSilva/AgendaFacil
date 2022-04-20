using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Caracteres de controle
    /// </summary>
    public static class ControlChars
    {
        /// <summary>
        /// Representa um caractere Nulo
        /// </summary>
        public static char NUL { get; set; }

        /// <summary>
        /// Representa o caractere Back space
        /// </summary>
        public static char BS { get; set; } = (char)8;

        /// <summary>
        /// Representa o caractere Carriage Return (quebra de linha)
        /// </summary>
        public static char CR { get; set; } = (char)13;

        /// <summary>
        /// Representa o caractere substituto
        /// </summary>
        public static char SUB { get; set; } = (char)26;
    }

    /// <summary>
    /// Verifica e valida o formato de arquivos.
    /// </summary>
    public class FormatsFiles
    {
        private readonly byte[] EXE_SIGNATURE = { 77, 90 };
        private StringBuilder sbMsgErros;

        /// <summary>
        /// Mensagens de erros encontrados no processo
        /// </summary>
        public string MsgErros
        {
            get
            {
                string sMsg = sbMsgErros.ToString();

                if (sMsg.Length > 2 && sMsg.Substring(sMsg.Length - 2) == Environment.NewLine)
                {
                    sMsg = sMsg.Substring(0, sMsg.Length - 2);
                }

                return sMsg;
            }
        }

        /// <summary>
        /// Retorna o código do erro
        /// </summary>
        public int CodigoErro { get; private set; } = int.MinValue;

        /// <summary>
        /// Configura o tamanho do CNAB a ser verificado
        /// </summary>
        public int TamanhoCnab { private get; set; } = int.MinValue;

        /// <summary>
        /// Inicializa uma nova instância da classe
        /// </summary>
        public FormatsFiles()
        {
        }

        /// <summary>
        /// Executa diversas validações no arquivo para garantir CNAB válido
        /// </summary>
        /// <param name="sFullPath">path do arquivo.</param>
        /// <returns>True se válido / False se inválido.</returns>
        public bool PreValidarCnab(string sFullPath = "")
        {
            if (string.IsNullOrEmpty(sFullPath))
                return false;

            CodigoErro = 0;
            sbMsgErros = new StringBuilder();

            try
            {
                if (VerificarExecutavel(sFullPath))
                {
                    CodigoErro = 1;
                    _ = sbMsgErros.Append("Arquivo é um executável");
                    return false;
                }

                if (VerificarBinario(sFullPath))
                {
                    CodigoErro = 2;
                    _ = sbMsgErros.Append("Arquivo é binário.");
                    return false;
                }

                if (!VerificarTamanhoValido(sFullPath))
                {
                    CodigoErro = 3;
                    _ = sbMsgErros.AppendFormat("Arquivo possui registro com tamanho diferente do esperado ({0} caracteres).", TamanhoCnab);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogErro.Gravar("FormatsFiles.ValidarCnab400() > " + ex.Message);
                CodigoErro = 4;
                _ = sbMsgErros.AppendFormat("Erro encontrado: {0} .", ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifica se o arquivo é executável
        /// </summary>
        /// <param name="sFullPath">path do arquivo</param>
        /// <returns>True: é executável / False: não é executável.</returns>
        private bool VerificarExecutavel(string sFullPath)
        {
            using FileStream oFS = new FileStream(sFullPath, FileMode.Open, FileAccess.Read);
            byte[] iBytes = new byte[2];
            _ = oFS.Read(iBytes, 0, iBytes.Length);

            return iBytes.SequenceEqual(EXE_SIGNATURE);
        }

        /// <summary>
        /// Verifica se o arquivo é binário
        /// </summary>
        /// <param name="sFullPath">path do arquivo</param>
        /// <returns>True: é binário / False: não é binário.</returns>
        private bool VerificarBinario(string sFullPath)
        {
            CultureInfo ciPTBR = new CultureInfo("pt-BR");
            Encoding encPTBR = Encoding.GetEncoding(ciPTBR.TextInfo.ANSICodePage);

            using FileStream oFS = new FileStream(sFullPath, FileMode.Open, FileAccess.Read);
            using (StreamReader oSR = new StreamReader(oFS, encPTBR))
            {
                int iCh = 0;
                while ((iCh = oSR?.Read() ?? 0) != -1)
                {
                    if (IsControlChar(iCh))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se a quantidade de caracteres do arquivo é válido.
        /// </summary>
        /// <param name="sFullPath">path do arquivo.</param>
        /// <returns>True: válido / False: inválido.</returns>
        private bool VerificarTamanhoValido(string sFullPath)
        {
            CultureInfo ciPTBR = new CultureInfo("pt-BR");
            Encoding encPTBR = Encoding.GetEncoding(ciPTBR.TextInfo.ANSICodePage);

            using FileStream oFS = new FileStream(sFullPath, FileMode.Open, FileAccess.Read);
            using StreamReader oSR = new StreamReader(oFS, encPTBR);
            string sRecordTmp = "";
            int iLinha = 0;

            while (sRecordTmp != null)
            {
                sRecordTmp = oSR?.ReadLine();
                iLinha++;

                if (iLinha > 3) //Trata apenas as 3 primeiras linhas
                {
                    break;
                }

                if (sRecordTmp?.Length != TamanhoCnab)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validação de arquivos XLS/XLSX
        /// </summary>
        /// <param name="sFullPath">path do arquivo.</param>
        /// <returns>True: xls/xlsx válido / False: inválido .</returns>
        public bool PreValidarXls(string sFullPath)
        {
            CodigoErro = 0;
            sbMsgErros = new StringBuilder();

            try
            {
                if (VerificarExecutavel(sFullPath))
                {
                    CodigoErro = 1;
                    _ = sbMsgErros.Append("Arquivo é um executável");
                    return false;
                }

                if (!VerificarBinario(sFullPath))
                {
                    CodigoErro = 2;
                    _ = sbMsgErros.Append("Arquivo não é xls (binário).");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogErro.Gravar("FormatsFiles.ValidarXls() > " + ex.Message);
                CodigoErro = 5;
                _ = sbMsgErros.AppendFormat("Erro encontrado: {0} .", ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifica se o caractere é de controle
        /// </summary>
        /// <param name="ch">caractere.</param>
        /// <returns>True: controle / False: não é controle.</returns>
        public static bool IsControlChar(int ch)
        {
            return (ch > ControlChars.NUL && ch < ControlChars.BS)
                || (ch > ControlChars.CR && ch < ControlChars.SUB);
        }
    }
}