using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PI4Sem.Model
{
    /// <summary>
    /// Base para classes do tipo Model, com funções e propriedades comuns.
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// Código da empresa / Id / Chave primária. Deve estar presente em todas as tabelas
        /// </summary>
        public int IdEmpresa { get; set; } = int.MinValue;

        /// <summary>
        /// Data de inclusão do registro. Deve estar presente em todas as tabelas
        /// </summary>
        public DateTime DataInclusao { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Usado para exclusão lógica: 1 = excluído. Deve estar presente em todas as tabelas
        /// </summary>
        public int Deleted { get; set; } = int.MinValue;

        /// <summary>
        /// Utilizado para pesquisas no banco de dados.
        /// Representa a quantidade máxima de registros a ser retornada.
        /// </summary>
        public int MaxRows { get; set; } = int.MinValue;

        /// <summary>
        /// Utilizado para pesquisas no banco de dados.
        /// Representa a o índice do registro a ser retornada, quando estiver paginando.
        /// </summary>
        public int StartRowIndex { get; set; } = int.MinValue;

        /// <summary>
        /// Utilizado para pesquisas no banco de dados.
        /// Representa o registro com o qual o resultado será ordenado.
        /// </summary>
        public string SortField { get; set; } = string.Empty;

        /// <summary>
        /// Utilizado para pesquisas no banco de dados.
        /// É um booleano 0 / 1 que representa se o numero de registros (Count) deverá ser usado
        /// </summary>
        public int CountAutomatico { get; set; } = int.MinValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        protected ModelBase()
        {
        }

        /// <summary>
        /// Retorna uma string em CAIXA ALTA.
        /// </summary>
        /// <param name="texto">a string.</param>
        /// <returns>string em caixa alta.</returns>
        public string Formatted(string texto)
        {
            return !string.IsNullOrEmpty(texto) ? texto.Trim().ToUpper() : texto;
        }

        /// <summary>
        /// Extensão para converter um objeto em outro. ex: oUsuario para oUserLoggedInfo
        /// </summary>
        /// <typeparam name="T">tipo do objeto alvo</typeparam>
        /// <param name="myobj">objeto a ser convertido.</param>
        /// <returns>objeto alvo.</returns>
        public T Cast<T>(Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => z.Select(c => c.Name) //d
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                if (myobj?.GetType()?.GetProperty(memberInfo.Name)?.GetValue(myobj, null) != null)
                {
                    value = myobj?.GetType()?.GetProperty(memberInfo.Name)?.GetValue(myobj, null);
                    if (value != null)
                    {
                        propertyInfo.SetValue(x, value, null);
                    }
                }
            }
            return (T)x;
        }
    }
}