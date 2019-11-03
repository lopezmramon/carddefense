using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class CardDisplayController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image background, cardImage;
	public TextMeshProUGUI cardName, description, cost, amountText;
	private Action<CardDisplayController> leftClickCallback, rightClickCallback;
	public Card card;
	private Outline outline;
	private int amountLeft;

	public void Initialize(Card card, int amount, Action<CardDisplayController> leftClickCallback, Action<CardDisplayController> rightClickCallback)
	{
		SetupOutline();
		this.card = card;
		name = card.name + " Display";
		background.sprite = card.backgroundSprite;
		cardImage.sprite = card.illustrationSprite;
		cardName.text = card.name;
		description.text = card.description;
		cost.text = card.cost.ToString();
		this.amountLeft = amount;
		amountText.text = amount.ToString();
		this.leftClickCallback = leftClickCallback;
		this.rightClickCallback = rightClickCallback;
	}

	private void SetupOutline()
	{
		outline = background.gameObject.AddComponent<Outline>();
		outline.effectDistance = new Vector2(6, 6);
		outline.effectColor = Color.yellow;
		outline.enabled = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			leftClickCallback(this);
		}
		else if (eventData.button == PointerEventData.InputButton.Right)
		{
			rightClickCallback(this);
		}
	}

	public void ChangeAmount(int change)
	{
		amountLeft += change;		
		amountText.text = amountLeft.ToString();
	}

	public void SetAmount(int amount)
	{
		this.amountLeft = amount;	
		amountText.text = amount.ToString();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		outline.enabled = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		outline.enabled = false;
	}
}
