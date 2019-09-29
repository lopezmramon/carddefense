public class EnemyDamageRequestEvent : CodeControl.Message
{
	public EnemyController enemy;
	public float damageAmount;

	public EnemyDamageRequestEvent(EnemyController enemy, float damageAmount)
	{
		this.enemy = enemy;
		this.damageAmount = damageAmount;
	}
}
