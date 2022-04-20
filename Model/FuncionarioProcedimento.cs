namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para Funcionários e Procedimentos
    /// </summary>
    public class FuncionarioProcedimento : ModelBase
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public FuncionarioProcedimento()
        {
        }

        /// <summary>
        /// Identificação do funcionário
        /// </summary>
        public int IdFuncionario { get; set; } = int.MinValue;

        /// <summary>
        /// Identificação do procedimento
        /// </summary>
        public int IdProcedimento { get; set; } = int.MinValue;
    }
}