using System.Windows.Input;
namespace woOshLaundrySystem.ViewModels;
public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute; private readonly Func<object?,bool>? _can;
    public RelayCommand(Action<object?> execute, Func<object?,bool>? can=null){_execute=execute;_can=can;}
    public bool CanExecute(object? p)=>_can==null||_can(p); public void Execute(object? p)=>_execute(p); public event EventHandler? CanExecuteChanged { add{CommandManager.RequerySuggested+=value;} remove{CommandManager.RequerySuggested-=value;} }
}
