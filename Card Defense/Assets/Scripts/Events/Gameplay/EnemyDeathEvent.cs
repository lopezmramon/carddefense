public class EnemyDeathEvent : CodeControl.Message
{
	public EnemyController enemy;

	public EnemyDeathEvent(EnemyController enemy)
	{
		this.enemy = enemy;
	}
}
