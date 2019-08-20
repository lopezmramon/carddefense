
public class CardConsumedEvent : CodeControl.Message
{
	public CardContainer consumedCard;

	public CardConsumedEvent(CardContainer consumedCard)
	{
		this.consumedCard = consumedCard;
	}
}
