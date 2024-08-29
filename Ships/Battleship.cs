using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships;

public sealed class Battleship : Ship
{
	private const int LENGTH = 4;

	public Battleship()
		: base(4)
	{
		m_Type = ShipType.Battleship;
	}

	private protected override void setTileSet()
	{
		base.TileSet = new List<byte[]>(4);
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
		base.TileSet.Add(TileService.GetTile(TileType.Battleship_1, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Battleship_2, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Battleship_3, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.Battleship_4, tileOrientation));
		if (base.Reversed)
		{
			base.TileSet.Reverse();
		}
	}
}
