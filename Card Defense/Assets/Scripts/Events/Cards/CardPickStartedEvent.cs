public class CardPickStartedEvent : CodeControl.Message
{
	public int amount;
	public bool unrestricted;

	public CardPickStartedEvent(int amount)
	{
		this.amount = amount;
		unrestricted = amount == 0;
	}

	public CardPickStartedEvent()
	{
		unrestricted = true;
	}
}
