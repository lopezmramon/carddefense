using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class MinimalCardDisplayController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image background, illustration;
	public TextMeshProUGUI cardName, amount;
	private System.Action<MinimalCardDisplayController> callback;
	public Card card;

	public void Initialize(Card card, int amount, System.Action<MinimalCardDisplayController> callback)
	{
		this.card = card;
		name = card.name;
		background.sprite = card.backgroundSprite;
		illustration.sprite = card.illustrationSprite;
		cardName.text = card.name;
		this.amount.text = amount.ToString();
		this.callback = callback;
	}

	public void SetAmount(int amount)
	{
		this.amount.text = amount.ToString();
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
