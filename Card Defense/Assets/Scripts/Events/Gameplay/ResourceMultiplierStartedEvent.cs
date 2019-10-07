public class ResourceMultiplierStartedEvent : CodeControl.Message
{
	public int multiplier;

	public ResourceMultiplierStartedEvent(int multiplier)
	{
		this.multiplier = multiplier;
	}
}
