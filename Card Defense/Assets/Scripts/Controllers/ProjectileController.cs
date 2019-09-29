using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private ProjectileMover projectileMover;
	private Projectile projectile;
	public float damage, aoe, speed;
	private int bouncesLeft, chainLength;
	private ProjectileMovementType primaryMovementType;
	private AOEDamageDealer AOEDamageDealer;

	internal void Initialize(Projectile projectile)
	{
		Element[] projectileElements = projectile.elements.ToArray();
		bouncesLeft = ElementUtility.BouncesFromElements(projectileElements);
		chainLength = ElementUtility.ChainLengthFromElements(projectileElements);
		speed = ElementUtility.SpeedFromElements(projectileElements);
		AOEDamageDealer = new AOEDamageDealer(ElementUtility.AOEFromElements(projectileElements) * projectile.areaOfEffectMultiplier, transform);
		primaryMovementType = ElementUtility.MovementForElement(projectile.elements.Peek());
		this.projectile = projectile;
		switch (primaryMovementType)
		{
			case ProjectileMovementType.BounceOnGround:

				Parabola[] parabolas = ParabolaCalculator.CalculateFullTrajectory(transform.position, projectile.target.position,
					ElementUtility.SpeedFromElements(projectileElements), transform.position.y, 0, bouncesLeft);
				projectileMover = new ProjectileMover(transform, parabolas, ElementUtility.SpeedFromElements(projectile.elements.ToArray()) * projectile.speedMultiplier);
				break;
			case ProjectileMovementType.StraightChain:
				projectileMover = new ProjectileMover(transform, projectile.target, ElementUtility.SpeedFromElements(projectile.elements.ToArray()) * projectile.speedMultiplier);
				break;
			case ProjectileMovementType.AOEAtTower:
				projectileMover = new ProjectileMover(projectile.speedMultiplier * speed);
				break;
		}
	}

	private void Update()
	{
		switch (primaryMovementType)
		{
			case ProjectileMovementType.StraightChain:
				projectileMover.MoveStraightToAssignedTarget((success) =>
				{
					if (success)
					{
						DispatchEnemyDamageRequestEvent(projectileMover.target.GetComponent<EnemyController>(), damage);
						DispatchElementContactParticleRequestEvent(transform.position);
						ContinueChain();
					}
					else
					{
						Destroy(gameObject);
					}
				}
				);
				break;
			case ProjectileMovementType.BounceOnGround:
				projectileMover.BounceOnTargetPosition((success) =>
				{
					if (success)
					{
						AnalyzeBounceResult();
					}
					else
					{
						Destroy(gameObject);
					}
				});
				break;
			case ProjectileMovementType.AOEAtTower:
				OnAOEAtTowerActivation();
				break;
		}
	}

	private void ContinueChain()
	{
		projectileMover.reassigning = true;
		if (chainLength > 0)
		{
			string targetName = projectileMover.target == null ? string.Empty : projectileMover.target.name;
			Transform closestEnemy = AOEDamageDealer.ClosestEnemy(transform.position, targetName);
			if (closestEnemy == null)
			{
				Destroy(gameObject);
			}
			else
			{
				chainLength--;
				projectileMover.target = closestEnemy;
				projectileMover.reassigning = false;
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void OnAOEAtTowerActivation()
	{
		AOEDamageDealer.DealAOEDamage(damage);
		DispatchElementContactParticleRequestEvent(AOEDamageDealer.EnemyPositionsNearPointWithCurrentAOE(transform.position));
		Destroy(gameObject);
	}

	private void AnalyzeBounceResult()
	{
		if (projectile.elements.Peek() == Element.Water)
		{
			DispatchGroundHazardPlacementRequestEvent(projectile, transform.position);
		}
		else
		{
			AOEDamageDealer.DealAOEDamage(damage);
			DispatchElementContactParticleRequestEvent(AOEDamageDealer.EnemyPositionsNearPointWithCurrentAOE(transform.position));
		}
	}

	private void DispatchGroundHazardPlacementRequestEvent(Projectile projectile, Vector3 position)
	{
		CodeControl.Message.Send(new GroundHazardPlacementRequestEvent(projectile, position));
	}

	private void DispatchEnemyDamageRequestEvent(EnemyController enemy, float damageAmount)
	{
		CodeControl.Message.Send(new EnemyDamageRequestEvent(enemy, damageAmount));
	}

	private void DispatchElementContactParticleRequestEvent(Vector3 placement)
	{
		CodeControl.Message.Send(new ElementalContactParticleRequestEvent(projectile.elements.Peek(), placement));
	}

	private void DispatchElementContactParticleRequestEvent(Vector3[] placements)
	{
		CodeControl.Message.Send(new ElementalContactParticleRequestEvent(projectile.elements.Peek(), placements));
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Obstacles") && primaryMovementType == ProjectileMovementType.BounceOnGround) Destroy(gameObject);
	}
}
