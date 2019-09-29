using UnityEngine;

public class CardDroppedEvent : CodeControl.Message
{
	public CardContainer card;
	public bool overDrawer;

	public CardDroppedEvent(CardContainer card, bool overDrawer)
	{
		this.card = card;
		this.overDrawer = overDrawer;
	}
}
