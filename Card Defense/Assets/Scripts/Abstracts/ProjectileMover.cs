using System;
using UnityEngine;

public class ProjectileMover 
{
	public Transform projectile, target;
	private Vector3 targetPos, startPos;
	private float speed, initialXDistance;
	
	public ProjectileMover(Transform projectile, Transform target, float speed)
	{
		this.projectile = projectile;
		this.target = target;
		startPos = projectile.position;
		initialXDistance = targetPos.x - startPos.x;
		this.speed = speed;
	}

	public ProjectileMover(Transform projectile, Vector3 targetLocation, float speed)
	{
		this.projectile = projectile;
		this.targetPos = targetLocation;
		startPos = projectile.position;
		initialXDistance = targetPos.x - startPos.x;
		this.speed = speed;
	}

	public void UpdateStartAndTargetPositionForNextBounce()
	{
		startPos = targetPos;
		targetPos.x += initialXDistance;
		targetPos.y = projectile.position.y;
	}

	public void BounceOnTargetPosition(Action<bool> onHit)
	{
		float x0 = startPos.x;
		float x1 = targetPos.x;
		float dist = x1 - x0;
		float arcHeight = startPos.y + 1f;
		float nextX = Mathf.MoveTowards(projectile.position.x, x1, speed * Time.deltaTime);
		float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
		float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
		Vector3 nextPos = new Vector3(nextX, baseY + arc, projectile.position.z);

		// Rotate to face the next position, and then move there
		projectile.rotation = LookAt2D(nextPos - projectile.position);
		projectile.position = nextPos;

		// Do something when we reach the target
		if (nextPos == targetPos)
		{
			UpdateStartAndTargetPositionForNextBounce();
			onHit(true);
		}
	}

	public void MoveStraightToAssignedTarget(Action<bool> OnHit)
	{
		if(target == null || !target.gameObject.activeInHierarchy)
		{
			OnHit(false);
		}
		Vector3 projectilePosition = projectile.position;
		Vector3 targetPosition = target == null ? targetPos : target.position;
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

	/// 
	/// This is a 2D version of Quaternion.LookAt; it returns a quaternion
	/// that makes the local +X axis point in the given forward direction.
	/// 
	/// forward direction
	/// Quaternion that rotates +X to align with forward
	public static Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
	}
}
