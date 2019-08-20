public class EnemyReachedDestinationEvent : CodeControl.Message
{
	public Enemy enemy;

	public EnemyReachedDestinationEvent(Enemy enemy)
	{
		this.enemy = enemy;
	}
}
