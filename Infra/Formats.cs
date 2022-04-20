using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Classe auxiliar com diversas funções de formatação
    /// </summary>
    public class Formats
    {
        /// <summary>
        /// Inicializa a instância da classe
        /// </summary>
        public Formats()
        {
        }

        /// <summary>
        /// Retorna a descrição do tipo de inscrição
        /// </summary>
        /// <param name="TipoInscricao">Tipo de inscrição.</param>
        /// <returns>Descrição do tipo de Inscrição.</returns>
        public static string GetDescTipoInscricao(object TipoInscricao)
        {
            return Convert.ToInt32(TipoInscricao) switch
            {
                1 => "CPF",
                2 => "CNPJ",
                _ => "",
            };
        }

        /// <summary>
        /// Retorna a Inscrição formatada de acordo com o tipo
        /// </summary>
        /// <param name="TipoInscricao">O tipo da inscrição.</param>
        /// <param name="Inscricao">A inscrição.</param>
        /// <returns>Inscrição formatada como CPF ou CNPJ.</returns>
        public static string GetInscricaoFormatada(object TipoInscricao = null, object Inscricao = null)
        {
            if (string.IsNullOrEmpty(TipoInscricao.ToString()))
            {
                TipoInscricao = (Inscricao.ToString().Length <= 11) ? 1 : 2;
            }

            return Convert.ToInt32(TipoInscricao) switch
            {
                1 => string.Format(@"{0:000\.000\.000\-00}", Convert.ToDouble(Inscricao)),
                2 => string.Format(@"{0:00\.000\.000\/0000\-00}", Convert.ToDouble(Inscricao)),
                _ => "",
            };
        }

        /// <summary>
        /// Retorna o CEP formatado
        /// </summary>
        /// <param name="CEP">CEP.</param>
        /// <returns>CEP Formatado.</returns>
        public static string GetCEPFormatado(object CEP)
        {
            CEP = CEP.ToString().Replace("-", "");
            return (!string.IsNullOrEmpty(CEP.ToString())) ? string.Format(@"{0:00000\-000}", Convert.ToDouble(CEP)) : "";
        }

        /// <summary>
        /// Retorna um telefone formatado
        /// </summary>
        /// <param name="Telefone">Telefone.</param>
        /// <returns>Telefone formatado.</returns>
        public static string GetTelefoneFormatado(object Telefone)
        {
            string sReturn = "";

            Telefone = Telefone.ToString().Replace("-", "");

            if (!string.IsNullOrEmpty(Telefone.ToString()))
            {
                sReturn = Telefone.ToString().Length == 9
                    ? string.Format(@"{0:0 0000\-0000}", Convert.ToDouble(Telefone))
                    : string.Format(@"{0:0000\-0000}", Convert.ToDouble(Telefone));
            }

            return sReturn;
        }

        /// <summary>
        /// Retorna a descrição quando boleano
        /// </summary>
        /// <param name="Codigo">Código.</param>
        /// <returns>0 = Não / 1 = Sim.</returns>
        public static string GetDescBoolean(object Codigo)
        {
            if (!string.IsNullOrEmpty(Codigo?.ToString()))
            {
                return Convert.ToInt32(Codigo) switch
                {
                    0 => "Não",
                    1 => "Sim",
                    _ => "",
                };
            }
            return "Não";
        }

        /// <summary>
        /// Verifica se um CPF é válido.
        /// </summary>
        /// <param name="vrCPF">CPF formatado ou não.</param>
        /// <returns>True: Válido / False: Inválido.</returns>
        public static bool ValidaCPF(string vrCPF)
        {
            string valor = GetNumbers(vrCPF);

            if (valor.Length > 11)
            {
                return false;
            }

            valor = valor.PadLeft(11, '0');

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
            {
                if (valor[i] != valor[0])
                {
                    igual = false;
                }
            }

            if (igual || valor == "12345678909")
            {
                return false;
            }

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
            {
                if (!int.TryParse(valor[i].ToString(), out int iNumero))
                {
                    return false;
                }
                numeros[i] = iNumero;
            }

            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    return false;
                }
            }
            else if (numeros[9] != (11 - resultado))
            {
                return false;
            }

            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                {
                    return false;
                }
            }
            else if (numeros[10] != (11 - resultado))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valida um CNPJ, formatado ou não
        /// </summary>
        /// <param name="vrCNPJ">CNPJ</param>
        /// <returns>True: válido / False: inválido.</returns>
        public static bool ValidaCNPJ(string vrCNPJ)
        {
            string CNPJ = GetNumbers(vrCNPJ);

            if (CNPJ.Length > 14)
            {
                return false;
            }

            if (CNPJ.Length < 14)
            {
                CNPJ = CNPJ.PadLeft(14, '0');
            }

            int[] digitos, soma, resultado;
            const string ftmt = "6543298765432";

            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;

            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;

            bool[] CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                int nrDig;
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)
                    {
                        soma[0] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1));
                    }

                    if (nrDig <= 12)
                    {
                        soma[1] += digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1));
                    }
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = soma[nrDig] % 11;

                    CNPJOk[nrDig] = (resultado[nrDig] == 0) || (resultado[nrDig] == 1)
                                    ? digitos[12 + nrDig] == 0
                                    : digitos[12 + nrDig] == (11 - resultado[nrDig]);
                }

                return CNPJOk[0] && CNPJOk[1];
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gera um hash MD5 de uma string
        /// </summary>
        /// <param name="data">texto.</param>
        /// <returns>Hash MD5 do texto.</returns>
        public static string CalculateMD5(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            MD5 md5 = MD5.Create();
            byte[] bHash = md5.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bHash.Length; i++)
            {
                _ = sb.Append(bHash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Criptografa um texto informado com SHA2
        /// </summary>
        /// <param name="sTexto">texto.</param>
        /// <param name="chave">chave para criptografia</param>
        /// <returns>Texto criptografado.</returns>
        public static string Criptografar(string sTexto = "", string chave = "")
        {
            Config.Config oConfig = new Config.Config();
            string EncryptionKey = string.IsNullOrEmpty(chave) ? oConfig.Key.PkCriptografia : chave;
            byte[] clearBytes = Encoding.Unicode.GetBytes(sTexto);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, 32);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                }
                sTexto = Convert.ToBase64String(ms.ToArray());
            }
            return sTexto;
        }

        /// <summary>
        /// Descriptografa um texto que foi criptografado com SHA2
        /// </summary>
        /// <param name="sTexto">Texto.</param>
        /// <returns>A string.</returns>
        public static string Descriptografar(string sTexto = "", string chave = "")
        {
            Config.Config oConfig = new Config.Config();
            string EncryptionKey = string.IsNullOrEmpty(chave) ? oConfig.Key.PkCriptografia : chave;

            sTexto = sTexto.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(sTexto);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, 32);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                }
                sTexto = Encoding.Unicode.GetString(ms.ToArray());
            }
            return sTexto;
        }

        /// <summary>
        /// Retorna a linha Digitável formatada
        /// </summary>
        /// <param name="LinhaCorrida">linha corrida.</param>
        /// <returns>Linha Digitável formatada.</returns>
        public static string FormataLinhaDigitavel(string LinhaCorrida)
        {
            string sTmp = LinhaCorrida.PadLeft(47, '0');

            return string.Format("{0}.{1} {2}.{3} {4}.{5} {6} {7}",
                    sTmp.Substring(0, 5), sTmp.Substring(5, 5),
                    sTmp.Substring(10, 5), sTmp.Substring(15, 6),
                    sTmp.Substring(21, 5), sTmp.Substring(26, 6),
                    sTmp.Substring(32, 1), sTmp.Substring(33, 14)
                    );
        }

        /// <summary>
        /// Testa se uma string possui apenas números.
        /// </summary>
        /// <param name="s">texto</param>
        /// <returns>True: apenas números / False.</returns>
        public static bool IsNumeric(string s)
        {
            return float.TryParse(s, out _);
        }

        /// <summary>
        /// Testa se um texto é uma data válida
        /// </summary>
        /// <param name="Data">texto.</param>
        /// <param name="DataFormatada">Retorna data formatada.</param>
        /// <param name="Formato">o formato.</param>
        /// <returns>True: é válido / False.</returns>
        public static bool IsDate(string Data, out DateTime DataFormatada, string Formato = "yyyyMMdd")
        {
            string sNull = new string('0', 8);
            Data = GetNumbers(Data);
            Data = Data.Length > 8 ? Data.Substring(0, 8) : Data;

            if (!string.IsNullOrEmpty(Data) && Data?.Length == 8 && Data != sNull)
            {
                switch (Formato)
                {
                    case "yyyyMMdd":
                        Data = Data.Substring(0, 4) + "-" + Data.Substring(4, 2) + "-" + Data.Substring(6, 2);
                        break;

                    case "ddMMyyyy":
                        Data = Data.Substring(0, 2) + "/" + Data.Substring(2, 2) + "/" + Data.Substring(4, 4);
                        break;

                    default:
                        break;
                }
            }

            return DateTime.TryParse(Data, out DataFormatada);
        }

        /// <summary>
        /// Formata uma data para o formato específico.
        /// </summary>
        /// <param name="Data">Data.</param>
        /// <param name="Formato">Formato.</param>
        /// <returns>Data formatada em texto.</returns>
        public static string GetDate(DateTime? Data = null, string Formato = "dd/MM/yyyy")
        {
            return Data == null ? string.Empty : Convert.ToDateTime(Data).ToString(Formato);
        }

        /// <summary>
        /// Converte o UF (sigla ou extenso) de acordo com o tipo
        /// </summary>
        /// <param name="sEstado">UF (sigla ou extenso).</param>
        /// <param name="iTipo">tipo de conversão.</param>
        /// <returns>UF</returns>
        public static string GetUF(string sEstado, int iTipo = 1)
        {
            if (iTipo == 1)
            {
                return GetUFSiglaExtenso(sEstado);
            }
            else if (iTipo == 2)
            {
                return GetUFExtensoSigla(sEstado);
            }
            return string.Empty;
        }

        /// <summary>
        /// Retorna a UF por extenso conforme a sigla recebida
        /// </summary>
        /// <param name="sEstado">sigla do estado.</param>
        /// <returns>UF por extenso</returns>
        private static string GetUFSiglaExtenso(string sEstado = "")
        {
            if (string.IsNullOrEmpty(sEstado))
                return string.Empty;

            return sEstado.ToUpper() switch
            {
                "AC" => "Acre",
                "AL" => "Alagoas",
                "AM" => "Amazonas",
                "AP" => "Amapá",
                "BA" => "Bahia",
                "CE" => "Ceará",
                "DF" => "Distrito Federal",
                "ES" => "Espírito Santo",
                "GO" => "Goiás",
                "MA" => "Maranhão",
                "MG" => "Minas Gerais",
                "MS" => "Mato Grosso do Sul",
                "MT" => "Mato Grosso",
                "PA" => "Pará",
                "PB" => "Paraíba",
                "PE" => "Pernambuco",
                "PI" => "Piauí",
                "PR" => "Paraná",
                "RJ" => "Rio de Janeiro",
                "RN" => "Rio Grande do Norte",
                "RO" => "Rondônia",
                "RR" => "Roraima",
                "RS" => "Rio Grande do Sul",
                "SC" => "Santa Catarina",
                "SE" => "Sergipe",
                "SP" => "São Paulo",
                "TO" => "Tocantins",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Retorna a sigla do estado recebido por extenso.
        /// </summary>
        /// <param name="sEstado">estado por extenso.</param>
        /// <returns>UF (sigla).</returns>
        private static string GetUFExtensoSigla(string sEstado = "")
        {
            switch (sEstado.ToUpper())
            {
                case "ACRE":
                    return "AC";

                case "ALAGOAS":
                    return "AL";

                case "AMAZONAS":
                    return "AM";

                case "AMAPÁ":
                case "AMAPA":
                    return "AP";

                case "BAHIA":
                    return "BA";

                case "CEARÁ":
                case "CEARA":
                    return "CE";

                case "DISTRITO FEDERAL":
                    return "DF";

                case "ESPÍRITO SANTO":
                case "ESPIRITO SANTO":
                    return "ES";

                case "GOIÁS":
                case "GOIAS":
                    return "GO";

                case "MARANHÃO":
                case "MARANHAO":
                    return "MA";

                case "MINAS GERAIS":
                    return "MG";

                case "MATO GROSSO DO SUL":
                    return "MS";

                case "MATO GROSSO":
                    return "MT";

                case "PARÁ":
                case "PARA":
                    return "PA";

                case "PARAÍBA":
                case "PARAIBA":
                    return "PB";

                case "PERNAMBUCO":
                    return "PE";

                case "PIAUÍ":
                case "PIAUI":
                    return "PI";

                case "PARANÁ":
                case "PARANA":
                    return "PR";

                case "RIO DE JANEIRO":
                    return "RJ";

                case "RIO GRANDE DO NORTE":
                    return "RN";

                case "RONDÔNIA":
                case "RONDONIA":
                    return "RO";

                case "RORAIMA":
                    return "RR";

                case "RIO GRANDE DO SUL":
                    return "RS";

                case "SANTA CATARINA":
                    return "SC";

                case "SERGIPE":
                    return "SE";

                case "SÃO PAULO":
                case "SAO PAULO":
                case "SAOPAULO":
                case "SAO_PAULO":
                    return "SP";

                case "TOCANTÍNS":
                case "TOCANTINS":
                    return "TO";

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Retorna a sigla da moeda de acordo com código
        /// </summary>
        /// <param name="sCodigo">código da moeda.</param>
        /// <returns>A string.</returns>
        public static string GetMoeda(string sCodigo)
        {
            return sCodigo.ToUpper() switch
            {
                "00" => "BRL",
                "01" => "USD",
                "02" => "EUR",
                _ => "BRL",
            };
        }

        /// <summary>
        /// Retorna o código da moeda de acordo com a sigla
        /// </summary>
        /// <param name="sCodigo">sigla da moeda.</param>
        /// <returns>código.</returns>
        public static string GetCodMoeda(string sCodigo)
        {
            return sCodigo.ToUpper() switch
            {
                "BRL" => "00",
                "USD" => "01",
                "EUR" => "02",
                _ => "",
            };
        }

        /// <summary>
        /// retorna um único objeto data com as horas informadas
        /// </summary>
        /// <param name="Horas">texto com horas.</param>
        /// <param name="Data">uma data.</param>
        /// <param name="dDataSaida">retorna data completa com as hora.</param>
        public static void AddHoras(string Horas, DateTime Data, out DateTime dDataSaida)
        {
            Horas = GetNumbers(Horas);
            dDataSaida = Data;

            if (!string.IsNullOrEmpty(Horas) && Horas?.Length == 6 && Horas.Trim() != "000000")
            {
                double h = Convert.ToDouble(Horas.Substring(0, 2));
                double m = Convert.ToDouble(Horas.Substring(2, 2));
                double s = Convert.ToDouble(Horas.Substring(4, 2));

                dDataSaida = Convert.ToDateTime(dDataSaida).AddHours(h).AddMinutes(m).AddSeconds(s);
            }
        }

        /// <summary>
        /// hack para int32: converte um texto para int .
        /// </summary>
        /// <param name="sValue">texto.</param>
        /// <returns>numero ou int.MinValue.</returns>
        public static int HackInt32(string sValue = "")
        {
            return !string.IsNullOrEmpty(sValue.Trim()) && int.TryParse(sValue.Trim(), out int aux)
                   ? aux
                   : int.MinValue;
        }

        /// <summary>
        /// hack para long: converte um texto para long .
        /// </summary>
        /// <param name="sValue">texto.</param>
        /// <returns>numero ou long.MinValue.</returns>
        public static long? HackInt64(string sValue = "")
        {
            return !string.IsNullOrEmpty(sValue.Trim()) && long.TryParse(sValue.Trim(), out long aux)
                    ? aux
                    : long.MinValue;
        }

        /// <summary>
        /// Hack para double: converte um texto para double.
        /// </summary>
        /// <param name="sValue">texto.</param>
        /// <returns>double ou double.MinValue .</returns>
        public static double HackDouble(string sValue = "")
        {
            if (string.IsNullOrEmpty(sValue.Trim()))
            {
                return double.MinValue;
            }

            if (Double.TryParse(sValue.Trim(), out double aux))
            {
                return aux;
            }
            return double.MinValue;
        }

        /// <summary>
        /// Remove todas as tags HTML de uma string
        /// </summary>
        /// <param name="input">string.</param>
        /// <returns>sem tags.</returns>
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        /// <summary>
        /// Retorna somente números de uma string
        /// </summary>
        /// <param name="s">string.</param>
        /// <returns>números.</returns>
        public static string GetNumbers(string s = "")
        {
            return Regex.Replace(s, "[^0-9]", "");
        }

        /// <summary>
        /// Formata um valor em reais
        /// </summary>
        /// <param name="valor">valor.</param>
        /// <param name="Simbolo">If true, inclui símbolo.</param>
        /// <returns>valor em reais.</returns>
        public static string EmReal(double valor = 0, bool Simbolo = false)
        {
            valor = Math.Round(valor, 2, 0);
            string real = valor.ToString("C", CultureInfo.CreateSpecificCulture("pt-BR"));
            return Simbolo ? real.Replace("R$", "R$ ") : real.Replace("R$", "");
        }

        /// <summary>
        /// Retorna uma data válida de acordo com formato
        /// </summary>
        /// <param name="sValue">texto.</param>
        /// <param name="Format">formato.</param>
        /// <param name="DateValid">a data válida, ou DateTime.MinValue.</param>
        /// <returns>True: data válida / false: data inválida .</returns>
        protected bool FormatarData(string sValue, string Format, out DateTime DateValid)
        {
            string sDate = "";

            switch (Format)
            {
                case "AAMMDD":
                    if (sValue.Length == 6)
                    {
                        sDate = sValue.Substring(4, 2) + "/" + sValue.Substring(2, 2) + "/" + sValue.Substring(0, 2);
                    }
                    break;

                case "AAAAMMDD":
                    if (sValue.Length == 8)
                    {
                        sDate = sValue.Substring(6, 2) + "/" + sValue.Substring(4, 2) + "/" + sValue.Substring(0, 4);
                    }
                    break;

                case "DDMMAA":
                    if (sValue.Length == 6)
                    {
                        sDate = sValue.Substring(0, 2) + "/" + sValue.Substring(2, 2) + "/" + sValue.Substring(4, 2);
                    }
                    break;

                case "DDMMAAAA":
                    if (sValue.Length == 8)
                    {
                        sDate = sValue.Substring(0, 2) + "/" + sValue.Substring(2, 2) + "/" + sValue.Substring(4, 4);
                    }
                    break;

                case "MMAAAA":
                    if (sValue.Length == 6)
                    {
                        sDate = sValue.Substring(0, 2) + "/" + sValue.Substring(2, 4);
                    }
                    break;

                case "AAAAMM":
                    if (sValue.Length == 6)
                    {
                        sDate = sValue.Substring(4, 2) + "/" + sValue.Substring(0, 4);
                    }
                    break;

                default:
                    break;
            }

            return DateTime.TryParse(sDate, out DateValid);
        }

        /// <summary>
        /// Formata uma string para numérico.
        /// </summary>
        /// <param name="sValue">texto.</param>
        /// <param name="iDecimalPlaces">casas decimais.</param>
        /// <param name="NumericValid">número válido.</param>
        /// <returns>True: válido / False: inválido.</returns>
        protected bool FormatarNumero(string sValue, int iDecimalPlaces, out double NumericValid)
        {
            sValue = sValue.Replace(" ", "X").Replace(".", "X").Replace(",", "X").Replace("-", "X");
            bool bReturn = double.TryParse(sValue, out NumericValid);

            if (!bReturn)
                return false;

            string decimalAux = "1".PadRight(iDecimalPlaces, '0');
            NumericValid /= Convert.ToInt32(decimalAux);

            return NumericValid != 0;
        }

        /// <summary>
        /// Retorna os caracteres a direita de uma string.
        /// </summary>
        /// <param name="original">A string original.</param>
        /// <param name="numberCharacters">Número de caracteres a ser retornado.</param>
        /// <returns>String com os caracteres à direita.</returns>
        public static string StringRight(string original, int numberCharacters)
        {
            if (original.Length > 0)
            {
                return original.Length - numberCharacters >= 0
                       ? original.Substring(original.Length - numberCharacters)
                       : original;
            }
            return "";
        }

        /// <summary>
        /// Verifica se uma string contém apenas números
        /// </summary>
        /// <param name="texto">O texto.</param>
        /// <returns>True / False.</returns>
        public static bool ContemSoNumeros(string texto) =>
            texto.Count(c => char.IsNumber(c)) == texto.Length;

        /// <summary>
        /// Retorna a descrição de um tipo de arquivo.
        /// </summary>
        /// <param name="Tipo">A sigla do tipo (R/T).</param>
        /// <returns>R = remessa / T = retorno.</returns>
        public static string GetDescOrigemArquivo(object Tipo = null)
        {
            switch (Tipo.ToString())
            {
                case "R":
                    return "Remessa";

                case "T":
                    return "Retorno";

                default:
                    break;
            }
            return "";
        }
    }
}