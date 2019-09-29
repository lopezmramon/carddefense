
public class GenerateTilesRequestEvent : CodeControl.Message
{
	public Level level;

	public GenerateTilesRequestEvent(Level level)
	{
		this.level = level;
	}
}
