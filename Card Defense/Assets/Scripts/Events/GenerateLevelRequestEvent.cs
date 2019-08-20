public class GenerateLevelRequestEvent : CodeControl.Message
{
	public int levelIndex;

	public GenerateLevelRequestEvent(int levelIndex)
	{
		this.levelIndex = levelIndex;
	}
}
