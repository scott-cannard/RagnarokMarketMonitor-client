using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace RMMClient
{
    public abstract class ObservableItem : DispatcherObject, INotifyPropertyChanged
    {
        private static readonly Dictionary<string, PropertyChangedEventArgs> eventArgCache;

        static ObservableItem()
        {
            eventArgCache = new Dictionary<string, PropertyChangedEventArgs>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (null != handler)
            {
                PropertyChangedEventArgs args = GetPropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(() => { }));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var lambda = expression as LambdaExpression;

            if (null == lambda) return;

            var memberExpression = (lambda.Body is UnaryExpression)
                ? ((UnaryExpression)lambda.Body).Operand as MemberExpression
                : lambda.Body as MemberExpression;
            this.OnPropertyChanged(memberExpression != null ? memberExpression.Member.Name : String.Empty);
        }

        protected virtual void NotifyPropertyChanged(string propertyName = "")
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private static PropertyChangedEventArgs GetPropertyChangedEventArgs(string propertyName)
        {
            PropertyChangedEventArgs args;

            lock (typeof(ObservableItem))
            {
                if (!eventArgCache.TryGetValue(propertyName, out args))
                    eventArgCache.Add(propertyName, new PropertyChangedEventArgs(propertyName));

                args = eventArgCache[propertyName];
            }
            return args;
        }

        public void AppDispatch(Action lambda)
        {
            if (null == lambda) return;
            App.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(lambda));
        }
    }
}
