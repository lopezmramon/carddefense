using Pathfinding;

public class EnemySpawnRequestEvent : CodeControl.Message
{
	public Enemy enemy;
	public Pathfinding.Path Path;

	public EnemySpawnRequestEvent(EnemyType enemyType, EnemySpecialAbility specialAbility, Path path)
	{
		enemy = new Enemy(enemyType, specialAbility);
		Path = path;
	}
}
