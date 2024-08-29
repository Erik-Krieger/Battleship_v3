using System.Collections.Generic;
using System.Windows.Input;
using Enemies;
using Services;
using Utility;

namespace Battleship.ViewModels;

public sealed class JoinMenuViewModel : BaseViewModel
{
    private ICommand m_CmdJoin;

    public string Hostname { get; set; } = "127.0.0.1";


    public ICommand CmdJoin => m_CmdJoin ?? new CommandHandler( delegate
    {
        connect();
    }, () => Hostname != "" );

    private void connect()
    {
        NetworkService.Instance.JoinServer( Hostname );
        GameManagerService.Instance.SelectDifficulty( Difficulty.Person );
    }

    public void BeginGame( List<ushort> theListOfYourShips, List<ushort> theListOfEnemyShips )
    {
        ( (EnemyPerson)GameManagerService.Instance.Opponent ).InjectNetworkPeer( NetworkService.Instance.NetworkPeer );
        ( (EnemyPerson)GameManagerService.Instance.Opponent ).EventEnabled = !GameManagerService.Instance.YourTurn;
        WindowManagerService.ChangeView( new GameViewModel( theListOfYourShips, theListOfEnemyShips ) );
    }
}
