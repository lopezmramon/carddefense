using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddModifiersToTowerRequestEvent : CodeControl.Message
{
	public TowerController tower;
	public PropertyModifier[] propertyModifiers;
	public float[] propertyModifierValues;
	public float duration;

	public AddModifiersToTowerRequestEvent(TowerController tower, PropertyModifier[] propertyModifiers, float[] propertyModifierValues, float duration)
	{
		this.tower = tower;
		this.propertyModifiers = propertyModifiers;
		this.propertyModifierValues = propertyModifierValues;
		this.duration = duration;
	}
}
