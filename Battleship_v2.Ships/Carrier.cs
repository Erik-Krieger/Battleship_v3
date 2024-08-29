using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships;

public sealed class Carrier : Ship
{
	private const int LENGTH = 5;

	public Carrier()
		: base(5)
	{
		m_Type = ShipType.Carrier;
	}

	private protected override void setTileSet()
	{
		base.TileSet = new List<byte[]>(5);
		TileOrientation tileOrientation = (IsHorizontal() ? TileOrientation.Left : TileOrientation.Up);
		if (base.Reversed)
		{
			switch (tileOrientation)
			{
			case TileOrientation.Left:
				tileOrientation = TileOrientation.Right;
				break;
			case TileOrientation.Up:
				tileOrientation = TileOrientation.Down;
				break;
			}
		}
		base.TileSet.Add(TileService.GetTile(TileType.Carrier_1, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Carrier_2, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Carrier_3, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Carrier_4, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Carrier_5, tileOrientation));
		if (base.Reversed)
		{
			base.TileSet.Reverse();
		}
	}
}
