public class DeckPickedEvent : CodeControl.Message
{
	public CardDeck deck;

	public DeckPickedEvent(CardDeck deck)
	{
		this.deck = deck;
	}
}
