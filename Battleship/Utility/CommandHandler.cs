using System;
using System.Windows.Input;

namespace Battleship.Utility;

public class CommandHandler : ICommand
{
    private Action m_Action;

    private Func<bool> m_CanExecute;

    public event EventHandler CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }
        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }

    public CommandHandler( Action theAction, Func<bool> canExecute = null )
    {
        m_Action = theAction;
        m_CanExecute = canExecute;
    }

    public bool CanExecute( object parameter )
    {
        if ( m_CanExecute != null )
        {
            return m_CanExecute();
        }
        return true;
    }

    public void Execute( object parameter )
    {
        m_Action();
    }
}
