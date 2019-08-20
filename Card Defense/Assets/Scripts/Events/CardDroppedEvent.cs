
public class CardDroppedEvent : CodeControl.Message
{
	public CardContainer card;
	public CardDroppedEvent(CardContainer card)
	{
		this.card = card;
	}
}
