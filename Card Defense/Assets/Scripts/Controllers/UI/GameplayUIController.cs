using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameplayUIController : MonoBehaviour
{
	public Text livesLeft, currentTowerBuildingResource, timeBetweenWaves, gameSpeed, countdown;
	public Button emptyHand, redrawHand;

	private void Awake()
	{
		CodeControl.Message.AddListener<LivesChangedEvent>(OnLivesChanged);
		CodeControl.Message.AddListener<WaveFinishedEvent>(OnWaveFinished);
		CodeControl.Message.AddListener<WaveStartedEvent>(OnWaveStarted);
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
		CodeControl.Message.AddListener<GameSpeedChangedEvent>(OnGameSpeedChanged);
		CodeControl.Message.AddListener<ResourceChangedEvent>(OnResourceChanged);
		CodeControl.Message.AddListener<CountdownStartedEvent>(OnCountdownStarted);
		emptyHand.onClick.AddListener(() =>
		{
			CodeControl.Message.Send(new EmptyHandRequestEvent());
		});
		redrawHand.onClick.AddListener(() =>
		{
			CodeControl.Message.Send(new RedrawHandRequestEvent());
		});
	}

	private void OnCountdownStarted(CountdownStartedEvent obj)
	{
		countdown.gameObject.SetActive(true);
		countdown.text = string.Format("Wave starting in {0}...", Math.Round(TimeManager.currentCountdown,0));
	}

	private void OnResourceChanged(ResourceChangedEvent obj)
	{
		currentTowerBuildingResource.text = string.Format("Resource: {0}", obj.resourceAmount);
	}

	private void Start()
	{
		SetSpeedText(GameManager.gameSpeedMultiplier);
	}

	private void OnGameSpeedChanged(GameSpeedChangedEvent obj)
	{
		SetSpeedText(obj.speed);
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		livesLeft.text = string.Format("Lives Left: {0}", obj.level.lives);
		currentTowerBuildingResource.text = string.Format("Resource: {0}", obj.level.startingTowerBuildingResource);
	}

	private void OnWaveStarted(WaveStartedEvent obj)
	{
		timeBetweenWaves.gameObject.SetActive(false);
		countdown.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (timeBetweenWaves.gameObject.activeInHierarchy)
		{
			timeBetweenWaves.text = string.Format("Time left for next wave: {0}", Math.Truncate(TimeManager.timeBetweenWaves));
		}
		if (countdown.gameObject.activeInHierarchy)
		{
			countdown.text = string.Format("Wave starting in {0}...", Math.Round(TimeManager.currentCountdown, 0));
		}
	}

	private void SetSpeedText(float speed)
	{
		gameSpeed.text = string.Format("Game Speed: {0}", speed);
	}

	private void OnWaveFinished(WaveFinishedEvent obj)
	{
		timeBetweenWaves.gameObject.SetActive(true);
	}

	private void OnLivesChanged(LivesChangedEvent obj)
	{
		livesLeft.text = string.Format("Lives Left: {0}", obj.totalLives.ToString());
	}

	public void DispatchSpeedSetRequestEvent(float speed)
	{
		CodeControl.Message.Send(new GameSpeedSetRequestEvent(speed));
	}

	public void DispatchSpeedChangeRequestEvent(float change)
	{
		CodeControl.Message.Send(new GameSpeedChangeRequestEvent(change));
	}

	public void DispatchNextWaveRequestEvent()
	{
		CodeControl.Message.Send(new HurryNextWaveRequestEvent());
	}
}
