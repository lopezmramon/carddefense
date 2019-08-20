
public class CursorEnterTileEvent : CodeControl.Message
{
	public Tile tile;

	public CursorEnterTileEvent(Tile tile)
	{
		this.tile = tile;
	}
}
