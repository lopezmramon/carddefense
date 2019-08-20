public class CursorExitTileEvent : CodeControl.Message
{
	public Tile tile;

	public CursorExitTileEvent(Tile tile)
	{
		this.tile = tile;
	}
}
