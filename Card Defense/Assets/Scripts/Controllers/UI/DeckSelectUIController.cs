using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeckSelectUIController : MonoBehaviour
{
	public DeckDisplayController deckDisplayControllerPrefab, selectedDeckDisplay;
	public Transform deckDisplayParent;
	public Sprite deckBack;
	public Button confirm, mainMenu;

	private void Awake()
	{
		SetupButtons();
		confirm.interactable = false;
	}

	private void SetupButtons()
	{
		confirm.onClick.AddListener(() =>
		{
			DispatchDeckPickedEvent();
			DispatchGenerateLevelRequestEvent();
			DispatchChangeViewRequestEvent();
			gameObject.SetActive(false);
		});

		mainMenu.onClick.AddListener(() =>
		{
			CodeControl.Message.Send(new ChangeViewRequestEvent(View.DeckSelect, View.MainMenu, true));
		});
	}

	private void OnEnable()
	{
		GenerateVisualDecks();
	}

	private void GenerateVisualDecks()
	{
		foreach (CardDeck deck in CollectionManager.currentDecks)
		{
			DeckDisplayController deckDisplay = Instantiate(deckDisplayControllerPrefab, deckDisplayParent);
			deckDisplay.Initialize(deckBack, deck, OnDeckClicked);
		}
	}

	private void OnDeckClicked(DeckDisplayController deckDisplay)
	{
		if (selectedDeckDisplay == deckDisplay)
		{
			selectedDeckDisplay.ToggleOutline(false);
			confirm.interactable = false;
			selectedDeckDisplay = null;
		}
		else
		{
			if (selectedDeckDisplay != null) selectedDeckDisplay.ToggleOutline(false);
			confirm.interactable = true;
			selectedDeckDisplay = deckDisplay;
			selectedDeckDisplay.ToggleOutline(true);
		}
	}

	private void DispatchDeckPickedEvent()
	{
		CodeControl.Message.Send(new DeckPickedEvent(selectedDeckDisplay.deck));
	}

	private void DispatchGenerateLevelRequestEvent()
	{
		CodeControl.Message.Send(new GenerateLevelRequestEvent(true));
	}

	private void DispatchChangeViewRequestEvent()
	{
		CodeControl.Message.Send(new ChangeViewRequestEvent(View.DeckSelect, View.Gameplay, true));
	}
}
