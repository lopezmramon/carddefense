using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
[System.Serializable]
public static class CollectionUtils
{
	public static CardDeck LoadCollection()
	{
		string path = $"{Application.dataPath}/Resources/Collection/collection.json";
		if (!File.Exists(path))
		{
			CardDeck baseCollection = new CardDeck(new List<int>());
			baseCollection.name = "Base Collection";
			int[] baseCards = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };
			baseCollection.cardIndexes.AddRange(baseCards);
			string json = JsonUtility.ToJson(baseCollection);
			File.WriteAllText(path, json);
			return baseCollection;
		}
		else
		{
			string json = File.ReadAllText(path);
			return JsonUtility.FromJson<CardDeck>(json);
		}
	}

	public static bool AddToCollection(Card card)
	{
		CardDeck currentCollection = LoadCollection();
		currentCollection.cardIndexes.Add(card.index);
		string json = JsonUtility.ToJson(currentCollection);
		string path = $"{Application.dataPath}/Resources/Collection/collection.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static bool RemoveFromCollection(Card card)
	{
		CardDeck currentCollection = LoadCollection();
		currentCollection.cardIndexes.Remove(card.index);
		string json = JsonUtility.ToJson(currentCollection);
		string path = $"{Application.dataPath}/Resources/Collection/collection.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static CardDeck RelativeCollection(CardDeck collection, CardDeck deck)
	{
		if (deck == null) return collection;
		CardDeck relativeCollection = new CardDeck(new List<int>());
		foreach (int card in collection.cardIndexes)
		{
			relativeCollection.cardIndexes.Add(card);
		}
		foreach (int card in deck.cardIndexes)
		{
			relativeCollection.cardIndexes.Remove(card);
		}
		return relativeCollection;
	}

	public static CardDeck NewDeck(string name)
	{
		CardDeck deck = new CardDeck(new List<int>());
		deck.name = name == string.Empty ? RandomWordGenerator.CreateRandomWordNumberCombination() : name;
		SaveDeck(deck);
		CollectionManager.currentDecks.Add(deck);
		return deck;
	}

	public static void DeleteDeck(CardDeck cardDeck)
	{
		CollectionManager.currentDecks.Remove(cardDeck);
		if (cardDeck.name == "Base Deck")
		{
			PlayerPrefs.SetInt("baseDeckDeleted", 1);
		}
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{cardDeck.name}.json";
		if (!File.Exists(path)) return;
		File.Delete(path);
	}

	public static bool SaveDeck(List<int> cards, string deckName)
	{
		CardDeck deck = new CardDeck(cards);
		string json = JsonUtility.ToJson(deck);
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{deckName}.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static bool SaveDeck(List<Card> cards, string deckName)
	{
		string json = JsonUtility.ToJson(CardDeckFromCardList(cards));
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{deckName}.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static bool SaveDeck(CardDeck deck, string deckName)
	{
		string json = JsonUtility.ToJson(deck);
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{deckName}.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static bool SaveDeck(CardDeck deck)
	{
		string json = JsonUtility.ToJson(deck);
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{deck.name}.json";
		File.WriteAllText(path, json);
		return File.Exists(path);
	}

	public static CardDeck LoadDeck(string deckName)
	{
		string path = $"{Application.dataPath}/Resources/Collection/Decks/{deckName}.json";
		if (!File.Exists(path)) return null;
		string json = File.ReadAllText(path);
		return JsonUtility.FromJson<CardDeck>(json);
	}

	public static List<CardDeck> LoadAllDecks()
	{
		List<CardDeck> decks = new List<CardDeck>();
		string directoryPath = $"{Application.dataPath}/Resources/Collection/Decks/";
		DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
		if (directoryInfo.GetFiles().Length == 0)
		{
			if (PlayerPrefs.HasKey("baseDeckDeleted") && PlayerPrefs.GetInt("baseDeckDeleted") == 1)
			{

			}
			else
			{
				List<int> baseCards = new List<int>();
				baseCards.AddRange(new int[] { 0, 1, 2, 3, 3, 4, 5, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 });
				CardDeck baseDeck = new CardDeck(baseCards);
				baseDeck.name = "Base Deck";
				SaveDeck(baseDeck, baseDeck.name);
				decks.Add(baseDeck);
			}
		}
		else
		{
			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				if (file.Extension != ".json") continue;
				string json = File.ReadAllText(file.FullName);
				decks.Add(JsonUtility.FromJson<CardDeck>(json));
			}
		}
		return decks;
	}

	public static int DuplicatesInList(List<Card> list, Card card)
	{
		return list.FindAll(x => x.index == card.index).Count;
	}

	public static int DuplicatesInList(List<int> list, int cardIndex)
	{
		return list.FindAll(x => x == cardIndex).Count;
	}

	public static int DuplicatesInDeck(CardDeck cardDeck, int cardIndex)
	{
		return DuplicatesInList(cardDeck.cardIndexes, cardIndex);
	}

	private static CardDeck CardDeckFromCardList(List<Card> cards)
	{
		List<int> cardIndexes = new List<int>();
		foreach (Card card in cards)
		{
			cardIndexes.Add(card.index);
		}
		return new CardDeck(cardIndexes);
	}

}
