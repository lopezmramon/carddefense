using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInfoUIDisplayRequestEvent : CodeControl.Message
{
	public Tower tower;

	public TowerInfoUIDisplayRequestEvent(Tower tower)
	{
		this.tower = tower;
	}
}
