using System;
using System.Web;
using System.Linq;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Classe que busca informações do cliente (browser).
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        protected Client()
        {
        }

        /// <summary>
        /// Retorna i IP do usuário.
        /// </summary>
        /// <returns>IP do usuário.</returns>
        public static string GetIPAddress()
        {
            return GetIPAddress(new HttpRequestWrapper(HttpContext.Current.Request));
        }

        /// <summary>
        /// Retorna o IP do usuário
        /// </summary>
        /// <param name="request">Requisição http.</param>
        /// <returns>O IP do usuário.</returns>
        internal static string GetIPAddress(HttpRequestBase request)
        {
            // Manipula o header padrão
            string forwarded = request.Headers["Forwarded"];
            if (!string.IsNullOrEmpty(forwarded))
            {
                foreach (var (ip, left, right) in from string segment in forwarded.Split(',')[0].Split(';')
                                                  let pair = segment.Trim().Split('=')
                                                  where pair.Length == 2 && pair[0].Equals("for", StringComparison.OrdinalIgnoreCase)
                                                  let ip = pair[1].Trim('"')// IPv6 são sempre envolvidos por aspas
                                                  let left = ip.IndexOf('[')
                                                  let right = ip.IndexOf(']')
                                                  select (ip, left, right))
                {
                    if (left == 0 && right > 0)
                    {
                        return ip.Substring(1, right - 1);
                    }
                    // separa a porta do IPv4
                    int colon = ip.IndexOf(':');
                    if (colon != -1)
                    {
                        return ip.Substring(0, colon);
                    }
                    // retorna IPv4, desconhecido e IPv4 ofuscado
                    return ip;
                }
            }

            // manipula header não padronizado
            string xForwardedFor = request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(xForwardedFor))
                return xForwardedFor.Split(',')[0];

            return request.UserHostAddress;
        }
    }
}