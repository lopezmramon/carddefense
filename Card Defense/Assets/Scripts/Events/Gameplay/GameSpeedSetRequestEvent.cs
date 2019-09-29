public class GameSpeedSetRequestEvent : CodeControl.Message
{
	public float speed;

	public GameSpeedSetRequestEvent(float speed)
	{
		this.speed = speed;
	}
}
