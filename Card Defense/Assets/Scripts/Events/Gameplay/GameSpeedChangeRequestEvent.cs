public class GameSpeedChangeRequestEvent : CodeControl.Message
{
	public float change;

	public GameSpeedChangeRequestEvent(float change)
	{
		this.change = change;
	}
}
