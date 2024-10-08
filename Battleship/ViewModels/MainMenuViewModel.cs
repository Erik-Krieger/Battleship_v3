using System.Windows.Input;
using Battleship.Services;
using Battleship.Utility;

namespace Battleship.ViewModels;

public sealed class MainMenuViewModel : BaseViewModel
{
    private ICommand m_CmdSingleplayer;

    public ICommand CmdSingleplayer => m_CmdSingleplayer ?? ( m_CmdSingleplayer = new CommandHandler( delegate
    {
        WindowManagerService.ChangeView( new SingleplayerSetupViewModel() );
    } ) );
}
