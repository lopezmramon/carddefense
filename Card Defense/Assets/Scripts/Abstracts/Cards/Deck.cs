using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Deck<T>
{
	private List<T> draw;
	public int CardsInDrawPile
	{
		get
		{
			return draw.Count;
		}
	}
	private List<T> discard;
	public int CardsInDiscardPile
	{
		get
		{
			return discard.Count;
		}
	}

	public Deck(List<T> cards)
	{
		this.draw = cards;
		discard = new List<T>();
	}
	public Deck(T[] cards)
	{
		List<T> cardList = new List<T>();
		foreach(T card in cards)
		{
			cardList.Add(card);
		}
		this.draw = cardList;
		discard = new List<T>();
	}

	// Shuffle the Current Deck of cards
	public void Shuffle()
	{
		for (int i = draw.Count - 1; i > 0; --i)
		{
			int j = Random.Range(0, i + 1);
			T card = draw[j];
			draw[j] = draw[i];
			draw[i] = card;
		}
	}

	// Return a list of drawn Cards from deck
	public List<T> Draw(int numberToDraw = 1)
	{
		if (numberToDraw > draw.Count)
			numberToDraw = draw.Count;
		List<T> drawnCards = new List<T>();
		for (int i = 0; i < numberToDraw; ++i)
		{
			drawnCards.Add(draw[0]);
			draw.RemoveAt(0);
		}
		return drawnCards;
	}


	// Return a list of drawn Cards from discard
	public List<T> DrawDiscard(int numberToDraw = 1)
	{
		if (numberToDraw > discard.Count)
			numberToDraw = discard.Count;
		List<T> drawnCards = new List<T>();
		for (int i = 0; i < numberToDraw; ++i)
		{
			drawnCards.Add(discard[0]);
			discard.RemoveAt(0);
		}
		return drawnCards;
	}

	// Put a card back, default is top  set bool to false to put it on the bottom
	public void ReturnCard(T card, bool onTop = true)
	{
		if (onTop)
			draw.Insert(0, card);
		else
			draw.Add(card);
	}

	public void DiscardCard(T card)
	{
		discard.Insert(0, card);
	}

	public void ShuffleDiscard()
	{
		foreach (T OneCard in discard)
		{
			draw.Add(OneCard);
		}
		discard.Clear();
		Shuffle();
	}
}