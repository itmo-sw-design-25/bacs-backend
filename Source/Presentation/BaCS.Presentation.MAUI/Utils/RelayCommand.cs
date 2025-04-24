namespace BaCS.Presentation.MAUI.Utils;

using System.Windows.Input;

public class RelayCommand<T> : ICommand
{
    private Action<T> execute;
    private Func<T, bool> canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        if (parameter is T tObject)
            return this.canExecute == null || this.canExecute(tObject);

        return false;
    }

    public void Execute(object parameter)
    {
        if (parameter is T tObject)
            this.execute(tObject);
    }
}

public class RelayCommand : ICommand
{
    private Action execute;
    private Func<bool> canExecute;

    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter = null)
    {
        return this.canExecute == null || this.canExecute();
    }

    public void Execute(object parameter = null)
    {
        this.execute();
    }
}

public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke() ?? true);
    }

    public async void Execute(object parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
