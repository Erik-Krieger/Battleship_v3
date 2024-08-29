using System.Collections.Generic;
using Battleship.Utility;

namespace Battleship.Ships;

public sealed class Carrier : Ship
{
    private const int LENGTH = 5;

    public Carrier()
        : base( LENGTH )
    {
        m_Type = ShipType.Carrier;
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
        TileSet.Add( TileService.GetTile( TileType.Carrier_1, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Carrier_2, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Carrier_3, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Carrier_4, tileOrientation ) );
        TileSet.Add( TileService.GetTile( TileType.Carrier_5, tileOrientation ) );
        if ( Reversed )
        {
            TileSet.Reverse();
        }
    }
}
