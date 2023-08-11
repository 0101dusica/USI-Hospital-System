using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZdravoCorp.Utils.Command
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public virtual bool CanExecute(object parameter) => true;
        public abstract void Execute(object? parameter);
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        protected void InvokeResultCallback(bool success)
        {
            ResultCallback?.Invoke(success);
        }
        protected Action<bool>? ResultCallback { get; set; }
    }
}
