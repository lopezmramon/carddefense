using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementRequestEvent : CodeControl.Message
{
	public Tile tile;
	public Element element;

	public TowerPlacementRequestEvent(Tile tile, Element element)
	{
		this.tile = tile;
		this.element = element;
	}
}
