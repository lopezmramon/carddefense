using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectileRequestEvent : CodeControl.Message
{
	public Projectile projectile;
	public Transform projectileOrigin;

	public ShootProjectileRequestEvent(Projectile projectile, Transform projectileOrigin)
	{
		this.projectile = projectile;
		this.projectileOrigin = projectileOrigin;
	}

	public ShootProjectileRequestEvent(float damageMultiplier, float speedMultiplier, float areaOfEffectMultiplier, Queue<Element> elements, Transform projectileOrigin, Transform target, Vector3 fixedTarget)
	{
		projectile = new Projectile(damageMultiplier, speedMultiplier, areaOfEffectMultiplier, elements, target);
		this.projectileOrigin = projectileOrigin;
	}
}
