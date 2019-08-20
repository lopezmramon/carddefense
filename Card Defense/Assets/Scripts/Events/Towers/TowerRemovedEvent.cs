using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRemovedEvent : CodeControl.Message
{
	public Tile tile;

	public TowerRemovedEvent(Tile tile)
	{
		this.tile = tile;
	}
}
