using System.Collections.Generic;
using System.Windows.Media;
using Battleship.Services;
using Battleship.Ships;
using Battleship.Utility;
using Battleship.ViewModels;

namespace Battleship.Models;

public class PlayingFieldModel : PropertyChangeHandler
{
    public const int GRID_SIZE = 10;

    public static readonly Color ActiveColor = Color.FromRgb( 0, byte.MaxValue, byte.MaxValue );

    public static readonly Color PassiveColor = Color.FromRgb( 0, 128, 128 );

    private bool[,] m_CellsHit = new bool[10, 10];

    public PlayingFieldViewModel ViewModel { get; private set; }

    public PlayingFieldModel( PlayingFieldViewModel theViewModel, bool isOwn, List<ushort> theShipList )
    {
        ViewModel = theViewModel;
        ViewModel.Ships = GameManagerService.Instance.GenerateShipList( theShipList );
    }

    private bool isInBounds( int theXPos, int theYPos )
    {
        if ( theXPos >= 0 && theXPos < 10 && theYPos >= 0 )
        {
            return theYPos < 10;
        }
        return false;
    }

    public void AcceptChanges()
    {
        ViewModel.Grid.AcceptChanges();
    }

    public void SetCell( int theXPos, int theYPos, object theValue )
    {
        if ( isInBounds( theXPos, theYPos ) )
        {
            if ( m_CellsHit[theXPos, theYPos] && theValue == TileService.GetTile( TileType.Miss ) )
            {
                AcceptChanges();
                return;
            }
            m_CellsHit[theXPos, theYPos] = true;
            ViewModel.Grid.Rows[theYPos][theXPos] = theValue;
            ViewModel.Grid.AcceptChanges();
        }
    }

    public void SetCell( Position thePosition, object theValue )
    {
        SetCell( thePosition.X, thePosition.Y, theValue );
    }

    public object GetCell( int theXPos, int theYPos )
    {
        if ( isInBounds( theXPos, theYPos ) )
        {
            return '\0';
        }
        return ( (string)ViewModel.Grid.Rows[theYPos][ViewModel.Grid.Columns[theXPos]] )[0];
    }

    public object GetCell( Position thePosition )
    {
        return GetCell( thePosition.X, thePosition.Y );
    }

    public void DrawAllShips( bool allSunk = false )
    {
        foreach ( Ship ship in ViewModel.Ships )
        {
            DrawShip( ship, allSunk );
        }
    }

    public void DrawShip( Ship theShip, bool isSunk = true )
    {
        for ( int i = 0; i < theShip.Length; i++ )
        {
            SetCell( theShip.Cells[i], theShip.TileSet[i] );
        }
        if ( isSunk )
        {
            ViewModel.Ships.Remove( theShip );
            if ( ViewModel.Ships.Count == 0 )
            {
                WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new GameOverViewModel( GameManagerService.Instance.YourTurn );
            }
        }
    }

    public void GridCellClicked( int theColumnIndex, int theRowIndex )
    {
        if ( GameManagerService.Instance.YourTurn )
        {
            GameManagerService.Instance.ProcessShot( theColumnIndex - 1, theRowIndex - 1 );
        }
    }

    public void SetTurnIndicator( bool isActive = false )
    {
        if ( !isActive )
        {
            ViewModel.BackgroundColor = new SolidColorBrush( ActiveColor );
        }
        else
        {
            ViewModel.BackgroundColor = new SolidColorBrush( PassiveColor );
        }
    }
}
