using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Battleship.Enemies;
using Battleship.Models;
using Battleship.Ships;
using Battleship.Utility;

namespace Battleship.Services;

public class GameManagerService : PropertyChangeHandler
{
    private static int m_RandomSeed = DateTime.UtcNow.Millisecond;

    private static Random aRng = new Random();

    public const int GRID_SIZE = 10;

    private PlayerType m_CurrentTurn;

    public Enemy Opponent { get; private set; }

    public PlayerType CurrentTurn
    {
        get
        {
            return m_CurrentTurn;
        }
        set
        {
            SetProperty( ref m_CurrentTurn, value, "CurrentTurn" );
            if ( OwnGrid != null && EnemyGrid != null )
            {
                OwnGrid.SetTurnIndicator( YourTurn );
                EnemyGrid.SetTurnIndicator( !YourTurn );
            }
        }
    }

    public bool YourTurn => CurrentTurn == PlayerType.You;

    public PlayingFieldModel OwnGrid { get; set; }

    public PlayingFieldModel EnemyGrid { get; set; }

    public TargetInputModel TargetInput { get; set; }

    public static GameManagerService Instance { get; private set; } = new GameManagerService();


    public GameManagerService InjectShipGridModel( PlayingFieldModel theModel, bool isOwnGrid = true )
    {
        if ( isOwnGrid )
        {
            OwnGrid = theModel;
        }
        else
        {
            EnemyGrid = theModel;
        }
        if ( OwnGrid != null && EnemyGrid != null )
        {
            CurrentTurn = CurrentTurn;
        }
        return this;
    }

    public GameManagerService InjectTargetInputModel( TargetInputModel theModel )
    {
        TargetInput = theModel;
        return this;
    }

    private GameManagerService()
    {
    }

    public PlayerType SetFirstTurnRandom()
    {
        CurrentTurn = PlayerType.You;
        return CurrentTurn;
    }

    public void SetFirstTurnFromInt( int thePlayerType )
    {
        switch ( thePlayerType )
        {
            case 0:
                CurrentTurn = PlayerType.You;
                break;
            case 1:
                CurrentTurn = PlayerType.Enemy;
                break;
            default:
                throw new ArgumentException( $"The value of the playerType can only be 0 or 1, {thePlayerType} is an invalid input", "thePlayerType" );
        }
    }

    private int convertLetterIndex( char theLetter )
    {
        int num = theLetter;
        num -= 65;
        return num >= 32 ? num - 32 : num;
    }

    private bool tryGetPosition( string theTargetString, out Position theMove )
    {
        theMove = null;
        if ( string.IsNullOrWhiteSpace( theTargetString ) )
        {
            return false;
        }
        if ( theTargetString.Length < 2 )
        {
            return false;
        }
        char c = theTargetString[0];
        string s = theTargetString.Substring( 1 );
        if ( !char.IsLetter( c ) )
        {
            return false;
        }
        if ( !int.TryParse( s, out var result ) )
        {
            return false;
        }
        theMove = new Position( convertLetterIndex( c ), result - 1 );
        return true;
    }

    public void ProcessShot( string theTargetString )
    {
        if ( tryGetPosition( theTargetString, out var theMove ) )
        {
            processShot( theMove, EnemyGrid );
            if ( Opponent is EnemyPerson )
            {
                NetworkService.Instance.NetworkPeer.SendMessage( theMove.ToMessage() );
            }
        }
    }

    public void ProcessShot( int theXPos, int theYPos )
    {
        Position position = new Position( theXPos, theYPos );
        processShot( position, EnemyGrid );
        if ( Opponent is EnemyPerson )
        {
            NetworkService.Instance.NetworkPeer.SendMessage( position.ToMessage() );
        }
    }

    private void processShot( Position theMove, PlayingFieldModel theGrid )
    {
        if ( !theMove.IsValid() )
        {
            changeTurns();
            return;
        }
        foreach ( Ship ship in theGrid.ViewModel.Ships )
        {
            switch ( ship.IsHit( theMove ) )
            {
                case HitType.Hit:
                    if ( ship.IsSunk() )
                    {
                        theGrid.DrawShip( ship );
                        theMove.WasSunk = true;
                        theMove.TheSunkShip = ship;
                    }
                    else
                    {
                        theGrid.SetCell( theMove.X, theMove.Y, TileService.GetTile( TileType.Hit ) );
                    }
                    changeTurns();
                    return;
                case HitType.Repeat:
                    theGrid.AcceptChanges();
                    changeTurns();
                    return;
            }
        }
        theGrid.SetCell( theMove.X, theMove.Y, TileService.GetTile( TileType.Miss ) );
        changeTurns();
    }

    private void changeTurns()
    {
        CurrentTurn = CurrentTurn != PlayerType.Enemy ? PlayerType.Enemy : PlayerType.You;
        if ( CurrentTurn == PlayerType.Enemy )
        {
            PlayNextMove();
        }
    }

    public void SelectDifficulty( Difficulty theDifficutly )
    {
        switch ( theDifficutly )
        {
            case Difficulty.Easy:
                Opponent = new EnemyEasy();
                break;
            case Difficulty.Medium:
                Opponent = new EnemyMedium();
                break;
            case Difficulty.Hard:
                Opponent = new EnemyHard();
                break;
            case Difficulty.Person:
                Opponent = new EnemyPerson();
                break;
        }
    }

    public void PlayNextMove( Position theNextMove = null )
    {
        if ( (object)theNextMove == null )
        {
            theNextMove = Opponent.NextMove();
        }
        if ( (object)theNextMove != null )
        {
            processShot( theNextMove, OwnGrid );
        }
    }

    public ObservableCollection<Ship> GenerateShipList( List<ushort> theShipList = null )
    {
        ObservableCollection<Ship> observableCollection = new ObservableCollection<Ship>
        {
            new Carrier(),
            new Warship(),
            new Warship(),
            new Submarine(),
            new Submarine(),
            new Destroyer(),
            new Destroyer(),
            new PatrolBoat(),
            new PatrolBoat(),
            new PatrolBoat()
        };
        if ( theShipList != null && theShipList.Count == observableCollection.Count )
        {
            for ( int i = 0; i < observableCollection.Count; i++ )
            {
                observableCollection[i].FromBitField( theShipList[i] );
            }
            return observableCollection;
        }
        placeShipsRandomly( observableCollection );
        return observableCollection;
    }

    private void placeShipsRandomly( ObservableCollection<Ship> theShipList )
    {
        Random random = new Random( m_RandomSeed++ );
        Position position = new Position();
        foreach ( Ship theShip in theShipList )
        {
            do
            {
                Orientation orientation = random.Next() % 2 != 0 ? Orientation.Vertical : Orientation.Horizontal;
                bool isReversed = random.Next() % 2 == 0;
                position.X = random.Next( 10 - theShip.Length + 1 );
                position.Y = random.Next( 10 );
                if ( orientation == Orientation.Vertical )
                {
                    position.Swap();
                }
                theShip.SetShipValues( position, orientation, isReversed );
            }
            while ( isColliding( theShip, theShipList ) );
        }
    }

    private bool isColliding( Ship theShip, ObservableCollection<Ship> theShipList )
    {
        foreach ( Ship theShip2 in theShipList )
        {
            if ( theShip2 == theShip )
            {
                break;
            }
            if ( theShip2.IntersectsWith( theShip ) )
            {
                return true;
            }
        }
        return false;
    }
}
