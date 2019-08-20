using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public Level[] levels;
	public static Level currentLevel;

	private void Awake()
	{
		CodeControl.Message.AddListener<GenerateLevelRequestEvent>(OnLevelGenerateRequested);
	}

	private void OnLevelGenerateRequested(GenerateLevelRequestEvent obj)
	{
		currentLevel = levels[obj.levelIndex];
	}

	private void Start()
	{
		currentLevel = levels[0];
		CodeControl.Message.Send(new GenerateTilesRequestEvent(currentLevel));
	}
}
