public class CardPickStartRequestEvent : CodeControl.Message
{
	public int amount;
	public HandModifier handModifier;
	public CardPickStartRequestEvent(int amount, HandModifier handModifier)
	{
		this.amount = amount;
		this.handModifier = handModifier;
	}
}
