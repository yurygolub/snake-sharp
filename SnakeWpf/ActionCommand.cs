using System;
using System.Windows.Input;

namespace SnakeWpf;

public class ActionCommand : ICommand
{
    private readonly Action execute;

    public ActionCommand(Action execute)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => this.execute();
}
