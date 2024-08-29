using System.Windows;
using System.Windows.Input;
using Battleship.Utility;

namespace Battleship.ViewModels;

public class AudioControlViewModel : BaseViewModel
{
    private Visibility m_WindowVisibility = Visibility.Hidden;

    private ICommand m_CmdToggleVisibilityButtonPressed;

    public Visibility WindowVisibility
    {
        get
        {
            return m_WindowVisibility;
        }
        set
        {
            m_WindowVisibility = value;
            NotifyPropertyChanged( "WindowVisibility" );
        }
    }

    public ICommand CmdToggleVisibilityButtonPressed => m_CmdToggleVisibilityButtonPressed ?? new CommandHandler( delegate
    {
    } );
}
