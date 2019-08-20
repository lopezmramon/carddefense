
public class ContextMenuRequestEvent : CodeControl.Message
{
	public Tile tile;

	public ContextMenuRequestEvent(Tile tile)
	{
		this.tile = tile;
	}
}
