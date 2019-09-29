public class DrawRandomCardsRequestEvent : CodeControl.Message
{
	public int amount;

	public DrawRandomCardsRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
