public class CardPickedEvent : CodeControl.Message
{
	public CardContainer card;
	public bool picked;
	public int maxAmount, amountPicked;

	public CardPickedEvent(CardContainer card, bool picked, int maxAmount, int amountPicked)
	{
		this.card = card;
		this.picked = picked;
		this.maxAmount = maxAmount;
		this.amountPicked = amountPicked;
	}
}
