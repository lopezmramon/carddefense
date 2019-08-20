using System;
using System.Collections;
using System.Collections.Generic;
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
		SpawnEnemy(obj.enemy, obj.startingPosition, obj.destination);
	}

	public void SpawnEnemy(Enemy enemy, Vector3 startingPosition, Vector3 destination)
	{
		EnemyController enemyController = Instantiate(enemyPrefabs[(int)enemy.enemyType]);
		enemyController.Initialize(enemy, startingPosition, destination);
	}
}
