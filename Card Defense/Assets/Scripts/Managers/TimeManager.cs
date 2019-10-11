using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static float timeBetweenWaves, timeBetweenEnemies, currentCountdown;
	public static bool inGameplay, betweenWaves, startingCountdown;
	private float baseTimeBetweenWaves = 5f, baseTimeBetweenEnemies = 1f, baseGameStartCountdown = 3f;

	private void Awake()
	{
		timeBetweenWaves = baseTimeBetweenWaves;
		timeBetweenEnemies = baseTimeBetweenEnemies;
		currentCountdown = baseGameStartCountdown;
		CodeControl.Message.AddListener<HurryNextWaveRequestEvent>(OnHurryNextWaveRequested);
		CodeControl.Message.AddListener<WaveStartedEvent>(OnWaveStarted);
		CodeControl.Message.AddListener<FirstWaveCountdownStartRequestEvent>(OnFirstWaveCountdownStartRequested);
		CodeControl.Message.AddListener<WaveFinishedEvent>(OnWaveFinished);
	}

	private void OnFirstWaveCountdownStartRequested(FirstWaveCountdownStartRequestEvent obj)
	{
		startingCountdown = true;
		StartFirstWaveCountdown();
		DispatchCountdownStartedEvent();
	}

	private void DispatchCountdownStartedEvent()
	{
		CodeControl.Message.Send(new CountdownStartedEvent(currentCountdown));
	}

	private void OnHurryNextWaveRequested(HurryNextWaveRequestEvent obj)
	{
		DispatchHurriedNextWaveEvent(timeBetweenWaves);
		timeBetweenWaves = 0;
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

	private void StartFirstWaveCountdown()
	{
		currentCountdown = baseGameStartCountdown;
	}

	private void Update()
	{
		if (timeBetweenWaves >= 0 && betweenWaves && inGameplay)
		{
			timeBetweenWaves -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		if (timeBetweenEnemies >= 0 && !betweenWaves && inGameplay)
		{
			timeBetweenEnemies -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (!betweenWaves && inGameplay)
		{
			timeBetweenEnemies = baseTimeBetweenEnemies;
		}
		if (currentCountdown > 0)
		{
			currentCountdown -= Time.deltaTime * GameManager.gameSpeedMultiplier;
		}
		else if (currentCountdown <= 0 && startingCountdown)
		{
			startingCountdown = false;
			DispatchNextWaveStartRequestEvent();	
		}
	}

	private void DispatchNextWaveStartRequestEvent()
	{
		CodeControl.Message.Send(new NextWaveStartRequestEvent());
	}

	private void DispatchHurriedNextWaveEvent(float timeHurried)
	{
		CodeControl.Message.Send(new HurriedNextWaveEvent(timeHurried));
	}
}
