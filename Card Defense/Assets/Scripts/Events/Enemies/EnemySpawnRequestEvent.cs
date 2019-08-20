using UnityEngine;

public class EnemySpawnRequestEvent : CodeControl.Message
{
	public Enemy enemy;
	public Vector3 startingPosition;
	public Vector3 destination;

	public EnemySpawnRequestEvent(Enemy enemy, Vector3 startingPosition, Vector3 destination)
	{
		this.enemy = enemy;
		this.startingPosition = startingPosition;
		this.destination = destination;
	}

	public EnemySpawnRequestEvent(EnemyType enemyType, EnemySpecialAbility specialAbility, Vector3 startingPosition, Vector3 destination)
	{
		enemy = new Enemy(enemyType, specialAbility);
		this.startingPosition = startingPosition;
		this.destination = destination;
	}

}
