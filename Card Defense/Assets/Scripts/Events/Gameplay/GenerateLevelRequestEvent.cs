public class GenerateLevelRequestEvent : CodeControl.Message
{
	public int levelIndex;
	public bool generateAlreadySelected = false;

	public GenerateLevelRequestEvent(int levelIndex)
	{
		this.levelIndex = levelIndex;
	}

	public GenerateLevelRequestEvent(bool generateAlreadySelected)
	{
		this.generateAlreadySelected = generateAlreadySelected;
	}
}
