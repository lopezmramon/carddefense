public class TowerSoldEvent : CodeControl.Message
{
	public TowerController tower;

	public TowerSoldEvent(TowerController tower)
	{
		this.tower = tower;
	}
}
