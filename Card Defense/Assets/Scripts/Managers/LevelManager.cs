using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public Level[] levels;
	public static Level currentLevel;
	private int selectedLevel;

	private void Awake()
	{
		CodeControl.Message.AddListener<GenerateLevelRequestEvent>(OnLevelGenerateRequested);
		CodeControl.Message.AddListener<LevelSelectedEvent>(OnLevelSelected);
	}

	private void OnLevelSelected(LevelSelectedEvent obj)
	{
		selectedLevel = obj.levelIndex;
	}

	private void Start()
	{
		LoadLevels();
	}

	private void LoadLevels()
	{
		levels = Resources.LoadAll<Level>("Levels");
		DispatchLevelsLoadedEvent();
	}

	private void DispatchLevelsLoadedEvent()
	{
		CodeControl.Message.Send(new LevelsLoadedEvent(levels));
	}

	private void OnLevelGenerateRequested(GenerateLevelRequestEvent obj)
	{
		currentLevel = levels[obj.generateAlreadySelected ? selectedLevel : obj.levelIndex];
		CodeControl.Message.Send(new GenerateTilesRequestEvent(currentLevel));
	}
}
