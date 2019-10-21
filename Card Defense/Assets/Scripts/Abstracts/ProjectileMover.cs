using System;
using UnityEngine;

public class ProjectileMover
{
	public Transform projectile, target;
	private Vector3 targetPos, startPos;
	private float speed, aoeAtTowerTimer, timeBetweenAOEsAtTower;
	private Parabola[] parabolas;
	private int currentParabolaIndex, currentPointInParabolaIndex;
	public bool reassigning;

	public ProjectileMover(Transform projectile, Transform target, float speed)
	{
		this.projectile = projectile;
		this.target = target;
		this.targetPos = target.position;
		startPos = projectile.position;
		this.speed = speed;
	}

	public ProjectileMover(Transform projectile, Parabola[] parabolas, float speed)
	{
		this.projectile = projectile;
		this.parabolas = parabolas;
		this.speed = speed;
		currentParabolaIndex = 0;
		currentPointInParabolaIndex = 0;
	}

	public ProjectileMover(float speed, float timeBetweenAOEsAtTower, Transform projectile)
	{
		this.projectile = projectile;
		this.speed = speed;
		this.timeBetweenAOEsAtTower = timeBetweenAOEsAtTower;
		aoeAtTowerTimer = speed;
	}

	public void SetParabolas(Parabola[] parabolas)
	{
		this.parabolas = parabolas;
	}

	public void CheckAOEAtTowerTimer(Action OnAOEAtTowerActivation)
	{
		if(aoeAtTowerTimer <= 0)
		{
			OnAOEAtTowerActivation();
			aoeAtTowerTimer = speed;
		}
		else
		{
			aoeAtTowerTimer -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
	}

	public void BounceOnTargetPosition(Action<bool> OnHit)
	{
		Vector3 destination = parabolas[currentParabolaIndex].points[currentPointInParabolaIndex];
		Vector3 projectilePosition = projectile.position;
		Vector3 direction = destination - projectilePosition;
		float distanceToTravel = speed * Time.deltaTime * GameManager.gameSpeedMultiplier;
		if (direction.magnitude <= distanceToTravel)
		{
			currentPointInParabolaIndex++;
			if(currentPointInParabolaIndex >= parabolas[currentParabolaIndex].points.Count)
			{
				OnHit(true);
				currentParabolaIndex++;
				currentPointInParabolaIndex = 0;
				if(currentParabolaIndex >= parabolas.Length)
				{
					OnHit(false);
				}
			}
		}
		else
		{
			projectile.Translate(direction.normalized * distanceToTravel);
		}
	}

	public void MoveStraightToAssignedTarget(Action<bool> OnHit)
	{
		if (reassigning) return;
		if (target == null || !target.gameObject.activeInHierarchy)
		{
			OnHit(false);
		}
		Vector3 projectilePosition = projectile.position;
		Vector3 targetPosition = target == null ? targetPos : target.position;
		Vector3 direction = targetPosition - projectilePosition;
		float distanceToTravel = speed * Time.deltaTime * GameManager.gameSpeedMultiplier;
		if (direction.magnitude <= distanceToTravel)
		{
			OnHit(true);
		}
		else
		{
			projectile.Translate(direction.normalized * distanceToTravel);
		}
	}

	public static Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
	}
}
