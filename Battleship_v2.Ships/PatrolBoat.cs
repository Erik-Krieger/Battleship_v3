using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships;

public sealed class PatrolBoat : Ship
{
	private const int LENGTH = 2;

	public PatrolBoat()
		: base(2)
	{
		m_Type = ShipType.PatrolBoat;
	}

	private protected override void setTileSet()
	{
		base.TileSet = new List<byte[]>(2);
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
		base.TileSet.Add(TileService.GetTile(TileType.PatrolBoat_1, tileOrientation));
		base.TileSet.Add(TileService.GetTile(TileType.PatrolBoat_2, tileOrientation));
		if (base.Reversed)
		{
			base.TileSet.Reverse();
		}
	}
}
