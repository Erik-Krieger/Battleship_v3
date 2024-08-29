using System.Windows.Input;
using Battleship_v2.Enemies;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels;

public sealed class SingleplayerSetupViewModel : BaseViewModel
{
	private ICommand m_CmdBegin;

	public ICommand CmdBegin => m_CmdBegin ?? new CommandHandler(delegate
	{
		GameManagerService.Instance.SelectDifficulty((Difficulty)DifficultyIndex);
		WindowManagerService.ChangeView(new GameViewModel());
	});

	public int DifficultyIndex { get; set; } = 1;

}
