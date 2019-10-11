public class DamageEnemiesRequestEvent : CodeControl.Message
{
	public bool all;
	public int amount;
	public float damage;

	public DamageEnemiesRequestEvent(bool all, float damage)
	{
		this.all = all;
		this.damage = damage;
	}

	public DamageEnemiesRequestEvent(int amount, float damage)
	{
		this.amount = amount;
		this.damage = damage;
	}
}
