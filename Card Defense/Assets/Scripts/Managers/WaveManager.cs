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
		CalculatePaths();
	}

	private void CalculatePaths()
	{
		startingPointIndex = 0;
		endingPointIndex = 0;
		Path path = ABPath.Construct(startingPoints[0].Vector3FromCoordinates, endingPoints[0].Vector3FromCoordinates, OnPathCalculationDone);
		AstarPath.StartPath(path);
	}

	private void OnPathCalculationDone(Path path)
	{
		if (path.CompleteState == PathCompleteState.Complete)
		{
			paths.Add(path);
			endingPointIndex++;
			if (endingPointIndex >= endingPoints.Length)
			{
				endingPointIndex = 0;
				startingPointIndex++;
			}
			if (startingPointIndex >= startingPoints.Length) return;
			Path nextPath = ABPath.Construct(startingPoints[startingPointIndex].Vector3FromCoordinates, endingPoints[endingPointIndex].Vector3FromCoordinates, OnPathCalculationDone);
			AstarPath.StartPath(nextPath);
		}
		else if (path.CompleteState == PathCompleteState.Error)
		{
			Debug.Log(path.error.ToString());
		}
	}

	private void ManageWave()
	{
		if (currentEnemyIndex < currentWave.enemies.Count)
		{
			Enemy enemy = currentWave.enemies[currentEnemyIndex];
			int randomPathIndex = UnityEngine.Random.Range(0, paths.Count);
			DispatchEnemySpawnRequestEvent(enemy.enemyType, enemy.specialAbility, paths[randomPathIndex]);
		}
		if (currentEnemyIndex >= currentWave.enemies.Count - 1)
		{
			CalculatePaths();
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

	private void DispatchEnemySpawnRequestEvent(EnemyType enemyType, EnemySpecialAbility specialAbility, Path path)
	{
		CodeControl.Message.Send(new EnemySpawnRequestEvent(enemyType, specialAbility, path));
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
