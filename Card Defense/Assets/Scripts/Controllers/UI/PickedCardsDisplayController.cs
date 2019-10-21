using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickedCardsDisplayController : MonoBehaviour
{
	public TextMeshProUGUI neededCards, flavor;
	public Button confirm;
	private bool unrestricted;
	private float value;
	private HandModifier currentHandModifier;

	private void Awake()
	{
		confirm.onClick.AddListener(() =>
		{
			DispatchConfirmPickedCardsRequestEvent();
		});
		ToggleVisualDisplay(false);
		CodeControl.Message.AddListener<CardPickStartedEvent>(OnCardPickStarted);
		CodeControl.Message.AddListener<CardPickEndedEvent>(OnCardPickEnded);
		CodeControl.Message.AddListener<CardPickedEvent>(OnCardPicked);
	}

	private void OnCardPickStarted(CardPickStartedEvent obj)
	{
		ToggleVisualDisplay(true);
		value = obj.value;
		currentHandModifier = obj.handModifier;
		unrestricted = obj.unrestricted;
		SetNeededCardsText(obj.maxAmount, 0);
		SetFlavorText();
	}

	private void OnCardPickEnded(CardPickEndedEvent obj)
	{
		ToggleVisualDisplay(false);
	}

	private void OnCardPicked(CardPickedEvent obj)
	{
		SetNeededCardsText(obj.maxAmount, obj.amountPicked);
		confirm.interactable = obj.maxAmount - obj.amountPicked <= 0 || unrestricted;
	}

	private void ToggleVisualDisplay(bool active)
	{
		neededCards.gameObject.SetActive(active);
		flavor.gameObject.SetActive(active);
		confirm.gameObject.SetActive(active);
		confirm.interactable = unrestricted;
	}

	private void SetFlavorText()
	{
		string flavorText = string.Empty;
		if (currentHandModifier == HandModifier.Sell)
		{
			flavorText = string.Format("Sell your cards for {0} each", value);
		}
		else
		{
			flavorText = "Don't worry, take your time...";
		}
		flavor.text = flavorText;
	}

	private void SetNeededCardsText(int maxAmount, int amountPicked)
	{
		string neededCardsText = string.Empty;
		if (unrestricted)
		{
			if (amountPicked == 0)
			{
				neededCardsText = "Pick your cards";
			}
			else
			{
				if (currentHandModifier == HandModifier.Sell)
				{
					neededCardsText = string.Format("You have picked {0} cards", amountPicked);
				}
				{
					neededCardsText = string.Format("You have picked {0} cards, for a total of {1} mana", amountPicked, amountPicked * value);
				}
			}
		}
		else
		{
			neededCardsText = string.Format("Select {0} more {1}", maxAmount,
				maxAmount == 1 ? "card" : maxAmount == 0 ? "Are you sure?" : "cards");
		}
		this.neededCards.text = neededCardsText;
	}

	private void DispatchConfirmPickedCardsRequestEvent()
	{
		CodeControl.Message.Send(new ConfirmCardsPickedRequestEvent());
	}
}
