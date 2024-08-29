using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships;

public sealed class Submarine : Ship
{
	private const int LENGTH = 3;

	public Submarine()
		: base(3)
	{
		m_Type = ShipType.Submarine;
	}

	private protected override void setTileSet()
	{
		base.TileSet = new List<byte[]>(3);
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
		base.TileSet.Add(TileService.GetTile(TileType.Submarine_1, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Submarine_2, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Submarine_3, tileOrientation));
		if (base.Reversed)
		{
			base.TileSet.Reverse();
		}
	}
}
