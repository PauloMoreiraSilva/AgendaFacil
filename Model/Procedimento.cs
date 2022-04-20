using System.Collections.Generic;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para procedimentos (serviços prestados)
    /// </summary>
    public class Procedimento : ModelBase
    {
        /// <summary>
        /// inicializa uma instância da classe
        /// </summary>
        public Procedimento()
        {
        }

        /// <summary>
        /// Identificação do procedimento na empresa
        /// </summary>
        public int IdProcedimento { get; set; } = int.MinValue;

        /// <summary>
        /// Nome do procedimento
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do procedimento
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Material e ferramentas necessárias para executar o procedimento
        /// </summary>
        public string MaterialNecessario { get; set; } = string.Empty;

        /// <summary>
        /// quantidade em minutos do tempo total previsto para a execução do procedimento
        /// </summary>
        public int TempoPrevisto { get; set; } = int.MinValue;

        /// <summary>
        /// flag que identifica se permite o atendimento de vários clientes de forma simultânea, ou seja, intercalado.
        /// </summary>
        public int EhIntercalavel { get; set; } = int.MinValue;

        /// <summary>
        /// se o procedimento é intercalável, armazena a quantidade de atendimentos simultâneos (pontos de atendimento) possíveis.
        /// </summary>
        public int QtdSimultaneo { get; set; } = int.MinValue;

        /// <summary>
        /// se o procedimento pode ser intercalável, armazena o tempo necessário de intercalação entre os atendimentos simultâneos
        /// </summary>
        public int TempoIntercalado { get; set; } = int.MinValue;

        /// <summary>
        /// Lista dos funcionários que executam esse procedimento
        /// </summary>
        public List<Funcionario> LstFuncionario { get; set; }

        public string NomeEmpresa { get; set; } = string.Empty;
    }
}