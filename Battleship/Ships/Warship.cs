using System.Collections.Generic;
using Battleship.Utility;

namespace Battleship.Ships;

public sealed class Warship : Ship
{
    private const int LENGTH = 4;

    public Warship()
        : base( LENGTH )
    {
        m_Type = ShipType.Battleship;
    }

    private protected override void setTileSet()
    {
        TileSet = new List<byte[]>( LENGTH );
        TileOrientation tileOrientation = IsHorizontal() ? TileOrientation.Left : TileOrientation.Up;
        if ( Reversed )
        {
            switch ( tileOrientation )
            {
                case TileOrientation.Left:
                    tileOrientation = TileOrientation.Right;
                    break;
                case TileOrientation.Up:
                    tileOrientation = TileOrientation.Down;
                    break;
            }
        }
        TileSet.Add( TileService.GetTile( TileType.Battleship_1, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Battleship_2, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Battleship_3, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Battleship_4, tileOrientation ) );
        if ( Reversed )
        {
            TileSet.Reverse();
        }
    }
}
