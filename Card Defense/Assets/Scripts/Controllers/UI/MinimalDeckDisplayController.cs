using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MinimalDeckDisplayController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image background;
	public TextMeshProUGUI deckName, cardAmount;
	private System.Action<MinimalDeckDisplayController> callback;
	public CardDeck deck;

	public void Initialize(CardDeck deck, System.Action<MinimalDeckDisplayController> callback)
	{
		this.deck = deck;
		deckName.text = deck.name;
		cardAmount.text = deck.cardIndexes.Count.ToString();
		this.callback = callback;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		callback(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{

	}

	public void OnPointerExit(PointerEventData eventData)
	{

	}
}
