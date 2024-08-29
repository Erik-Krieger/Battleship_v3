using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels;

public sealed class MultiplayerSetupViewModel : BaseViewModel
{
	private ICommand m_CmdHost;

	private ICommand m_CmdJoin;

	private ICommand m_CmdBackToMenu;

	public ICommand CmdHost => m_CmdHost ?? new CommandHandler(delegate
	{
		WindowManagerService.ChangeView(new HostMenuViewModel());
	});

	public ICommand CmdJoin => m_CmdJoin ?? new CommandHandler(delegate
	{
		WindowManagerService.ChangeView(new JoinMenuViewModel());
	});

	public ICommand CmdBackToMenu => m_CmdBackToMenu ?? new CommandHandler(delegate
	{
		WindowManagerService.ChangeView(new MainMenuViewModel());
	});
}
