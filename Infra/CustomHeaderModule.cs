using System;
using System.Web;

namespace PI4Sem.Infra
{
    /// <summary>
    /// Classe que possibilita o envio de header customizados
    /// </summary>
    public class CustomHeaderModule : IHttpModule
    {
        /// <summary>
        /// Inicializador.
        /// </summary>
        /// <param name="context">contexto.</param>
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        /// <summary>
        /// Elimina todas as instâncias
        /// </summary>
        public void Dispose()
        {
            // Método intencionalmente vazio
        }

        /// <summary>
        /// Permite enviar header customizado
        /// </summary>
        /// <param name="sender">objeto chamador.</param>
        /// <param name="e">evento.</param>
        public void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("Server"); Ou pode configurar algo
            HttpContext.Current.Response.Headers.Set("Server", "Server OK");
        }
    }
}
