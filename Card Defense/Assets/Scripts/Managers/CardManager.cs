using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public CardData cardData;
	private CardSpriteLoader spriteLoader;
	public CardContainer cardContainerPrefab;
	public static List<CardContainer> cardContainers = new List<CardContainer>();
	public Transform cardContainerPoolParent, hand;
	private CardContainer grabbedCard;

	private void Awake()
	{
		spriteLoader = new CardSpriteLoader();
		spriteLoader.LoadSpritesForCards(cardData.cards, OnLoadFinished);
		CodeControl.Message.AddListener<CardGrabbedEvent>(OnCardGrabbed);
		CodeControl.Message.AddListener<CardConsumedEvent>(OnCardConsumed);
	}

	private void OnCardConsumed(CardConsumedEvent obj)
	{
		cardContainers.Remove(obj.consumedCard);
		Destroy(obj.consumedCard.gameObject);
	}

	private void OnCardGrabbed(CardGrabbedEvent obj)
	{
		grabbedCard = obj.card;
	}

	public void DrawRandomCards(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			int randomCard = UnityEngine.Random.Range(0, cardContainers.Count);
			CardContainer card = Instantiate(cardContainers[randomCard], hand);
			card.name = card.cardName.text;
			card.gameObject.SetActive(true);
		}
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

	private void GenerateCardContainer(Card card)
	{
		CardContainer cardContainer = Instantiate(cardContainerPrefab, cardContainerPoolParent);
		cardContainer.Initialize(card);
		cardContainers.Add(cardContainer);
		cardContainer.gameObject.SetActive(false);
	}
}
