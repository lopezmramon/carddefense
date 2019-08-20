using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static float timeBetweenWaves, timeBetweenEnemies, gameStartCountdown;
	public static bool inGameplay, betweenWaves;
	private float baseTimeBetweenWaves = 5f, baseTimeBetweenEnemies = 1f, baseGameStartCountdown = 3f;

	private void Awake()
	{
		timeBetweenWaves = baseTimeBetweenWaves;
		timeBetweenEnemies = baseTimeBetweenEnemies;
		gameStartCountdown = baseGameStartCountdown;
		CodeControl.Message.AddListener<HurryNextWaveRequestEvent>(OnHurryNextWaveRequested);
		CodeControl.Message.AddListener<WaveStartedEvent>(OnWaveStarted);
		CodeControl.Message.AddListener<WaveFinishedEvent>(OnWaveFinished);
	}

	private void OnHurryNextWaveRequested(HurryNextWaveRequestEvent obj)
	{
		betweenWaves = false;
		timeBetweenWaves = baseTimeBetweenWaves;
	}

	private void OnWaveFinished(WaveFinishedEvent obj)
	{
		betweenWaves = true;
	}

	private void OnWaveStarted(WaveStartedEvent obj)
	{
		betweenWaves = false;
		timeBetweenWaves = baseTimeBetweenWaves;
		inGameplay = true;
	}

	private void Update()
	{
		if (timeBetweenWaves >= 0 && betweenWaves && inGameplay)
		{
			timeBetweenWaves -= Time.deltaTime;
		}

		if (timeBetweenEnemies >= 0 && !betweenWaves && inGameplay)
		{
			timeBetweenEnemies -= Time.deltaTime;
		}
		else if (!betweenWaves && inGameplay)
		{
			timeBetweenEnemies = baseTimeBetweenEnemies;
		}
	}
}
