using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIController : MonoBehaviour
{
	public LevelDisplayUIController levelDisplayUIControllerPrefab;
	private List<LevelDisplayUIController> levelDisplayUIControllers = new List<LevelDisplayUIController>();
	public ScrollRect scrollRect;
	private int selectedLevel = 0;
	private Level[] levels;
	public Button mainMenu, confirm;

	private void Awake()
	{
		CodeControl.Message.AddListener<LevelsLoadedEvent>(OnLevelsLoaded);
		SetupButtons();
	}

	private void SetupButtons()
	{
		mainMenu.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent(View.MainMenu);
		});
		confirm.onClick.AddListener(() =>
		{
			DispatchLevelSelectedEvent();
			DispatchChangeViewRequestEvent(View.DeckSelect);
		});
	}

	private void OnLevelsLoaded(LevelsLoadedEvent obj)
	{
		ClearScrollRect();
		levels = obj.levels;
		for (int i = 0; i < levels.Length; i++)
		{
			LevelDisplayUIController levelDisplayUI = Instantiate(levelDisplayUIControllerPrefab, scrollRect.content);
			levelDisplayUI.Initialize(levels[i].sprite, levels[i].levelName, i, (index) => SetSelectedLevel(index));
			levelDisplayUIControllers.Add(levelDisplayUI);
		}
	}

	private void SetSelectedLevel(int index)
	{
		selectedLevel = index;
		TurnOnSpecificOutline(index);
	}

	private void TurnOnSpecificOutline(int index)
	{
		foreach (LevelDisplayUIController display in levelDisplayUIControllers)
		{
			display.ToggleOutline(false);
		}
		levelDisplayUIControllers[index].ToggleOutline(true);
	}

	private void ClearScrollRect()
	{
		foreach (Transform child in scrollRect.content)
		{
			Destroy(child.gameObject);
		}
		levelDisplayUIControllers.Clear();
	}

	private void DispatchLevelSelectedEvent()
	{
		CodeControl.Message.Send(new LevelSelectedEvent(selectedLevel));
	}

	private void DispatchChangeViewRequestEvent(View view)
	{
		CodeControl.Message.Send(new ChangeViewRequestEvent(View.LevelSelect, view, true));
	}
}
