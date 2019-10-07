public class LivesChangedEvent : CodeControl.Message
{
	public int totalLives;

	public LivesChangedEvent(int totalLives)
	{
		this.totalLives = totalLives;
	}
}
