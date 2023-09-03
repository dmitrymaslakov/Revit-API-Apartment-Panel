using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfPanel.Domain;

namespace WpfPanel.ViewModel
{
    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _disposed;

        public ExternalEvent ExEvent { get; }

        public RequestHandler Handler { get; }

        public ViewModelBase(ExternalEvent exEvent, RequestHandler handler)
        {
            ExEvent = exEvent;
            Handler = handler;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;
        }
    }
}
