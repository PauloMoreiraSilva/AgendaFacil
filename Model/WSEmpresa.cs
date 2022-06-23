using System.Collections.Generic;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para empresa / salões
    /// </summary>
    public class WSEmpresa : ModelBase
    {
        /// <summary>
        /// inicializa uma nova instância da classe
        /// </summary>
        public WSEmpresa()
        {
        }

        /// <summary>
        /// Nome da empresa / salão
        /// </summary>
        public string cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Nome da empresa / salão
        /// </summary>
        public string nome { get; set; } = string.Empty;

        /// <summary>
        /// endereco da empresa
        /// </summary>
        public string logradouro { get; set; } = string.Empty;

        /// <summary>
        /// numero endereco da empresa
        /// </summary>
        public string numero { get; set; } = string.Empty;

        /// <summary>
        /// complemento do endereco da empresa
        /// </summary>
        public string complemento { get; set; } = string.Empty;

        /// <summary>
        /// cep da empresa
        /// </summary>
        public string cep { get; set; } = string.Empty;

        /// <summary>
        /// bairro do endereço da empresa
        /// </summary>
        public string bairro { get; set; } = string.Empty;

        /// <summary>
        /// cidade do endereço da empresa
        /// </summary>
        public string municipio { get; set; } = string.Empty;

        /// <summary>
        /// uf do endereço da empresa
        /// </summary>
        public string uf { get; set; } = string.Empty;

        /// <summary>
        /// email da empresa
        /// </summary>
        public string email { get; set; } = string.Empty;
    }
}