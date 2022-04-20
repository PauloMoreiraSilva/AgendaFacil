using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Controla o processamento de parâmetros passados via QueryString
    /// </summary>
    public class QueryStringModule : IHttpModule
    {
        /// <summary>
        /// nome do parâmetro para encaminhamento
        /// </summary>
        private const string PARAMETER_NAME = "enc=";

        /// <summary>
        /// Nome do parâmetro para chave de criptografia
        /// </summary>
        private const string ENCRYPTION_KEY = "key";

        private static readonly byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());

        /// <summary>
        /// Elimina a instância da memória
        /// </summary>
        public void Dispose()
        {
            // Método intencionalmente vazio
        }

        /// <summary>
        /// Inicializa o contexto para a aplicação
        /// </summary>
        /// <param name="context">contexto.</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }

        /// <summary>
        /// Evento delegado para executar ao iniciar o contexto
        /// </summary>
        /// <param name="sender">disparador do evento.</param>
        /// <param name="e">evento.</param>
        private void Context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context?.Request?.Url?.OriginalString?.Contains("aspx") == true && context?.Request?.RawUrl?.Contains("?") == true)
            {
                string query = ExtractQuery(context.Request.RawUrl);
                string path = GetVirtualPath();

                if (query.StartsWith(PARAMETER_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    string rawQuery = query.Replace(PARAMETER_NAME, string.Empty);
                    string decryptedQuery = Decrypt(rawQuery);
                    context.RewritePath(path, string.Empty, decryptedQuery);
                }
                else if (context.Request.HttpMethod == "GET")
                {
                    string encryptedQuery = Encrypt(query);
                    context.Response.Redirect(path + encryptedQuery);
                }
            }
        }

        /// <summary>
        /// Retorna o path virtual da URL atual
        /// </summary>
        /// <returns>A string.</returns>
        private static string GetVirtualPath()
        {
            string path = HttpContext.Current?.Request?.RawUrl ?? "";
            path = path.Substring(0, path.IndexOf("?"));
            path = path.Substring(path.LastIndexOf("/") + 1);
            return path;
        }

        /// <summary>
        /// Extrai a query da URL
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>querystring.</returns>
        private static string ExtractQuery(string url)
        {
            int index = url.IndexOf("?") + 1;
            return url.Substring(index);
        }

        /// <summary>
        /// Criptografa a string passada via QueryString
        /// </summary>
        /// <param name="inputText">texto.</param>
        /// <returns>texto criptografado.</returns>
        public static string Encrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(inputText);
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            using ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            return "?" + PARAMETER_NAME + Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        /// Descriptografa os parâmetros passados via QueryString.
        /// </summary>
        /// <param name="inputText">texto criptografado.</param>
        /// <returns>texto descriptografado.</returns>
        public static string Decrypt(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(inputText);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            using ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            using MemoryStream memoryStream = new MemoryStream(encryptedData);
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainText = new byte[encryptedData.Length];
            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
            return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
        }
    }
}