using System;
using System.Windows.Input;

namespace SimpleMVVM.ViewModels
{
    public class CommandViewModel : BaseViewModel
    {
        public CommandViewModel(string displayName, ICommand command)
        {
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public ICommand Command { get; private set; }
    }
}
