public class CardPickRequestEvent : CodeControl.Message
{
	public CardContainer card;

	public CardPickRequestEvent(CardContainer card)
	{
		this.card = card;
	}
}
