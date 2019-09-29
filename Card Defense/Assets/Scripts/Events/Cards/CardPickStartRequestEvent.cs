public class CardPickStartRequestEvent : CodeControl.Message
{
	public int amount;

	public CardPickStartRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
