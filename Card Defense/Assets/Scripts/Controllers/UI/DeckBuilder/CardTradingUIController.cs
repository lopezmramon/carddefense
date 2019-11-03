using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardTradingUIController : MonoBehaviour
{
	public Button sell, buy;
	public TextMeshProUGUI sellValue, buyCost, cardName, description, useCost;
	public Image illustration, background, cardBackground;
	private Action<Card> buyCallback, sellCallback;
	private Card card;

	private void Awake()
	{
		SetupButtons();
	}

	private void SetupButtons()
	{
		buy.onClick.AddListener(() => buyCallback(card));
		sell.onClick.AddListener(() => sellCallback(card));
	}

	internal void Initialize(Card card, Action<Card> buyCallback, Action<Card> sellCallback)
	{
		sellValue.text = "50";
		buyCost.text = "100";
		cardName.text = card.name;
		description.text = card.description;
		useCost.text = card.cost.ToString();
		illustration.sprite = card.illustrationSprite;
		cardBackground.sprite = card.backgroundSprite;
		this.card = card;
		this.buyCallback = buyCallback;
		this.sellCallback = sellCallback;
	}
}
