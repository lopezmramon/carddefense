public class CardsConsumeRequestEvent : CodeControl.Message
{
	public CardContainer[] cards;

	public CardsConsumeRequestEvent(CardContainer[] cards)
	{
		this.cards = cards;
	}
	public CardsConsumeRequestEvent(CardContainer card)
	{
		cards = new CardContainer[] { card };
	}
}
