using System.Collections.Generic;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para empresa / salões
    /// </summary>
    public class Empresa : ModelBase
    {
        /// <summary>
        /// inicializa uma nova instância da classe
        /// </summary>
        public Empresa()
        {
        }

        /// <summary>
        /// Nome da empresa / salão
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Nome fantasia da empresa / salão
        /// </summary>
        public string NomeFantasia { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de inscrição: 1 - CPF / 2 - CNPJ
        /// </summary>
        public int TipoInscricao { get; set; } = int.MinValue;

        /// <summary>
        /// Número de inscrição da empresa / salão
        /// </summary>
        public string NumInscricao { get; set; } = string.Empty;

        /// <summary>
        /// Número de inscrição da empresa / salão
        /// </summary>
        public string Inscricao { get; set; } = string.Empty;


        /// <summary>
        /// Endereço da empresa / salão
        /// </summary>
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Bairro da empresa / salão
        /// </summary>
        public string Bairro { get; set; } = string.Empty;

        /// <summary>
        /// Cidade da empresa / salão
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// UF da empresa / salão
        /// </summary>
        public string Uf { get; set; } = string.Empty;

        /// <summary>
        /// CEP da empresa / salão
        /// </summary>
        public string Cep { get; set; } = string.Empty;

        /// <summary>
        /// Lista dos meios de contato da empresa
        /// </summary>
        public List<ContatoEmpresa> LstContatoEmpresa { get; set; }
    }
}