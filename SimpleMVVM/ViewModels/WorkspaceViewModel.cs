using System;
using System.Windows.Input;
using SimpleMVVM.Commands;

namespace SimpleMVVM.ViewModels
{
    public abstract class WorkspaceViewModel : BaseViewModel
    {
        protected WorkspaceViewModel()
        {
        }

        private DelegateCommand m_closeCommand;

        public ICommand CloseCommand
        {
            get
            {
                if (m_closeCommand == null)
                {
                    m_closeCommand = new DelegateCommand(parameter => RaiseRequestCloseEvent());
                }

                return m_closeCommand;
            }
        }

        public event EventHandler RequestClose;

        protected void RaiseRequestCloseEvent()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
