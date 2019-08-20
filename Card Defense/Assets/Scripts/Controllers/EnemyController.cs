using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
	public float health, speed;
	private AILerp AILerp;
	private Enemy enemy;

	private void Awake()
	{
		AILerp = GetComponent<AILerp>();
		AILerp.onTargetReached += OnPathEndReached;
		AILerp.speed = speed;
	}

	private void OnPathEndReached()
	{
		DispatchEnemyReachedDestinationEvent();
		Die();
	}

	public void Damage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		gameObject.SetActive(false);
	}

	internal void Initialize(Enemy enemy, Vector3 startingPoint, Vector3 destination)
	{
		if (AILerp == null) AILerp = GetComponent<AILerp>();
		this.enemy = enemy;
		destination *= 2;
		transform.position = startingPoint;
		AILerp.destination = destination;
		AILerp.SearchPath();
	}

	private void DispatchEnemyReachedDestinationEvent()
	{
		CodeControl.Message.Send(new EnemyReachedDestinationEvent(enemy));
	}
}
