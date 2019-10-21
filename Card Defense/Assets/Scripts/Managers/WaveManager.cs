using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	private Level currentLevel;
	private Wave currentWave;
	private int currentWaveIndex, currentEnemyIndex, startingPointIndex, endingPointIndex;
	private Tile[] startingPoints, endingPoints;
	public List<Path> paths = new List<Path>();

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
		if (currentWaveIndex < currentLevel.waves.Count)
		{
			currentWave = currentLevel.waves[currentWaveIndex];
			DispatchWaveStartedEvent(currentWaveIndex);
			ManageWave();
		}
		else
		{
			DispatchAllWavesFinishedEvent();
		}
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		SetupForLevel(obj.level);
	}

	private void SetupForLevel(Level level)
	{
		currentWaveIndex = -1;
		currentEnemyIndex = 0;
		currentLevel = level;
		startingPoints = currentLevel.mapData.GetStartingPoints().ToArray();
		endingPoints = currentLevel.mapData.GetEndingPoints().ToArray();
	}

	private void ManageWave()
	{
		if (currentEnemyIndex < currentWave.enemies.Count)
		{
			Enemy enemy = currentWave.enemies[currentEnemyIndex];
			enemy.waveIndex = currentEnemyIndex;
			enemy.totalWaveEnemies = currentWave.enemies.Count;
			DispatchEnemySpawnRequestEvent(enemy, startingPoints,endingPoints);
		}
		if (currentEnemyIndex >= currentWave.enemies.Count - 1)
		{
			DispatchLastEnemyInWaveSpawnedEvent();
		}
		currentEnemyIndex++;
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

	private void DispatchEnemySpawnRequestEvent(Enemy enemy, Tile[] startingPoints, Tile[] endingPoints)
	{
		CodeControl.Message.Send(new EnemySpawnRequestEvent(enemy, startingPoints, endingPoints));
	}

	private void DispatchLastEnemyInWaveSpawnedEvent()
	{
		CodeControl.Message.Send(new LastEnemyInWaveSpawnedEvent());
	}

	private void DispatchWaveStartedEvent(int wave)
	{
		CodeControl.Message.Send(new WaveStartedEvent(wave));
	}

	private void DispatchAllWavesFinishedEvent()
	{
		CodeControl.Message.Send(new AllWavesFinishedEvent(currentLevel.waves.Count,
			currentLevel.TotalEnemiesInLevel()));
	}
}
