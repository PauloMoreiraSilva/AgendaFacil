namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para Parâmetros da aplicação (configurações gerais)
    /// </summary>
    public class Parametro : ModelBase
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public Parametro()
        {
        }

        /// <summary>
        /// Identificação do parâmetro
        /// </summary>
        public int IdParametro { get; set; } = int.MinValue;

        /// <summary>
        /// Nome do parâmetro
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// breve descrição do parâmetro (para que serve, o que faz)
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor do parâmetro
        /// </summary>
        public string Valor { get; set; } = string.Empty;

        /// <summary>
        /// Tipo do valor do parâmetro. para atender de uma forma genérica, o tipo pode ser numérico, valor, percentual, texto, etc.
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// observações adicionais sobre como aplicar a configuração, como funciona etc.
        /// </summary>
        public string Observacao { get; set; } = string.Empty;
    }
}