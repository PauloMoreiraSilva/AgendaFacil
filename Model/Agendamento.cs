using System;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para um agendamento
    /// </summary>
    public class Agendamento : ModelBase
    {
        /// <summary>
        /// Inicializa a instância da classe
        /// </summary>
        public Agendamento()
        {
        }

        /// <summary>
        /// Identificação do funcionário
        /// </summary>
        public int IdFuncionario { get; set; } = int.MinValue;

        /// <summary>
        /// Identificação da cliente na empresa
        /// </summary>
        public int IdCliente { get; set; } = int.MinValue;

        /// <summary>
        /// Identificação do procedimento
        /// </summary>
        public int IdProcedimento { get; set; } = int.MinValue;

        /// <summary>
        /// Identificação do agendamento
        /// </summary>
        public int IdAgendamento { get; set; } = int.MinValue;

        /// <summary>
        /// Data e hora do início do atendimento
        /// </summary>
        public DateTime DataInicio { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Data e hora do final do atendimento
        /// </summary>
        public DateTime DataFim { get; set; } = DateTime.MinValue;

        /// <summary>
        /// flag que identifica se 0 - previsto (agendado), 1- confirmado  2 - em andamento, 3-finalizado
        /// </summary>
        public int Situacao { get; set; } = int.MinValue;

        public string NomeEmpresa { get; set; }  = string.Empty;

        public string NomeFuncionario { get; set; } = string.Empty;

        public string NomeCliente { get; set; } = string.Empty;

        public string NomeProcedimento { get; set; } = string.Empty;
    }
}
