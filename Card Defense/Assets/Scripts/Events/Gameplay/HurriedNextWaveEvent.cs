public class HurriedNextWaveEvent : CodeControl.Message
{
	public float timeHurried;

	public HurriedNextWaveEvent(float timeHurried)
	{
		this.timeHurried = timeHurried;
	}
}
