
public class WaveStartedEvent : CodeControl.Message
{
	public int wave;

	public WaveStartedEvent(int wave)
	{
		this.wave = wave;
	}
}
