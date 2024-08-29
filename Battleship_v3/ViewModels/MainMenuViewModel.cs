using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels;

public sealed class MainMenuViewModel : BaseViewModel
{
	private ICommand m_CmdSingleplayer;

	public ICommand CmdSingleplayer => m_CmdSingleplayer ?? (m_CmdSingleplayer = new CommandHandler(delegate
	{
		WindowManagerService.ChangeView(new SingleplayerSetupViewModel());
	}));
}
