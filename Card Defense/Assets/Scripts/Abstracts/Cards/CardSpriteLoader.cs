using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CardSpriteLoader
{
	private Sprite[] loadedSprites;

	public CardSpriteLoader()
	{
		loadedSprites = Resources.LoadAll<Sprite>("Art/CardSprites/");
	}

	public void LoadSpritesForCards(Card[] cards, Action<Dictionary<Card, Sprite[]>> OnLoadFinished)
	{
		Dictionary<Card, Sprite[]> cardSprites = new Dictionary<Card, Sprite[]>();
		foreach (Card card in cards)
		{
			Sprite[] sprites = new Sprite[3];
			for (int i = 0; i < sprites.Length; i++)
			{
				string spriteName = i == 0 ? card.backgroundFileName : i == 1 ? card.illustrationFileName : card.resourceFileName;
				sprites[i] = Array.Find(loadedSprites, sprite => sprite.name == spriteName);
			}
			cardSprites.Add(card, sprites);
		}
		OnLoadFinished(cardSprites);
	}
}
