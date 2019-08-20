
public class LevelReadyEvent : CodeControl.Message
{
	public Level level;

	public LevelReadyEvent(Level level)
	{
		this.level = level;
	}
}
