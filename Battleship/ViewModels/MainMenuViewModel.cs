using System.Windows.Input;
using Services;
using Utility;

namespace Battleship.ViewModels;

public sealed class MainMenuViewModel : BaseViewModel
{
    private ICommand m_CmdSingleplayer;

    public ICommand CmdSingleplayer => m_CmdSingleplayer ?? ( m_CmdSingleplayer = new CommandHandler( delegate
    {
        WindowManagerService.ChangeView( new SingleplayerSetupViewModel() );
    } ) );
}
