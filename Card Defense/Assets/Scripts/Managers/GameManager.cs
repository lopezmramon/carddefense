using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
	public static float gameSpeedMultiplier = 5f;
	[HideInInspector]
	public float speed;

	private void Awake()
	{
		CodeControl.Message.AddListener<GameSpeedChangeRequestEvent>(OnGameSpeedChangeRequested);
		CodeControl.Message.AddListener<GameSpeedSetRequestEvent>(OnGameSpeedSetRequested);
	}

	private void OnGameSpeedChangeRequested(GameSpeedChangeRequestEvent obj)
	{
		gameSpeedMultiplier += obj.change;
		if (gameSpeedMultiplier < 0) gameSpeedMultiplier = 0;
		DispatchGameSpeedChangedEvent(gameSpeedMultiplier);
	}

	private void OnGameSpeedSetRequested(GameSpeedSetRequestEvent obj)
	{
		gameSpeedMultiplier = obj.speed;
		DispatchGameSpeedChangedEvent(obj.speed);
	}

	public void DispatchGameplayStartEvent()
	{
		CodeControl.Message.Send(new GameplayStartEvent());
	}

	public void DispatchFirstWaveCountdownStartRequestEvent()
	{
		CodeControl.Message.Send(new FirstWaveCountdownStartRequestEvent());
	}

	public void UpdateGameSpeed(float speed)
	{
		gameSpeedMultiplier = speed;
		DispatchGameSpeedChangedEvent(speed);
	}

	private void DispatchGameSpeedChangedEvent(float newSpeed)
	{
		CodeControl.Message.Send(new GameSpeedChangedEvent(newSpeed));
	}
}
