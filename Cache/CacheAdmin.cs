using System;
using System.Web;

namespace PI4Sem.Cache
{
    /// <summary>
    /// Administra o cache da aplicação.
    /// </summary>
    public static class CacheAdmin
    {
        /// <summary>
        /// Verifica se um item existe no cache.
        /// </summary>
        /// <param name="CacheName">Nome do item.</param>
        /// <returns>True / False.</returns>
        public static bool Exists(string CacheName) =>
            HttpRuntime.Cache[CacheName] != null;

        /// <summary>
        /// Retorna o valor do item encontrado no cache.
        /// </summary>
        /// <param name="CacheName">O nome do item procurado.</param>
        /// <returns>Um objeto contendo o valor ou nulo.</returns>
        public static object GetValue(string CacheName) =>
            HttpRuntime.Cache[CacheName];

        /// <summary>
        /// Insere um valor no cache da aplicação.
        /// </summary>
        /// <param name="CacheName">Nome do item (string).</param>
        /// <param name="CacheValue">Valor do item (objeto).</param>
        /// <param name="slidingExpiration">Tempo para expiração do item no cache.</param>
        public static void SetValue(string CacheName = "", object CacheValue = null, TimeSpan? slidingExpiration = null)
        {
            slidingExpiration = slidingExpiration ?? new TimeSpan(72, 0, 0);

            HttpRuntime.Cache.Insert(CacheName, CacheValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, (TimeSpan)slidingExpiration);
        }

        /// <summary>
        /// Remove um item do cache.
        /// </summary>
        /// <param name="CacheName">nome do item a ser removido.</param>
        /// <returns>True / False.</returns>
        public static bool RemoveValue(string CacheName)
        {
            if (Exists(CacheName))
            {
                HttpRuntime.Cache.Remove(CacheName);
                return true;
            }
            return false;
        }
    }
}