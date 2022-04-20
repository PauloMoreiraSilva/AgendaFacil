using System.Collections.Generic;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para Funcionário
    /// </summary>
    public class Funcionario : ModelBase
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public Funcionario()
        {
        }

        /// <summary>
        /// Identificação do funcionário
        /// </summary>
        public int IdFuncionario { get; set; } = int.MinValue;

        /// <summary>
        /// Nome do funcionário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CPF do funcionário
        /// </summary>
        public string Cpf { get; set; } = string.Empty;

        /// <summary>
        /// Telefone do funcionário
        /// </summary>
        public string Telefone { get; set; } = string.Empty;

        /// <summary>
        /// Email do funcionário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Flag que indica que é proprietário / sócio
        /// </summary>
        public int EhProprietario { get; set; } = int.MinValue;

        /// <summary>
        /// Lista dos procedimentos que o funcionário pode executar
        /// </summary>
        public List<Procedimento> LstProcedimento { get; set; }

        public string NomeEmpresa { get; set; } = string.Empty;
    }
}