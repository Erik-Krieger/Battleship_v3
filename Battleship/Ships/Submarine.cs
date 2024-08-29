using System.Collections.Generic;
using Battleship.Utility;

namespace Battleship.Ships;

public sealed class Submarine : Ship
{
    private const int LENGTH = 3;

    public Submarine()
        : base( LENGTH )
    {
        m_Type = ShipType.Submarine;
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
        TileSet.Add( TileService.GetTile( TileType.Submarine_1, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Submarine_2, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Submarine_3, tileOrientation ) );
        if ( Reversed )
        {
            TileSet.Reverse();
        }
    }
}
