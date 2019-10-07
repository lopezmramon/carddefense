public class ResourceMultiplierStartRequestEvent : CodeControl.Message
{
	public float duration, multiplier;

	public ResourceMultiplierStartRequestEvent(float duration, float multiplier)
	{
		this.duration = duration;
		this.multiplier = multiplier;
	}
}
