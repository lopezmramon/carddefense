public class TowerUpgradedEvent : CodeControl.Message
{
	public Tower tower;

	public TowerUpgradedEvent(Tower tower)
	{
		this.tower = tower;
	}
}
