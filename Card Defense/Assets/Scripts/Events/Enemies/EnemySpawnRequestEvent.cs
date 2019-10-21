using UnityEngine;

public class EnemySpawnRequestEvent : CodeControl.Message
{
	public Enemy enemy;
	public Tile[] startingPoints, endingPoints;

	public EnemySpawnRequestEvent(Enemy enemy, Tile[] startingPoints, Tile[] endingPoints)
	{
		this.enemy = enemy;
		this.startingPoints = startingPoints;
		this.endingPoints = endingPoints;
	}
}
