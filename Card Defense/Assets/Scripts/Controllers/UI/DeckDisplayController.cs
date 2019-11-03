using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeckDisplayController : MonoBehaviour
{
	public Button button;
	public TextMeshProUGUI deckInfo;
	private Outline outline;
	public CardDeck deck;

	public void Initialize(Sprite deckSprite, CardDeck deck, Action<DeckDisplayController> callback)
	{
		button.image.sprite = deckSprite;
		outline = GetComponentInChildren<Outline>();
		outline.effectColor = Color.yellow;
		this.deck = deck;
		ToggleOutline(false);
		deckInfo.text = $"{deck.name}, {deck.cardIndexes.Count} Cards";
		button.onClick.AddListener(() => callback(this));
	}

	public void ToggleOutline(bool active)
	{
		outline.enabled = active;
	}
}
