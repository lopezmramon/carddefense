public class ResourceChangedEvent : CodeControl.Message
{
	public int resourceAmount;

	public ResourceChangedEvent(int resourceAmount)
	{
		this.resourceAmount = resourceAmount;
	}
}
