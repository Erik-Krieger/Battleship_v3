using System.Collections.Generic;
using Models;
using Services;

namespace Battleship.ViewModels;

public sealed class GameViewModel : BaseViewModel
{
    private GameModel m_GameModel;

    private PlayingFieldViewModel m_OwnGrid;

    private PlayingFieldViewModel m_EnemyGrid;

    private TargetInputViewModel m_TargetInput;

    public PlayingFieldViewModel OwnGrid { get; set; }

    public PlayingFieldViewModel EnemyGrid { get; set; }

    public TargetInputViewModel TargetInput { get; set; } = new TargetInputViewModel();


    public GameViewModel( List<ushort> theOwnShipList, List<ushort> theEnemyShipList )
    {
        OwnGrid = new PlayingFieldViewModel( PlayerType.You, theOwnShipList );
        EnemyGrid = new PlayingFieldViewModel( PlayerType.Enemy, theEnemyShipList );
        m_GameModel = new GameModel( this );
    }

    public GameViewModel()
        : this( null, null )
    {
    }
}
