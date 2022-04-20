namespace PI4Sem.Model
{
    /// <summary>
    /// Modelo para usuário da aplicação
    /// </summary>
    public class Usuario : Funcionario
    {
        /// <summary>
        /// Inicializa uma instância da classe
        /// </summary>
        public Usuario()
        {
        }

        /// <summary>
        /// login de acesso
        /// </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// senha cadastrada
        /// </summary>
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// flag que identifica o status do usuário.\n0 - ativo\n1 - alterar senha\n2 - bloqueado
        /// </summary>
        public int Situacao { get; set; } = int.MinValue;

        public string NomeFuncionario { get; set; } = string.Empty;
        public string EmailFuncionario { get; set; } = string.Empty;
        public string NomeEmpresa { get; set; } = string.Empty;
    }
}