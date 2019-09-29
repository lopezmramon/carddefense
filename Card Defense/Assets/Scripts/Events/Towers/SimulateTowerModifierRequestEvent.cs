using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateTowerModifierRequestEvent : CodeControl.Message
{
	public TowerController tower;
	public PropertyModifier[] propertyModifiers;
	public float[] propertyModifierValues;

	public SimulateTowerModifierRequestEvent(TowerController tower, PropertyModifier[] propertyModifiers, float[] propertyModifierValues)
	{
		this.tower = tower;
		this.propertyModifiers = propertyModifiers;
		this.propertyModifierValues = propertyModifierValues;
	}
}
