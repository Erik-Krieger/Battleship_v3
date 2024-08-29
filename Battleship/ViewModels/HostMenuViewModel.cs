using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Battleship.Enemies;
using Battleship.Services;
using Battleship.Ships;
using Battleship.Utility;

namespace Battleship.ViewModels;

public sealed class HostMenuViewModel : BaseViewModel
{
    private ICommand m_CmdBegin;

    public ICommand CmdBegin => m_CmdBegin ?? new CommandHandler( delegate
    {
        startGame();
    }, () => NetworkService.Instance.NetworkPeer.PeerConnected );

    public HostMenuViewModel()
    {
        NetworkService.Instance.OpenServer();
        GameManagerService.Instance.SelectDifficulty( Difficulty.Person );
        ( (EnemyPerson)GameManagerService.Instance.Opponent ).InjectNetworkPeer( NetworkService.Instance.NetworkPeer );
    }

    private void startGame()
    {
        PlayerType playerType = GameManagerService.Instance.SetFirstTurnRandom() == PlayerType.You ? PlayerType.Enemy : PlayerType.You;
        ObservableCollection<Ship> theShipList = GameManagerService.Instance.GenerateShipList();
        ObservableCollection<Ship> theShipList2 = GameManagerService.Instance.GenerateShipList();
        string text = NetworkService.ConvertShipListToStringRep( theShipList );
        string text2 = NetworkService.ConvertShipListToStringRep( theShipList2 );
        NetworkService.Instance.NetworkPeer.SendMessage( $"start-game,{(int)playerType},{text},{text2}" );
        ( (EnemyPerson)GameManagerService.Instance.Opponent ).EventEnabled = !GameManagerService.Instance.YourTurn;
        List<ushort> theOwnShipList = NetworkService.ConvertStringRepToUshortList( text );
        List<ushort> theEnemyShipList = NetworkService.ConvertStringRepToUshortList( text2 );
        WindowManagerService.ChangeView( new GameViewModel( theOwnShipList, theEnemyShipList ) );
    }
}
