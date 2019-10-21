using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
	private ProjectileMover projectileMover;
	private Projectile projectile;
	private float damage, speed;
	private int bouncesLeft, chainLength;
	private ProjectileMovementType primaryMovementType;
	private AOEDamageDealer AOEDamageDealer;

	internal void Initialize(Projectile projectile)
	{
		Element[] projectileElements = projectile.elements.ToArray();
		damage = ElementUtility.ProjectileDamage(projectileElements);
		bouncesLeft = ElementUtility.Bounces(projectileElements);
		chainLength = ElementUtility.ChainLength(projectileElements);
		speed = ElementUtility.ProjectileSpeed(projectileElements);
		AOEDamageDealer = new AOEDamageDealer(ElementUtility.AoE(projectileElements) * projectile.areaOfEffectMultiplier, transform, projectile.elements);
		primaryMovementType = ElementUtility.Movement(projectile.elements.Peek());
		this.projectile = projectile;
		switch (primaryMovementType)
		{
			case ProjectileMovementType.BounceOnGround:
				Parabola[] parabolas = ParabolaCalculator.CalculateFullTrajectory(transform.position, projectile.target.position,
					ElementUtility.ProjectileSpeed(projectileElements), transform.position.y, 0, bouncesLeft);
				projectileMover = new ProjectileMover(transform, parabolas, ElementUtility.ProjectileSpeed(projectile.elements.ToArray()) * projectile.speedMultiplier);
				break;
			case ProjectileMovementType.StraightChain:
				projectileMover = new ProjectileMover(transform, projectile.target, ElementUtility.ProjectileSpeed(projectile.elements.ToArray()) * projectile.speedMultiplier);
				break;
			case ProjectileMovementType.AOEAtTower:
				projectileMover = new ProjectileMover(projectile.speedMultiplier * speed, 0, transform);
				OnAOEAtTowerActivation();
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
						ContinueChain();
					}
				});
				break;
			case ProjectileMovementType.AOEAtTower:
				//projectileMover.CheckAOEAtTowerTimer(OnAOEAtTowerActivation);
				break;
		}
	}

	private void ContinueChain()
	{
		if (primaryMovementType != ProjectileMovementType.StraightChain)
		{
			primaryMovementType = ProjectileMovementType.StraightChain;
		}
		projectileMover.reassigning = true;
		if (chainLength > 0)
		{
			Transform closestEnemy = AOEDamageDealer.ClosestEnemy(transform.position, projectileMover.target, 5f);
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
			CheckBouncesLeft();
		}
	}

	private void CheckBouncesLeft()
	{
		if (bouncesLeft <= 0)
		{
			Destroy(gameObject);
		}
		else
		{
			Vector3 bounceTargetPosition = transform.position + transform.forward*2f;
			bounceTargetPosition.y = 0;
			bounceTargetPosition.x += 0.1f;
			Parabola[] parabolas = ParabolaCalculator.CalculateFullTrajectory(transform.position, bounceTargetPosition,
	ElementUtility.ProjectileSpeed(projectile.elements.ToArray()), transform.position.y + 1f, 0, bouncesLeft);
			projectileMover.SetParabolas(parabolas);
			primaryMovementType = ProjectileMovementType.BounceOnGround;
		}
	}

	private void OnAOEAtTowerActivation()
	{
		AOEDamageDealer.DealAOEDamage(damage);
		DispatchElementContactParticleRequestEvent(AOEDamageDealer.EnemyPositionsNearPointWithCurrentAOE(transform.position));
		ContinueChain();
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
		bouncesLeft--;
	}

	private void DispatchGroundHazardPlacementRequestEvent(Projectile projectile, Vector3 position)
	{
		CodeControl.Message.Send(new GroundHazardPlacementRequestEvent(projectile, position));
	}

	private void DispatchEnemyDamageRequestEvent(EnemyController enemy, float damageAmount)
	{
		CodeControl.Message.Send(new EnemyDamageRequestEvent(enemy, damageAmount, projectile.elements));
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
