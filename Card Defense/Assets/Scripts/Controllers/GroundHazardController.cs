using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHazardController : MonoBehaviour
{
	private float damage, damageBaseTimer, damageCountdownTimer, duration;
	private AOEDamageDealer damageDealer;
	private Projectile projectile;
	private Element[] elements;

	internal void Initialize(Projectile projectile)
	{
		this.projectile = projectile;
		elements = projectile.elements.ToArray();
		damage = ElementUtility.DamageFromElements(elements);
		damageBaseTimer = ElementUtility.GroundHazardTimerFromElements(elements);
		damageCountdownTimer = damageBaseTimer;
		damageDealer = new AOEDamageDealer(0.35f, transform, projectile.elements);
		duration = ElementUtility.GroundHazardDurationFromElements(elements);
		Destroy(gameObject, duration);
	}

	private void Update()
	{
		if (damageCountdownTimer > Time.deltaTime)
		{
			damageCountdownTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else
		{
			damageCountdownTimer = damageBaseTimer;
			damageDealer.DealAOEDamage(damage);
			DispatchElementalContactParticlesRequestEvent(damageDealer.EnemyPositionsNearPointWithCurrentAOE(transform.position));
		}
	}

	private void DispatchElementalContactParticlesRequestEvent(Vector3[] positions)
	{
		CodeControl.Message.Send(new ElementalContactParticleRequestEvent(elements[0], positions));
	}
}
