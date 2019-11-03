using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public static List<CardContainer> baseCardContainers = new List<CardContainer>();
	private Deck<Card> deck;
	public CardData cardData;
	private CardSpriteLoader spriteLoader;
	public CardContainer cardContainerPrefab;
	public Transform cardContainerPoolParent, hand, pickedCardsDisplayLocation;
	private CardContainer grabbedCard;
	private HandModifierAnalyzer handModifierAnalyzer;
	private ExtraCardAnalyzer extraCardAnalyzer;
	private HashSet<CardContainer> pickedCards = new HashSet<CardContainer>();
	private int cardsToPick;
	private bool unrestricted;

	private void Awake()
	{
		spriteLoader = new CardSpriteLoader();
		handModifierAnalyzer = new HandModifierAnalyzer();
		extraCardAnalyzer = new ExtraCardAnalyzer();
		spriteLoader.LoadSpritesForCards(cardData.cards, OnLoadFinished);
		CodeControl.Message.AddListener<CardGrabbedEvent>(OnCardGrabbed);
		CodeControl.Message.AddListener<CardsConsumeRequestEvent>(OnCardsConsumeRequested);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		CodeControl.Message.AddListener<DrawRandomCardsRequestEvent>(OnDrawRandomCardsRequested);
		CodeControl.Message.AddListener<CardPickRequestEvent>(OnCardPickRequested);
		CodeControl.Message.AddListener<CardPickStartRequestEvent>(OnCardPickStartRequested);
		CodeControl.Message.AddListener<ConfirmCardsPickedRequestEvent>(OnPickedCardsConfirmed);
		CodeControl.Message.AddListener<CardSellProcessBeginRequestEvent>(OnCardSellProcessBeginRequested);
		CodeControl.Message.AddListener<DeckPickedEvent>(OnDeckPicked);
	}

	private void OnDeckPicked(DeckPickedEvent obj)
	{
		FillDeck(obj.deck);
	}

	private void FillDeck(CardDeck deck)
	{
		List<Card> cards = new List<Card>();
		for (int i = 0; i < deck.cardIndexes.Count; i++)
		{
			cards.Add(cardData.cards[deck.cardIndexes[i]]);
		}
		this.deck = new Deck<Card>(cards);
		DrawRandomCards(5);
	}

	private void OnCardSellProcessBeginRequested(CardSellProcessBeginRequestEvent obj)
	{
		StartPickingCards(0);
		unrestricted = true;
	}

	private void OnPickedCardsConfirmed(ConfirmCardsPickedRequestEvent obj)
	{
		handModifierAnalyzer.FinalizeHandModifierOperation(pickedCards);
		pickedCards.Clear();
		cardsToPick = 0;
		unrestricted = false;
		DispatchCardPickEndEvent();
	}

	private void OnCardPickStartRequested(CardPickStartRequestEvent obj)
	{
		StartPickingCards(obj.amount);
	}

	private void OnCardPickRequested(CardPickRequestEvent obj)
	{
		CheckPickedCards(obj.card);
	}

	private void OnCardsConsumeRequested(CardsConsumeRequestEvent obj)
	{
		ConsumeCards(obj.cards);
	}

	private void OnCardDropped(CardDroppedEvent obj)
	{
		Card card = obj.card.card;
		if (card.cost > ResourceManager.currentResourceAmount || obj.overDrawer || card.propertyModifiers[0] != PropertyModifier.None) return;
		ConsumeCard(obj.card);
		if (card.cardType == CardType.HandModifier)
			handModifierAnalyzer.AnalyzeModifiers(card.handModifier, card.handModifierValue);
		if (card.cardType == CardType.Extra)
			extraCardAnalyzer.AnalyzeExtraCard(card);
		DispatchResourceChangeRequestEvent(card.cost);
	}

	private void OnDrawRandomCardsRequested(DrawRandomCardsRequestEvent obj)
	{
		DrawCards(obj.amount);
	}

	private void OnCardGrabbed(CardGrabbedEvent obj)
	{
		grabbedCard = obj.card;
	}

	private void OnLoadFinished(Dictionary<Card, Sprite[]> cardSprites)
	{
		foreach (KeyValuePair<Card, Sprite[]> card in cardSprites)
		{
			card.Key.backgroundSprite = card.Value[0];
			card.Key.illustrationSprite = card.Value[1];
			card.Key.resourceSprite = card.Value[2];
			GenerateCardContainer(card.Key);
		}
	}

	private void StartPickingCards(int amount)
	{
		if (hand.childCount == 0) return;
		cardsToPick = amount;
		if (cardsToPick > hand.childCount)
		{
			cardsToPick = hand.childCount;
		}
		DispatchCardPickStartedEvent(cardsToPick, handModifierAnalyzer.currentHandModifier, handModifierAnalyzer.currentHandModifierValue);
	}

	private void CheckPickedCards(CardContainer card)
	{
		if (pickedCards.Contains(card))
		{
			PutCardBackOnDrawer(card);
		}
		else
		{
			PresentCard(card);
		}
	}

	private void PresentCard(CardContainer card)
	{
		int cardsLeftToPick = cardsToPick - pickedCards.Count;
		if (cardsLeftToPick > 0 || unrestricted)
		{
			card.transform.SetParent(pickedCardsDisplayLocation);
			pickedCards.Add(card);
			DispatchCardPickedEvent(card, true, cardsToPick - pickedCards.Count);
		}
		else
		{
			PutCardBackOnDrawer(card);
		}
	}

	private void PutCardBackOnDrawer(CardContainer card)
	{
		card.transform.SetParent(hand);
		pickedCards.Remove(card);
		DispatchCardPickedEvent(card, false, cardsToPick - pickedCards.Count);
	}

	public void DrawCards(int amount)
	{
		List<Card> cardsToDraw = deck.Draw(amount);
		for (int i = 0; i < cardsToDraw.Count; i++)
		{
			CardContainer cardToInstantiate = baseCardContainers.Find(x => x.card == cardsToDraw[i]);
			CardContainer card = Instantiate(cardToInstantiate, hand);
			DispatchCardDrawnEvent(card);
			card.name = card.cardName.text;
			card.gameObject.SetActive(true);
		}
	}

	private void DrawRandomCards(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			int randomCard = UnityEngine.Random.Range(0, baseCardContainers.Count);
			CardContainer card = Instantiate(baseCardContainers[randomCard], hand);
			DispatchCardDrawnEvent(card);
			card.name = card.cardName.text;
			card.gameObject.SetActive(true);
		}
	}

	private void GenerateCardContainer(Card card)
	{
		CardContainer cardContainer = Instantiate(cardContainerPrefab, cardContainerPoolParent);
		cardContainer.Initialize(card);
		baseCardContainers.Add(cardContainer);
		cardContainer.gameObject.SetActive(false);
	}

	private void ConsumeCard(CardContainer card)
	{
		deck.DiscardCard(card.card);
		card.Discard();
		DispatchCardConsumedEvent(card);
	}

	private void ConsumeCards(CardContainer[] cards)
	{
		foreach (CardContainer card in cards)
		{
			ConsumeCard(card);
		}
	}

	private void DispatchCardPickedEvent(CardContainer card, bool picked, int amountLeft)
	{
		CodeControl.Message.Send(new CardPickedEvent(card, picked, amountLeft, pickedCards.Count));
	}

	private void DispatchCardPickEndEvent()
	{
		CodeControl.Message.Send(new CardPickEndedEvent());
	}

	private void DispatchCardDrawnEvent(CardContainer card)
	{
		CodeControl.Message.Send(new CardDrawnEvent(card, deck.CardsInDrawPile));
	}

	private void DispatchEmptyHandRequestEvent()
	{
		CodeControl.Message.Send(new EmptyHandRequestEvent());
	}

	private void DispatchCardConsumedEvent(CardContainer card)
	{
		CodeControl.Message.Send(new CardConsumedEvent(card, deck.CardsInDiscardPile));
	}

	private void DispatchResourceChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new ResourceChangeRequestEvent(amount));
	}

	private void DispatchCardPickStartedEvent(int cardsToPick, HandModifier handModifier, float value)
	{
		CodeControl.Message.Send(new CardPickStartedEvent(cardsToPick, handModifier, value));
	}
}
