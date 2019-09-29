public class EnemySpawnedEvent : CodeControl.Message
{
	public EnemyController enemy;

	public EnemySpawnedEvent(EnemyController enemy)
	{
		this.enemy = enemy;
	}
}
