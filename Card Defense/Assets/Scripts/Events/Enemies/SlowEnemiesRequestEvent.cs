public class SlowEnemiesRequestEvent : CodeControl.Message
{
	public bool all;
	public int amount;
	public float duration;
	public float slowAmount;

	public SlowEnemiesRequestEvent(bool all, float duration, float slowAmount)
	{
		this.all = all;
		this.duration = duration;
		this.slowAmount = slowAmount;
	}

	public SlowEnemiesRequestEvent(int amount, float duration, float slowAmount)
	{
		this.amount = amount;
		this.duration = duration;
		this.slowAmount = slowAmount;
	}
}
