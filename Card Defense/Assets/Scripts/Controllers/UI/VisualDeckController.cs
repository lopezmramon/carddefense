using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VisualDeckController : MonoBehaviour
{
	public Image cardBackImage;
	public TextMeshProUGUI amountLeft;

	public void Initialize(Sprite cardBackSprite)
	{
		if (cardBackSprite == null) return;
		cardBackImage.sprite = cardBackSprite;
	}

	public void SetAmountLeft(int amount)
	{
		amountLeft.text = amount.ToString();
	}
}
