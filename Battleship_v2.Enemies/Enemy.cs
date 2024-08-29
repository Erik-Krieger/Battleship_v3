using System;
using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies;

public abstract class Enemy
{
	protected Random aRng = new Random();

	protected List<Position> m_ValidMoves;

	public Enemy()
	{
		m_ValidMoves = new List<Position>(100);
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				m_ValidMoves.Add(new Position(j, i));
			}
		}
	}

	public abstract Position NextMove();

	private protected Position getRandomMove()
	{
		int index = aRng.Next(m_ValidMoves.Count);
		Position result = m_ValidMoves[index];
		m_ValidMoves.RemoveAt(index);
		return result;
	}
}
