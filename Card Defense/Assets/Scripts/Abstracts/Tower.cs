using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower 
{
	public Transform transform;
	public Queue<Element> elements;
	public PropertyModifierHandler modifierHandler;

	public Tower(Transform transform, Queue<Element> elements, PropertyModifierHandler modifierHandler)
	{
		this.transform = transform;
		this.elements = elements;
		this.modifierHandler = modifierHandler;
	}
}
