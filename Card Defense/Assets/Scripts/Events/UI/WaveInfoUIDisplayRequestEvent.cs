public class WaveInfoUIDisplayRequestEvent : CodeControl.Message
{
	public Wave wave;

	public WaveInfoUIDisplayRequestEvent(Wave wave)
	{
		this.wave = wave;
	}
}
