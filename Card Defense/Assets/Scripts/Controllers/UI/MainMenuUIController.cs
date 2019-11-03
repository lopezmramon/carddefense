using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
	public Button play, settings, levelEditor, deckBuilder, quit;

	private void Awake()
	{
		SetupButtons();
	}

	private void SetupButtons()
	{
		play.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent(View.LevelSelect, true);
		});
		settings.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent(View.Settings, false);
		});
		levelEditor.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent(View.LevelEditor, true);
		});
		deckBuilder.onClick.AddListener(() =>
		{
			DispatchChangeViewRequestEvent(View.DeckBuilder, true);
		});
		quit.onClick.AddListener(() =>
		{
			Application.Quit();
		});
	}

	private void DispatchChangeViewRequestEvent(View view, bool turnOffPrevious)
	{
		CodeControl.Message.Send(new ChangeViewRequestEvent(View.MainMenu, view, turnOffPrevious));
	}
}
