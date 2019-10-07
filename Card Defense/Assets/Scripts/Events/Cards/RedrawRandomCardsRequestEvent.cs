public class RedrawRandomCardsRequestEvent : CodeControl.Message
{
	public int amount;
	public RedrawRandomCardsRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
