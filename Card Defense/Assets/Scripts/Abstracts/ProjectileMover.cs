using System;
using UnityEngine;

public class ProjectileMover 
{
	public Transform projectile, target;
	private Vector3 targetLocation;
	private float speed;
	private ProjectileMovementType primaryMovementType;

	public ProjectileMover(Transform projectile, Transform target, float speed, ProjectileMovementType primaryMovementType)
	{
		this.primaryMovementType = primaryMovementType;
		this.projectile = projectile;
		this.target = target;
		this.speed = speed;
	}

	public ProjectileMover(Transform projectile, Vector3 targetLocation, float speed, ProjectileMovementType primaryMovementType)
	{
		this.primaryMovementType = primaryMovementType;
		this.projectile = projectile;
		this.targetLocation = targetLocation;
		this.speed = speed;
	}

	public void MoveAsRequired(System.Action<bool> OnHit)
	{
		switch (primaryMovementType)
		{
			case ProjectileMovementType.StraightChain:
				MoveStraightToAssignedTarget(OnHit);
				break;
			case ProjectileMovementType.BounceOnGround:
				BounceOnTargetPosition(OnHit);
				break;
		}
	}

	private void BounceOnTargetPosition(Action<bool> onHit)
	{

	}

	public void MoveStraightToAssignedTarget(System.Action<bool> OnHit)
	{
		if(target == null || !target.gameObject.activeInHierarchy)
		{
			OnHit(false);
		}
		Vector3 projectilePosition = projectile.position;
		Vector3 targetPosition = target == null ? targetLocation : target.position;
		Vector3 direction = targetPosition - projectilePosition;
		float distanceToTravel = speed * Time.deltaTime;
		if(direction.magnitude <= distanceToTravel)
		{
			OnHit(true);
		}
		else
		{
			projectile.Translate(direction.normalized * distanceToTravel);	
		}
	}	

	
}
