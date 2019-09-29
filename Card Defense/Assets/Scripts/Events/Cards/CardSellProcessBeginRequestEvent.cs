public class CardSellProcessBeginRequestEvent : CodeControl.Message
{
	public int resourcePerCard;

	public CardSellProcessBeginRequestEvent(int resourcePerCard)
	{
		this.resourcePerCard = resourcePerCard;
	}
}
