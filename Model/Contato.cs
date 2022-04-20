using System;

namespace Guasti.Model
{
    public class Contato : ModelBase
    {
        private int _Codigo;
        private int _CodigoCedente;
        private int _CodigoSacado;
        private string _CodigoSAP = "";
        private string _Nome = "";
        private string _CodigoFuncaoContato = "";
        private string _DescFuncaoContato = "";
        private string _DDD = "";
        private string _Telefone = "";
        private string _DDDCelular = "";
        private string _Celular = "";
        private string _Ramal = "";
        private string _Email1 = "";
        private string _Email2 = "";
        private string _Email3 = "";

        private string _DebitoVencido = "";
        private string _RemessaEmail = "";
        private string _EnvioDebito10 = "";
        private string _EnvioDebito25 = "";
        private string _EnvioDebitoAmbos = "";

        private DateTime? _DataCadastro = DateTime.MinValue;
        private DateTime? _DataAlteracao = DateTime.MinValue;
        private DateTime? _DataExclusao = DateTime.MinValue;

        private DateTime _DataInclusao = DateTime.MinValue;
        private int _CodigoArquivo;

        private string _NomePagador = "";
        private string _NumInscricao = "";

        public Contato()
        {
        }

        public int Codigo { get => _Codigo; set => _Codigo = value; }
        public int CodigoSacado { get => _CodigoSacado; set => _CodigoSacado = value; }
        public string CodigoSAP { get => _CodigoSAP; set => _CodigoSAP = value; }
        public string Nome { get => Formatted(_Nome); set => _Nome = value; }
        public string CodigoFuncaoContato { get => _CodigoFuncaoContato; set => _CodigoFuncaoContato = value; }
        public string DescFuncaoContato { get => _DescFuncaoContato; set => _DescFuncaoContato = value; }
        public string DDD { get => _DDD; set => _DDD = value; }
        public string Telefone { get => _Telefone; set => _Telefone = value; }
        public string Ramal { get => _Ramal; set => _Ramal = value; }
        public string Email1 { get => Formatted(_Email1); set => _Email1 = value; }
        public string Email2 { get => Formatted(_Email2); set => _Email2 = value; }
        public string Email3 { get => Formatted(_Email3); set => _Email3 = value; }
        public string DebitoVencido { get => _DebitoVencido; set => _DebitoVencido = value; }
        public string RemessaEmail { get => _RemessaEmail; set => _RemessaEmail = value; }
        public string EnvioDebito10 { get => _EnvioDebito10; set => _EnvioDebito10 = value; }
        public string EnvioDebito25 { get => _EnvioDebito25; set => _EnvioDebito25 = value; }
        public string EnvioDebitoAmbos { get => _EnvioDebitoAmbos; set => _EnvioDebitoAmbos = value; }
        public DateTime? DataCadastro { get => _DataCadastro; set => _DataCadastro = value; }
        public DateTime? DataAlteracao { get => _DataAlteracao; set => _DataAlteracao = value; }
        public DateTime? DataExclusao { get => _DataExclusao; set => _DataExclusao = value; }
        public DateTime DataInclusao { get => _DataInclusao; set => _DataInclusao = value; }
        public int CodigoArquivo { get => _CodigoArquivo; set => _CodigoArquivo = value; }
        public string NomePagador { get => _NomePagador; set => _NomePagador = value; }
        public string NumInscricao { get => _NumInscricao; set => _NumInscricao = value; }
        public int CodigoCedente { get => _CodigoCedente; set => _CodigoCedente = value; }
        public string DDDCelular { get => _DDDCelular; set => _DDDCelular = value; }
        public string Celular { get => _Celular; set => _Celular = value; }

        //Campos que aparecem no formulário e que podem ter seu valor apagado ou zerado em uma alteração
        //ou seja, tinha um valor e está retornando a vazio ou zerado
        public string[] CamposZerados()
        {
            return new string[] { "DDD", "Telefone", "Ramal", "DDDCelular", "Celular", "Email2", "Email3",
                                  "DebitoVencido", "RemessaEmail", "EnvioDebito10", "EnvioDebito25", "EnvioDebitoAmbos" };

        }
    }
}