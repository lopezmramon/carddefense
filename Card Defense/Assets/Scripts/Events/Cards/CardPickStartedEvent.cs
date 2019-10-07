public class CardPickStartedEvent : CodeControl.Message
{
	public int maxAmount;
	public bool unrestricted;
	public HandModifier handModifier;
	public float value;

	public CardPickStartedEvent(int maxAmount)
	{
		this.maxAmount = maxAmount;
		unrestricted = maxAmount == 0;
	}

	public CardPickStartedEvent()
	{
		unrestricted = true;
	}

	public CardPickStartedEvent(int amount, bool unrestricted, HandModifier handModifier) : this(amount)
	{
		this.unrestricted = unrestricted;
		this.handModifier = handModifier;
	}

	public CardPickStartedEvent(int maxAmount, HandModifier handModifier) : this(maxAmount)
	{
		this.handModifier = handModifier;
		if (maxAmount == 0) unrestricted = true;
	}

	public CardPickStartedEvent(int maxAmount, bool unrestricted, HandModifier handModifier, float value) : this(maxAmount, unrestricted, handModifier)
	{
		this.value = value;
	}

	public CardPickStartedEvent(int maxAmount, HandModifier handModifier, float value) : this(maxAmount, handModifier)
	{
		if (maxAmount == 0) unrestricted = true;
		this.value = value;
	}
}
