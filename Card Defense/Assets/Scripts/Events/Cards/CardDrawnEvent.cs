public class CardDrawnEvent : CodeControl.Message
{
	public CardContainer card;
	public int cardsLeftInDrawPile;

	public CardDrawnEvent(CardContainer card, int cardsLeftInDrawPile)
	{
		this.card = card;
		this.cardsLeftInDrawPile = cardsLeftInDrawPile;
	}
}
