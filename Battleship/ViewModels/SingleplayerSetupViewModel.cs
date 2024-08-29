using System.Windows.Input;
using Battleship.Enemies;
using Battleship.Services;
using Battleship.Utility;

namespace Battleship.ViewModels;

public sealed class SingleplayerSetupViewModel : BaseViewModel
{
    private ICommand m_CmdBegin;

    public ICommand CmdBegin => m_CmdBegin ?? new CommandHandler( delegate
    {
        GameManagerService.Instance.SelectDifficulty( (Difficulty)DifficultyIndex );
        WindowManagerService.ChangeView( new GameViewModel() );
    } );

    public int DifficultyIndex { get; set; } = 1;

}
