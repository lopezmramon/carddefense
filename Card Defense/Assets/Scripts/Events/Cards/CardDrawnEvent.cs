public class CardDrawnEvent : CodeControl.Message
{
	public CardContainer card;

	public CardDrawnEvent(CardContainer card)
	{
		this.card = card;
	}
}
