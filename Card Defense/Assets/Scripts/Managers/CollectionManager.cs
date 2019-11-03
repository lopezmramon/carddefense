using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
	public static List<CardDeck> currentDecks;
	public static CardDeck collection;

	private void Awake()
	{
		collection = CollectionUtils.LoadCollection();
		currentDecks = CollectionUtils.LoadAllDecks();
	}
}
