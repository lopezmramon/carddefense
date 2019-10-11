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
			case ExtraModifier.Slow:
				DispatchSlowEnemiesRequestEvent(false, (int)card.propertyModifierValues[0], card.duration, card.extraModifierValue);
				break;
			case ExtraModifier.SlowAll:
				DispatchSlowEnemiesRequestEvent(true, 0, card.duration, card.extraModifierValue);
				break;
			case ExtraModifier.Damage:
			case ExtraModifier.DamageAll:
				DispatchDamageEnemiesRequestEvent(card.extraModifier == ExtraModifier.DamageAll, (int)card.duration, card.extraModifierValue);
				break;
		}
	}

	private void DispatchSlowEnemiesRequestEvent(bool all, int amount, float duration, float slowAmount)
	{
		if (all)
		{
			CodeControl.Message.Send(new SlowEnemiesRequestEvent(all, duration, slowAmount));
		}
		else
		{
			CodeControl.Message.Send(new SlowEnemiesRequestEvent(amount, duration, slowAmount));
		}
	}

	private void DispatchDamageEnemiesRequestEvent(bool all, int amount, float damage)
	{
		if (all)
		{
			CodeControl.Message.Send(new DamageEnemiesRequestEvent(all, damage));
		}
		else
		{
			CodeControl.Message.Send(new DamageEnemiesRequestEvent(amount, damage));
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
