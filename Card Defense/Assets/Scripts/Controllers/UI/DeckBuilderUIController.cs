using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeckBuilderUIController : MonoBehaviour
{
	public ScrollRect cardsDisplayScrollRect, deckDisplayScrollRect;
	public CardDisplayController cardDisplayControllerPrefab;
	private List<CardDisplayController> cardDisplays = new List<CardDisplayController>();
	public MinimalCardDisplayController minimalCardDisplayControllerPrefab;
	private List<MinimalCardDisplayController> minimalCardDisplays = new List<MinimalCardDisplayController>();
	public MinimalDeckDisplayController minimalDeckDisplayControllerPrefab;
	private List<MinimalDeckDisplayController> deckDisplays = new List<MinimalDeckDisplayController>();
	public TextMeshProUGUI warning, nothingToDisplay;
	public TMP_InputField deckNameInput;
	private WaitForSeconds flashTiming = new WaitForSeconds(0.5f);
	public Button newDeck, deleteDeck, doneButton, mainMenuButton;
	public GameObject deckDataUI;
	public CardTradingUIController cardTradingUIController;
	private CardDeck openDeck;

	private void Awake()
	{
		SetupButtons();
	}

	private void Start()
	{
		InitializeCardDisplays();
		DisplayCurrentDecks();
	}

	private void InitializeCardDisplays()
	{
		foreach (CardContainer card in CardManager.baseCardContainers)
		{
			CardDisplayController cardDisplay = Instantiate(cardDisplayControllerPrefab, cardsDisplayScrollRect.content);
			cardDisplay.Initialize(card.card, CollectionUtils.DuplicatesInDeck(CollectionManager.collection, card.card.index), AddCardToOpenDeck, ShowCardTradingUI);
			cardDisplays.Add(cardDisplay);
		}
		DisplayFullCollection();
	}

	private void ShowCardTradingUI(CardDisplayController cardDisplay)
	{
		cardTradingUIController.gameObject.SetActive(true);
		cardTradingUIController.Initialize(cardDisplay.card, SellCard, BuyCard);
	}

	private void SellCard(Card card)
	{
		CollectionManager.collection.cardIndexes.Remove(card.index);
		DisplayCollectionRelativeToDeck(openDeck);
	}

	private void BuyCard(Card card)
	{
		CollectionManager.collection.cardIndexes.Add(card.index);
		DisplayCollectionRelativeToDeck(openDeck);
	}

	private void DisplayFullCollection()
	{
		foreach (CardDisplayController cardDisplay in cardDisplays)
		{
			cardDisplay.SetAmount(CollectionUtils.DuplicatesInDeck(CollectionManager.collection, cardDisplay.card.index));
		}
	}

	private void DisplayCollectionRelativeToDeck(CardDeck cardDeck)
	{
		CardDeck relativeDeck = CollectionUtils.RelativeCollection(CollectionManager.collection, cardDeck);
		foreach (CardDisplayController cardDisplay in cardDisplays)
		{
			cardDisplay.SetAmount(CollectionUtils.DuplicatesInDeck(relativeDeck, cardDisplay.card.index));
		}
	}

	private void DisplayCurrentDecks()
	{
		nothingToDisplay.gameObject.SetActive(CollectionManager.currentDecks.Count == 0);
		nothingToDisplay.text = "No Decks to Display, create a new deck!";
		foreach (CardDeck cardDeck in CollectionManager.currentDecks)
		{
			MinimalDeckDisplayController existingDeckDisplay = deckDisplays.Find(x => x.deck == cardDeck);
			if (deckDisplays.Count == 0 || existingDeckDisplay == null)
			{
				MinimalDeckDisplayController deckDisplay = Instantiate(minimalDeckDisplayControllerPrefab, deckDisplayScrollRect.content);
				deckDisplay.Initialize(cardDeck, OnMinimalDeckDisplayClicked);
				deckDisplays.Add(deckDisplay);
			}
			else
			{
				existingDeckDisplay.gameObject.SetActive(true);
			}
		}
	}

	private void OnMinimalDeckDisplayClicked(MinimalDeckDisplayController display)
	{
		OpenDeck(display.deck);
	}

	private void SetupButtons()
	{
		newDeck.onClick.AddListener(() =>
		{
			NewDeckButtonClicked();
		});

		doneButton.onClick.AddListener(() =>
		{
			CloseDeck();
		});

		deleteDeck.onClick.AddListener(() =>
		{
			DeleteDeck();
		});

		mainMenuButton.onClick.AddListener(() =>
		{
			CodeControl.Message.Send(new ChangeViewRequestEvent(View.DeckBuilder, View.MainMenu, true));
		});
	}

	private void NewDeckButtonClicked()
	{
		if (openDeck == null)
		{
			CreateNewDeck();
		}
		else
		{
			DisplayDeckInfoUI(openDeck);
		}
	}

	private void DisplayDeckInfoUI(CardDeck openDeck)
	{
		Debug.Log("Displaying info for open deck");
	}

	private void DeleteDeck()
	{
		CollectionUtils.DeleteDeck(openDeck);
		CloseDeck();
	}

	private void CreateNewDeck()
	{
		CardDeck newDeck = CollectionUtils.NewDeck(deckNameInput.text);
		OpenDeck(newDeck);
	}

	private void OpenDeck(CardDeck cardDeck)
	{
		openDeck = cardDeck;
		SwitchNewDeckButton();
		DeactivateAllDeckDisplays();
		DisplayCollectionRelativeToDeck(cardDeck);
		DisplayCardsInDeck(cardDeck);
	}

	private void DisplayCardsInDeck(CardDeck cardDeck)
	{
		DeactivateAllMinimalCardDisplays();
		nothingToDisplay.gameObject.SetActive(cardDeck.cardIndexes.Count == 0);
		nothingToDisplay.text = "This deck is empty. Add some cards!";
		foreach (int index in cardDeck.cardIndexes)
		{
			MinimalCardDisplayController minimalCardDisplay = minimalCardDisplays.Find(x => x.card.index == index);
			if (minimalCardDisplays.Count == 0 || minimalCardDisplay == null)
			{
				Card card = CardManager.baseCardContainers.Find(x => x.card.index == index).card;
				minimalCardDisplay = Instantiate(minimalCardDisplayControllerPrefab, deckDisplayScrollRect.content);
				minimalCardDisplay.Initialize(card, CollectionUtils.DuplicatesInList(cardDeck.cardIndexes, index), OnMinimalCardDisplayClicked);
				minimalCardDisplays.Add(minimalCardDisplay);
			}
			else
			{
				minimalCardDisplay.SetAmount(CollectionUtils.DuplicatesInList(cardDeck.cardIndexes, index));
			}
			minimalCardDisplay.gameObject.SetActive(true);
		}
	}

	private void OnMinimalCardDisplayClicked(MinimalCardDisplayController minimalCardDisplay)
	{
		RemoveCardFromDeck(minimalCardDisplay);
	}

	private void CloseDeck()
	{
		openDeck = null;
		DeactivateAllMinimalCardDisplays();
		SwitchNewDeckButton();
		DisplayCurrentDecks();
		DisplayCollectionRelativeToDeck(null);
	}

	private void SwitchNewDeckButton()
	{
		newDeck.GetComponentInChildren<TextMeshProUGUI>().text = openDeck == null ? "New Deck" : openDeck.name;
	}

	private void DeactivateAllMinimalCardDisplays()
	{
		foreach (MinimalCardDisplayController display in minimalCardDisplays)
		{
			display.gameObject.SetActive(false);
		}
	}

	private void DeactivateAllDeckDisplays()
	{
		foreach (MinimalDeckDisplayController display in deckDisplays)
		{
			display.gameObject.SetActive(false);
		}
	}

	private void AddCardToOpenDeck(CardDisplayController card)
	{
		if (openDeck == null) return;
		if (CollectionUtils.DuplicatesInList(openDeck.cardIndexes, card.card.index) < 3 &&
			CollectionManager.collection.cardIndexes.Contains(card.card.index))
		{
			openDeck.cardIndexes.Add(card.card.index);
		}
		UpdateOpenDeck();
	}

	private void RemoveCardFromDeck(MinimalCardDisplayController card)
	{
		openDeck.cardIndexes.Remove(card.card.index);
		UpdateOpenDeck();
	}

	private void SaveCurrentDeck()
	{
		if (deckNameInput.text == "")
		{
			StartCoroutine(WarningTextFlash());
		}
		else
		{
			CollectionUtils.SaveDeck(openDeck, deckNameInput.text);
		}
	}

	private void UpdateOpenDeck()
	{
		DisplayCardsInDeck(openDeck);
		DisplayCollectionRelativeToDeck(openDeck);
		CollectionUtils.SaveDeck(openDeck);
	}

	private IEnumerator WarningTextFlash()
	{
		warning.text = "Deck name cannot be empty!";
		yield return flashTiming;
		warning.text = string.Empty;
		yield return flashTiming;
		warning.text = "Deck name cannot be empty!";
		yield return flashTiming;
		warning.text = string.Empty;
		yield return null;
	}
}
