using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrabbedEvent : CodeControl.Message
{
	public CardContainer card;

	public CardGrabbedEvent(CardContainer card)
	{
		this.card = card;
	}
}
