namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para formas de contato da empresa
    /// </summary>
    public class ContatoEmpresa : ModelBase
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public ContatoEmpresa()
        {
        }

        /// <summary>
        /// Identificação do contato na empresa
        /// </summary>
        public int IdContato { get; set; } = int.MinValue;

        /// <summary>
        /// Tipo: W: Whatsapp\nT: Telefone fixo\nC: celular\nF: URL do Facebook\nS: site\nI: Instagram
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Valor de acordo com o tipo de contato especificado
        /// </summary>
        public string Valor { get; set; } = string.Empty;
    }
}