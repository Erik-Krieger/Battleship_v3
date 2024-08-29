using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships;

public abstract class Ship
{
	private protected ShipType m_Type;

	private protected Orientation m_Orientation;

	private List<Position> m_Cells;

	private int m_Length;

	private byte[] m_TileSprite;

	private int m_HitCount;

	private Position m_Position = new Position();

	public bool Reversed { get; private protected set; }

	public List<Position> Cells
	{
		get
		{
			return m_Cells;
		}
		private set
		{
			m_Cells = value;
		}
	}

	public int Length => m_Length;

	public int XPos => m_Position.X;

	public int YPos => m_Position.Y;

	public byte[] TileSprite
	{
		get
		{
			return m_TileSprite;
		}
		set
		{
			m_TileSprite = value;
		}
	}

	public int HitCount
	{
		get
		{
			return m_HitCount;
		}
		private set
		{
			m_HitCount = value;
		}
	}

	public Position Location
	{
		get
		{
			return m_Position;
		}
		set
		{
			m_Position = value;
		}
	}

	public List<byte[]> TileSet { get; protected set; }

	public Ship(int theLength)
	{
		m_Length = theLength;
		Cells = new List<Position>(m_Length);
		for (int i = 0; i < m_Length; i++)
		{
			m_Cells.Add(new Position());
		}
	}

	public void SetShipValues(Position thePosition, Orientation theOrientation, bool isReversed = false)
	{
		Location.SetValuesFrom(thePosition);
		m_Orientation = theOrientation;
		Reversed = isReversed;
		setShipCells();
	}

	private void setShipCells()
	{
		for (int i = 0; i < m_Length; i++)
		{
			if (IsHorizontal())
			{
				Cells[i].X = Location.X + i;
				Cells[i].Y = Location.Y;
			}
			else
			{
				Cells[i].X = Location.X;
				Cells[i].Y = Location.Y + i;
			}
		}
		setTileSet();
	}

	private protected abstract void setTileSet();

	private static bool isLegalPosition(int theXPos, int theYPos, int theLength, Orientation theOrientation)
	{
		if (theOrientation == Orientation.Horizontal)
		{
			if (theXPos >= 0 && theYPos + theLength <= 10 && theYPos >= 0)
			{
				return theYPos < 10;
			}
			return false;
		}
		if (theXPos >= 0 && theXPos < 10 && theYPos >= 0)
		{
			return theYPos + theLength <= 10;
		}
		return false;
	}

	public HitType IsHit(Position thePosition, bool isAICheck = false)
	{
		foreach (Position cell in m_Cells)
		{
			if (thePosition == cell)
			{
				if (isAICheck)
				{
					return HitType.Hit;
				}
				if (cell.WasHit)
				{
					return HitType.Repeat;
				}
				cell.WasHit = true;
				thePosition.WasHit = true;
				return HitType.Hit;
			}
		}
		return HitType.None;
	}

	public bool IntersectsWith(Ship theShip)
	{
		return Cells.Intersect(theShip.Cells).ToList().Count > 0;
	}

	public bool IsSunk()
	{
		return m_Cells.TrueForAll((Position e) => e.WasHit);
	}

	public bool IsHorizontal()
	{
		return m_Orientation == Orientation.Horizontal;
	}

	public bool IsVertical()
	{
		return m_Orientation == Orientation.Vertical;
	}

	public bool NotPlaced()
	{
		return !Location.IsValid();
	}

	public ushort ToBitField()
	{
		int num = 0;
		num |= (int)m_Type << 13;
		int cellIndex = Location.GetCellIndex();
		num |= cellIndex << 6;
		if (IsVertical())
		{
			num |= 0x20;
		}
		if (Reversed)
		{
			num |= 0x10;
		}
		return (ushort)num;
	}

	public void FromBitField(ushort theBitField)
	{
		if (((theBitField >> 13) & 7) != (int)m_Type)
		{
			throw new ArgumentException("The ship type in the bit field, does not match the type of the ship", "theBitField");
		}
		int theCellIndex = (theBitField >> 6) & 0x7F;
		Location.FromCellIndex(theCellIndex);
		m_Orientation = ((((theBitField >> 5) & 1) == 1) ? Orientation.Vertical : Orientation.Horizontal);
		Reversed = ((theBitField >> 4) & 1) == 1;
		setShipCells();
	}

	public override string ToString()
	{
		return Enum.GetName(typeof(ShipType), m_Type);
	}
}
