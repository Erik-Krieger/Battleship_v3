using System.Windows;
using System.Windows.Input;
using Battleship.Services;
using Battleship.Utility;

namespace Battleship.ViewModels;

public class BaseViewModel : PropertyChangeHandler
{
    private ICommand m_CmdOpenMainMenu;

    private ICommand m_CmdClose;

    private ICommand m_CmdOpenMultiplayerMenu;

    public ICommand CmdOpenMainMenu => m_CmdOpenMainMenu ?? new CommandHandler( delegate
    {
        WindowManagerService.OpenMainMenu();
    } );

    public ICommand CmdClose => m_CmdClose ?? ( m_CmdClose = new CommandHandler( delegate
    {
        Application.Current.Shutdown();
    } ) );

    public ICommand CmdOpenMultiplayerMenu => m_CmdOpenMultiplayerMenu ?? new CommandHandler( delegate
    {
        WindowManagerService.OpenMultiplayerMenu();
    } );
}
