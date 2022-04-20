using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI4Sem.Config;

namespace PI4Sem.Business
{
    /// <summary>
    /// Classe de base para todas as classes do tipo Biz. Possui propriedades e métodos comuns.
    /// Business Logic Layer - Camada de Lógica / Regras de Negócios
    /// </summary>
    public class BusinessBase
    {
        /// <summary>
        /// Dados do Web.Config
        /// </summary>
        public Config.Config WebConfig { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessBase"/> class.
        /// </summary>
        public BusinessBase()
        {
            WebConfig = new Config.Config();
        }
    }
}
