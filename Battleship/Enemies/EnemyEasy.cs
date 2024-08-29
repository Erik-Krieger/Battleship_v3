using Battleship.Utility;

namespace Battleship.Enemies;

public class EnemyEasy : Enemy
{
    public override Position NextMove()
    {
        return getRandomMove();
    }
}
