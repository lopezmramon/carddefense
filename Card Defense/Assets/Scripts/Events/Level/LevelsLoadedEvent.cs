public class LevelsLoadedEvent : CodeControl.Message
{
	public Level[] levels;

	public LevelsLoadedEvent(Level[] levels)
	{
		this.levels = levels;
	}
}
