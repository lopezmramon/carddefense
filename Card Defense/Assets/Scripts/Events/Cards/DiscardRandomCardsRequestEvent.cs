public class DiscardRandomCardsRequestEvent : CodeControl.Message
{
	public int amount;

	public DiscardRandomCardsRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
