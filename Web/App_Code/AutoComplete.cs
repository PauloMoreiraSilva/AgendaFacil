using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using PI4Sem.DAL;
using PI4Sem.Infra;
using PI4Sem.Model;

namespace WebServiceAutoComplete
{
    /// <summary>
    /// Implementa o serviço de AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    // Para permitir que esse Web Service seja chamado via script usando ASP.NET AJAX, ative a linha abaixo.
    // [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoComplete"/> class.
        /// </summary>
        public AutoComplete()
        {
            //Se estiver usando em componentes ative a linha abaixo
#pragma warning disable S125 // Sessões não devem conter comentários
            //InitializeComponent();
#pragma warning restore S125 // Sessões não devem conter comentários
        }

        /// <summary>
        /// Retorna a lista de compradores para o combo AutoComplete
        /// </summary>
        /// <param name="prefixText">parte do teto.</param>
        /// <param name="count">contador.</param>
        /// <returns>lista de compradores.</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public List<string> GetAutoCompleteListSacado(string prefixText, int count = 0)
        {
            int iLimite = count <= 0 ? 30 : count;
            //AdmComprador oAdmComprador = new AdmComprador();
            //List<Comprador> lstComprador = oAdmComprador.SelectRows(MaxRows: iLimite, Nome: prefixText);
            List<string> sComprador = new List<string>();

            //foreach (Comprador oComprador in lstComprador)
            //{
            //    string sAuxNomeComprador = oComprador.Nome.Length > 38 ? oComprador.Nome.Substring(0, 34) + "..." : oComprador.Nome;
            //    sComprador.Add(Formats.GetInscricaoFormatada(oComprador.TipoInscricao, oComprador.NumInscricao) + "|" + oComprador.CodigoSAP + "|" + sAuxNomeComprador);
            //}
            return sComprador;
        }
    }
}