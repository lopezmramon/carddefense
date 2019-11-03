public class CardConsumedEvent : CodeControl.Message
{
	public CardContainer consumedCard;
	public int cardsLeftInDiscardPile;

	public CardConsumedEvent(CardContainer consumedCard, int cardsLeftInDiscardPile)
	{
		this.consumedCard = consumedCard;
		this.cardsLeftInDiscardPile = cardsLeftInDiscardPile;
	}
}
