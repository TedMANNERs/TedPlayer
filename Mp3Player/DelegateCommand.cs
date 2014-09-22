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
        private readonly CommandCanExecute _canExecute;
        private readonly Command _command;

        public DelegateCommand(Command command, CommandCanExecute canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public void Execute(object parameter)
        {
            _command(parameter);
        }
    }
}