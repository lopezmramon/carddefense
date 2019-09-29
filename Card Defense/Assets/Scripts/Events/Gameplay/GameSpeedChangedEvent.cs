public class GameSpeedChangedEvent : CodeControl.Message
{
	public float speed;

	public GameSpeedChangedEvent(float speed)
	{
		this.speed = speed;
	}
}
