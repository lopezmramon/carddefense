public class LivesChangeRequestEvent : CodeControl.Message
{
	public int amount;

	public LivesChangeRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
