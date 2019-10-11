using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
	public EnemyController[] enemyPrefabs;

	private void Awake()
	{
		CodeControl.Message.AddListener<EnemySpawnRequestEvent>(OnEnemySpawnRequested);
	}

	private void OnEnemySpawnRequested(EnemySpawnRequestEvent obj)
	{
		SpawnEnemy(obj.enemy, obj.Path);
	}

	private void SpawnEnemy(Enemy enemy, Path path)
	{
		EnemyController enemyController = Instantiate(enemyPrefabs[(int)enemy.enemyType]);
		enemyController.Initialize(enemy, path);
		DispatchEnemySpawnedEvent(enemyController);
	}

	private void DispatchEnemySpawnedEvent(EnemyController enemy)
	{
		CodeControl.Message.Send(new EnemySpawnedEvent(enemy));
	}

}
