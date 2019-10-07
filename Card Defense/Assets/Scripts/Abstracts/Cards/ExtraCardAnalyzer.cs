using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCardAnalyzer
{
	public void AnalyzeExtraCard(Card card)
	{
		switch (card.extraModifier)
		{
			case ExtraModifier.Lives:
				DispatchLivesChangeRequestEvent(card.extraModifierValue);
				break;
			case ExtraModifier.Resources:
				DispatchResourceChangeRequestEvent(card.extraModifierValue);
				break;
			case ExtraModifier.ResourceMultiplier:
				DispatchResourceMultiplierRequestEvent(card.duration, card.extraModifierValue);
				break;
		}
	}

	private void DispatchResourceMultiplierRequestEvent(float duration, float multiplier)
	{
		CodeControl.Message.Send(new ResourceMultiplierStartRequestEvent(duration, multiplier));
	}

	private void DispatchLivesChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new LivesChangeRequestEvent(amount));
	}

	private void DispatchResourceChangeRequestEvent(int amount)
	{
		CodeControl.Message.Send(new ResourceChangeRequestEvent(amount));
	}
}
