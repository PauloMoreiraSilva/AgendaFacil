using System;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para cliente
    /// </summary>
    public class Cliente : ModelBase
    {
        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public Cliente()
        {
        }

        /// <summary>
        /// Identificação da cliente na empresa
        /// </summary>
        public int IdCliente { get; set; } = int.MinValue;

        /// <summary>
        /// Nome da cliente
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Telefone da cliente
        /// </summary>
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// E-mail principal da cliente
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento da cliente
        /// </summary>
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Endereço da cliente
        /// </summary>
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Bairro da cliente
        /// </summary>
        public string Bairro { get; set; } = string.Empty;

        /// <summary>
        /// Cidade da cliente
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// UF da cliente
        /// </summary>
        public string Uf { get; set; } = string.Empty;

        /// <summary>
        /// CEP da cliente
        /// </summary>
        public string Cep { get; set; } = string.Empty;

        /// <summary>
        /// Observações diversas para/sobre a cliente
        /// </summary>
        public string Notas { get; set; } = string.Empty;

        public string NomeEmpresa { get; set; } = string.Empty;
    }
}