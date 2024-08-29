using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels;

public sealed class GameOverViewModel : BaseViewModel
{
	private ICommand m_CmdPlayAgain;

	private ICommand m_CmdGoToMenu;

	public object GameStatusPanel { get; set; }

	public ICommand CmdPlayAgain => m_CmdPlayAgain ?? new CommandHandler(delegate
	{
		playAgain();
	});

	public ICommand CmdGoToMenu => m_CmdGoToMenu ?? new CommandHandler(delegate
	{
		WindowManagerService.ChangeView(new MainMenuViewModel());
	});

	public GameOverViewModel(bool isWinner)
	{
		if (isWinner)
		{
			GameStatusPanel = new WinnerViewModel();
		}
		else
		{
			GameStatusPanel = new LoserViewModel();
		}
	}

	private void playAgain()
	{
		WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new MainMenuViewModel();
	}
}
