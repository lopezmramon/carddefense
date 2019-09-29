public class EnemyReachedDestinationEvent : CodeControl.Message
{
	public EnemyController enemy;

	public EnemyReachedDestinationEvent(EnemyController enemy)
	{
		this.enemy = enemy;
	}
}
