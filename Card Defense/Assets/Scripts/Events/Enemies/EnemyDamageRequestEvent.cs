using System.Collections.Generic;

public class EnemyDamageRequestEvent : CodeControl.Message
{
	public EnemyController enemy;
	public float damageAmount;
	public Queue<Element> elements;

	public EnemyDamageRequestEvent(EnemyController enemy, float damageAmount, Queue<Element> elements)
	{
		this.enemy = enemy;
		this.damageAmount = damageAmount;
		this.elements = elements;
	}
}
