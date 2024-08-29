using System.Collections.Generic;
using System.Windows.Controls;
using Battleship_v2.Ships;

namespace Battleship_v2.Utility;

public sealed class Position
{
	public int X { get; set; }

	public int Y { get; set; }

	public (int, int) Coordinates
	{
		get
		{
			return (X, Y);
		}
		set
		{
			(X, Y) = value;
		}
	}

	public bool WasHit { get; set; }

	public bool WasSunk { get; set; }

	public Ship TheSunkShip { get; set; }

	public static bool operator ==(Position theLeft, Position theRight)
	{
		if ((object)theLeft == null || (object)theRight == null)
		{
			return false;
		}
		if (theLeft.X == theRight.X)
		{
			return theLeft.Y == theRight.Y;
		}
		return false;
	}

	public static bool operator !=(Position theLeft, Position theRight)
	{
		return !(theLeft == theRight);
	}

	public override bool Equals(object theObject)
	{
		if (theObject == null || !(theObject is Position))
		{
			return false;
		}
		Position position = (Position)theObject;
		if (X == position.X)
		{
			return Y == position.Y;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (X.GetHashCode() * 17) ^ (Y.GetHashCode() * 47) ^ (WasHit.GetHashCode() * 89);
	}

	public Position(int theXPos, int theYPos)
	{
		X = theXPos;
		Y = theYPos;
	}

	public Position()
		: this(-1, -1)
	{
	}

	public void SetValuesFrom(Position thePosition)
	{
		X = thePosition.X;
		Y = thePosition.Y;
		WasHit = thePosition.WasHit;
	}

	public bool IsValid()
	{
		if (X >= 0 && X < 10 && Y >= 0)
		{
			return Y < 10;
		}
		return false;
	}

	public void Swap()
	{
		int y = Y;
		int x = X;
		X = y;
		Y = x;
	}

	public override string ToString()
	{
		char c = (char)(X + 65);
		return $"Position ({c}/{Y + 1})";
	}

	public string ToMessage()
	{
		return $"{X},{Y}";
	}

	public Position Clone()
	{
		return new Position(X, Y);
	}

	public Position GetNeighbour(Orientation theOrientation = Orientation.Horizontal, int theDirection = 1)
	{
		if (theOrientation != 0)
		{
			return new Position(X, Y + theDirection);
		}
		return new Position(X + theDirection, Y);
	}

	public List<Position> GetNeighbours(int theMinPos = int.MinValue, int theMaxPos = int.MaxValue)
	{
		List<Position> list = new List<Position>(4);
		Position position = Clone().MoveUp();
		Position position2 = Clone().MoveRight();
		Position position3 = Clone().MoveDown();
		Position position4 = Clone().MoveLeft();
		if (position.InBounds(theMinPos, theMaxPos))
		{
			list.Add(position);
		}
		if (position2.InBounds(theMinPos, theMaxPos))
		{
			list.Add(position2);
		}
		if (position3.InBounds(theMinPos, theMaxPos))
		{
			list.Add(position3);
		}
		if (position4.InBounds(theMinPos, theMaxPos))
		{
			list.Add(position4);
		}
		return list;
	}

	public bool InBounds(int theMinValue = 0, int theMaxValue = 10)
	{
		if (X >= theMinValue && X < theMaxValue && Y >= theMinValue)
		{
			return Y < theMaxValue;
		}
		return false;
	}

	public int GetCellIndex()
	{
		return Y * 10 + X;
	}

	public void FromCellIndex(int theCellIndex)
	{
		if (theCellIndex >= 0 && theCellIndex <= 99)
		{
			X = theCellIndex % 10;
			Y = theCellIndex / 10;
		}
	}

	public Position MoveRight()
	{
		X++;
		return this;
	}

	public Position MoveLeft()
	{
		X--;
		return this;
	}

	public Position MoveDown()
	{
		Y++;
		return this;
	}

	public Position MoveUp()
	{
		Y--;
		return this;
	}
}
