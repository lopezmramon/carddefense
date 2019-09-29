public class CardPickedEvent : CodeControl.Message
{
	public CardContainer card;
	public bool picked;
	public int amountLeft;

	public CardPickedEvent(CardContainer card, bool picked, int amountLeft)
	{
		this.card = card;
		this.picked = picked;
		this.amountLeft = amountLeft;
	}
}
