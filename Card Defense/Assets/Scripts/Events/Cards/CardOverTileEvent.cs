public class CardOverTileEvent : CodeControl.Message
{
	public CardContainer card;
	public Tile tile;

	public CardOverTileEvent(CardContainer card, Tile tile)
	{
		this.card = card;
		this.tile = tile;
	}
}
