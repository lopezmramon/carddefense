using System;
using UnityEngine;
using UnityEngine.UI;

public class PickedCardsDisplayController : MonoBehaviour
{
	public Text neededCards, flavor;
	public Button confirm;
	private bool unrestricted;

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
		SetNeededCardsText(obj.amount);
		SetFlavorText();
	}

	private void OnCardPickEnded(CardPickEndedEvent obj)
	{
		ToggleVisualDisplay(false);
	}

	private void OnCardPicked(CardPickedEvent obj)
	{
		SetNeededCardsText(obj.amountLeft);
		confirm.interactable = obj.amountLeft <= 0;
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
		if (unrestricted)
		{
			flavorText = "You can pick as many as you want";
		}
		else
		{
			flavorText = "Don't worry, take your time...";
		}
		flavor.text = flavorText;
	}

	private void SetNeededCardsText(int neededCards)
	{
		string neededCardsText = string.Empty;
		if (unrestricted)
		{
			neededCardsText = "Pick your cards";
		}
		else
		{
			neededCardsText = string.Format("Select {0} more {1}", neededCards,
				neededCards == 1 ? "card" : neededCards == 0 ? "Are you sure?" : "cards");
		}
		this.neededCards.text = neededCardsText;
	}

	private void DispatchConfirmPickedCardsRequestEvent()
	{
		CodeControl.Message.Send(new ConfirmCardsPickedRequestEvent());
	}
}
