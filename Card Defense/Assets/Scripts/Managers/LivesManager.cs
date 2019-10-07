using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesManager : MonoBehaviour
{
	private int currentLevelLives, currentLives;

	private void Awake()
	{
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
		CodeControl.Message.AddListener<EnemyReachedDestinationEvent>(OnEnemyReachedDestination);
		CodeControl.Message.AddListener<LivesChangeRequestEvent>(OnLivesChangeRequested);
	}

	private void OnLivesChangeRequested(LivesChangeRequestEvent obj)
	{
		ChangeLives(obj.amount);
	}

	private void OnEnemyReachedDestination(EnemyReachedDestinationEvent obj)
	{
		ChangeLives(-1);
	}

	private void ChangeLives(int amount)
	{
		currentLives += amount;
		DispatchLivesChangedEvent();
		if (currentLives <= 0)
		{
			DispatchLevelLostEvent();
		}
	}

	private void DispatchLevelLostEvent()
	{
		CodeControl.Message.Send(new LevelLostEvent());
	}

	private void DispatchLivesChangedEvent()
	{
		CodeControl.Message.Send(new LivesChangedEvent(currentLives));
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		currentLevelLives = obj.level.lives;
		currentLives = obj.level.lives;
	}
}
