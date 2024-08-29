using System.Collections.Generic;
using Battleship.Utility;

namespace Battleship.Ships;

public sealed class PatrolBoat : Ship
{
    private const int LENGTH = 2;

    public PatrolBoat()
        : base( LENGTH )
    {
        m_Type = ShipType.PatrolBoat;
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
        TileSet.Add( TileService.GetTile( TileType.PatrolBoat_1, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.PatrolBoat_2, tileOrientation ) );
        if ( Reversed )
        {
            TileSet.Reverse();
        }
    }
}
