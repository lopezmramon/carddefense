using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfoUIDisplayRequestEvent : CodeControl.Message
{
	public Enemy enemy;

	public EnemyInfoUIDisplayRequestEvent(Enemy enemy)
	{
		this.enemy = enemy;
	}
}
