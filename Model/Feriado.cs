using System;

namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para feriados
    /// </summary>
    public class Feriado : ModelBase
    {
        /// <summary>
        /// inicializa instância da classe
        /// </summary>
        public Feriado()
        {
        }

        /// <summary>
        /// Identificação do feriado na empresa
        /// </summary>
        public int IdFeriado { get; set; } = int.MinValue;

        /// <summary>
        /// Flag que indica se o feriado é fixo (mesma data em todos os anos): 1 = Sim / 0 = Não
        /// </summary>
        public int Fixo { get; set; } = int.MinValue;

        /// <summary>
        /// Se o feriado é fixo, qual o dia
        /// </summary>
        public string Dia { get; set; } = string.Empty;

        /// <summary>
        /// Se o feriado é fixo, qual o mês
        /// </summary>
        public string Mes { get; set; } = string.Empty;

        /// <summary>
        /// Para feriados móveis, a data (se fixo, DateTime.MinValue)
        /// </summary>
        public DateTime Data { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Descrição do feriado
        /// </summary>
        public string Descricao { get; set; } = string.Empty;
    }
}