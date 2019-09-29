using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public CardData cardData;
	private CardSpriteLoader spriteLoader;
	public CardContainer cardContainerPrefab;
	public static List<CardContainer> baseCardContainers = new List<CardContainer>();
	public Transform cardContainerPoolParent, hand, pickedCardsDisplayLocation;
	private CardContainer grabbedCard;
	private HandModifierAnalyzer handModifierAnalyzer;
	private HashSet<CardContainer> pickedCards = new HashSet<CardContainer>();
	private int cardsToPick;

	private void Awake()
	{
		spriteLoader = new CardSpriteLoader();
		handModifierAnalyzer = new HandModifierAnalyzer();
		spriteLoader.LoadSpritesForCards(cardData.cards, OnLoadFinished);
		CodeControl.Message.AddListener<CardGrabbedEvent>(OnCardGrabbed);
		CodeControl.Message.AddListener<CardsConsumeRequestEvent>(OnCardsConsumeRequested);
		CodeControl.Message.AddListener<CardDroppedEvent>(OnCardDropped);
		CodeControl.Message.AddListener<DrawRandomCardsRequestEvent>(OnDrawRandomCardsRequested);
		CodeControl.Message.AddListener<CardPickRequestEvent>(OnCardPickRequested);
		CodeControl.Message.AddListener<CardPickStartRequestEvent>(OnCardPickStartRequested);
		CodeControl.Message.AddListener<ConfirmCardsPickedRequestEvent>(OnPickedCardsConfirmed);
		CodeControl.Message.AddListener<CardSellProcessBeginRequestEvent>(OnCardSellProcessBeginRequested);
	}

	private void OnCardSellProcessBeginRequested(CardSellProcessBeginRequestEvent obj)
	{
		DispatchCardPickStartedEvent(0);
	}

	private void OnPickedCardsConfirmed(ConfirmCardsPickedRequestEvent obj)
	{
		handModifierAnalyzer.FinalizeHandModifierOperation(pickedCards);
		pickedCards.Clear();
		cardsToPick = 0;
		DispatchCardPickEndEvent();
	}

	private void OnCardPickStartRequested(CardPickStartRequestEvent obj)
	{
		cardsToPick = obj.amount;
		if (cardsToPick > hand.childCount)
		{
			cardsToPick = hand.childCount;
		}
		DispatchCardPickStartedEvent(cardsToPick);
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
		if (card.cardType != CardType.HandModifier || card.cost > ResourceManager.currentResourceAmount || obj.overDrawer) return;
		handModifierAnalyzer.AnalyzeModifiers(card.handModifiers, card.handModifierValues);
		ConsumeCard(obj.card);
		DispatchResourceChangeRequestEvent(card.cost);
	}

	private void OnDrawRandomCardsRequested(DrawRandomCardsRequestEvent obj)
	{
		DrawRandomCards(obj.amount);
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
		if (cardsLeftToPick > 0)
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

	public void DrawRandomCards(int amount)
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
		DispatchCardDrawnEvent(cardContainer);
	}

	private void ConsumeCard(CardContainer card)
	{
		Destroy(card.gameObject);
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
		CodeControl.Message.Send(new CardPickedEvent(card, picked, amountLeft));
	}

	private void DispatchCardPickEndEvent()
	{
		CodeControl.Message.Send(new CardPickEndedEvent());
	}

	private void DispatchCardDrawnEvent(CardContainer card)
	{
		CodeControl.Message.Send(new CardDrawnEvent(card));
	}

	private void DispatchEmptyHandRequestEvent()
	{
		CodeControl.Message.Send(new EmptyHandRequestEvent());
	}

	private void DispatchCardConsumedEvent(CardContainer card)
	{
		CodeControl.Message.Send(new CardConsumedEvent(card));
	}

	private void DispatchResourceChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new ResourceChangeRequestEvent(amount));
	}

	private void DispatchCardPickStartedEvent(int cardsToPick)
	{
		CodeControl.Message.Send(new CardPickStartedEvent(cardsToPick));
	}
}
