using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private List<EnemyController> enemies = new List<EnemyController>();

	private void Awake()
	{
		CodeControl.Message.AddListener<EnemyDeathEvent>(OnEnemyDeath);
		CodeControl.Message.AddListener<EnemyReachedDestinationEvent>(OnEnemyReachedDestination);
		CodeControl.Message.AddListener<EnemySpawnedEvent>(OnEnemySpawned);
		CodeControl.Message.AddListener<EnemyDamageRequestEvent>(OnEnemyDamageRequested);
	}

	private void OnEnemyDeath(EnemyDeathEvent obj)
	{
		enemies.Remove(obj.enemy);
	}

	private void OnEnemyReachedDestination(EnemyReachedDestinationEvent obj)
	{
		enemies.Remove(obj.enemy);
	}

	private void OnEnemySpawned(EnemySpawnedEvent obj)
	{
		enemies.Add(obj.enemy);
	}

	private void OnEnemyDamageRequested(EnemyDamageRequestEvent obj)
	{
		obj.enemy.Damage(obj.damageAmount);
	}
}
