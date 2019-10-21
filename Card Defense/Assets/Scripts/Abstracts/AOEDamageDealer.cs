using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageDealer
{
	public float areaOfEffect;
	private Transform effectCenter;
	private Queue<Element> elements;

	public AOEDamageDealer(float areaOfEffect, Transform effectCenter, Queue<Element> elements)
	{
		this.areaOfEffect = areaOfEffect;
		this.effectCenter = effectCenter;
		this.elements = elements;
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

	public Collider[] EnemiesNearPointWithSpecifiedAOE(Vector3 center, float aoe)
	{
		return Physics.OverlapSphere(center, aoe, 1 << LayerMask.NameToLayer("Enemy"));
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
		CodeControl.Message.Send(new EnemyDamageRequestEvent(enemy, damageAmount, elements));
	}

	public Transform ClosestEnemy(Vector3 position, Transform currentTarget, float aoe)
	{
		Transform closestEnemy = null;
		foreach (Collider collider in EnemiesNearPointWithSpecifiedAOE(position, aoe))
		{
			if (collider.transform == currentTarget)
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
