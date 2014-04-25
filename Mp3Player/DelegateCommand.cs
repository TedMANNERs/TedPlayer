using System;
using System.Windows.Input;

namespace Mp3Player
{
    public delegate bool CommandCanExecute();
    public delegate void Command(object parameter);

    /// <summary>
    /// Helper class to make commands execute methods
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Command command;
        private readonly CommandCanExecute canExecute;

        public DelegateCommand(Command command, CommandCanExecute canExecute)
        {
            this.command = command;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            command(parameter);
        }
    }
}
