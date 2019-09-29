using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModifierAnalyzer
{
	private HandModifier[] currentHandModifiers;
	private int[] currentHandModifierValues;
	public void AnalyzeModifiers(HandModifier[] handModifiers, int[] handModifierValues)
	{
		currentHandModifiers = handModifiers;
		currentHandModifierValues = handModifierValues;
		for (int i = 0; i < handModifiers.Length; i++)
		{
			switch (handModifiers[i])
			{
				case HandModifier.DiscardRandomCards:
					DispatchDiscardRandomCardsRequestEvent(handModifierValues[i]);
					break;
				case HandModifier.Draw:
					DispatchDrawRandomCardsRequestEvent(handModifierValues[i]);
					break;
				case HandModifier.Redraw:
					DispatchCardPickStartRequestEvent(handModifierValues[i]);
					break;
				case HandModifier.RedrawWholeHand:
					DispatchRedrawHandRequestEvent();
					break;
				case HandModifier.ResourceChange:
					DispatchResourceChangeRequestEvent(handModifierValues[i]);
					break;
				case HandModifier.Sell:
					DispatchCardSellProcessBeginRequestEvent(handModifierValues[i]);
					break;
			}
		}
	}

	public void FinalizeHandModifierOperation(HashSet<CardContainer> pickedCards)
	{
		if (pickedCards == null || pickedCards.Count == 0) return;
		CardContainer[] cards = new CardContainer[pickedCards.Count];
		pickedCards.CopyTo(cards);
		for (int i = 0; i < currentHandModifiers.Length; i++)
		{
			switch (currentHandModifiers[i])
			{
				case HandModifier.DiscardRandomCards:
					break;
				case HandModifier.Draw:
					break;
				case HandModifier.Redraw:
					DispatchCardsConsumeRequestEvent(cards);
					DispatchDrawRandomCardsRequestEvent(cards.Length);
					break;
				case HandModifier.RedrawWholeHand:
					break;
				case HandModifier.ResourceChange:
					break;
				case HandModifier.Sell:
					DispatchResourceChangeRequestEvent(pickedCards.Count * currentHandModifierValues[i]);
					break;
			}
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

	private void DispatchCardPickStartRequestEvent(int amount)
	{
		CodeControl.Message.Send(new CardPickStartRequestEvent(amount));
	}

	private void DispatchDrawRandomCardsRequestEvent(int amount)
	{
		CodeControl.Message.Send(new DrawRandomCardsRequestEvent(amount));
	}

	private void DispatchDiscardRandomCardsRequestEvent(int amount)
	{
		CodeControl.Message.Send(new DiscardRandomCardsRequestEvent(amount));
	}

	private void DispatchCardsConsumeRequestEvent(CardContainer[] cards)
	{
		CodeControl.Message.Send(new CardsConsumeRequestEvent(cards));
	}
}
