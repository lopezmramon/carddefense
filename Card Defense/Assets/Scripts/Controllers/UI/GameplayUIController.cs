using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameplayUIController : MonoBehaviour
{
	public Text livesLeftText, currentTowerBuildingResourceText, timeBetweenWavesText;
	public Button placeTowerButton;
	public InputField towerX, towerY, towerElement;

	private void Awake()
	{
		CodeControl.Message.AddListener<LivesChangedEvent>(OnLivesChanged);
		CodeControl.Message.AddListener<WaveFinishedEvent>(OnWaveFinished);
		CodeControl.Message.AddListener<WaveStartedEvent>(OnWaveStarted);
		CodeControl.Message.AddListener<LevelReadyEvent>(OnLevelReady);
		placeTowerButton.onClick.AddListener(() =>
		{
			Tile tile = LevelManager.currentLevel.mapData.FindTileByCoordinates(int.Parse(towerX.text), int.Parse(towerY.text));
			CodeControl.Message.Send(new TowerPlacementRequestEvent(tile, (Element)int.Parse(towerElement.text)));
		});
	}

	private void OnLevelReady(LevelReadyEvent obj)
	{
		livesLeftText.text = string.Format("Lives Left: {0}", obj.level.lives);
		currentTowerBuildingResourceText.text = string.Format("Resource: {0}", obj.level.startingTowerBuildingResource);
	}

	private void OnWaveStarted(WaveStartedEvent obj)
	{
		timeBetweenWavesText.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (timeBetweenWavesText.gameObject.activeInHierarchy)
		{
			timeBetweenWavesText.text = string.Format("Time left for next wave: {0}", Math.Truncate(TimeManager.timeBetweenWaves));
		}
	}

	private void OnWaveFinished(WaveFinishedEvent obj)
	{
		timeBetweenWavesText.gameObject.SetActive(true);
	}

	private void OnLivesChanged(LivesChangedEvent obj)
	{
		livesLeftText.text = string.Format("Lives Left: {0}", obj.totalLives.ToString());
	}
}
