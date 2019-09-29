public class LevelSelectedEvent : CodeControl.Message
{
	public int levelIndex;

	public LevelSelectedEvent(int levelIndex)
	{
		this.levelIndex = levelIndex;
	}
}
