using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModifierAnalyzer
{
	public HandModifier currentHandModifier;
	public int currentHandModifierValue;
	public void AnalyzeModifiers(HandModifier handModifier, int handModifierValue)
	{
		currentHandModifier = handModifier;
		currentHandModifierValue = handModifierValue;
		switch (handModifier)
		{
			case HandModifier.RedrawRandomCards:
				DispatchRedrawRandomCardsRequestEvent(handModifierValue);
				break;
			case HandModifier.Draw:
				DispatchDrawRandomCardsRequestEvent(handModifierValue);
				break;
			case HandModifier.Redraw:
				DispatchCardPickStartRequestEvent(handModifierValue, handModifier);
				break;
			case HandModifier.RedrawHand:
				DispatchRedrawHandRequestEvent();
				break;
			case HandModifier.ResourceChange:
				DispatchResourceChangeRequestEvent(handModifierValue);
				break;
			case HandModifier.Sell:
				DispatchCardSellProcessBeginRequestEvent(handModifierValue);
				break;
		}
	}

	public void FinalizeHandModifierOperation(HashSet<CardContainer> pickedCards)
	{
		if (pickedCards == null || pickedCards.Count == 0) return;
		CardContainer[] cards = new CardContainer[pickedCards.Count];
		pickedCards.CopyTo(cards);
		switch (currentHandModifier)
		{
			case HandModifier.RedrawRandomCards:
				break;
			case HandModifier.Draw:
				break;
			case HandModifier.Redraw:
				DispatchCardsConsumeRequestEvent(cards);
				DispatchDrawRandomCardsRequestEvent(cards.Length);
				break;
			case HandModifier.RedrawHand:
				break;
			case HandModifier.ResourceChange:
				break;
			case HandModifier.Sell:
				DispatchResourceChangeRequestEvent(pickedCards.Count * currentHandModifierValue);
				break;
		}
		DispatchCardsConsumeRequestEvent(cards);
	}

	private void DispatchCardSellProcessBeginRequestEvent(int amountPerCard)
	{
		CodeControl.Message.Send(new CardSellProcessBeginRequestEvent(amountPerCard));
	}

	private void DispatchResourceChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new ResourceChangeRequestEvent(amount));
	}

	private void DispatchRedrawHandRequestEvent()
	{
		CodeControl.Message.Send(new RedrawHandRequestEvent());
	}

	private void DispatchCardPickStartRequestEvent(int amount, HandModifier handModifier)
	{
		CodeControl.Message.Send(new CardPickStartRequestEvent(amount, handModifier));
	}

	private void DispatchDrawRandomCardsRequestEvent(int amount)
	{
		CodeControl.Message.Send(new DrawRandomCardsRequestEvent(amount));
	}

	private void DispatchRedrawRandomCardsRequestEvent(int amount)
	{
		CodeControl.Message.Send(new RedrawRandomCardsRequestEvent(amount));
	}

	private void DispatchCardsConsumeRequestEvent(CardContainer[] cards)
	{
		CodeControl.Message.Send(new CardsConsumeRequestEvent(cards));
	}
}
