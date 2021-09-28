using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleMVVM.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected BaseViewModel()
        {
        }

        public virtual string DisplayName { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyName([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
