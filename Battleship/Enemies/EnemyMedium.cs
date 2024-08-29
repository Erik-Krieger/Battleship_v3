using System.Collections.Generic;
using System.Linq;
using Battleship.Ships;
using Battleship.Utility;

namespace Battleship.Enemies;

public sealed class EnemyMedium : Enemy
{
    private bool isSeries;

    private int m_NeighbourCount;

    private List<Position> m_LastMoves = new List<Position>( 100 );

    private List<Position> m_NextMoves = new List<Position>( 5 );

    public override Position NextMove()
    {
        Position randomMove;
        if ( m_LastMoves.Count == 0 )
        {
            randomMove = getRandomMove();
            m_LastMoves.Add( randomMove );
            return randomMove;
        }
        if ( wasLastMoveSunk() )
        {
            Ship theSunkShip = m_LastMoves.Last().TheSunkShip;
            for ( int i = 0; i < m_LastMoves.Count; i++ )
            {
                if ( theSunkShip.IsHit( m_LastMoves[i], isAICheck: true ) == HitType.Hit )
                {
                    m_LastMoves.RemoveRange( i, m_LastMoves.Count - i );
                    break;
                }
            }
            m_NextMoves.Clear();
            m_NeighbourCount = 0;
        }
        if ( wasLastMoveHit() )
        {
            if ( setValidNeighboursAsNextMove( m_LastMoves.Last() ) )
            {
                return makeMoveFromOptions();
            }
        }
        else
        {
            if ( m_NextMoves.Count > 0 )
            {
                return makeMoveFromOptions();
            }
            while ( m_LastMoves.Count > 0 && !wasLastMoveHit() )
            {
                m_LastMoves.Remove( m_LastMoves.Last() );
            }
            if ( m_LastMoves.Count > 0 && setValidNeighboursAsNextMove( m_LastMoves.Last() ) )
            {
                return makeMoveFromOptions();
            }
        }
        if ( m_NeighbourCount > 0 )
        {
            return makeMoveFromOptions();
        }
        randomMove = getRandomMove();
        m_LastMoves.Add( randomMove );
        return randomMove;
    }

    private Position makeMoveFromOptions()
    {
        Position position = m_NextMoves.First();
        m_NextMoves.Remove( position );
        m_ValidMoves.Remove( position );
        m_LastMoves.Add( position );
        m_NeighbourCount--;
        return position;
    }

    private bool wasLastMoveSunk()
    {
        if ( m_LastMoves.Count == 0 )
        {
            return false;
        }
        return m_LastMoves.Last().WasSunk;
    }

    private bool wasLastMoveHit()
    {
        if ( m_LastMoves.Count == 0 )
        {
            return false;
        }
        return m_LastMoves.Last().WasHit;
    }

    private bool wasHitInRecentMoves()
    {
        for ( int num = m_LastMoves.Count - 1; num >= 0; num-- )
        {
            if ( m_LastMoves[num].WasHit )
            {
                return true;
            }
        }
        return false;
    }

    private bool setValidNeighboursAsNextMove( Position theMove )
    {
        List<Position> neighbours = theMove.GetNeighbours( 0, 10 );
        neighbours = neighbours.Intersect( m_ValidMoves ).ToList();
        if ( neighbours == null )
        {
            return false;
        }
        m_NextMoves.InsertRange( 0, neighbours );
        m_NeighbourCount = neighbours.Count;
        return neighbours.Count > 0;
    }
}
