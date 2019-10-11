using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoUIDisplayRequestEvent : CodeControl.Message
{
	public TowerController tower;

	public TowerInfoUIDisplayRequestEvent(TowerController tower)
	{
		this.tower = tower;
	}
}
