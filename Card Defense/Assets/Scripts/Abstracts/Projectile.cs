using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
	public float damageMultiplier = 1f, speedMultiplier = 1f, areaOfEffectMultiplier = 1f;
	public Queue<Element> elements;
	public Transform target;

	public Projectile(float damageMultiplier, float speedMultiplier, float areaOfEffectMultiplier, Queue<Element> elements, Transform target)
	{
		this.damageMultiplier = damageMultiplier;
		this.speedMultiplier = speedMultiplier;
		this.areaOfEffectMultiplier = areaOfEffectMultiplier;
		this.elements = elements;
		this.target = target;
	}
}
