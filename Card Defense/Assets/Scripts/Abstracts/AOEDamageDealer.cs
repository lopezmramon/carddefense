using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageDealer
{
	private float areaOfEffect;
	private Transform effectCenter;

	public AOEDamageDealer(float areaOfEffect, Transform effectCenter)
	{
		this.areaOfEffect = areaOfEffect;
		this.effectCenter = effectCenter;
	}

	public void IncreaseAOE(float increase)
	{
		areaOfEffect += increase;
	}

	public void DecreaseAOE(float decrease)
	{
		areaOfEffect -= decrease;
	}

	public void MultiplyAOE(float factor)
	{
		areaOfEffect *= factor;
	}

	public void DivideAOE(float factor)
	{
		areaOfEffect /= factor;
	}

	public void DealAOEDamage(float damageAmount)
	{
		foreach (Collider collider in EnemiesNearPointWithCurrentAOE(effectCenter.position))
		{
			if (collider.GetComponent<EnemyController>() != null)
			{
				DispatchEnemyDamageRequestEvent(collider.GetComponent<EnemyController>(), damageAmount);
			}
		}
	}

	public Collider[] EnemiesNearPointWithCurrentAOE(Vector3 center)
	{
		return Physics.OverlapSphere(center, areaOfEffect, 1 << LayerMask.NameToLayer("Enemy"));
	}

	public Vector3[] EnemyPositionsNearPointWithCurrentAOE(Vector3 center)
	{
		List<Vector3> positions = new List<Vector3>();
		foreach (Collider enemy in EnemiesNearPointWithCurrentAOE(center))
		{
			positions.Add(enemy.transform.position);
		}
		return positions.ToArray();
	}

	private void DispatchEnemyDamageRequestEvent(EnemyController enemy, float damageAmount)
	{
		CodeControl.Message.Send(new EnemyDamageRequestEvent(enemy, damageAmount));
	}

	public Transform ClosestEnemy(Vector3 position, string currentTargetName)
	{
		Transform closestEnemy = null;
		foreach (Collider collider in EnemiesNearPointWithCurrentAOE(position))
		{
			if (collider.name == currentTargetName)
			{
				continue;
			}
			if (closestEnemy == null) closestEnemy = collider.transform;
			if (Vector3.Distance(collider.transform.position, position) <
				Vector3.Distance(closestEnemy.position, position))
			{
				closestEnemy = collider.transform;
			}
		}
		return closestEnemy;
	}
}
