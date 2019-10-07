public class ResourceChangeRequestEvent : CodeControl.Message
{
	public int amount;

	public ResourceChangeRequestEvent(int amount)
	{
		this.amount = amount;
	}
}
