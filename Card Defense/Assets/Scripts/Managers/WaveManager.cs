using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	private Level currentLevel;
	private Wave currentWave;
	private int currentWaveIndex, currentEnemyIndex;
	private Tile[] startingPoints, endingPoints;

	private void Awake()
	{
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
		CodeControl.Message.AddListener<NextWaveStartRequestEvent>(OnNextWaveStartRequested);
	}

	private void OnNextWaveStartRequested(NextWaveStartRequestEvent obj)
	{
		StartNextWave();
	}

	private void StartNextWave()
	{
		currentWaveIndex++;
		currentEnemyIndex = 0;
		currentWave = currentLevel.waves[currentWaveIndex];
		DispatchWaveStartedEvent(currentWaveIndex);
		ManageWave();
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		currentWaveIndex = -1;
		currentEnemyIndex = 0;
		currentLevel = obj.level;
		startingPoints = currentLevel.mapData.GetStartingPoints().ToArray();
		endingPoints = currentLevel.mapData.GetEndingPoints().ToArray();
	}

	private void ManageWave()
	{
		int randomStartingPointIndex = UnityEngine.Random.Range(0, startingPoints.Length);
		int randomEndingPointIndex = UnityEngine.Random.Range(0, endingPoints.Length);
		if (currentEnemyIndex < currentWave.enemies.Count)
		{
			Enemy enemy = currentWave.enemies[currentEnemyIndex];
			DispatchEnemySpawnRequestEvent(enemy.enemyType, enemy.specialAbility, startingPoints[randomStartingPointIndex].Vector3FromCoordinates(), endingPoints[randomEndingPointIndex].Vector3FromCoordinates());
		}
		if (currentEnemyIndex == currentWave.enemies.Count - 1)
		{
			DispatchLastEnemyInWaveSpawnedEvent();
			DispatchWaveEndedEvent();
		}
		currentEnemyIndex++;
	}

	private void DispatchWaveEndedEvent()
	{
		CodeControl.Message.Send(new WaveFinishedEvent());
	}

	private void Update()
	{
		if (!TimeManager.betweenWaves && TimeManager.timeBetweenEnemies <= 0)
		{
			ManageWave();
		}
		else if (TimeManager.betweenWaves && TimeManager.timeBetweenWaves <= 0)
		{
			StartNextWave();
		}
	}

	private void DispatchEnemySpawnRequestEvent(EnemyType enemyType, EnemySpecialAbility specialAbility, Vector3 startPosition, Vector3 destination)
	{
		CodeControl.Message.Send(new EnemySpawnRequestEvent(enemyType, specialAbility, startPosition, destination));
	}

	private void DispatchLastEnemyInWaveSpawnedEvent()
	{
		CodeControl.Message.Send(new LastEnemyInWaveSpawnedEvent());
	}

	private void DispatchWaveStartedEvent(int wave)
	{
		CodeControl.Message.Send(new WaveStartedEvent(wave));
	}

	private void AllWavesFinishedEvent()
	{
		CodeControl.Message.Send(new AllWavesFinishedEvent(currentLevel.waves.Count, 
			currentLevel.TotalEnemiesInLevel()));
	}

}
