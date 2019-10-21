public class TileVFXRequestEvent : CodeControl.Message
{
	public Element element;
	public Tile tile;

	public TileVFXRequestEvent(Tile tile, Element element)
	{
		this.tile = tile;
		this.element = element;
	}
}
