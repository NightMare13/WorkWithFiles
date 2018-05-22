using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithFiles.Utility
{
    public class BindableBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Обновляет свойство если оно изменилось
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="value">Новое значение</param>
        /// <param name="storage">Backing Field для свойства</param>
        /// <param name="property">Имя свойства</param>
        protected virtual void UpdateValue<TProperty>(TProperty value, ref TProperty storage,
            [CallerMemberName] string property = default(string))
        {
            if (EqualityComparer<TProperty>.Default.Equals(storage, value))
            {
                return;
            }
            storage = value;
            this.OnPropertyChanged(property);
        }

        /// <summary>
        /// Классическая реализация вызова события <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// </summary>
        /// <param name="property">Имя свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string property = default(string))
        {
#if DEBUG
            this.AssertProperty(property);
#endif
            var arguments = new PropertyChangedEventArgs(property);
            PropertyChanged?.Invoke(this, arguments);
        }

        /// <summary>
        /// Обновляет свойство если оно изменилось (с помощью дерева выражений)
        /// Назначение - чтобы не писать "PropertyName" а использовать () => PropertyName
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="value">Новое значение</param>
        /// <param name="storage">Backing Field для свойства</param>
        /// <param name="expression">Выражение</param>
        protected void UpdateValue<TProperty>(TProperty value, ref TProperty storage,
            Expression<Func<TProperty>> expression) =>
            this.UpdateValue(value, ref storage, expression.ToName());

        /// <summary>
        /// Обновляет свойство если оно изменилось
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression">Выражение</param>
        protected void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> expression) =>
            this.OnPropertyChanged(expression.ToName());
    }
}
