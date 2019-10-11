public class CountdownStartedEvent : CodeControl.Message
{
	public float duration;

	public CountdownStartedEvent(float duration)
	{
		this.duration = duration;
	}
}
